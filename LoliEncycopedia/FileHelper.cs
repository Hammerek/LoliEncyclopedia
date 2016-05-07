using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LoliEncycopedia
{
    public class FileHelper
    {
        public static StorageFolder GalleriesDirectory { get; private set; }
        public static StorageFolder ImagesDirectory { get; private set; }
        public static StorageFolder ZipFilesDirectory { get; private set; }

        public static async void PrepareDirectories()
        {
            ImagesDirectory = await ApplicationData.Current.LocalFolder.CreateFolderAsync("data\\images", CreationCollisionOption.OpenIfExists);
            GalleriesDirectory = await ApplicationData.Current.LocalFolder.CreateFolderAsync("data\\gallery", CreationCollisionOption.OpenIfExists);
            ZipFilesDirectory = await ApplicationData.Current.LocalFolder.CreateFolderAsync("data\\zips", CreationCollisionOption.OpenIfExists);
        }

        public static async Task<StorageFile> ImageToSave(string title)
        {
            return await ImagesDirectory.CreateFileAsync(title + ".png", CreationCollisionOption.ReplaceExisting);
        }

        public static async Task<StorageFile> ZipToSave(string title)
        {
            return await ZipFilesDirectory.CreateFileAsync(title + ".loli", CreationCollisionOption.ReplaceExisting);
        }

        public static async void UnZipGallery(StorageFile zipFile)
        {
            if (zipFile == null ||
                !Path.GetExtension(zipFile.DisplayName).Equals(".loli", StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.WriteLine("Wrong loli file!");
                return;
            }
            var zipMemStream = await zipFile.OpenStreamForReadAsync();
            using (var zip = new ZipArchive(zipMemStream, ZipArchiveMode.Read))
            {
                foreach (var entry in zip.Entries)
                {
                    await Unzip(entry, entry.FullName, Path.GetFileNameWithoutExtension(zipFile.DisplayName));
                }
            }
        }

        private static async Task Unzip(ZipArchiveEntry entry, string path, string title)
        {
            var dir = await GalleriesDirectory.CreateFolderAsync(title, CreationCollisionOption.OpenIfExists);
            using (var stream = entry.Open())
            {
                var buffer = new byte[entry.Length];
                stream.Read(buffer, 0, buffer.Length);
                var file = await dir.CreateFileAsync(entry.Name, CreationCollisionOption.ReplaceExisting);
                using (var uFileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var outputStream = uFileStream.AsStreamForWrite())
                    {
                        outputStream.Write(buffer,0,buffer.Length);
                        outputStream.Flush();
                    }
                }
            }
        }

        public static Uri GetImagePath()
        {

            return null;
        }
    }
}
