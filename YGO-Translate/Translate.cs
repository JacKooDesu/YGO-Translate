using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using BepInEx.IL2CPP;
using YgomGame.Card;
using System.Data.SQLite;
using Object = UnityEngine.Object;

namespace YGOTranslate
{
    public class Translate
    {
        public static bool isActive = true;

        public static string lastSelectCard = "";

        public Translate(IntPtr ptr) { }

        [HarmonyPrefix]
        public static bool GetName_Pre(ref string __result, out bool __state, int cardId, bool replaceAlnum = true)
        {
            if (!isActive)
            {
                __state = false;
                return true;
            }

            __state = FindCardName(ref __result, cardId);
            return __state;
        }

        [HarmonyPostfix]
        public static void GetName_Post(ref string __result, bool __state, int cardId, bool replaceAlnum = true)
        {
            if (__result == String.Empty)
                return;

            lastSelectCard = __result;

            if (!__state)
                return;

            Data.LogInvalid(__result, cardId);

            lastSelectCard = __result;
        }

        [HarmonyPrefix]
        public static bool GetRubyName_Pre(YgomGame.Card.Content __instance, ref string __result, int cardId, bool replaceAlnum = true)
        {
            if (!isActive)
            {
                return true;
            }

            return FindCardName(ref __result, cardId);
        }

        static bool FindCardName(ref string result, int cardId)
        {
            if (Data.FindById(cardId) == null)
            {
                return true;
            }
            else
            {
                result = Data.FindById(cardId).cn;
                return false;
            }
        }

        [HarmonyPrefix]
        public static bool GetDesc_Pre(ref string __result, int cardId, bool replaceAlnum = true)
        {
            if (!isActive)
            {
                return true;
            }

            if (Data.FindById(cardId) == null)
            {
                return true;
            }
            else
            {
                var temp = "";
                temp += Data.FindById(cardId).cnDesc;
                temp = temp.Replace("\\n", "\n");
                __result = temp;
                return false;
            }
        }
    }
}
