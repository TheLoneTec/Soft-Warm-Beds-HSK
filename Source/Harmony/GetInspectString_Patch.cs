using HarmonyLib;
using System.Text;
using RimWorld;
using Verse;

namespace SoftWarmBeds
{
    //Adds info on used bedding material to the inspector pane
    [HarmonyPatch(typeof(Building_Bed), "GetInspectString")]
    public class GetInspectString_Patch
    {
        public static void Postfix(object __instance, ref string __result)
        {
            if (__instance is Building_Bed bed)
            {
                CompMakeableBed bedComp = bed.TryGetComp<CompMakeableBed>();
                if (bedComp != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine();
                    if (bedComp.Loaded)
                    {
                        stringBuilder.AppendLine("BedMade".Translate(bedComp.blanketStuff.LabelCap, bedComp.blanketStuff));
                    }
                    else
                    {
                        stringBuilder.AppendLine("BedNotMade".Translate());
                    }
                    __result += stringBuilder.ToString().TrimEndNewlines();
                }
            }
        }
    }
}
