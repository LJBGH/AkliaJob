using System;
using System.Collections.Generic;
using System.Text;

namespace AkliaJob.App
{
    public class AkliaSocket
    {

        private static AkliaSocket _this;
        public static AkliaSocket This
        {
            get
            {
                if (_this == null)
                {
                    _this = new AkliaSocket();
                }
                return _this;
            }
        }

        public WWebSocketClientHelper wSocketClient = new WWebSocketClientHelper("wss://localhost:5000/");
        public void Go()
        {
            wSocketClient.OnOpen -= WSocketClient_OnOpen;
            wSocketClient.OnMessage -= WSocketClient_OnMessage;
            wSocketClient.OnClose -= WSocketClient_OnClose;
            wSocketClient.OnError -= WSocketClient_OnError;

            wSocketClient.OnOpen += WSocketClient_OnOpen;
            wSocketClient.OnMessage += WSocketClient_OnMessage;
            wSocketClient.OnClose += WSocketClient_OnClose;
            wSocketClient.OnError += WSocketClient_OnError;
            wSocketClient.Open();
        }

        private void WSocketClient_OnError(object sender, Exception ex)
        {

        }

        private void WSocketClient_OnClose(object sender, EventArgs e)
        {

        }

        private void WSocketClient_OnMessage(object sender, string data)
        {
            //处理的消息错误将会忽略
            try
            {
                if (data.Contains("放映"))
                {
                    Console.WriteLine(data);
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void WSocketClient_OnOpen(object sender, EventArgs e)
        {

        }
    }
}
