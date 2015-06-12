using System.IO;
using System.Web;
using Umbraco.Core.Models.Membership;
using Umbraco.Web;

namespace TheDashboard.Core
{
    public class UserAvatarProvider
    {
        public static string GetAvatarUrl(IUser user)
        {
            if (HttpContext.Current.Application["dashboard_user_" + user.Id] == null)
            {
                var value = string.Empty;
                
                var path = UmbracoContext.Current.HttpContext.Server.MapPath("/app_plugins/thedashboard/avatars/");

                if (File.Exists(path + "\\" + user.Id + ".jpg"))
                {
                    value =  "/app_plugins/thedashboard/avatars/" + user.Id + ".jpg";
                }
                else
                {
                    value = string.Format("http://www.gravatar.com/avatar/{0}&s=55", GravatarHelper.HashEmailForGravatar(user.Email));    
                }

                HttpContext.Current.Application["dashboard_user_" + user.Id] = value;
            }

            return HttpContext.Current.Application["dashboard_user_" + user.Id].ToString();

        }
    }
}