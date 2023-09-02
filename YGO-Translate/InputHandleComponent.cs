using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
using BeUnityEngine = BepInEx.Unity.IL2CPP.UnityEngine;
using Input = BepInEx.Unity.IL2CPP.UnityEngine.Input;
using HarmonyLib;

namespace YGOTranslate
{
    public class InputHandleComponent : MonoBehaviour
    {
        public static BeUnityEngine.KeyCode activeKey;
        
        public static BeUnityEngine.KeyCode copyKey;

        public InputHandleComponent(IntPtr ptr) : base(ptr)
        {
            BepInExLoader.log.LogMessage("Switch Component Constructor!");
        }

        [HarmonyPostfix]
        public static void Update()
        {
            if (Input.GetKeyInt(activeKey) && Event.current.type == EventType.KeyDown && Event.current.control)
            {
                Translate.isActive = !Translate.isActive;
                BepInExLoader.log.LogMessage("YGO-Translate is " + (Translate.isActive ? "on" : "off"));
                Event.current.Use();
            }

            if (!BepInExLoader.copyEnable.Value)
                return;
            if (Input.GetKeyInt(copyKey) && Event.current.type == EventType.KeyDown && Event.current.control && Event.current.shift)
            {
                GUIUtility.systemCopyBuffer = Translate.lastSelectCard;
                BepInExLoader.log.LogMessage("[ "+ Translate.lastSelectCard + " ] has copied!");
                Event.current.Use();
            }
        }
    }
}
