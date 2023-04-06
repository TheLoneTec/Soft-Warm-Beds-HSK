using HarmonyLib;
using RimWorld;
using Verse;

namespace SoftWarmBeds
{
	//Preventing Beddings from being targeted by apparel recipes regardless of not being either tainted or clean. 2/2
	//Solution by NanoCE
	[HarmonyPatch(typeof(SpecialThingFilterWorker_DeadmansApparel), "Matches")]
	public static class Patch_SpecialThingFilterWorker_DeadmansApparel
	{
		public static void Postfix(ref bool __result, ref Thing t)
		{
			if (t != null && t is Bedding)
			{
				__result = false;
			}
		}
	}
}
