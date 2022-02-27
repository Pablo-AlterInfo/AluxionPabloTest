using AluxionTest.Models.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.IO;

namespace AluxionTest.Repositories.Interfaces
{
    public class UnsplashRepository : IUnsplashRepository
    {
        private readonly string unsplashBaseURL = @"https://api.unsplash.com/";
        private readonly string ClientID = "fEAQstRbQ-00-43YJ1SYDRMM-fQeu9EBON3lsvHZo1I";



        public async Task<List<UnsplashImageItem>> ListImagesByQuery(string query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Oauth", "Client-ID " + ClientID);
            string URI = unsplashBaseURL + "photos?query=" + query;
            try
            {
                var response = await client.GetAsync(URI);
                var responseString = response.Content.ReadAsStringAsync().Result;
                List<UnsplashImageItem> result = JsonConvert.DeserializeObject<List<UnsplashImageItem>>(responseString);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UnsplashImageItem> SelectImageById(string imageId)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(unsplashBaseURL);

            // Agregar Token de Unsplash API como "Oauth Token"
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Oauth", "Client-ID " + ClientID);
            string URI = unsplashBaseURL + "photos/" + imageId + "/";
            try
            {
                var response = await client.GetAsync(URI);
                var responseString = response.Content.ReadAsStringAsync().Result;
                UnsplashImageItem result = JsonConvert.DeserializeObject<UnsplashImageItem>(responseString);
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<StreamData> DownloadImage(UnsplashImageItem image)
        {
            HttpClient client = new HttpClient();
            byte[] responseByteArray;
            Stream responseStream;
            StreamData streamData = new StreamData();
            try
            {
                var response = await client.GetAsync(image.urls.small);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new StreamData { Id = "Error", StreamInfo = null };
                }
                responseByteArray = await response.Content.ReadAsByteArrayAsync();
                responseStream = new MemoryStream(responseByteArray);
                streamData.Id = image.id;
                streamData.StreamInfo = responseStream;

            }
            catch
            {
                return null;
            }

            return streamData;
        }
    }
}
