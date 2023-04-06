using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace SoftWarmBeds
{
    public static class BedMakingWorkGiverUtility
    {
        public static bool CanMakeBed(Pawn pawn, Building_Bed t, bool forced = false)
        {
            CompMakeableBed CompMakeableBed = t.TryGetComp<CompMakeableBed>();
            if (CompMakeableBed == null || CompMakeableBed.Loaded)
            {
                return false;
            }
            if (!t.IsForbidden(pawn))
            {
                LocalTargetInfo target = t;
                if (pawn.CanReserve(target, 1, -1, null, forced))
                {
                    if (t.Faction != pawn.Faction)
                    {
                        return false;
                    }
                    if (FindBestBedding(pawn, t) == null)
                    {
                        ThingFilter beddingFilter = new ThingFilter();
                        beddingFilter.SetAllow(t.TryGetComp<CompMakeableBed>().allowedBedding, true);
                        JobFailReason.Is("NoSuitableBedding".Translate(beddingFilter.Summary), null);
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public static Job BedMakingJob(Pawn pawn, Building_Bed t, bool forced = false, JobDef customJob = null)
        {
            Thing t2 = FindBestBedding(pawn, t);
            return new Job(customJob ?? SoftWarmBeds_JobDefOf.MakeBed, t, t2);
        }

        private static Thing FindBestBedding(Pawn pawn, Building_Bed bed)
        {
            //Log.Message(pawn + " is looking for a bedding type " + bed.TryGetComp<CompMakeableBed>().blanketDef + " for " + bed);
            ThingFilter beddingFilter = new ThingFilter();
            beddingFilter.SetAllow(bed.TryGetComp<CompMakeableBed>().allowedBedding, true);
            ThingFilter stuffFilter = new ThingFilter();
            stuffFilter = bed.TryGetComp<CompMakeableBed>().settings.filter;
            Predicate<Thing> predicate = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false) && beddingFilter.Allows(x) && stuffFilter.Allows(x.Stuff);
            IntVec3 position = pawn.Position;
            Map map = pawn.Map;
            ThingRequest bestThingRequest = beddingFilter.BestThingRequest;
            PathEndMode peMode = PathEndMode.ClosestTouch;
            TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
            Predicate<Thing> validator = predicate;
            return GenClosest.ClosestThingReachable(position, map, bestThingRequest, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
        }
    }
}