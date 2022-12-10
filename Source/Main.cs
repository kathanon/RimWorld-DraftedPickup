using HarmonyLib;
using Verse;
using UnityEngine;
using RimWorld;

namespace DraftedPickup {
    [StaticConstructorOnStartup]
    public class Main {
        static Main() {
            var harmony = new Harmony(Strings.ID);
            harmony.PatchAll();
        }
    }
}
