namespace Loggly.Config
{
    public class TransportConfiguration : ITransportConfiguration
    {
        public LogTransport LogTransport { get; set; }

        public string EndpointHostname { get; set; }

        public int EndpointPort { get; set; }
    }
}