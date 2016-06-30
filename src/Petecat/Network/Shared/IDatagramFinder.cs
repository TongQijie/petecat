using System.IO;

namespace Petecat.Network.Shared
{
    public interface IDatagramFinder
    {
        bool TryGetDatagram(Stream stream, out IDatagram datagram);

        bool TryGetDatagram(byte[] buffer, int offset, out IDatagram datagram);
    }
}