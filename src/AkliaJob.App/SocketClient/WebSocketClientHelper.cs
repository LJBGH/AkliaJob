using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkliaJob.App.SocketClient
{
    public class WebSocketClientHelper
    {
        /// <summary>
        /// WebSocket客户端
        /// </summary>
        ClientWebSocket _wsClient = null;

        /// <summary>
        /// 服务端链接
        /// </summary>
        Uri url = null;

        /// <summary>
        /// WebSocket状态
        /// </summary>
        public WebSocketState? State { get => _wsClient?.State; }

        public WebSocketClientHelper(string wsUrl)
        {
         
            this.url = new Uri(wsUrl);
            _wsClient = new ClientWebSocket();
        }

        /// <summary>
        /// 连接建立时触发
        /// </summary>
        public event EventHandler OnOpen;

        ///// <summary>
        ///// 客户端接收服务端数据时触发
        ///// </summary>
        //public event MessageEventHandler OnMessage;
        ///// <summary>
        ///// 通信发生错误时触发
        ///// </summary>
        //public event ErrorEventHandler OnError;

        /// <summary>
        /// 连接关闭时触发
        /// </summary>
        public event EventHandler OnClose;


        public void Open()
        {
            Task.Run(async () =>
            {
                if (_wsClient.State == WebSocketState.Connecting || _wsClient.State == WebSocketState.Open)
                    return;

                string strError = string.Empty;

                //初始化链接
                try
                {
                    await _wsClient.ConnectAsync(url, CancellationToken.None);

                    await SendMsg("客户端连接成功");

                    //全部消息容器
                    List<byte> bs = new List<byte>();
                    //缓冲区
                    var buffer = new byte[1024 * 4];
                    //监听Socket信息
                    var result = await _wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    do
                    {
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            bs.AddRange(buffer.Take(result.Count));

                            //消息是否已接收完全
                            if (result.EndOfMessage)
                            {
                                //发送过来的消息
                                string userMsg = Encoding.UTF8.GetString(bs.ToArray(), 0, bs.Count);;

                                //清空消息容器
                                bs = new List<byte>();
                            }
                        }
                        //继续监听Socket信息
                        result = await _wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    } while (!result.CloseStatus.HasValue);
                }
                catch (Exception ex)
                {
                    strError = "发生错误" + ex.Message;
                    Console.WriteLine(strError);
                }
            });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<bool> SendMsg(string msg) 
        {
            if (_wsClient.State != WebSocketState.Open)
                return false;
            try
            {
                var msgBuffer = Encoding.UTF8.GetBytes(msg);
                await _wsClient.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送失败"+ ex.Message);
                return false;
            }    
        }
    }
}
