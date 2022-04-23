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
            Version = "1.4";

        public static BepInEx.Logging.ManualLogSource log;

        static ConfigEntry<string> databasePath;
        static ConfigEntry<bool> TMP_fallback_setting;
        static ConfigEntry<string> enableKey;
        static ConfigEntry<string> tmpKey;

        public override void Load()
        {
            log = Log;

            databasePath = Config.Bind("General", "dataPath", "YGOTranslate\\data.csv");
            Data.Setup(databasePath.Value);

            TMP_fallback_setting = Config.Bind("General", "TMP_fallback", true, "This is an copy version of XUnity fallback script, still in test!");

            enableKey = Config.Bind("Key", "switchKey", "F9", "This value is using Unity KeyCode! https://docs.unity3d.com/ScriptReference/KeyCode.html");
            tmpKey = Config.Bind("Key", "tmpKey", "F12", "You need set TMP_fallback_settin to true first");

            BindMethod<Content, Translate>("GetName", "GetName_Pre", "GetName_Post");
            BindPreMethod<Content, Translate>("GetRubyName", "GetRubyName_Pre");
            BindPreMethod<Content, Translate>("GetDesc", "GetDesc_Pre");

            // BindPostMethod<UnityEngine.AssetBundle, Translate>("LoadAssetAsync", "TestInject");

            BindEnableKey();

            if(TMP_fallback_setting.Value)
                BindTMP();

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
                SwitchComponent.activeKey = (BepInEx.IL2CPP.UnityEngine.KeyCode)Enum.Parse(typeof(BepInEx.IL2CPP.UnityEngine.KeyCode), enableKey.Value);

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

        public void BindTMP()
        {
            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<FontHelper>();

                var go = new UnityEngine.GameObject("FontHelper");
                go.AddComponent<FontHelper>();
                go.AddComponent<TMPro.TextMeshProUGUI>();
                Object.DontDestroyOnLoad(go);

                FontHelper.tmpKey = (BepInEx.IL2CPP.UnityEngine.KeyCode)Enum.Parse(typeof(BepInEx.IL2CPP.UnityEngine.KeyCode), tmpKey.Value);
            }
            catch
            {
                log.LogError("Failed to register IL2CPP - FontHelper");
            }

            BindPostMethod<CardPictureCreator, FontHelper>("Update", "Update");
        }

        public void BindDebugHelper()
        {
            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<DebugHelper>();

                var go = new UnityEngine.GameObject("DebugHelper");
                go.AddComponent<DebugHelper>();
                FontHelper.instance= go;
                Object.DontDestroyOnLoad(go);
            }
            catch
            {
                log.LogError("Failed to register IL2CPP - DebugHelper");
            }

            // 目前Debug隨便綁定一個monobehaviour物件
            BindPostMethod<CardPictureCreator, DebugHelper>("Update", "Update");
        }

#region Method_Binding_Method
        public void BindMethod<T1,T2>(string origin,string prefix,string postfix)
        {
            try
            {
                var harmony = new Harmony("jackoo.ygotranslate.il2cpp");
                var originMethod = AccessTools.Method(typeof(T1), origin);
                var prefixMethod = AccessTools.Method(typeof(T2), prefix);
                var postfixMethod = AccessTools.Method(typeof(T2), postfix);

                harmony.Patch(originMethod, prefix: new HarmonyMethod(prefixMethod), postfix: new HarmonyMethod(postfixMethod));
            }
            catch
            {
                log.LogError("Method " + origin + " cannot bind!");
            }
        }
        public void BindPreMethod<T1, T2>(string origin, string prefix)
        {
            try
            {
                var harmony = new Harmony("jackoo.ygotranslate.il2cpp");
                var originMethod = AccessTools.Method(typeof(T1), origin);
                var prefixMethod = AccessTools.Method(typeof(T2), prefix);

                harmony.Patch(originMethod, prefix: new HarmonyMethod(prefixMethod));
            }
            catch
            {
                log.LogError("Method Prefix " + origin + " cannot bind!");
            }
        }
        public void BindPostMethod<T1, T2>(string origin,string postfix)
        {
            try
            {
                var harmony = new Harmony("jackoo.ygotranslate.il2cpp");
                var originMethod = AccessTools.Method(typeof(T1), origin);
                var postfixMethod = AccessTools.Method(typeof(T2), postfix);

                harmony.Patch(originMethod, postfix: new HarmonyMethod(postfixMethod));
            }
            catch
            {
                log.LogError("Method Postfix " + origin + " cannot bind!");
            }
        }


        #endregion
    }
}
