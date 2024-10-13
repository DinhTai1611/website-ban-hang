using System.Globalization;

namespace SV20T1020544.Web
{
    public static class Converter
    {
        /// <summary>
        /// chuyen chuoi s sang gia tri datetime (neu khong thanh cong tra ve null)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string s, string format = "d/M/yyyy;d-M-yyyyld.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, format.Split(';'), CultureInfo.InvariantCulture);
            }
            catch { return null; }
        }
    }
}
