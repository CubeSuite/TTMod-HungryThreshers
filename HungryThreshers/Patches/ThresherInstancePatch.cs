using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquinoxsModUtils;
using HarmonyLib;
using UnityEngine;

namespace HungryThreshers.Patches
{
    public class ThresherInstancePatch
    {
        [HarmonyPatch(typeof(ThresherInstance), "UpdateCrafting")]
        [HarmonyPrefix]
        private static void clearOutputsForCrafting(ThresherInstance __instance) {
            Inventory inputInventory = __instance.GetInputInventory();
            if (inputInventory.myStacks[0].isEmpty) return;
            
            Inventory outputInventory = __instance.GetOutputInventory();
            SchematicsRecipeData recipe = ModUtils.TryFindThresherRecipe(inputInventory.myStacks[0].id);

            if (isSlotFull(__instance, 0) && isSlotFull(__instance, 1)) {
                if (HungryThreshersPlugin.PauseIfBothFull.Value) {
                    return;
                }
            }

            for (int i = 0; i < outputInventory.myStacks.Count(); i++) {
                if(isSlotFull(__instance, i)) {
                    outputInventory.RemoveResourcesFromSlot(i, recipe.outputQuantities[i]);
                }
            }
        }

        private static bool isSlotFull(ThresherInstance __instance, int index) {
            Inventory inputInventory = __instance.GetInputInventory();
            Inventory outputInventory = __instance.GetOutputInventory();
            SchematicsRecipeData recipe = ModUtils.TryFindThresherRecipe(inputInventory.myStacks[0].id);

            if (outputInventory.myStacks[index].isEmpty) return false;

            int toCraft = recipe.outputQuantities[index];
            int threshold = outputInventory.myStacks[index].maxStack - toCraft;
            return outputInventory.myStacks[index].count >= threshold;
        }
    }
}
