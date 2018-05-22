using ClientApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientApplication.BusinessLogic
{
    public interface IAPIHandler
    {
         Task<string> GetCurveHeadersAsync();
         Task<bool> InvokeDataTransfferAsync(string curves);
         Task<bool> StopDataTransfferAsync();
         Task<string> ExportXMLDataAsync();
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
            else
            {
                Logger.WrieException("Failed to read curve headers");
                throw new Exception("Failed to read curve headers");
            }
        }

        public async Task<bool> InvokeDataTransfferAsync(string curves)
        {

            HttpResponseMessage responseMessage3 = await client.GetAsync(url + "/InvokeTransfer/?curves=" + curves).ConfigureAwait(false);
            if (responseMessage3.IsSuccessStatusCode)
            {
                
                return true;
            }
            else
            {
                Logger.WrieException("Failed to invoke data transfer");
                throw new Exception("Failed to invoke data transfer");
            }
        }

        public async Task<bool> StopDataTransfferAsync()
        {
            HttpResponseMessage responseMessage3 = await client.GetAsync(url + "/StopTransfer/").ConfigureAwait(false);
            if (responseMessage3.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Logger.WrieException("Failed to stop data transfer");
                throw new Exception("Failed to stop data transfer");
            }
        }
      
        public async Task<string> ExportXMLDataAsync()
        {
            HttpResponseMessage responseMessage3 = await client.PostAsJsonAsync<List<CurveInfo>>(url + "/ExportData", CurvesData.LSTIndexInfo).ConfigureAwait(false);
            if (responseMessage3.IsSuccessStatusCode)
            {
                return responseMessage3.Content.ReadAsStringAsync().Result;
            }
            else
            {
                Logger.WrieException("Failed to export data as xml");
                throw new Exception("Failed to export data as xml");
            }
        }
    }
}
