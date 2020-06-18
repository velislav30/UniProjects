using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VT.Business.DTOs;

namespace VT.Website
{
    public static class SessionExtension
    {
        public static void SetCurrentUser(this ISession session, string key, UserDto user)
        {
            session.SetString(key, JsonConvert.SerializeObject(user));
        }

        public static UserDto GetCurrentUser(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(UserDto) : JsonConvert.DeserializeObject<UserDto>(value);
        }


        public static String GetCurrentUserCredentials(this ISession session, string key)
            {
                var value = session.GetString(key);
                var user = value == null ? default(UserDto) : JsonConvert.DeserializeObject<UserDto>(value);

                return user == null ? "" : user.Username + ":" + user.Password;
            }


        public static int GetCurrentUserId(this ISession session, string key)
        {
            var value = session.GetString(key);
            var user = value == null ? default(UserDto) : JsonConvert.DeserializeObject<UserDto>(value);

            return user == null ? -1 : user.Id;
        }
    }
}
