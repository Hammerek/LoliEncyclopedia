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




namespace LoliEncycopedia
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

        public static LoliInfo GetLoliInfo(string name)
        {
            using (var db = DbConnection)
            {
                return db.Table<LoliInfo>().First(info => info.Name == name);
            }
        }

        public static void AddLoliInfo(LoliInfo info)
        {
            using (var db = DbConnection)
            {
                db.Insert(info);
            }
        }

        public static bool ContainsLoli(string name)
        {
            using (var db = DbConnection)
            {
                var i = (from infos in db.Table<LoliInfo>() where infos.Name == name select infos).FirstOrDefault();
                return i != null;
            }
        }

        /// <summary>
        /// string = Loli title, string = Loli icon 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetHarem()
        {
            var d = new Dictionary<string, string>();
            using (var db = DbConnection)
            {
                var haremArray = db.Table<LoliInfo>().ToArray();
                foreach (var info in haremArray)
                {
                    d.Add(info.Name, info.Icon);
                }
            }
            return d;
        }
        public static SQLiteConnection DbConnection => new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path,"LoliDatabase.sqlite"));
    }
}
