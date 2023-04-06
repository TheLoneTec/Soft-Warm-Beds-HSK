using HarmonyLib;
using RimWorld;
using Verse;

namespace SoftWarmBeds
{
    //Preventing people without beds from using the default BedRestEffectiveness value (80%). Switching to the minimun value instead. 
    [HarmonyPatch(typeof(Need_Rest), "TickResting")]
    public class Need_Rest_Patch
    {
        public static bool Prefix(float restEffectiveness, Pawn ___pawn)
        {
            if (___pawn.RaceProps.Humanlike && ___pawn.CurrentBed() == null && ___pawn.Faction != null && ___pawn.Faction.IsPlayer && restEffectiveness == StatDefOf.BedRestEffectiveness.valueIfMissing)
            {
                ___pawn.needs.rest.TickResting(StatDefOf.BedRestEffectiveness.minValue);
                return false;
            }
            return true;
        }
    }
}
