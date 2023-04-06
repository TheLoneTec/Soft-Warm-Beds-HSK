using HarmonyLib;
using RimWorld;
using Verse;

namespace SoftWarmBeds
{
    //Replaced by CompMakeableBed.PostDraw on RW 1.3
    //Command to draw the blanket ()
    [HarmonyPatch(typeof(Building), "Draw")]
    public class Draw_Patch
    {
        public static void Postfix(object __instance)
        {
            if (__instance is Building_Bed bed)
            {
                CompMakeableBed bedComp = bed.TryGetComp<CompMakeableBed>();
                if (bedComp != null && bedComp.loaded)
                {
                    bedComp.DrawBed();
                }
            }
        }
    }
}
