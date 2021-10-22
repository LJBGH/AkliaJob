using AkliaJob.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AkliaJob.Shared;
using Microsoft.Extensions.Logging;
using AkliaJob.App.StartupModels;

namespace AkliaJob.App
{
    class Program
    {
        private static ClientWebSocket client = new ClientWebSocket();
        private static CancellationTokenSource _socketLoopTokenSource;
        private static CancellationTokenSource _sendLoopTokenSource;

        static async Task Main(string[] args)
        {         
            //构建服务
            var serviceProvider = CommonServiceModel.ServicesBilder();

            var testService = serviceProvider.GetService<ITestService>();

            var logger = serviceProvider.GetLogger<Program>();


            //开启WenSockert连接
            var isConn  = await TryConnectWebSocket();

            if (isConn) 
            {
                await StartReceiving(client, testService, logger);
            }

            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {

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
                byte[] resultbuffer = System.Text.Encoding.Default.GetBytes($"客户端连接成功");
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
        /// 监听接收消息
        /// </summary>
        /// <param name="client"></param>
        static async Task StartReceiving(ClientWebSocket client, ITestService testService, ILogger<Program> logger)
        {
            try
            {
                var cancellationToken = _socketLoopTokenSource.Token;
                WebSocketReceiveResult result = null;
                var buffer = new byte[4096];
                while (client.State != WebSocketState.Closed && !cancellationToken.IsCancellationRequested)
                {
                    result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
