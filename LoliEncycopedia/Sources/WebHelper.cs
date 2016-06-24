using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Newtonsoft.Json;

namespace LoliEncyclopedia.Sources
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
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
                throw new Exception("Error: " + ex.HResult.ToString("X"), ex);
            }
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
                throw new Exception("Error: " + ex.HResult.ToString("X"), ex);
            }
        }
        public static async Task<Dictionary<string, string>> GetLatestIconHashes()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            var uri = new Uri(Host + "/hash_icons.json");
            var op = httpClient.GetStringAsync(uri);
            try
            {
                var httpResponse = await httpClient.GetAsync(uri);
                httpResponse.EnsureSuccessStatusCode();
                var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                var deso = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpResponseBody);
                return deso;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
                throw new Exception("Error: " + ex.HResult.ToString("X"), ex);
            }
        }

        private static async Task<Dictionary<string, string>> GetLatestGalleryHashes(string loliTitle)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            var uri = new Uri(Host + "/lolis/hash_" + loliTitle + ".json");
            var op = httpClient.GetStringAsync(uri);
            try
            {
                var httpResponse = await httpClient.GetAsync(uri);
                httpResponse.EnsureSuccessStatusCode();
                var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                var deso = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpResponseBody);
                return deso;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
                throw new Exception("Error: " + ex.HResult.ToString("X"), ex);
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
                try
                {
                    var progress = await download.StartAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(title + ": " + e);
                    await outFile.DeleteAsync(StorageDeleteOption.Default);
                }
            }
        }

        public static async Task DownloadLoliGallery(string loliTitle)
        {
            try
            {
                var galleryHashes = await GetLatestGalleryHashes(loliTitle);
                var galleryList = await GetLatestLoliGallery();
                if (galleryList.ContainsKey(loliTitle))
                {
                    var gallery = galleryList[loliTitle];
                    foreach (var image in gallery)
                    {
                        if (await FileHelper.GalleryFileExists(loliTitle, image))
                        {
                            var currentFileHash = await FileHelper.GetGalleryFileHash(loliTitle, image);
                            var newFileHash = galleryHashes[image];
                            if (!currentFileHash.Equals(newFileHash))
                            {
                                await DownloadGalleryImage(loliTitle, image);
                            }
                        }
                        else
                        {
                            await DownloadGalleryImage(loliTitle, image);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Problem with gallery downloading.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem with gallery downloading.");
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
