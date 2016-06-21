using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Text;
using Windows.Storage;
using Windows.System.UserProfile;
using SQLite.Net.Platform.WinRT;
using SQLite.Net;




namespace LoliEncyclopedia
{
    public class LoliInfoDatabase
    {
        public static void CreateDatabase()
        {
            try
            {
                using (var db = DbConnection)
                {
                    db.CreateTable<LoliInfo>();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public static LoliInfo GetLoliInfo(string title)
        {
            using (var db = DbConnection)
            {
                return db.Table<LoliInfo>().First(info => info.Title == title);
            }
        }

        public static void AddLoliInfo(LoliInfo info)
        {
            using (var db = DbConnection)
            {
                db.Insert(info);
            }
        }

        public static bool ContainsLoli(string title)
        {
            using (var db = DbConnection)
            {
                var i = (from infos in db.Table<LoliInfo>() where infos.Title == title select infos).FirstOrDefault();
                return i != null;
            }
        }
        public static void UpdateLoliInfo(LoliInfo newLoliInfo)
        {
            var oldLoliInfo = GetLoliInfo(newLoliInfo.Title);
            newLoliInfo.Id = oldLoliInfo.Id;

            using (var db = DbConnection)
            {
                db.Update(newLoliInfo);
            }
        }

        /// <summary>
        /// string = Loli title, string = Loli icon 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLoliTitles()
        {
            var d = new List<string>();
            using (var db = DbConnection)
            {
                var haremArray = db.Table<LoliInfo>().ToArray();
                foreach (var info in haremArray)
                {
                    d.Add(info.Title);
                }
            }
            return d;
        }
        public static SQLiteConnection DbConnection => new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "LoliDatabase.sqlite"));


    }
}
