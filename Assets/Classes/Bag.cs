using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Classes
{
    public static class Bag
    {
        public static int GearsCount;
        public static int BeamsCount;
        public static int ChipsCount;
        private static TextMeshProUGUI GearsCountText =
            GameObject.Find("GearsCountText").GetComponent<TextMeshProUGUI>();

        private static TextMeshProUGUI BeamsCountText =
            GameObject.Find("BeamsCountText").GetComponent<TextMeshProUGUI>();

        private static TextMeshProUGUI ChipsCountText =
            GameObject.Find("ChipsCountText").GetComponent<TextMeshProUGUI>();

        public static void SpendResources(int gearsCount, int beamsCount, int chipsCount)
        {
            GearsCount -= gearsCount;
            BeamsCount -= beamsCount;
            ChipsCount -= chipsCount;
            Update();
        }

        public static void GetResources(int gearsCount, int beamsCount, int chipsCount)
        {
            GearsCount += gearsCount;
            BeamsCount += beamsCount;
            ChipsCount += chipsCount;
            Update();
        }

        static void Update()
        {
            GearsCountText.text = GearsCount.ToString();
            BeamsCountText.text = BeamsCount.ToString();
            ChipsCountText.text = ChipsCount.ToString();
        }
    }
}