using NLog.Config;
using System;

namespace NLog.Targets
{
    [NLogConfigurationItem]
    public class KafkaBroker
    {
        public KafkaBroker()
        {
            this.Address = string.Empty;
        }
        public KafkaBroker(string address)
        {
            this.Address = address;
        }
        [RequiredParameter]
        public string Address { get; set; }
    }
}
