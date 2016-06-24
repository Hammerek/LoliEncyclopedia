using SQLite.Net.Attributes;

namespace LoliEncyclopedia.Sources
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
        public string Description { get; set; }

        public LoliInfo() { }

        public LoliInfo(string anime, string name, int age, float heigth, float weigth, float chestSize, float waistSize, float hipSize, string description)
        {       
            Anime = anime;
            Name = name;
            Age = age;
            Heigth = heigth;
            Weigth = weigth;
            ChestSize = chestSize;
            WaistSize = waistSize;
            HipSize = hipSize;
            Description = description;
        }
    }
}
