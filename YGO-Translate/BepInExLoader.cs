using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using Object = UnityEngine.Object;
using UnhollowerRuntimeLib;
using YgomGame.Card;
using BepInEx.Configuration;

namespace YGOTranslate
{
    [BepInPlugin(Guid,ModName,Version)]
    public class BepInExLoader : BepInEx.IL2CPP.BasePlugin
    {
        public const string
            ModName = "YGOTranslate",
            Guid = "com.jackoo.YGOTranslate",
            Version = "1.3";

        public static BepInEx.Logging.ManualLogSource log;

        ConfigEntry<string> databasePath;
        ConfigEntry<string> enableKey;

        public override void Load()
        {
            log = Log;

            databasePath = Config.Bind("General", "dataPath", "YGOTranslate\\data.csv");
            Data.Setup(databasePath.Value);

            enableKey = Config.Bind("Key", "switchKey", "F9", "This value is using Unity KeyCode! https://docs.unity3d.com/ScriptReference/KeyCode.html");

            try
            {
                var harmony = new Harmony("jackoo.helloworld.il2cpp");

                var originName = AccessTools.Method(typeof(Content), "GetName");
                var preName = AccessTools.Method(typeof(Translate), "GetName_Pre");
                var postName = AccessTools.Method(typeof(Translate), "GetName_Post");
                
                var originRubyName = AccessTools.Method(typeof(Content), "GetRubyName");
                var preRubyName = AccessTools.Method(typeof(Translate), "GetRubyName_Pre");

                var originDesc = AccessTools.Method(typeof(Content), "GetDesc");
                var preDesc = AccessTools.Method(typeof(Translate), "GetDesc_Pre");

                harmony.Patch(originName, prefix: new HarmonyMethod(preName),postfix: new HarmonyMethod(postName));
                harmony.Patch(originRubyName, prefix: new HarmonyMethod(preRubyName));
                harmony.Patch(originDesc, prefix: new HarmonyMethod(preDesc));                
            }
            catch
            {
                log.LogError("Broken");
            }

            BindEnableKey();

            // FOR DEBUG ONLY !!
            // BindDebugHelper();
        }

        public void BindEnableKey()
        {
            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<SwitchComponent>();

                var go = new UnityEngine.GameObject("SwitchComponent");
                go.AddComponent<SwitchComponent>();
                Object.DontDestroyOnLoad(go);
            }
            catch
            {
                log.LogError("Failed to register IL2CPP - DebugHelper");
            }

            try
            {
                SwitchComponent.key = (BepInEx.IL2CPP.UnityEngine.KeyCode)Enum.Parse(typeof(BepInEx.IL2CPP.UnityEngine.KeyCode), enableKey.Value);

                var harmony = new Harmony("jackoo.helloworld.il2cpp");

                // 目前Debug隨便綁定一個monobehaviour物件
                var bindUpdate = AccessTools.Method(typeof(CardPictureCreator), "Update");
                var postUpdate = AccessTools.Method(typeof(SwitchComponent), "Update");
                harmony.Patch(bindUpdate, postfix: new HarmonyMethod(postUpdate));
            }
            catch
            {
                log.LogError("ENABLE KEY BIND FAILED!");
            }
        }

        public void BindDebugHelper()
        {
            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<DebugHelper>();

                var go = new UnityEngine.GameObject("DebugHelper");
                go.AddComponent<DebugHelper>();
                Object.DontDestroyOnLoad(go);
            }
            catch
            {
                log.LogError("Failed to register IL2CPP - DebugHelper");
            }

            try
            {
                var harmony = new Harmony("jackoo.helloworld.il2cpp");

                // 目前Debug隨便綁定一個monobehaviour物件
                var bindUpdate = AccessTools.Method(typeof(CardPictureCreator), "Update");
                var postUpdate = AccessTools.Method(typeof(DebugHelper), "Update");
                harmony.Patch(bindUpdate, postfix: new HarmonyMethod(postUpdate));
            }
            catch
            {
                log.LogError("DEBUG KEY BIND FAILED!");
            }
        }
    }
}
