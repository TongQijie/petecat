using System;
using System.IO;
using System.Text;

using Petecat.Utility;
using Petecat.Extension;

namespace Petecat.Formatter
{
    public abstract class FormatterBase : IFormatter
    {
        public virtual object ReadObject(Type targetType, Stream stream)
        {
            throw new NotImplementedException();
        }

        public virtual object ReadObject(Type targetType, string path)
        {
            using (var inputStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return ReadObject(targetType, inputStream);
            }
        }

        public virtual object ReadObject(Type targetType, string stringValue, Encoding encoding)
        {
            using (var inputStream = new MemoryStream(encoding.GetBytes(stringValue)))
            {
                return ReadObject(targetType, inputStream);
            }
        }

        public virtual object ReadObject(Type targetType, byte[] byteValues, int offset, int count)
        {
            using (var inputStream = new MemoryStream(byteValues.SubArray(offset, count)))
            {
                return ReadObject(targetType, inputStream);
            }
        }

        public virtual T ReadObject<T>(Stream stream)
        {
            return Converter.Assignable<T>(ReadObject(typeof(T), stream));
        }

        public virtual T ReadObject<T>(string path)
        {
            return Converter.Assignable<T>(ReadObject(typeof(T), path));
        }

        public virtual T ReadObject<T>(string stringValue, Encoding encoding)
        {
            return Converter.Assignable<T>(ReadObject(typeof(T), stringValue, encoding));
        }

        public virtual T ReadObject<T>(byte[] byteValues, int offset, int count)
        {
            return Converter.Assignable<T>(ReadObject(typeof(T), byteValues, offset, count));
        }

        public virtual void WriteObject(object instance, Stream stream)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteObject(object instance, string path)
        {
            using (var outputStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                WriteObject(instance, outputStream);
            }
        }

        public virtual string WriteString(object instance, Encoding encoding)
        {
            return encoding.GetString(WriteBytes(instance));
        }

        public virtual byte[] WriteBytes(object instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                WriteObject(instance, memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}