using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace SoftWarmBeds
{
    //Adjusts the info report on comfortable temperatures - InfoCard
    [HarmonyPatch(typeof(StatPart_ApparelStatOffset), "GetInfoCardHyperlinks")]
    public class GetInfoCardHyperlinks_Patch
    {
        public static IEnumerable<Dialog_InfoCard.Hyperlink> Postfix(IEnumerable<Dialog_InfoCard.Hyperlink> original, StatRequest req, StatDef ___apparelStat)
        {
            if (req.HasThing && req.Thing != null)
            {
                Pawn pawn = req.Thing as Pawn;
                if (pawn != null && pawn.InBed())
                {
                    yield return new Dialog_InfoCard.Hyperlink(pawn.CurrentBed(), -1);
                }
            }
            yield break;
        }
    }
}
