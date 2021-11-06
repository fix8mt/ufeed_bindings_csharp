using System;
using ZeroMQ;

namespace UFE.UFEedClient
{
	/// <summary>
	/// xxxMessageReceived event arguments
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class UFEMessageEventArgs : EventArgs
	{
		/// <summary>
		/// Constructs event arguments
		/// </summary>
		/// <param name="message">Received message</param>
		public UFEMessageEventArgs(UFEMessage message)
		{
			Message = message;
		}

		/// <summary>
		/// Received message
		/// </summary>
		public UFEMessage Message { get; }
	}

	/// <summary>
	/// ZeroMQ error event arguments
	/// Consumer shall set <c>Continue</c> property to false in case of unrecoverable failure
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class UFEZeroMQErrorEventArgs : EventArgs
	{
		/// <summary>
		/// Constructs event arguments
		/// </summary>
		/// <param name="error">ZeroMQ error</param>
		/// <param name="socketType">Socket type: "PUB", "SUB", etc</param>
		public UFEZeroMQErrorEventArgs(ZError error, string socketType)
		{
			Error = error;
			SocketType = socketType;
			Continue = true;
		}

		/// <summary>
		/// ZeroMQ error
		/// </summary>
		public ZError Error { get; }

		/// <summary>
		/// Socket type: "PUB", "SUB", etc
		/// </summary>
		public string SocketType { get; }

		/// <summary>
		/// Must be set to false in case of unrecoverable failure
		/// </summary>
		public bool Continue { get; set; }
	}

	/// <summary>
	/// UFE error event arguments
	/// Consumer shall set <c>Continue</c> property to false in case of unrecoverable failure
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class UFEErrorEventArgs : EventArgs
	{
		/// <summary>
		/// Constructs event arguments
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="exception">Exception reference</param>
		public UFEErrorEventArgs(string message, Exception exception = null)
		{
			Message = message;
			Exception = exception;
			Continue = true;
		}

		/// <summary>
		/// Error message
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Exception reference
		/// </summary>
		public Exception Exception { get; }

		/// <summary>
		/// Must be set to false in case of unrecoverable failure
		/// </summary>
		public bool Continue { get; set; }
	}

	/// <summary>
	/// UFE authenticate event
	/// Consumer shall set <c>Accept</c> property to false in case of authentication failure
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class UFEAuthEventArgs : EventArgs
	{
		/// <summary>
		/// Constructs UFE auth event arguments
		/// </summary>
		/// <param name="user">user login</param>
		/// <param name="password">user password</param>
		public UFEAuthEventArgs(string user, string password)
		{
			User = user;
			Password = password;
			Accept = true;
		}

		/// <summary>
		/// User login
		/// </summary>
		public string User { get; }

		/// <summary>
		/// User password
		/// </summary>
		public string Password { get; }

		/// <summary>
		/// Must be set to false in case of authentication failure
		/// </summary>
		public bool Accept { get; set; }
	}
}
