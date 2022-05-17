using DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HospitalReservation.Classes
{
    public static class Extensions
    {
        public static void SaveUser(this Controller c, User u)
        {
            if (u == null)
            {
                c.HttpContext.Session.Remove("UserObject");
                return;
            }

            c.HttpContext.Session.SetString("UserObject", JsonConvert.SerializeObject(u, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
        }

        public static User GetUser(this Controller c)
        {
            var u = c.HttpContext.Session.GetString("UserObject");
            if (u == null)
                return null;
            return JsonConvert.DeserializeObject<User>(u);
        }
    }
}
