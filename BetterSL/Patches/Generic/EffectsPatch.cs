using CustomPlayerEffects;
using HarmonyLib;
using System;
using UnityEngine;
using PluginAPI.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSL.StatusEffects;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BetterSL.Patches.Generic
{
    [HarmonyPatch(typeof(PlayerEffectsController), "Awake")]
    public class EffectsPatch
    {
        public static List<StatusEffectBase> CustomEffects = new List<StatusEffectBase>()
        {
            new Radiation(),
            new BetterBleeding(),
            new Fear(),
            new BetterScp207()
        };

        public static void Prefix(PlayerEffectsController __instance)
        {
            try
            {
                var effectsObjectField = AccessTools.Field(typeof(PlayerEffectsController), "effectsGameObject");
                GameObject effectsObject = (GameObject)effectsObjectField.GetValue(__instance);
                foreach(var effect in CustomEffects)
                {
                    Type effectType = effect.GetType();
                    foreach(Transform t in effectsObject.transform)
                    {
                        if (t.name == effectType.Name)
                        {
                            continue;
                        }
                    }
                    GameObject newObj = GameObject.Instantiate(new GameObject(), Vector3.zero, Quaternion.Euler(0, 0, 0), effectsObject.transform);
                    newObj.name = effectType.Name;
                    if(newObj.GetComponent(effectType) == null)
                    {
                        newObj.AddComponent(effectType);
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}