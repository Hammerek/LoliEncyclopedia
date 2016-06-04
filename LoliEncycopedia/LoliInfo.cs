using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLite.Net.Platform.WinRT;
using SQLite.Net;

namespace LoliEncycopedia
{
    public class LoliInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }      
        public string Anime { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public float Heigth { get; set; }
        public float Weigth { get; set; }
        public float ChestSize { get; set; }
        public float WaistSize { get; set; }
        public float HipSize { get; set; }       

        public LoliInfo() { }

        public LoliInfo(string anime, string name, int age, float heigth, float weigth, float chestSize, float waistSize, float hipSize)
        {       
            Anime = anime;
            Name = name;
            Age = age;
            Heigth = heigth;
            Weigth = weigth;
            ChestSize = chestSize;
            WaistSize = waistSize;
            HipSize = hipSize;           
        }
    }
}
