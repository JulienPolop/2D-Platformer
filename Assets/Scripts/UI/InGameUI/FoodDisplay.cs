using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class FoodDisplay : MonoBehaviour
    {
        public Text text;
        static int CurrentFood;
        static int MaxFood;

        private void Update()
        {
            text.text = "" + CurrentFood + " / "+ MaxFood;
        }


        public static void setFood(int currentFood, int maxFood)
        {
            CurrentFood = currentFood;
            MaxFood = maxFood;
        }
    }
}
