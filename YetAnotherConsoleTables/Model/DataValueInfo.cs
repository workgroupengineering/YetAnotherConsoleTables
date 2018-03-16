﻿using System;
using System.Reflection;
using YetAnotherConsoleTables.Attributes;

namespace YetAnotherConsoleTables.Model
{
    internal class DataValueInfo
    {
        private FieldInfo field;
        private PropertyInfo property;

        private TableIgnoreAttribute ignoreAttr;
        private TableDisplayNameAttribute displayNameAttr;

        internal DataValueInfo(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Field)
            {
                field = member as FieldInfo;
                ReadAttributes(member);
            }
            else if (member.MemberType == MemberTypes.Property)
            {
                property = member as PropertyInfo;
                ReadAttributes(member);
            }
            else
            {
                throw new Exception("Member must be of type FieldInfo or PropertyInfo");
            }
        }

        internal bool IsIgnored => ignoreAttr != null;

        internal bool CanRead => field != null ? true : property.CanRead;

        internal string Name
        {
            get
            {
                if (displayNameAttr != null)
                {
                    return displayNameAttr.Name;
                }
                else if (field != null)
                {
                    return field.Name;
                }
                else
                {
                    return property.Name;
                }
            }
        }

        internal string GetValue(object obj)
        {
            if (field != null)
            {
                return field.GetValue(obj)?.ToString();
            }
            else
            {
                return property.GetValue(obj)?.ToString();
            }
        }

        private void ReadAttributes(MemberInfo member)
        {
            ignoreAttr = (TableIgnoreAttribute)Attribute
                .GetCustomAttribute(member, typeof(TableIgnoreAttribute));
            displayNameAttr = (TableDisplayNameAttribute)Attribute
                .GetCustomAttribute(member, typeof(TableDisplayNameAttribute));
        }
    }
}
