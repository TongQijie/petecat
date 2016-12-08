using System.Reflection;

using Petecat.Extending;
using Petecat.DependencyInjection.Attribute;

namespace Petecat.Data
{
    [DependencyInjectable(Inference = typeof(IReplicator), Singleton = true)]
    public class Replicator : IReplicator
    {
        public object ShallowCopy(object obj)
        {
            var type = obj.GetType();

            var copy = type.CreateInstance();
            if (type.IsValueType)
            {
                copy = obj;
            }
            else if (type == typeof(string))
            {
                if (obj == null)
                {
                    copy = null;
                }
                else
                {
                    copy = string.Copy(obj as string);
                }
            }
            else
            {
                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                if (fields == null || fields.Length == 0)
                {
                    return copy;
                }

                foreach (var field in fields)
                {
                    field.SetValue(copy, field.GetValue(obj));
                }
            }

            return copy;
        }

        public T ShallowCopy<T>(object obj)
        {
            return (T)ShallowCopy(obj);
        }

        public object DeepCopy(object obj)
        {
            var type = obj.GetType();

            var copy = type.CreateInstance();
            if (type.IsValueType)
            {
                copy = obj;
            }
            else if (type == typeof(string))
            {
                if (obj == null)
                {
                    copy = null;
                }
                else
                {
                    copy = string.Copy(obj as string);
                }
            }
            else
            {
                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                if (fields == null || fields.Length == 0)
                {
                    return copy;
                }

                foreach (var field in fields)
                {
                    if (field.FieldType.IsValueType)
                    {
                        field.SetValue(copy, field.GetValue(obj));
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        var stringValue = field.GetValue(obj) as string;
                        if (stringValue == null)
                        {
                            field.SetValue(copy, null);
                        }
                        else
                        {
                            field.SetValue(copy, string.Copy(stringValue));
                        }
                    }
                    else
                    {
                        var objectValue = field.GetValue(obj);
                        if (objectValue == null)
                        {
                            field.SetValue(copy, null);
                        }
                        else
                        {
                            field.SetValue(copy, DeepCopy(field.GetValue(obj)));
                        }
                    }
                }
            }

            return copy;
        }

        public T DeepCopy<T>(object obj)
        {
            return (T)DeepCopy(obj);
        }
    }
}
