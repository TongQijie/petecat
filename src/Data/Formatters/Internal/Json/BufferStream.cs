using System.IO;
using Petecat.Extension;
using System;
namespace Petecat.Data.Formatters.Internal.Json
{
    public class BufferStream : IBufferStream
    {
        public BufferStream(Stream stream, int capacity)
        {
            Capacity = capacity;
            _InternalStream = stream;
            _InternalBuffer = new byte[capacity];
        }

        private Stream _InternalStream = null;

        private byte[] _InternalBuffer = null;

        public int Capacity { get; private set; }

        private int _Index = 0;

        private int _Count = 0;

        public int ReadByte()
        {
            if (_Index < _Count)
            {
                return _InternalBuffer[_Index++];
            }
            else
            {
                if (Fill())
                {
                    return ReadByte();
                }
                else
                {
                    return -1;
                }
            }
        }

        public bool Go(byte targetByte)
        {
            var index = _InternalBuffer.IndexOf(targetByte, _Index, _Count - _Index);
            if (index == -1)
            {
                if (Fill())
                {
                    return Go(targetByte);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                _Index = index + 1;
                return true;
            }
        }

        public int FirstOrDefault(Predicate<int> predicate)
        {
            while (_Index < _Count)
            {
                if (predicate(_InternalBuffer[_Index]))
                {
                    return _InternalBuffer[_Index++];
                }
                else
                {
                    _Index++;
                }
            }

            if (Fill())
            {
                return FirstOrDefault(predicate);
            }
            else
            {
                return -1;
            }
        }

        public int Except(byte byteValue)
        {
            while (_Index < _Count)
            {
                if (_InternalBuffer[_Index] != byteValue)
                {
                    return _InternalBuffer[_Index++];
                }
                else
                {
                    _Index++;
                }
            }

            if (Fill())
            {
                return Except(byteValue);
            }
            else
            {
                return -1;
            }
        }

        public byte[] GetBytes(byte[] byteValues, byte terminator)
        {
            var index = _InternalBuffer.IndexOf(terminator, _Index, _Count - _Index);
            if (index != -1)
            {
                byteValues = byteValues.Append(_InternalBuffer.SubArray(_Index, index - _Index));
                _Index = index + 1;
                return byteValues;
            }
            else
            {
                byteValues = byteValues.Append(_InternalBuffer.SubArray(_Index, _Count - _Index));

                if (Fill())
                {
                    return GetBytes(byteValues, terminator);
                }
                else
                {
                    return null;
                }
            }
        }

        public byte[] GetBytes(byte[] byteValues, byte[] terminators)
        {
            var startIndex = _Index;
            while (_Index < _Count)
            {
                if (terminators.Exists(x => x == _InternalBuffer[_Index]))
                {
                    break; 
                }
                else
                {
                    _Index++;
                }
            }

            if (_Index < _Count)
            {
                _Index += 1;
                return byteValues.Append(_InternalBuffer.SubArray(startIndex, _Index - startIndex));
            }
            else
            {
                byteValues = byteValues.Append(_InternalBuffer.SubArray(startIndex, _Count - startIndex));
                if (Fill())
                {
                    return GetBytes(byteValues, terminators);
                }
                else
                {
                    return null;
                }
            }
        }

        private bool Fill()
        {
            _Count = _InternalStream.Read(_InternalBuffer, 0, _InternalBuffer.Length);
            _Index = 0;
            if (_Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}
