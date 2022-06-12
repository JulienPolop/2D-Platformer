using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helpers
{
    public class GameSaveObject
    {
        public string LevelSavePoint;
        public string GuidStringSavePoint;
        public int PlayerMaxHealth;
        public int PlayerActualHealth;


        public Dictionary<string, List<SaveObjectBase>> LevelsSavableObjects { get; set; }
    }
}
