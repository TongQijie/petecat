﻿using Petecat.Utility;

using System;
using System.Reflection;

namespace Petecat.IoC
{
    public class DefaultPropertyDefinition : IPropertyDefinition
    {
        public DefaultPropertyDefinition(PropertyInfo propertyInfo)
        {
            Info = propertyInfo;
        }

        public string PropertyName { get { return Info.Name; } }

        public MemberInfo Info { get; private set; }

        public void SetValue(object instance, object value)
        {
            var propertyInfo = Info as PropertyInfo;

            object typeChangedValue;
            if (Converter.TryBeAssignable(value, propertyInfo.PropertyType, out typeChangedValue))
            {
                propertyInfo.SetValue(instance, typeChangedValue);
            }
            else
            {
                propertyInfo.SetValue(instance, null);
            }
        }
    }
}