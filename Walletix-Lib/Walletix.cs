using System;
using System.Net;
using System.Xml.Linq;
using System.Linq;
using System.Diagnostics;


namespace Walletix
{
	public class WalletixService
	{
		/*
		 * User supplied authentication keys
		 */
		private long walletixVendorID;
		private string walletixAPIKey;
		

		/*
		 * Walletix Serice API URLs
		 */
		private string API_PATH;
		private string GENERATE_PAYMENT_CODE = "paymentcode";
		private string DELETE_PAYMENT = "deletepayment";
		private string VERIFY_PAYMENT = "paymentverification";
		
		
		public WalletixService(long vedorID, string APIKey, bool enableSandboxTesting = false) 
		{
			if(String.IsNullOrEmpty(APIKey))
				throw new ArgumentNullException("APIKey");
			
			this.walletixVendorID = vedorID;
			this.walletixAPIKey = APIKey;
						
			if(enableSandboxTesting == true)
				this.API_PATH = "https://www.walletix.com/sandbox/api/";
			else
				this.API_PATH = "https://www.walletix.com/api/";
		}
		
		/*
		 * Generates a payment code
		 */
		public string GeneratePaymentCode(long purchaseID, double amount, string callbackURL)
		{
			//if(String.IsNullOrEmpty(callbackURL))
			//	throw new ArgumentNullException("callbackURL");
			
			string postParams = "vendorID=" + this.walletixVendorID.ToString()
								+ "&apiKey=" + Uri.EscapeUriString(this.walletixAPIKey)
								+ "&purchaseID=" + purchaseID.ToString()
					 			+ "&amount=" + amount.ToString()
								+ "&format=xml"
								+ "&callbackurl=" + Uri.EscapeUriString(callbackURL);
			
#if DEBUG_1
			Console.WriteLine("GeneratePaymentCode()");
			Console.WriteLine("URL: {0}", this.API_PATH + this.GENERATE_PAYMENT_CODE);
			Console.WriteLine("Payload: {0}", postParams);
#endif
			
			string result = "";
			using (WebClient wc = new WebClient())
			{
    			wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				result = wc.UploadString(this.API_PATH + this.GENERATE_PAYMENT_CODE, postParams);
			}
			
			XElement xmlResult = XElement.Parse(result);
			
			if(!xmlResult.Elements("status").Any() || !xmlResult.Elements("code").Any())
				throw new WalletixError(-9999, "Wrong XML result, API might have changed!");
			
			int status = Int32.Parse(xmlResult.Elements("status").First().Value);
			string code = xmlResult.Elements("code").First().Value;
			
			if(status == 1)	//success
				return code;
			else
				throw new WalletixError(status, code);
			
		}
		
		/*
		 * Verifies if the payment associated to @param paymentCode has been performed by the customer.
		 * Returns True if the payment has been performed, false otherwise.
		 */
		public bool VerifyPayment(string paymentCode)
		{
			if(String.IsNullOrEmpty(paymentCode))
				throw new ArgumentException("paymentCode");
			
			string postParams = "vendorID=" + this.walletixVendorID
								+ "&apiKey=" + Uri.EscapeUriString(this.walletixAPIKey)
								+ "&paiementCode=" + Uri.EscapeUriString(paymentCode)
								+ "&format=xml";
			
#if DEBUG_1	
			Console.WriteLine("VerifyPayment()");
			Console.WriteLine("URL: {0}", this.API_PATH + this.VERIFY_PAYMENT);
			Console.WriteLine("Payload: {0}", postParams);
#endif
			
			string result = "";
			using (WebClient wc = new WebClient())
			{
    			wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				result = wc.UploadString(this.API_PATH + this.VERIFY_PAYMENT, postParams);
				
			}
			
			XElement xmlResult = XElement.Parse(result);
			
			if(!xmlResult.Elements("status").Any() || !xmlResult.Elements("result").Any())
				throw new WalletixError(-9999, "Wrong XML result, API might have changed!");
			
			int status = Int32.Parse(xmlResult.Elements("status").First().Value);
			
			if(status == 1)	//success
			{
				int res = Int32.Parse(xmlResult.Elements("result").First().Value);
				return res == 1 ? true : false;
			}
			else
			{
				string res = xmlResult.Elements("result").First().Value;
				throw new WalletixError(status, res);
			}
		}
		
		/*
		 * Aborts the current transaction associated to @param paymentCode
		 * Returns True if the customer has not yet paid the transaction, False otherwise.
		 */
		public bool DeletePayment(string paymentCode)
		{
			if(String.IsNullOrEmpty(paymentCode))
				throw new ArgumentException("paymentCode");
			
			string postParams = "vendorID=" + this.walletixVendorID
								+ "&apiKey=" + Uri.EscapeUriString(this.walletixAPIKey)
								+ "&paiementCode=" + Uri.EscapeUriString(paymentCode)
								+ "&format=xml";
			
#if DEBUG_1
			Console.WriteLine("DeletePayment()");
			Console.WriteLine("URL: {0}", this.API_PATH + this.DELETE_PAYMENT);
			Console.WriteLine("Payload: {0}", postParams);
#endif
			
			string result = "";
			using (WebClient wc = new WebClient())
			{
    			wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				result = wc.UploadString(this.API_PATH + this.DELETE_PAYMENT, postParams);
			}
			
			XElement xmlResult = XElement.Parse(result);
			
			if(!xmlResult.Elements("status").Any() || !xmlResult.Elements("result").Any())
				throw new WalletixError(-9999, "Wrong XML result, API might have changed!");
			
			int status = Int32.Parse(xmlResult.Elements("status").First().Value);
			
			if(status == 1)	//success
			{
				int res = Int32.Parse(xmlResult.Elements("result").First().Value);
				if(res == 1)
					return true;
				else if(res == 0)
					return false;
				else
					throw new WalletixError(-9999, "Deletion of payment request status=1, but the result code is not " +
					                        "conform to the API specification");
			}
			else
			{
				string res = xmlResult.Elements("result").First().Value;
				throw new WalletixError(status, res);
			}
		}
	}
}

