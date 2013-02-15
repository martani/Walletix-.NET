using System;
using Walletix;
using System.Net;

namespace Walletix_Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Warning: This disables checking for the certificate of Walletix server
			//Handle your production code properly!
			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };			
			
			//Prepare Walletix service
			long myVendorID = 12269;
			string myAPIKey = "CAv5rCmixvt1MHo1D6hDVhSHLrvj2X7u";
			WalletixService walletix = new WalletixService(myVendorID, myAPIKey, true);
			
			string transactionCode = "";
			
			//Generate a transaction code. Amout = 500, ID = 99, callback URL = http://test-server.com/
			//Should enclose calls to WalletixService always un try..catch
			try {
				transactionCode = walletix.GeneratePaymentCode(99, 500, "http://test-server.com/");
				Console.WriteLine("The transaction code is: {0}", transactionCode);
				
			} catch (WalletixError wex) {
				//check https://www.walletix.com/documentation-api for error codes and their meanings
				Console.WriteLine("Walletix Error: status={0}, code={1}", wex.Status, wex.Result);
				
			} catch (Exception ex) {
				//These exceptions can be due to network errors, XML parsing issues etc
				Console.WriteLine("General Error: {0}", ex.Message);
			}
			
			
			
			//Verify if the payment for a transaction has been performed by the customer or not yet
			//Should enclose calls to WalletixService always un try..catch
			try {
				//string transactionCode = "SOME TRANSACTION CODE";
				bool isTransactionPaid = walletix.VerifyPayment(transactionCode);
				Console.WriteLine("The transaction {0} {1}", transactionCode, 
				                  		isTransactionPaid ? "was paid." : "is not paid yet.");
				
			} catch (WalletixError wex) {
				//check https://www.walletix.com/documentation-api for error codes and their meanings
				Console.WriteLine("Walletix Error: status={0}, code={1}", wex.Status, wex.Result);
				
			} catch (Exception ex) {
				//These exceptions can be due to network errors, XML parsing issues etc
				Console.WriteLine("General Error: {0}", ex.Message);
			}
			
			
			//Delete a transaction
			//Should enclose calls to WalletixService always un try..catch
			try {
				//string transactionCode = "SOME TRANSACTION CODE";
				bool transactionDeleted = walletix.DeletePayment(transactionCode);
				Console.WriteLine("The transaction {0} {1}", transactionCode, 
				                  		transactionDeleted ? 
				                  		"was deleted." : 
				                  		"was not deleted, custmer might have already paid.");
				
			} catch (WalletixError wex) {
				//check https://www.walletix.com/documentation-api for error codes and their meanings
				Console.WriteLine("Walletix Error: status={0}, code={1}", wex.Status, wex.Result);
				
			} catch (Exception ex) {
				//These exceptions can be due to network errors, XML parsing issues etc
				Console.WriteLine("General Error: {0}", ex.Message);
			}
			
		}
		
//		static void Test(WalletixService walletix)
//		{
//			Console.Write(@"walletix.VerifyPayment(""NONEXISTANT"")");
//			try {
//				if(walletix.VerifyPayment("test"))
//					Console.WriteLine("\tresult: {0}", true);
//				else
//					Console.WriteLine("\tresult: {0}", false);	
//			} catch (Exception ex) {
//				Console.WriteLine("\tError: {0}", ex.Message);	
//			}
//			
//			
//			Console.WriteLine(@"walletix.GeneratePaymentCode(1, 10, ""test"")");
//			string codePayment;
//			for (int i = 0; i < 10; i++) {
//				try {
//					codePayment = walletix.GeneratePaymentCode(1, 20, "http://google.com");
//					Console.Write("Payment Code: {0}", codePayment);	
//					
//					Console.WriteLine("\t\tVerify payment (should be False) == {0}", walletix.VerifyPayment(codePayment));
//					Console.ReadLine();
//					Console.Write("Delete payment (Should be True) == {0}", walletix.DeletePayment(codePayment));
//					Console.WriteLine("\t\t -- Delete payment twice (Should be False) == {0}", walletix.DeletePayment(codePayment));
//					
//				} catch (WalletixError wex) {
//					Console.WriteLine("\tError: {0}", wex.Result);	
//				} catch (Exception ex) {
//					Console.WriteLine("\tError: {0}", ex.Message);	
//				}	
//			}
//		}
	}
}
