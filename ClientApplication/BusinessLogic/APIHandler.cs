using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ClientApplication.BusinessLogic
{
    public interface IAPIHandler
    {
         Task<string> GetCurveHeadersAsync();
         Task<bool> InvokeDataTransfferAsync(string curves);
         Task<bool> StopDataTransfferAsync();
    }

   public  class APIHandler : IAPIHandler
    {
        HttpClient client;
        string url = "http://localhost:3118/api/Curves";

        public APIHandler()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetCurveHeadersAsync()
        {
            
            HttpResponseMessage responseMessage3 = await client.GetAsync(url + "/GetCurveHeaders/").ConfigureAwait(false);

            if (responseMessage3.IsSuccessStatusCode)
            {
                var responseData = responseMessage3.Content.ReadAsStringAsync().Result;

                string curveHeaders = JsonConvert.DeserializeObject<string>(responseData);
                return curveHeaders;
            }

            return string.Empty;
        }

        public async Task<bool> InvokeDataTransfferAsync(string curves)
        {
            HttpResponseMessage responseMessage3 = await client.GetAsync(url + "/InvokeTransfer/?curves=" + curves).ConfigureAwait(false);
            if (responseMessage3.IsSuccessStatusCode)
            {
                
                return true;
            }

            return false;
        }

        public async Task<bool> StopDataTransfferAsync()
        {
            HttpResponseMessage responseMessage3 = await client.GetAsync(url + "/StopTransfer/").ConfigureAwait(false);
            if (responseMessage3.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
