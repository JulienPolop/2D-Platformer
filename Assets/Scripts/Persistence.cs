using Assets.Scripts.Helpers;
using static Player_Controller;

namespace Assets.Scripts
{
    static class Persistence
    {
        public static string ActualLevel { get; set; }
        public static string NextLevel { get; set; }
        public static int? IndexNextLevelEntrie { get; set; }
        public static DirectionEnum PlayerDirection { get; set; }
        public static LevelManager LevelManager { get; set; }


       public static GameSaveObject GameSave{ get; set; }
}
}
