using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweaksAndFixes
{
    internal static class Extensions
    {
        public static object GetPrivateField(this object obj, string field)
        {
            return Traverse.Create(obj).Field(field).GetValue();
        }

        public static T GetPrivateField<T>(this object obj, string field)
        {
            return (T)obj.GetPrivateField(field);
        }

        public static void SetPrivateField(this object obj, string field, object value)
        {
            Traverse.Create(obj).Field(field).SetValue(value);
        }

        public static object InvokePrivateMethod(this object obj, string method, params object[] parameters)
        {
            return AccessTools.Method(obj.GetType(), method).Invoke(obj, parameters);
        }

        public static T InvokePrivateMethod<T>(this object obj, string method, params object[] parameters)
        {
            return (T)obj.InvokePrivateMethod(method, parameters);
        }
    }
}
