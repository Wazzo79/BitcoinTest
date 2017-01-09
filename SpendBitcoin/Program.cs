using NBitcoin;
using QBitNinja.Client;
using System;
using System.Linq;
using System.Text;

namespace SpendBitcoin
{
    class Program
    {
//          private key L1qVDci614EoauxwWPiWhiHps3mbUnu54Q4ckGFFqDkLBAF66E8H
//          address 1BpfG5CPYY4caubagUZkMqW3LnKQiUJvSB

        static void Main(string[] args)
        {
            var bitcoinPrivateKey = new BitcoinSecret("L1qVDci614EoauxwWPiWhiHps3mbUnu54Q4ckGFFqDkLBAF66E8H");
            var address = bitcoinPrivateKey.GetAddress();
            Console.WriteLine("Private Key: " + bitcoinPrivateKey);
            Console.WriteLine("Address: " + address);

            var client = new QBitNinjaClient(Network.Main);
            var balance = client.GetBalance(address).Result;
            var total = balance.Operations.Sum(s => s.Amount.ToDecimal(MoneyUnit.BTC));
            Console.WriteLine("Balance: " + total);


            OutPoint outPointToSpend = null;

            foreach(var coin in balance.Operations.SelectMany(a => a.ReceivedCoins))
            {
                if (coin.TxOut.ScriptPubKey == bitcoinPrivateKey.ScriptPubKey)
                {
                    outPointToSpend = coin.Outpoint;
                }
            }
            
            
            var transaction = new Transaction();
            transaction.Inputs.Add(new TxIn {
                PrevOut = outPointToSpend
            });

            var bitXAddress = new BitcoinPubKeyAddress("1Kp5yQxpjVzDosM3qzWZhgjq77d7PwZh14");

            var bitXAmount = new Money(0.001m, MoneyUnit.BTC);
            var minerFee = new Money(0.0005m, MoneyUnit.BTC);
            var txInAmount = balance.Operations.First().ReceivedCoins[(int)outPointToSpend.N].TxOut.Value;
            var changeBackAmount = txInAmount - bitXAmount - minerFee;

            TxOut bitXTxOut = new TxOut {
                ScriptPubKey = bitXAddress.ScriptPubKey,
                Value = bitXAmount
            };

            TxOut changeBackTxOut = new TxOut {
                ScriptPubKey = bitcoinPrivateKey.ScriptPubKey,
                Value = changeBackAmount
            };

            transaction.Outputs.Add(bitXTxOut);
            transaction.Outputs.Add(changeBackTxOut);

            var message = "sent from VS to BitX!";
            var bytes = Encoding.UTF8.GetBytes(message);
            transaction.Outputs.Add(new TxOut()
            {
                Value = Money.Zero,
                ScriptPubKey = TxNullDataTemplate.Instance.GenerateScriptPubKey(bytes)
            });

            transaction.Inputs[0].ScriptSig = bitcoinPrivateKey.ScriptPubKey;
            transaction.Sign(bitcoinPrivateKey, false);

            var broadcastResponse = client.Broadcast(transaction).Result;

            if (broadcastResponse.Success)
            {
                Console.WriteLine("Successfully sent BTC");
            }
        }
    }
}
