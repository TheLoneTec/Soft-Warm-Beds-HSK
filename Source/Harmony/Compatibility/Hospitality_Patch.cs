using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using System.Reflection;
using Verse;

namespace SoftWarmBeds
{
    //Interface to Hospitality for seamless guest bed switching
    [StaticConstructorOnStartup]
    public static class Hospitality_Patch
    {
        static Hospitality_Patch()
        {
            if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageIdPlayerFacing.StartsWith("Orion.Hospitality")))
            {
                var harmonyInstance = new Harmony("JPT_SoftWarmBeds.Hospitality");

                Log.Message("[SoftWarmBeds] Hospitality detected! Adapting...");

                harmonyInstance.Patch(original: AccessTools.Method("Hospitality.Building_GuestBed:Swap", new[] { typeof(Building_Bed) }),
                    prefix: new HarmonyMethod(typeof(Hospitality_Patch), nameof(SwapPatch)), postfix: null, transpiler: null);

                harmonyInstance.Patch(original: AccessTools.Method("Hospitality.Building_GuestBed:GetInspectString"),
                    prefix: null, postfix: new HarmonyMethod(typeof(GetInspectString_Patch), nameof(GetInspectString_Patch.Postfix)), transpiler: null);
            }
        }

        public static bool SwapPatch(object __instance, Building_Bed bed)
        {
            CompMakeableBed bedComp = bed.TryGetComp<CompMakeableBed>();
            if (bedComp != null)
            {
                bedComp.NotTheBlanket = false;
                Swap(__instance, bed, bedComp.settings, bedComp);
                return false;
            }
            return true;
        }

        public static void Swap(object __instance, Building_Bed bed, StorageSettings settings, CompMakeableBed compMakeable)
        {
            //reflection info
            Type guestBed = AccessTools.TypeByName("Hospitality.Building_GuestBed");
            MethodInfo makeBedinfo = AccessTools.Method(guestBed, "MakeBed", new[] { typeof(Building_Bed), typeof(string) });
            //
            Building_Bed newBed;
            string newName;
            if (bed.GetType() == guestBed)
            {
                newName = bed.def.defName.Split(new string[] { "Guest" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            else
            {
                newName = bed.def.defName + "Guest";
            }
            // Thanks again to @Zamu for figuring out it was actually very simple!
            newBed = (Building_Bed)makeBedinfo.Invoke(__instance, new object[] { bed, newName });
            newBed.SetFactionDirect(bed.Faction);
            var spawnedBed = (Building_Bed)GenSpawn.Spawn(newBed, bed.Position, bed.Map, bed.Rotation);
            spawnedBed.HitPoints = bed.HitPoints;
            spawnedBed.ForPrisoners = bed.ForPrisoners;
            // This should be on Hospitality, Orion! 
            spawnedBed.AllComps.Clear();
            spawnedBed.AllComps.AddRange(bed.AllComps);
            foreach (ThingComp comp in spawnedBed.AllComps)
            {
                comp.parent = spawnedBed;
            }
            compMakeable.parent.Notify_ColorChanged();
            Find.Selector.Select(spawnedBed, false, true);
        }
    }
}
