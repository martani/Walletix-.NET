using System;

namespace Walletix
{
	public class WalletixError : Exception
	{
		public WalletixError (int status, string message) : base(message)
		{
			this.Status = status;
			this.Result = message;
		}
		
		public int Status { get; private set; }
		public string Result { get; private set; }
	}
}

