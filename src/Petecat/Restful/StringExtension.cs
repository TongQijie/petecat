using System;
using System.Collections.Generic;
namespace Petecat.Restful
{
    /// <summary>
    /// String Extension Method.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Split To List.
        /// </summary>
        /// <param name="str">Input String.</param>
        /// <returns>Ruturn String List.</returns>
        public static List<string> SplitToList(this string str)
        {
            List<string> result;
            if (string.IsNullOrEmpty(str))
            {
                result = new List<string>();
            }
            else
            {
                result = new List<string>(str.Split(new char[]
				{
					','
				}, StringSplitOptions.RemoveEmptyEntries));
            }
            return result;
        }

        /// <summary>
        /// List IgnoreCase Containes Items.
        /// </summary>
        /// <param name="list">Targer List.</param>
        /// <param name="strInput">Contains Item.</param>
        /// <returns>Return Containes Input list. </returns>
        public static bool ContainsIgnoreCase(this IEnumerable<string> list, string strInput)
        {
            bool bl = false;
            foreach (string item in list)
            {
                if (item.ToLower() == strInput.ToLower())
                {
                    bl = true;
                    break;
                }
            }
            return bl;
        }

        /// <summary>
        /// Equals IngoreCase TrimSpace.
        /// </summary>
        /// <param name="a">Input String a.</param>
        /// <param name="b">Input String b.</param>
        /// <returns>Return ISEquals.</returns>
        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
    }
}
