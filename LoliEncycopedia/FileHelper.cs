using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;

namespace LoliEncycopedia
{
    public class FileHelper
    {
        public static StorageFolder GalleriesDirectory { get; private set; }
        public static StorageFolder IconDirectory { get; private set; }

        public static async void PrepareDirectories()
        {
            IconDirectory = await ApplicationData.Current.LocalFolder.CreateFolderAsync("data\\icon", CreationCollisionOption.OpenIfExists);
            GalleriesDirectory = await ApplicationData.Current.LocalFolder.CreateFolderAsync("data\\gallery", CreationCollisionOption.OpenIfExists);
        }

        public static async Task<StorageFile> IconToSave(string title)
        {
            return await IconDirectory.CreateFileAsync(title + ".png", CreationCollisionOption.ReplaceExisting);
        }

        public static async Task<StorageFile> GalleryImageToSave(string title, string fileName)
        {
            var gallery = await GalleriesDirectory.CreateFolderAsync(title, CreationCollisionOption.OpenIfExists);
            return await gallery.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        }

        public static async Task<Dictionary<string, string>> GetGalleryFiles(string title)
        {
            var loliDir = await GalleriesDirectory.CreateFolderAsync(title, CreationCollisionOption.OpenIfExists);
            var files = await loliDir.GetFilesAsync();
            var d = new Dictionary<string, string>();
            foreach (var file in files)
            {
                d.Add(file.Name, file.Path);
            }
            return d;
        }

        public static async Task<bool> IconFileExistsAsync(string loliTitle)
        {
            return await FileExists(IconDirectory, loliTitle + ".png");
        }

        public static async Task<bool> GalleryFileExists(string loliTitle, string fileName)
        {
            return await FileExists(GalleriesDirectory, loliTitle + "/" + fileName);
        }

        private static async Task<bool> FileExists(StorageFolder dir, string fileName)
        {
            try
            {
                await dir.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<string> GetIconFileHash(string loliTitle)
        {
            var img = await GetImage(IconDirectory, loliTitle + ".png");
            var buff = await FileIO.ReadBufferAsync(img);
            var md5Alg = HashAlgorithmNames.Md5;
            var algProvider = HashAlgorithmProvider.OpenAlgorithm(md5Alg);
            var hashData = algProvider.HashData(buff);
            var cryptoHashData = CryptographicBuffer.EncodeToHexString(hashData);
            return cryptoHashData;
        }

        private static async Task <StorageFile> GetImage(StorageFolder dir, string fileName)
        {
            return await dir.GetFileAsync(fileName);
        }
    }
}
