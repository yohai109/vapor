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
            List<string> result = session.GetString(key).Split(';').ToList();
            return result;
        }
    }
}
