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
        public string Icon { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public float Heigth { get; set; }
        public float Weigth { get; set; }
        public float ChestSize { get; set; }
        public float WaistSize { get; set; }
        public float HipSize { get; set; }

        public LoliInfo() { }

        public LoliInfo(string icon, string name, int age, float heigth, float weigth, float chestSize, float waistSize, float hipSize)
        {
            Icon = icon;
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
