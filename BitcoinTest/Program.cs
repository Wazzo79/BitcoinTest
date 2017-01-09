using NBitcoin;
using System;

namespace BitcoinTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Key privateKey = new Key();
            BitcoinSecret mainNetPrivateKey = privateKey.GetBitcoinSecret(Network.Main);
            Console.WriteLine(mainNetPrivateKey);
            var address = privateKey.PubKey.GetAddress(Network.Main);
            Console.WriteLine(address.ToString());
        }
    }
}
