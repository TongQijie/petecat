using System;
using System.Collections.Generic;
using System.Text;

namespace Petecat.Network.Shared
{
    public class DataTypeHandler
    {
        private delegate void Encode(ref ByteArray storage, object data);

        private delegate object Decode(byte[] storage, ref int offset);

        private static Dictionary<Type, DataTypeHandler.Encode> mEncoders;

        private static Dictionary<Type, DataTypeHandler.Decode> mDecoders;

        static DataTypeHandler()
        {
            DataTypeHandler.mEncoders = new Dictionary<Type, DataTypeHandler.Encode>();
            DataTypeHandler.mEncoders.Add(typeof(byte), new DataTypeHandler.Encode(DataTypeHandler.EncodeByte));
            DataTypeHandler.mEncoders.Add(typeof(short), new DataTypeHandler.Encode(DataTypeHandler.EncodeInt16));
            DataTypeHandler.mEncoders.Add(typeof(int), new DataTypeHandler.Encode(DataTypeHandler.EncodeInt32));
            DataTypeHandler.mEncoders.Add(typeof(ushort), new DataTypeHandler.Encode(DataTypeHandler.EncodeUInt16));
            DataTypeHandler.mEncoders.Add(typeof(uint), new DataTypeHandler.Encode(DataTypeHandler.EncodeUInt32));
            DataTypeHandler.mEncoders.Add(typeof(string), new DataTypeHandler.Encode(DataTypeHandler.EncodeString));
            DataTypeHandler.mEncoders.Add(typeof(Guid), new DataTypeHandler.Encode(DataTypeHandler.EncodeGuid));
            DataTypeHandler.mEncoders.Add(typeof(decimal), new DataTypeHandler.Encode(DataTypeHandler.EncodeDecimal));
            DataTypeHandler.mDecoders = new Dictionary<Type, DataTypeHandler.Decode>();
            DataTypeHandler.mDecoders.Add(typeof(byte), new DataTypeHandler.Decode(DataTypeHandler.DecodeByte));
            DataTypeHandler.mDecoders.Add(typeof(short), new DataTypeHandler.Decode(DataTypeHandler.DecodeInt16));
            DataTypeHandler.mDecoders.Add(typeof(int), new DataTypeHandler.Decode(DataTypeHandler.DecodeInt32));
            DataTypeHandler.mDecoders.Add(typeof(ushort), new DataTypeHandler.Decode(DataTypeHandler.DecodeUInt16));
            DataTypeHandler.mDecoders.Add(typeof(uint), new DataTypeHandler.Decode(DataTypeHandler.DecodeUInt32));
            DataTypeHandler.mDecoders.Add(typeof(string), new DataTypeHandler.Decode(DataTypeHandler.DecodeString));
            DataTypeHandler.mDecoders.Add(typeof(Guid), new DataTypeHandler.Decode(DataTypeHandler.DecodeGuid));
            DataTypeHandler.mDecoders.Add(typeof(decimal), new DataTypeHandler.Decode(DataTypeHandler.DecodeDecimal));
        }

        public static bool EncodeField(ref ByteArray storage, object data, Type type)
        {
            if (DataTypeHandler.mEncoders.ContainsKey(type))
            {
                DataTypeHandler.Encode encode = DataTypeHandler.mEncoders[type];
                if (encode != null)
                {
                    encode(ref storage, data);
                    return true;
                }
            }
            return false;
        }

        public static object DecodeField(byte[] storage, ref int offset, Type type)
        {
            if (DataTypeHandler.mDecoders.ContainsKey(type))
            {
                DataTypeHandler.Decode decode = DataTypeHandler.mDecoders[type];
                if (decode != null)
                {
                    return decode(storage, ref offset);
                }
            }
            return null;
        }

        private static void EncodeByte(ref ByteArray storage, object data)
        {
            storage.Add((byte)data);
        }

        private static object DecodeByte(byte[] storage, ref int offset)
        {
            return storage[offset++];
        }

        private static void EncodeInt16(ref ByteArray storage, object data)
        {
            storage.Add((short)data);
        }

        private static object DecodeInt16(byte[] storage, ref int offset)
        {
            short num = BitConverter.ToInt16(storage, offset);
            offset += 2;
            return num;
        }

        private static void EncodeInt32(ref ByteArray storage, object data)
        {
            storage.Add((int)data);
        }

        private static object DecodeInt32(byte[] storage, ref int offset)
        {
            int num = BitConverter.ToInt32(storage, offset);
            offset += 4;
            return num;
        }

        private static void EncodeUInt16(ref ByteArray storage, object data)
        {
            storage.Add((ushort)data);
        }

        private static object DecodeUInt16(byte[] storage, ref int offset)
        {
            ushort num = BitConverter.ToUInt16(storage, offset);
            offset += 2;
            return num;
        }

        private static void EncodeUInt32(ref ByteArray storage, object data)
        {
            storage.Add((uint)data);
        }

        private static object DecodeUInt32(byte[] storage, ref int offset)
        {
            uint num = BitConverter.ToUInt32(storage, offset);
            offset += 4;
            return num;
        }

        private static void EncodeFloat(ref ByteArray storage, object data)
        {
            storage.Add((float)data);
        }

        private static object DecodeFloat(byte[] storage, ref int offset)
        {
            float num = BitConverter.ToSingle(storage, offset);
            offset += 4;
            return num;
        }

        private static void EncodeDouble(ref ByteArray storage, object data)
        {
            storage.Add((double)data);
        }

        private static object DecodeDouble(byte[] storage, ref int offset)
        {
            double num = BitConverter.ToDouble(storage, offset);
            offset += 8;
            return num;
        }

        private static void EncodeString(ref ByteArray storage, object data)
        {
            string p = (string)data;
            storage.Add(p);
        }

        private static object DecodeString(byte[] storage, ref int offset)
        {
            int num = int.Parse(DataTypeHandler.DecodeUInt16(storage, ref offset).ToString());
            string @string = Encoding.UTF8.GetString(storage, offset, num);
            offset += num;
            return @string;
        }

        private static void EncodeGuid(ref ByteArray storage, object data)
        {
            Guid guid = (Guid)data;
            storage.Add(guid.ToByteArray());
        }

        private static object DecodeGuid(byte[] storage, ref int offset)
        {
            byte[] array = new byte[16];
            Array.Copy(storage, offset, array, 0, 16);
            offset += 16;
            return new Guid(array);
        }

        private static void EncodeDecimal(ref ByteArray storage, object data)
        {
            string data2 = data.ToString();
            DataTypeHandler.EncodeString(ref storage, data2);
        }

        private static object DecodeDecimal(byte[] storage, ref int offset)
        {
            string s = (string)DataTypeHandler.DecodeString(storage, ref offset);
            decimal num = 0.0m;
            decimal.TryParse(s, out num);
            return num;
        }
    }
}
