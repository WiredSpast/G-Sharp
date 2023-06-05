using System.Net.Sockets;

namespace GSharp.Extensions
{
    public class Extension : ExtensionBase
    {
        //protected FlagsCheckListener flagRequestCallback = null;

        private string[] _args;
        private volatile bool _isCorrupted = false;
        private static readonly string[] _PORT_FLAG = { "--port", "-p" };
        private static readonly string[] _FILE_FLAG = { "--filename", "-f" };
        private static readonly string[] _COOKIE_FLAG = { "--auth-token", "-c" };

        private volatile bool _delayed_init = false;

        private Stream _outStream = null;

        private String GetArgument(String[] args, params String[] arg)
        {
            for(int i = 0; i < args.Length - 1; i++)
            {
                foreach(string str in arg)
                {
                    if (args[i].ToLower() == str.ToLower())
                    {
                        return args[i + 1];
                    }
                }
            }

            return null;
        }

        public Extension(string[] args) : base()
        {
            // todo
        }

    }
}
