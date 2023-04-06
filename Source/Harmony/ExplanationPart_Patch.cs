using HarmonyLib;
using System.Text;
using RimWorld;
using Verse;

namespace SoftWarmBeds
{
    //Adjusts the info report on comfortable temperatures - explanation part
    [HarmonyPatch(typeof(StatPart_ApparelStatOffset), "ExplanationPart")]
    public class ExplanationPart_Patch
    {
        public static string Postfix(string original, StatRequest req, StatDef ___apparelStat)
        {
            if (req.HasThing && req.Thing != null)
            {
                Pawn pawn = req.Thing as Pawn;
                if (pawn != null && pawn.InBed())
                {
                    StringBuilder alteredText = new StringBuilder();
                    bool subtract = ___apparelStat == StatDefOf.Insulation_Cold;
                    StatDef modifier = subtract ? BedInsulationCold.Bed_Insulation_Cold : BedInsulationHeat.Bed_Insulation_Heat;
                    float bedStatValue = pawn.CurrentBed().GetStatValue(modifier, true);
                    float bedOffset = subtract ? bedStatValue * -1 : bedStatValue;
                    string signal = subtract ? null : "+";
                    alteredText.AppendLine("StatsReport_BedInsulation".Translate() + ": " + signal + bedOffset.ToStringTemperature());
                    return alteredText.ToString();
                }
            }
            return original;
        }
    }
}
