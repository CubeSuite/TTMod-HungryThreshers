using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace HungryThreshers.Patches
{
    public class ThresherInstancePatch
    {
        [HarmonyPatch(typeof(ThresherInstance), "UpdateCrafting")]
        [HarmonyPrefix]
        private static void clearOutputsForCrafting(ThresherInstance __instance) {
            Inventory outputInventory = __instance.GetOutputInventory();
            if (!outputInventory.myStacks[0].isEmpty && !outputInventory.myStacks[1].isEmpty) {
                if (outputInventory.myStacks[0].count >= 490 && outputInventory.myStacks[1].count >= 490) {
                    if (HungryThreshersPlugin.PauseIfBothFull.Value) {
                        return;
                    }
                }
            }

            for(int i = 0; i < outputInventory.myStacks.Count(); i++) {
                if (!outputInventory.myStacks[i].isEmpty && outputInventory.myStacks[i].count >= 490) {
                    outputInventory.RemoveResourcesFromSlot(i, 10);
                }
            }
        }
    }
}
