using RimWorld;
using System.Text;
using Verse;

namespace SoftWarmBeds
{
    public class StatPart_BedStuff : StatPart
    {
        private float Addend;

        private StatDef additiveStat = null;

        private float Factor;

        private StatDef multiplierStat = null;

        public override string ExplanationPart(StatRequest req)
        {
            Building_Bed bed = req.Thing as Building_Bed;
            if (req.HasThing && bed != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                CompMakeableBed bedComp = req.Thing.TryGetComp<CompMakeableBed>();
                string material = null;
                if (bedComp != null)
                {
                    if (bedComp.Loaded)
                    {
                        ThingDef BedStuff = bedComp.blanketStuff;
                        material = BedStuff.label;
                    }
                    else
                    {
                        material = "NoBeddings".Translate();
                    }
                }
                else if (req.StuffDef != null)
                {
                    material = req.StuffDef.label;
                }
                if (material != null)
                {
                    string number = Addend.ToStringByStyle(parentStat.ToStringStyleUnfinalized, ToStringNumberSense.Absolute);
                    if (additiveStat != null)
                    {
                        stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + material + "): +" + number);
                    }
                    if (multiplierStat != null)
                    {
                        stringBuilder.AppendLine("StatsReport_StuffEffectMultiplier".Translate() + ": x" + Factor.ToStringPercent("F0"));
                    }
                    return stringBuilder.ToString().TrimEndNewlines();
                }
            }
            return null;
        }

        public override void TransformValue(StatRequest req, ref float value)
        {
            if (!req.HasThing || !(req.Thing is Building_Bed bed)) return;
            float addend = (additiveStat != null) ? SelectValue(req, additiveStat) : 0f;
            float factor = (multiplierStat != null) ? SelectValue(req, multiplierStat) : 0f;
            if (multiplierStat != null)
            {
                if (additiveStat != null)
                {
                    value += factor * addend;
                    goto Done;
                };
                value *= factor;
                goto Done;
            }
            value += addend;
            Done:
                Factor = factor;
                Addend = addend;
                return;
        }

        private float SelectValue(StatRequest req, StatDef stat)
        {
            if (stat == null) return 0f;
            CompMakeableBed bedComp = req.Thing.TryGetComp<CompMakeableBed>();
            ThingDef stuff = null;
            if (bedComp == null)
            {
                if (req.StuffDef != null) stuff = req.StuffDef; // no comp (Bedroll)
                else if (stat == additiveStat) return 0f; // no comp, no stuff (SleepingSpot)
            }
            else
            {
                if (bedComp.Loaded) stuff = bedComp.blanketStuff; // comp = stuff from bedding
                else if (stat == additiveStat) return 0f; // unmade bed = additive zero
            }
            bool both = additiveStat != null && multiplierStat != null;
            if (both)
            {
                if (stat == additiveStat) return stuff.GetStatValueAbstract(stat, null); // if additive = get it from bed/bedding stuff
                //return req.Def.GetStatValueAbstract(stat, null); // Changed on 1.1: method transfered to a Def child:
                return req.BuildableDef.GetStatValueAbstract(stat, null); // if multiplier = get it from bed
            }
            if (stat == additiveStat) return stuff.GetStatValueAbstract(stat, null); // just additive = get it from bed/bedding stuff
            //return req.Def.GetStatValueAbstract(stat, null); // Changed on 1.1: method transfered to a Def child:
            return req.BuildableDef.GetStatValueAbstract(stat, null); // just multiplier (just in case) = get from bed
        }
    }
}