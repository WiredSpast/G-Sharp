using GSharp.Extensions;
using GSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExtension
{
    [ExtensionInfo("Test extension", "Just a test", "0.1", "IUseYahoo")]
    internal class TestExtension : Extension
    {
        public TestExtension(string[] args) : base(args) { }

        protected override void InitExtension()
        {
            Intercept(HDirection.TOCLIENT, onToClientPacket);
            Intercept(HDirection.TOSERVER, onToServerPacket);
        }

        protected override void OnStartConnection()
        {
        }

        protected override void onEndConnection()
        {
        }

        public void onToClientPacket(HMessage msg)
        {
            Console.Write("To client:  ");
            Console.WriteLine(msg.GetPacket().HeaderId);
        }

        public void onToServerPacket(HMessage msg)
        {
            Console.Write("To server:  ");
            Console.WriteLine(msg.GetPacket().HeaderId);
        }
    }
}
