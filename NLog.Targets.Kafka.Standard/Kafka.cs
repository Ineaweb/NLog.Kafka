using NLog.Common;
using NLog.Config;
using System;
using System.Collections.Generic;
using KafkaClient.Protocol;
using System.Threading.Tasks;
using KafkaClient;
using System.Threading;

namespace NLog.Targets
{
    [Target("Kafka")]
    public class Kafka : TargetWithLayout
    {
        public Kafka()
        {
            Brokers = new List<KafkaBroker>();
        }
        protected override void Write(LogEventInfo logEvent)
        {
            var message = this.Layout.Render(logEvent);
            SendMessageToQueue(message).Wait();
            base.Write(logEvent);
        }

        private async Task SendMessageToQueue(string message)
        {
            try
            {
                var queueMessage = new Message(Topic, message);
                var producer = await this.GetProducer();
                await producer.SendAsync(new[] { queueMessage }, Topic, CancellationToken.None);
            }
            catch (Exception ex)
            {
                InternalLogger.Error("Unable to send message to kafka queue", ex);
            }
        }

        protected override void CloseTarget()
        {
            KafkaConnectionHelper.CloseProducer();
            base.CloseTarget();
        }


        [RequiredParameter]
        public string Topic { get; set; }

        [RequiredParameter]
        [ArrayParameter(typeof(KafkaBroker), "broker")]
        public IList<KafkaBroker> Brokers { get; set; }

    }
}
