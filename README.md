Walletix .NET
=============

A .NET wrapper for the [Walletix](https://www.walletix.com) online payment service. This can be used from .NET desktop and ASP.NET projects alike.


How to use
-----------
You need only to reference the project [`Walletix-Lib`](Walletix-Lib) and add `using Walletix;` to your code.

Methods
-------

* **Initialize the service:**
`````csharp
long vendorID = 11111;  //Your Vendor ID
string APIKey = "YOUR API KEY HERE";
//Use True to test in the Sandbox environment, False in production environment
WalletixService walletix = new WalletixService(myVendorID, myAPIKey, true);
`````

* **Generate a payment (transaction) code:**
`````csharp
//Generate a transaction code. Amout = 500, ID = 99, callback URL = http://test-server.com/
//Should enclose calls to WalletixService always un try..catch
string transactionCode = walletix.GeneratePaymentCode(99, 500, "http://test-server.com/");
Console.WriteLine("The transaction code is: {0}", transactionCode);
`````

* **Verify if a transaction was paid by a customer:**
`````csharp
//Verify if the payment for a transaction has been performed by the customer or not yet
//Should enclose calls to WalletixService always un try..catch
string transactionCode = "SOME TRANSACTION CODE";
bool isTransactionPaid = walletix.VerifyPayment(transactionCode);
Console.WriteLine("The transaction {0} {1}", transactionCode, isTransactionPaid ? "was paid." : "is not paid yet.");
`````

* **Delete a transaction:**
`````csharp
string transactionCode = "SOME TRANSACTION CODE";
bool transactionDeleted = walletix.DeletePayment(transactionCode);
Console.WriteLine("The transaction {0} {1}", transactionCode, 
						transactionDeleted ? 
						"was deleted." : 
						"was not deleted, custmer might have already paid.");
`````

**Check [Walletix-Test/Main.cs](Walletix-Test/Main.cs) on how to handle errors while using the Walletix service.**