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

        }
    }
}
