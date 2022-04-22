using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using BepInEx.IL2CPP;
using YgomGame.Card;

namespace YGOTranslate
{
    public class Translate
    {
        public static bool isActive = true;

        public Translate(IntPtr ptr)
        {

        }

        [HarmonyPrefix]
        public static bool GetName_Pre(ref string __result, out bool __state, int cardId,bool replaceAlnum=true)
        {
            if (!isActive)
            {
                __state = false;
                return true;
            }
                

            if (Data.FindById(cardId) == null)
            {
                __state = true;
                return true;
            }
            else
            {
                __state = false;
                __result = Data.FindById(cardId).cn;
                return false;
            }            
        }

        [HarmonyPostfix]
        public static void GetName_Post(ref string __result, bool __state, int cardId, bool replaceAlnum = true)
        {
            if (!__state)
                return;
            var setting = Data.FindByName(__result,cardId);
            if (setting != null)
                __result = setting.cn;
            else
                BepInExLoader.log.LogMessage("Card " + __result + " not found! / id = " + cardId.ToString());
        }

        [HarmonyPrefix]
        public static bool GetRubyName_Pre(ref string __result, int cardId, bool replaceAlnum = true)
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
                __result = Data.FindById(cardId).cn;
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
                temp+=Data.FindById(cardId).cnDesc;
                temp=temp.Replace("\\n", "\n");
                __result = temp;
                return false;
            }
        }
    }
}
