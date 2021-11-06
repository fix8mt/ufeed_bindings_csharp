using static UFE.UFEedClient.Consts;

namespace UFE.UFEedClient
{
	/// <summary>
	/// UFEedClient configuration class
	/// <example>
	///	using (var uc = new UFEedClient(new UFEedConfiguration{Subscriber = SUBSCRIBER_DEFAULT, Publisher = "tcp://*:56747"}))
	///	{
	/// 		...
	/// 	}
	/// </example>
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class UFEedConfiguration
	{
		/// <summary>
		/// Subscriber endpoint, defaults to "tcp://127.0.0.1:55745"
		/// </summary>
		public string Subscriber { get; set; } = SUBSCRIBER_DEFAULT;

		/// <summary>
		/// Requester endpoint, defaults to "tcp://127.0.0.1:55746"
		/// </summary>
		public string Requester { get; set; } = REQUESTER_DEFAULT;

		/// <summary>
		/// Publisher endpoint, defaults to "tcp://*:55747"
		/// </summary>
		public string Publisher { get; set; } = PUBLISHER_DEFAULT;

		/// <summary>
		/// Responder endpoint, defaults to "tcp://*:55748"
		/// </summary>
		public string Responder { get; set; } = RESPONDER_DEFAULT;

		/// <summary>
		/// Subscriber topic, defaults to "ufegw-publisher"
		/// </summary>
		public string SubscriberTopic { get; set; } = SUBSCRIBER_TOPIC_DEFAULT;

		/// <summary>
		/// Requester topic, defaults to "ufegw-responder"
		/// </summary>
		public string RequesterTopic { get; set; } = REQUESTER_TOPIC_DEFAULT;

		/// <summary>
		/// Publisher topic, defaults to "ufeedclient-publisher"
		/// </summary>
		public string PublisherTopic { get; set; } = PUBLISHER_TOPIC_DEFAULT;

		/// <summary>
		/// Responder topic, defaults to "ufeedclient-responder"
		/// </summary>
		public string ResponderTopic { get; set; } = RESPONDER_TOPIC_DEFAULT;

		/// <summary>
		/// Max IO threads for ZMQ background work, defaults to 1
		/// </summary>
		public int MaxIoThreads { get; set; } = 1;

		/// <summary>
		/// Poll interval in milliseconds, defaults to 10
		/// </summary>
		public uint PollIntervalMs { get; set; } = 10;
	}
}
