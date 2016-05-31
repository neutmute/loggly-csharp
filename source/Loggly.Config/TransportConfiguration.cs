namespace Loggly.Config
{
    public class TransportConfiguration : ITransportConfiguration
    {
        public LogTransport LogTransport { get; set; }

        public string EndpointHostname { get; set; }

        public int EndpointPort { get; set; }

		public bool IsOmitTimestamp { get; set; }

        public override string ToString()
        {
			return string.Format("LogTransport={0}, EndpointHostname={1}, EndpointPort={2}, IsOmitTimestamp={3}", LogTransport, EndpointHostname, EndpointPort, IsOmitTimestamp);
        }
    }
}