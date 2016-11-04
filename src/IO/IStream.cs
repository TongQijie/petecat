using System;

namespace Petecat.IO
{
    public interface IStream
    {
        /// <summary>
        /// number of bytes that have read.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// reads a byte
        /// </summary>
        /// <returns>if -1, indicate it has no bytes in buffer.</returns>
        int ReadByte();

        /// <summary>
        /// reads the specified number of bytes
        /// </summary>
        /// <param name="count">the specified number</param>
        /// <returns>if null, indicate buffer does not have specified number of bytes to read.</returns>
        byte[] ReadBytes(int count);

        /// <summary>
        /// reads bytes until reading specified terminal byte.
        /// </summary>
        /// <param name="terminator">terminal byte</param>
        /// <returns>byte array that does not contain terminal byte. if null, indicate buffer cannot find terminal byte.</returns>
        byte[] ReadBytesUntil(byte terminator);

        /// <summary>
        /// reads bytes until reading one of specified terminal bytes.
        /// </summary>
        /// <param name="terminators">terminal bytes</param>
        /// <returns>byte array that contain terminal byte. if null, indicate buffer cannot find terminal byte.</returns>
        byte[] ReadBytesUntil(byte[] terminators);

        /// <summary>
        /// seeks to byte that is equal to target byte.
        /// </summary>
        /// <param name="targetByte">target byte</param>
        /// <returns>if false, indicate target byte does not exist in buffer, else true.</returns>
        bool SeekBytesUntilEqual(byte targetByte);

        /// <summary>
        /// seeks to byte that is not equal to target byte.
        /// </summary>
        /// <param name="targetByte">target byte</param>
        /// <returns>return byte that is not equal to target byte. if -1, indicate buffer have no bytes that is not equal to target byte.</returns>
        int SeekBytesUntilNotEqual(byte targetByte);

        /// <summary>
        /// seeks to byte that is visiable excluding whitespace.
        /// </summary>
        /// <returns>return byte that is visiable excluding whitespace. if -1, indicate buffer have no bytes that is not equal to target byte.</returns>
        int SeekBytesUntilVisiableChar();

        /// <summary>
        /// seeks to byte that meets the specified condition.
        /// </summary>
        /// <param name="predicate">the specified condition</param>
        /// <returns>return byte that meets the specified conditione. if -1, indicate buffer have no bytes that meets the specified condition.</returns>
        int SeekBytesUntilMeets(Predicate<int> predicate);
    }
}

