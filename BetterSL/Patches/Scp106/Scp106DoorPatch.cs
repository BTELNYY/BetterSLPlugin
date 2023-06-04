using HarmonyLib;
using PlayerRoles.PlayableScps.Scp106;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI.Core;
using PlayerRoles.FirstPersonControl;
using Interactables.Interobjects.DoorUtils;

namespace BetterSL.Patches.Scp106
{
    //Broken, do not enable, I will if needed -btelnyy
    //[HarmonyPatch(typeof(Scp106MovementModule), "ProcessCollider")]
    public class Scp106DoorPatch
    {
        public static bool InDoor = false;
        public static void Prefix(Scp106MovementModule __instance, Collider col)
        {
            bool flag = false;
            if (col.gameObject.layer == 14 && col is BoxCollider)
            {
                col.isTrigger = true;
                flag = true;
            }

            Transform parent = col.transform;
            DoorVariant component;
            while (!parent.TryGetComponent<DoorVariant>(out component))
            {
                parent = parent.parent;
                if (!(parent != null))
                {
                    if (flag)
                    {
                        InDoor = true;
                    }
                    InDoor = false;
                    return;
                }
            }

            IScp106PassableDoor scp106PassableDoor = component as IScp106PassableDoor;
            if (scp106PassableDoor != null && scp106PassableDoor.IsScp106Passable)
            {
                col.isTrigger = true;
                InDoor = true;
                return;
            }
            InDoor = false;
        }
    }
}
