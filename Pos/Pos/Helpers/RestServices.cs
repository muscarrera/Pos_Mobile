using Newtonsoft.Json;
using Pos.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Helpers
{
    public class RestServices  
        
    {
        HttpClient client;

        public RestServices()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded' "));
        }


        public async  Task<List<Article>> RefreshDataAsync()
        {
            List<Article> arts;
              Uri uri = new Uri(string.Format("http://Localhost:8000/api/art/gr"));
            
                HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                arts = JsonConvert.DeserializeObject<List<Article>>(content);

                
                return arts;
            }

            return null;
        }

    }
}
