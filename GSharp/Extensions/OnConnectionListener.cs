using GSharp.Protocol.Connection;

namespace GSharp.Extensions
{
    public interface OnConnectionListener
    {
        void OnConnection(string host, int port, string hotelVersion, string clientIdentifier, HClient clientType);
    }
}
