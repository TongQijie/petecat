namespace Petecat.Network.Shared
{
    public interface IDatagram
    {
        ushort Command { get; }

        byte[] Data { get; }
    }
}
