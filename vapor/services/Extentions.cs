using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.services
{
    public static class Extentions
    {
        public static void SetListOfString(this ISession session, string key, List<String> val)
        {
            var str = String.Join(";", val.ToArray());
            session.SetString(key, str);
        }

        public static List<String> GetListOfString(this ISession session, string key)
        {
            String cartString = session.GetString(key);
            if (cartString != null)
            {
                List<string> result = cartString.Length > 0 ? cartString.Split(';').ToList() : new List<string>();
                return result;
            }
            else
            {
                return new List<string>();
            }
        }
    }
}
