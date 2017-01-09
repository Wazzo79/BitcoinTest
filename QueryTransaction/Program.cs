using NBitcoin;
using QBitNinja.Client;
using System;

namespace QueryTransaction
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new QBitNinjaClient(Network.Main);
            var transactionId = uint256.Parse("27226ac6ceea08e2dc8f3a985ed025c1d2a849fe384ae6f8ac8235e0c769c719");
            var response = client.GetTransaction(transactionId).Result;
            var transaction = response.Transaction;

            foreach (var output in transaction.Outputs)
            {
                var amount = output.Value;

                Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
                var paymentScript = output.ScriptPubKey;
                Console.WriteLine(paymentScript);
                var address = paymentScript.GetDestinationAddress(Network.Main);
                Console.WriteLine(address);
                Console.WriteLine(transaction.GetFee(response.SpentCoins.ToArray()).ToDecimal(MoneyUnit.BTC));
                Console.WriteLine();
            }
            

            //foreach (var input in transaction.Inputs)
            //{
            //    var previousOutoint = input.PrevOut;
            //    Console.WriteLine(previousOutoint.Hash);
            //    Console.WriteLine(previousOutoint.N);
            //    Console.WriteLine();
            //}

            Console.Read();
        }
    }
}
