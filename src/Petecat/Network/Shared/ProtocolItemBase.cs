using System;
using System.Collections.Generic;
using System.Reflection;

namespace Petecat.Network.Shared
{
    public class ProtocolItemBase
    {
        private List<PropertyItem> mPropertyItems { get; set; }

        public ProtocolItemBase()
        {
            this.mPropertyItems = this.GetAllFields();
        }

        public virtual byte[] Create()
        {
            ByteArray byteArray = new ByteArray();
            foreach (PropertyItem current in this.mPropertyItems)
            {
                object value = current.PropertyInfo.GetValue(this, null);
                DataTypeHandler.EncodeField(ref byteArray, value, current.PropertyInfo.PropertyType);
            }
            return byteArray.Pack();
        }

        public virtual bool From(byte[] pack, ref int offset)
        {
            foreach (PropertyItem current in this.mPropertyItems)
            {
                object value = DataTypeHandler.DecodeField(pack, ref offset, current.PropertyInfo.PropertyType);
                current.PropertyInfo.SetValue(this, value, null);
            }
            return true;
        }

        private List<PropertyItem> GetAllFields()
        {
            List<PropertyItem> list = new List<PropertyItem>();
            PropertyInfo[] properties = base.GetType().GetProperties();
            if (properties == null)
            {
                return list;
            }
            PropertyInfo[] array = properties;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo propertyInfo = array[i];
                object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(FieldItemAttribute), true);
                if (customAttributes != null && customAttributes.Length > 0)
                {
                    PropertyItem item = new PropertyItem(propertyInfo, customAttributes[0] as FieldItemAttribute);
                    list.Add(item);
                }
            }
            list.Sort(new Comparison<PropertyItem>(ProtocolItemBase.ComparePropertyItem));
            return list;
        }

        private static int ComparePropertyItem(PropertyItem item1, PropertyItem item2)
        {
            return item1.FieldItemAttribute.Index - item2.FieldItemAttribute.Index;
        }
    }
}
