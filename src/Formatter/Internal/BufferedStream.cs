using System;
using System.IO;

namespace Petecat.Formatter.Internal
{
    internal class BufferedStream : IStream
    {
        public BufferedStream(Stream stream, int capacity)
        {
            _InternalStream = stream;
            _InternalBuffer = new byte[capacity];
        }

        private Stream _InternalStream = null;

        private byte[] _InternalBuffer = null;

        private int _Index = 0;

        private int _Count = 0;

        private int _TotalCount = 0;

        public int Position
        {
            get { return _TotalCount + _Index; }
        }

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

        public byte[] ReadBytes(int count)
        {
            var buf = new byte[count];
            for (int i = 0; i < count; i++)
            {
                var b = ReadByte();
                if (b != -1)
                {
                    buf[i] = (byte)b;
                }
                else
                {
                    return null;
                }
            }
            return buf;
        }

        public byte[] ReadBytesUntil(byte terminator)
        {
            return InternalReadBytesUntil(new byte[0], terminator);
        }

        public byte[] ReadBytesUntil(byte[] terminators)
        {
            return InternalReadBytesUntil(new byte[0], terminators);
        }

        public bool SeekBytesUntilEqual(byte targetByte)
        {
            var index = InternalIndexOf(targetByte, _Index, _Count - _Index);
            if (index == -1)
            {
                if (Fill())
                {
                    return SeekBytesUntilEqual(targetByte);
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

        public int SeekBytesUntilNotEqual(byte targetByte)
        {
            while (_Index < _Count)
            {
                if (_InternalBuffer[_Index] != targetByte)
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
                return SeekBytesUntilNotEqual(targetByte);
            }
            else
            {
                return -1;
            }
        }

        public int SeekBytesUntilVisiableChar()
        {
            while (_Index < _Count)
            {
                if (_InternalBuffer[_Index] > 0x20 && _InternalBuffer[_Index] <= 0x7E)
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
                return SeekBytesUntilVisiableChar();
            }
            else
            {
                return -1;
            }
        }

        public int SeekBytesUntilMeets(Predicate<int> predicate)
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
                return SeekBytesUntilMeets(predicate);
            }
            else
            {
                return -1;
            }
        }

        private byte[] InternalReadBytesUntil(byte[] byteValues, byte terminator)
        {
            var index = InternalIndexOf(terminator, _Index, _Count - _Index);
            if (index != -1)
            {
                byteValues = Concat(byteValues, 0, byteValues.Length, _InternalBuffer, _Index, index - _Index);
                _Index = index + 1;
                return byteValues;
            }
            else
            {
                byteValues = Concat(byteValues, 0, byteValues.Length, _InternalBuffer, _Index, _Count - _Index);

                if (Fill())
                {
                    return InternalReadBytesUntil(byteValues, terminator);
                }
                else
                {
                    return null;
                }
            }
        }

        private byte[] InternalReadBytesUntil(byte[] byteValues, byte[] terminators)
        {
            var startIndex = _Index;
            while (_Index < _Count)
            {
                var i = 0;
                for (; i < terminators.Length; i++)
                {
                    if (terminators[i] == _InternalBuffer[_Index])
                    {
                        break;
                    }
                }
                if (i < terminators.Length)
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
                return Concat(byteValues, 0, byteValues.Length, _InternalBuffer, startIndex, _Index - startIndex);
            }
            else
            {
                byteValues = Concat(byteValues, 0, byteValues.Length, _InternalBuffer, startIndex, _Count - startIndex);
                if (Fill())
                {
                    return InternalReadBytesUntil(byteValues, terminators);
                }
                else
                {
                    return null;
                }
            }
        }

        private int InternalIndexOf(byte targetByte, int startIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_InternalBuffer[i + startIndex] == targetByte)
                {
                    return i + startIndex;
                }
            }

            return -1;
        }

        private bool Fill()
        {
            _TotalCount += _Count;
            _Count = _InternalStream.Read(_InternalBuffer, 0, _InternalBuffer.Length);
            _Index = 0;
            if (_Count == 0)
            {
                return false;
            }

            return true;
        }

        private byte[] Concat(byte[] firstArray, int firstStart, int firstCount, byte[] secondArray, int secondStart, int secondCount)
        {
            var buf = new byte[firstCount + secondCount];

            Array.Copy(firstArray, firstStart, buf, 0, firstCount);

            Array.Copy(secondArray, secondStart, buf, firstCount, secondCount);

            return buf;
        }
    }
}

