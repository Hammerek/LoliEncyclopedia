using System;
using System.Collections.Generic;
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

        public static void SaveFile()
        {

        }

        public void UnZipGallery()
        {

        }

        public string GetImagePath()
        {

            return null;
        }
    }
}
