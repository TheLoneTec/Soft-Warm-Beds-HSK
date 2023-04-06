using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace SoftWarmBeds
{
    //Adjusts the info report on comfortable temperatures - final value
    [HarmonyPatch(typeof(StatsReportUtility), "StatsToDraw", new Type[] { typeof(Thing) })]
    public class StatsToDraw_Patch
    {
        public static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> original, Thing thing)
        {
            foreach (StatDrawEntry entry in original)
            {
                Pawn pawn = thing as Pawn;
                StatDef statDef = entry.stat;
                if (pawn != null && pawn.InBed() && (statDef == StatDefOf.ComfyTemperatureMin || statDef == StatDefOf.ComfyTemperatureMax))
                {
                    if (!statDef.Worker.IsDisabledFor(thing))
                    {
                        //sneak transform:
                        float statValue = statDef.Worker.GetValueUnfinalized(StatRequest.For(thing), true);
                        bool subtract = statDef == StatDefOf.ComfyTemperatureMin;
                        StatDef modifier = subtract ? BedInsulationCold.Bed_Insulation_Cold : BedInsulationHeat.Bed_Insulation_Heat;
                        float bedStatValue = pawn.CurrentBed().GetStatValue(modifier, true);
                        float bedOffset = subtract ? bedStatValue * -1 : bedStatValue;
                        statValue += bedOffset;
                        //
                        if (statDef.showOnDefaultValue || statValue != statDef.defaultBaseValue)
                        {
                            yield return new StatDrawEntry(statDef.category, statDef, statValue, StatRequest.For(thing), ToStringNumberSense.Undefined, null, false);
                        }
                    }
                    else
                    {
                        yield return new StatDrawEntry(statDef.category, statDef);
                    }
                }
                else
                {
                    yield return entry;
                }
            }
        }
    }
}
