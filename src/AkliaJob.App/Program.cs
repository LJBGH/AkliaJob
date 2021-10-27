using AkliaJob.App.Services;
using AkliaJob.App.StartupModels;
using AkliaJob.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AkliaJob.Serilog;

namespace AkliaJob.App
{
    class Program
    {
        private static ClientWebSocket client = new ClientWebSocket();
        private static CancellationTokenSource _socketLoopTokenSource;
        private static CancellationTokenSource _sendLoopTokenSource;

        static async Task Main(string[] args)
        {
            await Task.CompletedTask;

            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                 .AddSerilog()
                .ConfigureServices(async (hostContext, services) =>
                {
                    //构建服务
                    var servicePrivier = services.ServicesBilder();
                    var testService = servicePrivier.GetService<ITestService>();
                    var logger = servicePrivier.GetLogger<Program>();

                    //开启WenSockert连接
                    var isConn = await TryConnectWebSocket();

                    if (isConn)
                    {
                        StartReceiving(client, testService, logger);
                        Heartbeat();
                    }
                });
               


        /// <summary>
        /// WebSocket连接开启
        /// </summary>
        /// <returns></returns>
        static async Task<bool> TryConnectWebSocket()
        {
            _socketLoopTokenSource = new CancellationTokenSource();
            _sendLoopTokenSource = new CancellationTokenSource();
            if (client == null)
            {
                client = new ClientWebSocket();
            }
            if (client.State == WebSocketState.Open)
            {
                return true;
            }
            try
            {
                client.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None).Wait();
                byte[] resultbuffer = System.Text.Encoding.UTF8.GetBytes($"客户端连接成功");
                await client.SendAsync(new ArraySegment<byte>(resultbuffer, 0, resultbuffer.Length),
                          WebSocketMessageType.Text,
                          true,
                          CancellationToken.None);

                var linkmage = new ArraySegment<byte>(Encoding.UTF8.GetBytes("客户端发送消息测试"));
                await client.SendAsync(linkmage, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        static void Heartbeat()
        {
            Task.Run(async () =>
            {
                var token = _sendLoopTokenSource.Token;
                var data = Encoding.UTF8.GetBytes("sent ping to server");
                var i = 0;
                while (i < 10 && !token.IsCancellationRequested)
                {
                    await Task.Delay(1000, token);
                    if (client?.State == WebSocketState.Open)
                    {
                        try
                        {
                            if (!token.IsCancellationRequested)
                            {
                                await client.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    i++;
                }
            });
        }



        /// <summary>
        /// 监听接收消息
        /// </summary>
        /// <param name="client"></param>
        static void StartReceiving(ClientWebSocket client, ITestService testService, ILogger<Program> logger)
        {
            var cancellationToken = _socketLoopTokenSource.Token;
            Task.Run(async () =>
            {
                try
                {
                    WebSocketReceiveResult result = null;
                    var buffer = new byte[4096];
                    while (client.State != WebSocketState.Closed && !cancellationToken.IsCancellationRequested)
                    {
                        result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (client.State == WebSocketState.CloseReceived && result.MessageType == WebSocketMessageType.Close)
                            {
                                _sendLoopTokenSource.Cancel();
                                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "关闭客户端", CancellationToken.None);
                            }

                            if (client.State == WebSocketState.Open)
                            {
                                //文本类型消息
                                if (result.MessageType == WebSocketMessageType.Text)
                                {
                                    await testService.TestAsync();
                                    string msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                                    logger.LogInformation($"{msg}");
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    if (client.State != WebSocketState.Closed)
                    {
                        client.Abort();
                    }
                    client.Dispose();
                }
            });

        }
    }
}
