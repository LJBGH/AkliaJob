using Microsoft.Extensions.Hosting;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkliaJob.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StartWebSocket();

            CreateHostBuilder(args).Build().Run();
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                });


        /// <summary>
        /// Socket客户端连接
        /// </summary>
        /// <returns></returns>
        static async Task StartWebSocket() 
        {
            try
            {
                var client = new ClientWebSocket();
                client.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None).Wait();
                await StartReceiving(client);
                byte[] resultbuffer = System.Text.Encoding.Default.GetBytes($"客户端连接成功");
                await client.SendAsync(new ArraySegment<byte>(resultbuffer, 0, resultbuffer.Length),
                          WebSocketMessageType.Text,
                          true,
                          CancellationToken.None);

                //向服务端发送消息测试
                string line;
                while ((line = Console.ReadLine()) != "exit")
                {
                    var array = new ArraySegment<byte>(Encoding.UTF8.GetBytes(line));
                    await client.SendAsync(array, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 监听接收消息
        /// </summary>
        /// <param name="client"></param>
        static async Task StartReceiving(ClientWebSocket client)
        {
            while (true)
            {
                var array = new byte[4096];
                var result = await client.ReceiveAsync(new ArraySegment<byte>(array), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string msg = Encoding.UTF8.GetString(array, 0, result.Count);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("--> {0}", msg);
                }
            }
        }
    }
}
