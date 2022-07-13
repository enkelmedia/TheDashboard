using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Umbraco.Cms.Core.Cache;
using Umbraco.Extensions;

namespace Our.Umbraco.TheDashboard.Extensions
{
    public static class UserExtensions
    {
        private static HttpClient Client = new HttpClient();
         /// <summary>
        /// Tries to lookup the user's Gravatar to see if the endpoint can be reached, if so it returns the valid URL
        /// </summary>
        
        /// <returns>
        /// A list of 5 different sized avatar URLs
        /// </returns>
        public static string[] GetUserAvatarUrls(int userId,string userEmail, string userAvatar, IAppCache cache)
        {
            // If FIPS is required, never check the Gravatar service as it only supports MD5 hashing.  
            // Unfortunately, if the FIPS setting is enabled on Windows, using MD5 will throw an exception
            // and the website will not run.
            // Also, check if the user has explicitly removed all avatars including a Gravatar, this will be possible and the value will be "none"
            if (userAvatar == "none" || CryptoConfig.AllowOnlyFipsAlgorithms)
            {
                return new string[0];
            }

            if (userAvatar.IsNullOrWhiteSpace())
            {
                var gravatarHash = HashEmailForGravatar(userEmail);
                var gravatarUrl = "https://www.gravatar.com/avatar/" + gravatarHash + "?d=404";

                //try Gravatar
                var gravatarAccess = cache.GetCacheItem<bool>("UserAvatar" + userId, () =>
                {
                    // Test if we can reach this URL, will fail when there's network or firewall errors
                    Client.Timeout = new TimeSpan(0,0,10);
                    var fetchTask = Client.GetAsync(gravatarUrl);
                    
                    try
                    {
                        var res = fetchTask.Result;
                        return res.IsSuccessStatusCode;
                    }
                    catch (Exception)
                    {
                        // There was an HTTP or other error, return an null instead
                        return false;
                    }
                });

                if (gravatarAccess)
                {
                    return new[]
                    {
                        gravatarUrl  + "&s=30",
                        gravatarUrl  + "&s=60",
                        gravatarUrl  + "&s=90",
                        gravatarUrl  + "&s=150",
                        gravatarUrl  + "&s=300"
                    };
                }

                return new string[0];
            }

            var customAvatarUrl = "/media/" + userAvatar;
          
            return new []
            {
                GetAvatarCrop(customAvatarUrl,30),
                GetAvatarCrop(customAvatarUrl,60),
                GetAvatarCrop(customAvatarUrl,90),
                GetAvatarCrop(customAvatarUrl,150),
                GetAvatarCrop(customAvatarUrl,300),
            };

        }

         internal static string GetAvatarCrop(string url, int dimensions)
         {
             return url + $"?width={dimensions}&height={dimensions}&mode=crop";
         }

         internal static string HashEmailForGravatar(string email)
         {
             MD5 md5Hasher = MD5.Create();
             byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));

             StringBuilder sBuilder = new StringBuilder();

             for (int i = 0; i < data.Length; i++)
             {
                 sBuilder.Append(data[i].ToString("x2"));
             }

             return sBuilder.ToString();
         }

    }
}
