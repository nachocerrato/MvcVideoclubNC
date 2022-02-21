using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, object value)
        {
            string json = JsonConvert.SerializeObject(value);
            session.SetString(key, json);
        }

        public static T GetObject<T>(this ISession session, string key)      
        {
            string json = session.GetString(key);
            if(json == null)
            {
                return default(T);
            }
            T data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
    }
}
