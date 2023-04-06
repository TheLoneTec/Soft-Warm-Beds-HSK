using RimWorld;
using Verse;

namespace SoftWarmBeds
{
    public class CompProperties_MakeableBed : CompProperties_Flickable
    {
        public CompProperties_MakeableBed()
        {
            compClass = typeof(CompMakeableBed);
            commandLabelKey = "CommandUnmakeBed";
            commandDescKey = "CommandUnmakeBedDesc";
        }

        public ThingDef blanketDef;

        public ThingDef beddingDef;

        public bool blanketInvisible = false;
    }
}