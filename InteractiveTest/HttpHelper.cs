using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace InteractiveTest
{
    static class HttpHelper
    {
        public static void CheckHttpError(this HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
                string messageJsonString = result.Content.ReadAsStringAsync().Result;

                string message;

                try
                {
                    var returnedObject = JObject.Parse(messageJsonString);
                    message = string.Format("{0}{1}", returnedObject["Message"], returnedObject["error_description"]);
                }
                catch
                {
                    message = "Impossibile recuperare ulteriori informazioni sull'errore\r\n" + messageJsonString;
                }

                throw new HttpRequestException(string.Format("HTTP error {0} {1}\r\n{2}", (int) result.StatusCode, result.ReasonPhrase, message));
            }
        }

        #region HttpClient.OptionsAsync

        public static Task<HttpResponseMessage> OptionsAsync(this HttpClient httpClient, string requestUri)
        {
            return httpClient.OptionsAsync(CreateUri(requestUri));
        }

        public static Task<HttpResponseMessage> OptionsAsync(this HttpClient httpClient, Uri requestUri)
        {
            return httpClient.OptionsAsync(requestUri, System.Threading.CancellationToken.None);
        }

        private static Uri CreateUri(string uri)
        {
            if (string.IsNullOrEmpty(uri))
                return null;
            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }

        public static Task<HttpResponseMessage> OptionsAsync(this HttpClient httpClient, Uri requestUri, System.Threading.CancellationToken cancellationToken)
        {
            return httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Options, requestUri), cancellationToken);
        }

        #endregion

    }
}
