using RimWorld;
using Verse;

namespace SoftWarmBeds
{
    public class ITab_Bedding : ITab_Storage
    {
        public ITab_Bedding()
        {
            labelKey = "TabBedding";
        }

        protected override IStoreSettingsParent SelStoreSettingsParent
        {
            get
            {
                Thing thing = base.SelObject as Thing;
                CompMakeableBed comp = thing.TryGetComp<CompMakeableBed>();
                if (comp as IStoreSettingsParent != null)
                {
                    return comp as IStoreSettingsParent;
                }
                return null;
            }
        }

        protected override bool IsPrioritySettingVisible
        {
            get
            {
                return false;
            }
        }
    }
}