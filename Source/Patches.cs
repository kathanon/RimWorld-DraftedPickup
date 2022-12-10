using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace DraftedPickup {
    [HarmonyPatch]
    public static class Patches {
        private static bool menuActive = false;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
        public static void AddHumanlikeOrders_Pre(Pawn pawn) 
            => menuActive = pawn?.Drafted ?? false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
        public static void AddHumanlikeOrders_Post() 
            => menuActive = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PawnUtility), nameof(PawnUtility.CanPickUp))]
        public static void CanPickUp(ref bool __result, Pawn pawn) {
            if (menuActive) {
                __result |= pawn.inventory != null;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PawnUtility), nameof(PawnUtility.GetMaxAllowedToPickUp), typeof(Pawn), typeof(ThingDef))]
        public static void GetMaxAllowedToPickUp_Post(ref int __result) {
            if (menuActive) {
                __result = int.MaxValue;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Pawn_DraftController), nameof(Pawn_DraftController.Drafted), MethodType.Setter)]
        public static void Drafted_Set_Pre(bool __0, bool ___draftedInt, Pawn ___pawn) {
            if (!__0 && ___draftedInt) {
                ___pawn.inventory.UnloadEverything = true;
            }
        }
    }
}
