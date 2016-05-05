using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoliEncycopedia
{
    public class WebHelper
    {
        public static async Task<Dictionary<string, LoliInfo>> GetLatestLoliInfos()
        {
            
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            var uri = new Uri("http://localhost:81/lolitki.json");
            var op = httpClient.GetStringAsync(uri);
            var httpResponseBody = "";
            try
            {
                var httpResponse = await httpClient.GetAsync(uri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                var deso = JsonConvert.DeserializeObject<Dictionary<string, LoliInfo>>(httpResponseBody);
                return deso;
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }

            return null;
        }
    }
}
