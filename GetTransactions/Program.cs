using NBitcoin;
using QBitNinja.Client;
using System;

namespace GetTransactions
{
    class Program
    {
        static void Main(string[] args)
        {
            var bitcoinPrivateKey = new BitcoinSecret("L1qVDci614EoauxwWPiWhiHps3mbUnu54Q4ckGFFqDkLBAF66E8H");
            var address = bitcoinPrivateKey.GetAddress();
            Console.WriteLine("Private Key: " + bitcoinPrivateKey);
            Console.WriteLine("Address: " + address);

            var client = new QBitNinjaClient(Network.Main);
            var balance = client.GetBalance(address).Result;

            Console.WriteLine();

            foreach (var operation in balance.Operations)
            {
                Console.WriteLine("Received Transactions");
                foreach(var coin in operation.ReceivedCoins)
                {
                    Console.WriteLine(((Money)coin.Amount).ToDecimal(MoneyUnit.BTC) + " " + operation.TransactionId + " " + operation.FirstSeen);
                }

                Console.WriteLine();
                Console.WriteLine("Sent Transactions");
                foreach (var coin in operation.SpentCoins)
                {
                    Console.WriteLine(((Money)coin.Amount).ToDecimal(MoneyUnit.BTC) + " " + operation.TransactionId + " " + operation.FirstSeen);
                }
            }

            

            Console.Read();
        }
    }
}
