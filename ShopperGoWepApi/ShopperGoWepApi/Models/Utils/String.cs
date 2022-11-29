// ===============================================================
// File name: String.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

namespace ShopperGoWepApi.Models.Utils
{
    public class String
    {
        /// <summary>
        /// Impostra tutte le proprietà di un'oggetto in uppercase
        /// </summary>
        /// <param name="Object">Ritornano tutte le proprietà di un'oggetto in uppercase</param>
        public static void ToUpper(object? Object)
        {
            if (Object == null)
                return;

            Type type = Object.GetType();
            System.Reflection.PropertyInfo[] properties = type.GetProperties();

            foreach (System.Reflection.PropertyInfo pi in properties)
                if (pi.PropertyType.Name == "String" && pi.CanWrite)
                {
                    object? o = pi.GetValue(Object, null);
                    if (o != null)
                    {
                        string s = ((string)o).Trim();
                        s = s.ToUpper();

                        pi.SetValue(Object, s, null);
                    }
                }
        }
    }
}
