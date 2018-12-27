using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace InteractiveTest.OAuth2
{
    public class OAuth2Client
    {
        public OAuth2Client(string tokenUri, string logoutUri)
        {
            TokenUri = tokenUri;
            LogoutUri = logoutUri;
        }

        public string TokenUri { get; set; }
        public string LogoutUri { get; set; }

        public OAuth2Token Token { get; set; }

        public void Login(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                string serializedData = string.Format("grant_type=password&username={0}&password={1}", userName, password);
                var content = new StringContent(serializedData, Encoding.UTF8, "application/x-www-form-urlencoded");

                using (var result = client.PostAsync(String.Format("{0}", TokenUri), content).Result)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string jsonString = result.Content.ReadAsStringAsync().Result;
                        Token = JsonConvert.DeserializeObject<OAuth2Token>(jsonString);
                    }
                    else
                    {
                        result.CheckHttpError();
                    }
                }
            }
        }

        public void Logout()
        {
            using (var client = new HttpClient())
            {
                SetHttpClientAuthorization(client);

                using (var result = client.GetAsync(String.Format("{0}", LogoutUri)).Result)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        Token = null;
                    }
                    else
                    {
                        result.CheckHttpError();
                    }
                }
            }
        }

        public void SetHttpClientAuthorization(HttpClient httpClient)
        {
            if (Token == null || string.IsNullOrEmpty(Token.AccessToken))
                throw new InvalidOperationException("Please login first");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);

        }
    }
}
