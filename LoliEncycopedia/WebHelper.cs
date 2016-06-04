using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoliEncycopedia
{
    public class WebHelper
    {
        private const string Host = "http://lolienc.grmdev.pl";

        public static async Task<Dictionary<string, LoliInfo>> GetLatestLoliInfos()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            var uri = new Uri(Host + "/loli_data.json");
            var op = httpClient.GetStringAsync(uri);
            try
            {
                var httpResponse = await httpClient.GetAsync(uri);
                httpResponse.EnsureSuccessStatusCode();
                var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                var deso = JsonConvert.DeserializeObject<Dictionary<string, LoliInfo>>(httpResponseBody);
                return deso;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
            }
            return null;
        }

        public static async Task<Dictionary<string, List<string>>> GetLatestLoliGallery()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            var uri = new Uri(Host + "/loli_img.json");
            var op = httpClient.GetStringAsync(uri);
            try
            {
                var httpResponse = await httpClient.GetAsync(uri);
                httpResponse.EnsureSuccessStatusCode();
                var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                var deso = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(httpResponseBody);
                return deso;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
                return null;
            }
        }

        public static async Task DownloadIconImage(string title)
        {
            Uri source;
            if (!Uri.TryCreate(Host + "/lolis/images/" + title + ".png", UriKind.Absolute, out source))
            {
                Debug.WriteLine("Invalid Uri: " + Host + "/lolis/images/" + title + ".png");
            }
            else
            {
                var outFile = await FileHelper.IconToSave(title);
                var downloader = new BackgroundDownloader();
                var download = downloader.CreateDownload(source, outFile);
                download.Priority = BackgroundTransferPriority.High;
                var progress = await download.StartAsync();
            }
        }

        public static async void DownloadLoliGallery(string title)
        {
            var galleryList = await GetLatestLoliGallery();
            if (galleryList.ContainsKey(title))
            {
                var gallery = galleryList[title];
                foreach (var image in gallery)
                {
                    await DownloadGalleryImage(title, image);
                }
            }
        }

        private static async Task<StorageFile> DownloadGalleryImage(string title, string fileName)
        {
            Uri source;
            if (!Uri.TryCreate($"{Host}/lolis/galleries/{title}/{fileName}", UriKind.Absolute, out source))
            {
                Debug.WriteLine("Invalid Uri: " + $"{Host}/lolis/galleries/{title}/{fileName}");
                return null;
            }
            var outFile = await FileHelper.GalleryImageToSave(title, fileName);
            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(source, outFile);
            download.Priority = BackgroundTransferPriority.High;
            var progress = await download.StartAsync();
            return outFile;
        }
    }
}
