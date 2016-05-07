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
        public static async Task<Dictionary<string, LoliInfo>> GetLatestLoliInfos()
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            var uri = new Uri("https://dl.dropboxusercontent.com/s/7ktmo6zhuhu4xk8/lolitki.json");
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

        public static async Task<StorageFile> DownloadImage(string title, string path)
        {
            Uri source;
            if (!Uri.TryCreate(path, UriKind.Absolute, out source))
            {
                Debug.WriteLine("Invalid Uri: " + path);
                return null;
            }
            var outFile = await FileHelper.ImageToSave(title);
            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(source, outFile);
            download.Priority = BackgroundTransferPriority.High;
            var progress = await download.StartAsync();
            return outFile;
        }

        public static async Task<StorageFile> DownloadGallery(string title, string path)
        {
            Uri source;
            if (!Uri.TryCreate(path, UriKind.Absolute, out source))
            {
                Debug.WriteLine("Invalid Uri: " + path);
                return null;
            }
            var outFile = await FileHelper.ZipToSave(title);
            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(source, outFile);
            download.Priority = BackgroundTransferPriority.Default;
            var progress = await download.StartAsync();
            return outFile;
        }
    }
}
