using System;
using System.IO;
namespace Petecat.Data.Formatters.Internal.Json
{
    public interface IBufferStream
    {
        int ReadByte();

        bool Go(byte targetByte);

        int FirstOrDefault(Predicate<int> predicate);

        int Except(byte byteValue);

        byte[] GetBytes(byte[] byteValues, byte endByte);

        byte[] GetBytes(byte[] byteValues, byte[] endBytes);
    }
}
