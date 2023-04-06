using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using UnityEngine;
using Verse;

namespace SoftWarmBeds
{
    //Instruction to draw pawn body when on a bed that's unmade (RW 1.3 only)
    [HarmonyPatch(typeof(PawnRenderer), "GetBodyPos", new Type[] { typeof(Vector3), typeof(bool) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Out })]
    public static class GetBodyPos_Patch
    {
        public static void Postfix(Pawn ___pawn, ref bool showBody)
        {
            if (showBody) return;
            Building_Bed softWarmBed = ___pawn.CurrentBed();
            CompMakeableBed bedComp = softWarmBed.TryGetComp<CompMakeableBed>();
            showBody = ___pawn.RaceProps.Humanlike && bedComp != null && !bedComp.Loaded && !bedComp.Props.blanketInvisible;
        }
    }
}
