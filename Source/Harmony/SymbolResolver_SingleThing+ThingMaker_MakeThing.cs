using HarmonyLib;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace SoftWarmBeds
{
    //Generates beddings for beds on generated bases.
    [HarmonyPatch(typeof(SymbolResolver_SingleThing), "Resolve")]
    public class SymbolResolver_SingleThing_Resolve
    {
        public static Faction cachedFaction;
        public static void Prefix(ResolveParams rp)
        {
            cachedFaction = rp.faction ?? null;
            ThingMaker_MakeThing.Act = cachedFaction != null;
        }
    }

    [HarmonyPatch(typeof(ThingMaker), "MakeThing")]
    public class ThingMaker_MakeThing
    {
        public static bool Act;
        public static void Postfix(ThingDef def, ref Thing __result)
        {
            if (def.IsBed && Act)
            {
                var comp = __result.TryGetComp<CompMakeableBed>();
                if (comp != null)
                {
                    ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(comp.Props.beddingDef, SymbolResolver_SingleThing_Resolve.cachedFaction, null);
                    if (stuff != null) comp.LoadBedding(stuff);
                }
            }
            Act = false;
        }
    }
}

