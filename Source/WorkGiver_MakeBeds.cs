using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace SoftWarmBeds
{
    public class WorkGiver_MakeBeds : WorkGiver_Scanner
    {
        public virtual JobDef JobStandard
        {
            get
            {
                return SoftWarmBeds_JobDefOf.MakeBed;
            }
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public virtual bool CanMakeBedThing(Thing t)
        {
            return (t is Building_Bed);
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return CanMakeBedThing(t) && BedMakingWorkGiverUtility.CanMakeBed(pawn, (Building_Bed)t, forced);
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return BedMakingWorkGiverUtility.BedMakingJob(pawn, (Building_Bed)t, forced, JobStandard);
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (Building_Bed bed in pawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
            {
                if (bed.TryGetComp<CompMakeableBed>() != null)
                {
                    yield return bed as Thing;
                }
            }
            yield break;
        }
    }
}