using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SundooqLanding.Models
{
    public class Constants
    {
        public const string NOT_AUTHORIZED = "Not Authorized";
        public const string FACEBOOK_LOGIN_COMPLETE_URL = "http://localhost:4508/Home/FacebookLoginComplete";
        public const string FACEBOOK_USER_DENIED = "user_denied";
        public const string FACEBOOK_ERROR_REASON = "error_reason";

        /// <summary>
        /// This will be on the Application Settings on FB
        /// </summary>
        public const string FACEBOOK_APP_ID = "354831701310539";
        public const string FACEBOOK_SECRET = "e0b25f722c04a722eb7ab10b39a26f19";

        /// <summary>
        /// Permissions that we need from the user to interact with the User's FB account
        /// <seealso cref="http://developers.facebook.com/docs/authentication/permissions/" />
        public const string FACEBOOK_SCOPE = "public_profile, email"; //change as needed


        public static string GetFacebookLoginUrl(string returnUrl)
        {
            //var url = string.Format(@"https://www.facebook.com/dialog/oauth/?client_id={0}&amp;redirect_uri={1}&amp;scope={2}&amp;state={3}",
            //                                               FACEBOOK_APP_ID,
            //                                              FACEBOOK_LOGIN_COMPLETE_URL,
            //                                               FACEBOOK_SCOPE,
            //                                               (returnUrl ?? "http://localhost:2994"));

            var url = "https://graph.facebook.com/oauth/authorize? type=web_server& client_id=" + FACEBOOK_APP_ID + "& redirect_uri=http://localhost:4508/Home/FacebookLoginComplete";
            return url;

        }
    }
}