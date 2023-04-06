using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace SoftWarmBeds
{
    //Makes the bed stats cover for a possible lack of apparel when calculating comfortable temperature range for pawns in bed
    [HarmonyPatch(typeof(GenTemperature), "ComfortableTemperatureRange", new Type[] { typeof(Pawn) })]
    public class ComfortableTemperatureRange_Patch
    {
        public static void Postfix(Pawn p, ref FloatRange __result)
        {
            if (p.InBed())
            {
                Building_Bed bed = p.CurrentBed();
                float InsulationCold = bed.GetStatValue(BedInsulationCold.Bed_Insulation_Cold, true);
                float InsulationHeat = bed.GetStatValue(BedInsulationHeat.Bed_Insulation_Heat, true);
                if (InsulationCold != 0 || InsulationHeat != 0)
                {
                    ThingDef raceDef = p.kindDef.race;
                    FloatRange altResult = new FloatRange(raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null), raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null));
                    altResult.min -= InsulationCold;
                    altResult.max += InsulationHeat;
                    if (__result.min > altResult.min) __result.min = altResult.min;
                    if (__result.max < altResult.max) __result.max = altResult.max;
                    //Log.Message(bed+" insulation cold is "+bed.GetStatValue(BedInsulationCold.Bed_Insulation_Cold, true));
                    //Log.Message("comfortable range modified for " + p + " by bed" + bed + ": " + __result.ToString());
                }
            }
        }
    }
}
