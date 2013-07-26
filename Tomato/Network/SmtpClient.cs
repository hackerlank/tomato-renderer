using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Tomato.Network
{
	/// <summary>
	/// Defines a SMTP client class.
	/// </summary>
	public sealed class SmtpClient
	{
		private System.Net.Mail.SmtpClient m_smtpClient = null;

		/// <summary>
		/// Gets the sender's email address.
		/// </summary>
		public string SenderEmailAddress { get; private set; }

		/// <summary>
		/// Gets the sender's email account password.
		/// </summary>
		public string SenderEmailPassword { get; private set; }

		/// <summary>
		/// Gets the SMTP host address.
		/// </summary>
		public string SmtpHostAddress { get; private set; }

		/// <summary>
		/// Creates a SmtpClient instance.
		/// </summary>
		/// <param name="senderEmailAddress"></param>
		/// <param name="senderEmailPassword"></param>
		/// <param name="smtpHostAddress"></param>
		public SmtpClient( string senderEmailAddress, string senderEmailPassword, string smtpHostAddress )
		{
			SenderEmailAddress = senderEmailAddress;
			SenderEmailPassword = senderEmailPassword;
			SmtpHostAddress = smtpHostAddress;

			// Initialize SmtpClient instance.
			m_smtpClient = new System.Net.Mail.SmtpClient( smtpHostAddress );
			m_smtpClient.Credentials = new System.Net.NetworkCredential( senderEmailAddress, senderEmailPassword );
		}

		/// <summary>
		/// Sends an email synchronously.
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="receivers"></param>
		public void Send( string subject, string body, params string[] receivers )
		{
			SendAs( SenderEmailAddress, null, subject, body, receivers );
		}

		/// <summary>
		/// Sends an email synchronously.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="displayName"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="receivers"></param>
		public void SendAs( string sender, string displayName, string subject, string body, params string[] receivers )
		{
			MailMessage mailMessage = new MailMessage();

			mailMessage.From = new MailAddress( sender, displayName );
			foreach( string receiver in receivers )
			{
				mailMessage.To.Add( new MailAddress( receiver ) );
			}
			mailMessage.Subject = subject;
			mailMessage.Body = body;

			m_smtpClient.Send( mailMessage );
		}

		/// <summary>
		/// Sends an email asynchronously.
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="receivers"></param>
		public void SendAsync( string subject, string body, params string[] receivers )
		{
			SendAsAsync( SenderEmailAddress, subject, body, receivers );
		}

		/// <summary>
		/// Sends an email asynchronously.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="receivers"></param>
		public void SendAsAsync( string sender, string subject, string body, params string[] receivers )
		{
			MailMessage mailMessage = new MailMessage();

			mailMessage.From = new MailAddress( sender );
			foreach( string receiver in receivers )
			{
				mailMessage.To.Add( new MailAddress( receiver ) );
			}
			mailMessage.Subject = subject;
			mailMessage.Body = body;

			m_smtpClient.SendAsync( mailMessage, null );
		}
	}
}
