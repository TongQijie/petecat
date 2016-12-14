using System.Reflection;

using Petecat.Extending;
using Petecat.DependencyInjection.Attribute;
using System;

namespace Petecat.Data
{
    [DependencyInjectable(Inference = typeof(IReplicator), Singleton = true)]
    public class Replicator : IReplicator
    {
        public object ShallowCopy(object obj)
        {
            var type = obj.GetType();

            
            if (type.IsValueType)
            {
                return obj;
            }
            else if (type == typeof(string))
            {
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return string.Copy(obj as string);
                }
            }
            else if (type.IsArray)
            {
                var array = obj as Array;

                var copy = Array.CreateInstance(type.GetElementType(), array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    copy.SetValue(array.GetValue(i), i);
                }

                return copy;
            }
            else
            {
                var copy = type.CreateInstance();

                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    field.SetValue(copy, field.GetValue(obj));
                }

                return copy;
            }
        }

        public T ShallowCopy<T>(object obj)
        {
            return (T)ShallowCopy(obj);
        }

        public object DeepCopy(object obj)
        {
            var type = obj.GetType();

            if (type.IsValueType)
            {
                return obj;
            }
            else if (type == typeof(string))
            {
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return string.Copy(obj as string);
                }
            }
            else if (type.IsArray)
            {
                var array = obj as Array;

                var copy = Array.CreateInstance(type.GetElementType(), array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    copy.SetValue(DeepCopy(array.GetValue(i)), i);
                }

                return copy;
            }
            else
            {
                var copy = type.CreateInstance();

                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
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

                return copy;
            }
        }

        public T DeepCopy<T>(object obj)
        {
            return (T)DeepCopy(obj);
        }
    }
}
