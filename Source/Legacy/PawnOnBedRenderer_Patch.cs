using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using UnityEngine;
using Verse;

namespace SoftWarmBeds
{
    //Replaced by GetBodyPos_Patch on RW 1.3
    //Instruction to draw pawn body when on a bed that's unmade ()
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnAt", new Type[] { typeof(Vector3) })]
    public static class PawnOnBedRenderer_Patch
    {
        public static void Postfix(PawnRenderer __instance, Pawn ___pawn, Vector3 drawLoc)
        {
            Building_Bed softWarmBed = ___pawn.CurrentBed();
            CompMakeableBed bedComp = softWarmBed.TryGetComp<CompMakeableBed>();
            if (___pawn.RaceProps.Humanlike && bedComp != null)
            {
                if (!bedComp.Loaded)
                {
                    // Thanks to @Zamu & @Mehni!
                    var rotDrawMode = (RotDrawMode)AccessTools.Property(typeof(PawnRenderer), "CurRotDrawMode").GetGetMethod(true).Invoke(__instance, new object[0]);
                    MethodInfo layingFacing = AccessTools.Method(type: typeof(PawnRenderer), name: "LayingFacing");
                    Rot4 rot = (Rot4)layingFacing.Invoke(__instance, new object[0]);
                    float angle;
                    Vector3 rootLoc;
                    Rot4 rotation = softWarmBed.Rotation;
                    rotation.AsInt += 2;
                    angle = rotation.AsAngle;
                    AltitudeLayer altLayer = (AltitudeLayer)Mathf.Max((int)softWarmBed.def.altitudeLayer, 15);
                    Vector3 vector2 = ___pawn.Position.ToVector3ShiftedWithAltitude(altLayer);
                    Vector3 vector3 = vector2;
                    vector3.y += 0.02734375f;
                    float d = -__instance.BaseHeadOffsetAt(Rot4.South).z;
                    Vector3 a = rotation.FacingCell.ToVector3();
                    rootLoc = vector2 + a * d;
                    rootLoc.y += 0.0078125f;
                    MethodInfo renderPawnInternal = AccessTools.Method(type: typeof(PawnRenderer), name: "RenderPawnInternal", parameters: new[] { typeof(Vector3), typeof(float), typeof(bool), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(bool), typeof(bool), typeof(bool) });
                    renderPawnInternal.Invoke(__instance, new object[] { rootLoc, angle, true, rot, rot, rotDrawMode, false, false, false });
                }
            }
        }
    }
}
