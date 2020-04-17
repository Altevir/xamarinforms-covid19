using System;

namespace XFCovid19.Helpers
{
    public static class ExtensionMethods
    {
        public static string TransformNumberToString(this int number)
        {
            switch (number.ToString().Length)
            {
                case 1:
                case 2:
                case 3:
                    return number.ToString();
                case 4:
                    return $"{number.ToString().Insert(1, ",").Substring(0, 3)}k";
                case 5:
                    return $"{number.ToString().Insert(2, ",").Substring(0, 4)}k";
                case 6:
                    return $"{number.ToString().Insert(3, ",").Substring(0, 5)}k";
                default:
                    return $"{number.ToString().Insert(1, ",").Substring(0, 3)}m";
            }
        }

        public static string FormatDateTime(this DateTime dateTime, string cultureInfo)
        {
            if (cultureInfo.Equals("pt"))
                return string.Format("{0:d MMMM, yyyy | HH:mm:ss}", System.Convert.ToDateTime(dateTime));
            else
                return string.Format("{0:MMMM d, yyyy | HH:mm:ss}", System.Convert.ToDateTime(dateTime));
        }

        public static DateTime TransformLongToDateTime(this long value)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(value).ToLocalTime();
        }

        public static string RemoveAccents(this string texto)
        {
            string listA = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string listB = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int a = 0; a < listA.Length; a++)
            {
                texto = texto.Replace(listA[a].ToString(), listB[a].ToString());
            }

            return texto;
        }
    }
}

