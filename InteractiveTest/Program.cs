using System;
using System.Net.Http;
using InteractiveTest.OAuth2;

namespace InteractiveTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const string baseUri = "http://localhost:53969/";

            OAuth2Client oAuth2Client = new OAuth2Client(baseUri + "token", baseUri + "api/Account/Logout");
            oAuth2Client.Login("MyTenant\\MyUserName", "Password");

            using (HttpClient httpClient = new HttpClient())
            {
                oAuth2Client.SetHttpClientAuthorization(httpClient);
                string result = httpClient.GetStringAsync(baseUri + "api/Values").Result;
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }
    }
}
