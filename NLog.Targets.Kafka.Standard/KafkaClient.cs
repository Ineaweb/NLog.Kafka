using KafkaClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NLog.Targets
{
    public static class KafkaConnectionHelper
    {
        static Producer _producer = null;

        public static async Task<Producer> GetProducer(this Kafka kafkaObj)
        {
            if (_producer == null)
            {
                var addresses = from x in kafkaObj.Brokers
                                select new Uri(x.Address);
                var router = await new KafkaOptions(addresses.ToArray()).CreateRouterAsync();
                _producer = new Producer(router);
            }
            return _producer;
        }

        public static void CloseProducer()
        {
            if (_producer != null)
                _producer.Dispose();
            _producer = null;
        }
    }
}
