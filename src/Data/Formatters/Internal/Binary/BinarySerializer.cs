using Petecat.Utility;
using Petecat.Collection;
using Petecat.Data.Attributes;

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Petecat.Data.Formatters.Internal.Binary
{
    internal class BinarySerializer : IKeyedObject<string>
    {
        public BinarySerializer(Type type)
        {
            Type = type;
            Initialize();
        }

        public string Key { get { return Type.FullName; } }

        public Type Type { get; private set; }

        #region Serializer Cache

        private static ThreadSafeKeyedObjectCollection<string, BinarySerializer> _Serializers = null;

        private static ThreadSafeKeyedObjectCollection<string, BinarySerializer> Serializers
        {
            get { return _Serializers ?? (_Serializers = new ThreadSafeKeyedObjectCollection<string, BinarySerializer>()); }
        }

        public static BinarySerializer GetSerializer(Type targetType)
        {
            if (!Serializers.ContainsKey(targetType.FullName))
            {
                Serializers.Add(new BinarySerializer(targetType));
            }

            return _Serializers.Get(targetType.FullName, null);
        }

        #endregion

        #region Binary Properties

        private ThreadSafeKeyedObjectCollection<string, BinaryProperty> _JsonProperties = null;

        public IEnumerable<BinaryProperty> JsonProperties { get { return _JsonProperties.Values; } }

        private object _SyncLocker = new object();

        public void Initialize()
        {
            lock (_SyncLocker)
            {
                _JsonProperties = new ThreadSafeKeyedObjectCollection<string, BinaryProperty>();

                var runtimeType = GetRuntimeType(Type);
                if (runtimeType == RuntimeType.Object)
                {
                    foreach (var propertyInfo in Type.GetProperties().Where(x => x.CanRead && x.CanWrite))
                    {
                        var attribute = ReflectionUtility.GetCustomAttribute<BinaryPropertyAttribute>(propertyInfo);
                        _JsonProperties.Add(new BinaryProperty(propertyInfo, attribute == null ? null : attribute.Alias, attribute.Index));
                    }
                }
            }
        }

        #endregion

        #region Runtime Type

        private RuntimeType _CurrentRuntimeType = RuntimeType.Unknown;

        public RuntimeType CurrentRuntimeType
        {
            get
            {
                if (_CurrentRuntimeType == RuntimeType.Unknown)
                {
                    _CurrentRuntimeType = GetRuntimeType(Type);
                }

                return _CurrentRuntimeType;
            }
        }

        public RuntimeType GetRuntimeType(Type targetType)
        {
            if (typeof(ICollection).IsAssignableFrom(targetType))
            {
                return RuntimeType.Collection;
            }
            else if (targetType.IsClass && targetType != typeof(String))
            {
                return RuntimeType.Object;
            }
            else
            {
                return RuntimeType.Value;
            }
        }

        public enum RuntimeType
        {
            Unknown,

            Value,

            Collection,

            Object,
        }

        #endregion
    }
}
