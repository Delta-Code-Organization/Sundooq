using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;

namespace SundooqLanding.Models
{
    public class FacebookService
    {
        public static dynamic WriteMessageToFacebookWall(string message, long facebookId, string token)
        {
            dynamic postResponse;
            try
            {
                postResponse = PostToFacebookWall(token, message, facebookId);
                return postResponse;
            }
            catch (Exception ex)
            {
                //log it
            }

            return null;
        }

        public static string GetFacebookAccessToken(string code, string returnUrl, string fbRedirectUri)
        {
            var f = new FacebookClient();
            dynamic result = f.Get("oauth/access_token", new
            {
                client_id = Constants.FACEBOOK_APP_ID,
                client_secret = Constants.FACEBOOK_SECRET,
                redirect_uri = fbRedirectUri,
                code = code,
                state = returnUrl
            });
            return result.access_token as string;
        }

        public static dynamic GetFacebookResponse(string actionUrl, string accessToken)
        {
            FacebookClient FbApp;
            if (string.IsNullOrEmpty(accessToken))
            {
                FbApp = new FacebookClient();
            }
            else
            {
                FbApp = new FacebookClient(accessToken);
            }
            return FbApp.Get(actionUrl) as JsonObject;
        }

        private static dynamic PostToFacebookWall(string token, string message, long facebookId)
        {
            FacebookClient FbApp = new FacebookClient(token);
            IDictionary<string, object> postArgs = new Dictionary<string, object>();
            postArgs["message"] = message;
            if (token != null) postArgs["access_token"] = token;
            postArgs["uid"] = facebookId;
            var result = FbApp.Post("/" + facebookId + "/feed", postArgs);
            return result;
        }
    }
}