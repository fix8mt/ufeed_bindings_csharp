using System;

namespace UFE.UFEedClient
{
	/// <summary>
	/// UFEedClient exception class. Can be thrown by UFEedClient methods
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class UFEedClientException : Exception
	{
		/// <summary>
		/// Exception message text
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Constructs exception
		/// </summary>
		/// <param name="text">Exception message text</param>
		public UFEedClientException(string text) : base(text)
		{
			Text = text;
		}
	}
}
