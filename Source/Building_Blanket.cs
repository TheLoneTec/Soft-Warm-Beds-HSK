using UnityEngine;
using Verse;

namespace SoftWarmBeds
{
    public class Building_Blanket : ThingWithComps
    {
        public Color colorTwo;

        private Color linenDelta = new Color(1f, 1f, 1f);

        public float deltaFactor
        {
            get
            {
                return Mathf.Round(SoftWarmBedsSettings.colorWash * 10) / 10;
            }
        }

        public override Color DrawColorTwo
        {
            get
            {
                return Color.Lerp(colorTwo, linenDelta, deltaFactor);
            }
        }
    }
}