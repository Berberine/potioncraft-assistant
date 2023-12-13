using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using PotionCraft.ObjectBased.Haggle;
using UnityEngine;

namespace berberine
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("Potion Craft.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "berberine.PotionCraftAssistant.AutoHaggle";
        public const string PLUGIN_NAME = "Auto Haggle";
        public const string PLUGIN_VERSION = "1.1.0";

        void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Plugin), PLUGIN_GUID);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Pointer), "Update")]
        static void Patch(Pointer __instance)
        {
            HaggleWindow window = HaggleWindow.Instance;
            List<BonusInfo> bonuses = window.currentBonuses;
            if (
                !window.isPaused
                && __instance.state == Pointer.State.Moving
                && bonuses != null
                && bonuses.Count > 2
            )
            {
                bonuses = bonuses.GetRange(1, bonuses.Count - 2);
                foreach (BonusInfo info in bonuses)
                {
                    if (
                        Mathf.Abs(info.haggleBonus.Position - __instance.Position)
                        <= info.size / 2f
                    )
                    {
                        __instance.CheckBonuses();
                        break;
                    }
                }
            }
        }
    }
}
