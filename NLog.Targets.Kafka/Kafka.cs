﻿using NLog.Common;
using NLog.Config;
using System;
using System.Collections.Generic;

namespace NLog.Targets
{
    [Target("Kafka")]
    public class Kafka : TargetWithLayout
    {
        protected KafkaClient client
        {
            get
            {
                if (_client == null)
                {
                    lock (lockObj)
                    {
                        if (_client == null)
                        {
                            //TODO: Pass host URL from config
                            _client = new KafkaClient();
                        }
                    }
                }
                return _client;
            }
        }

        private KafkaClient _client;
        Object lockObj = new Object();

        protected override void Write(LogEventInfo logEvent)
        {
            var message = this.Layout.Render(logEvent);
            SendMessageToQueue(message);
            base.Write(logEvent);
        }

        private void SendMessageToQueue(string message)
        {
            //client.Recieve();
            client.Post(message);
        }

        protected override void CloseTarget()
        {
            if (_client != null)
                _client.Dispose();
            base.CloseTarget();
        }

        [RequiredParameter]
        public string host { get; set; }

        [RequiredParameter]
        public string port { get; set; }

        [RequiredParameter]
        public string topic { get; set; }
    }
}
