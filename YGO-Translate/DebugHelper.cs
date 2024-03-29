﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
using Input = BepInEx.Unity.IL2CPP.UnityEngine.Input;
using HarmonyLib;

namespace YGOTranslate
{
    public class DebugHelper : MonoBehaviour
    {
        public DebugHelper(IntPtr ptr) : base(ptr)
        {
            BepInExLoader.log.LogMessage("Debug Helper Constructor!");
        }

        [HarmonyPostfix]
        public static void Update()
        {
            if (Input.GetKeyInt(BepInEx.Unity.IL2CPP.UnityEngine.KeyCode.F7) && Event.current.type == EventType.KeyDown && Event.current.control)
            {
                BepInExLoader.log.LogMessage("Debug Key Pressed!");   
                Event.current.Use();
            }
        }
    }
}
