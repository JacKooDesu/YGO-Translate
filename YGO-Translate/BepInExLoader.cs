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
            Version = "1.0";

        public static BepInEx.Logging.ManualLogSource log;

        ConfigEntry<string> databasePath;

        public override void Load()
        {
            log = Log;

            databasePath = Config.Bind("General", "dataPath", "YGOTranslate\\data.csv");
            Data.Setup(databasePath.Value);

            try
            {
                var harmony = new Harmony("jackoo.helloworld.il2cpp");

                var originName = AccessTools.Method(typeof(Content), "GetName");
                var preName = AccessTools.Method(typeof(Translate), "GetName_Pre");
                var postName = AccessTools.Method(typeof(Translate), "GetName_Post");

                var originDesc = AccessTools.Method(typeof(Content), "GetDesc");
                var preDesc = AccessTools.Method(typeof(Translate), "GetDesc_Pre");

                harmony.Patch(originName, prefix: new HarmonyMethod(preName),postfix: new HarmonyMethod(postName));
                harmony.Patch(originDesc, prefix: new HarmonyMethod(preDesc));
            }
            catch
            {
                log.LogError("Broken");
            }
        }
    }
}
