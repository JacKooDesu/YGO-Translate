using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using BepInEx.IL2CPP;
using YgomGame.Card;
using YgomGame.Deck;
using Object = UnityEngine.Object;
using UnityEngine.Events;
using YgomSystem.UI;

namespace YGOTranslate
{
    public class CardActionMenuModify
    {
        public CardActionMenuModify(IntPtr ptr)
        {

        }

        //[HarmonyPostfix]
        //public static void SetOnClickCallback_Post(UnityAction callback)
        //{
        //    callback = new UnityEngine.Events.UnityAction();
        //    callback+=Test;
        //}

        //[HarmonyPatch(typeof(CardActionMenu),"Open",new Type[]{
        //    typeof(int),
        //    typeof(int),
        //    typeof(int),
        //    typeof(int),
        //    typeof(CardCollectionInfo.Premium),
        //    typeof(bool),
        //    typeof(bool) })]
        //public void Open_Post(int cardID, int inDeckNormal, int inDeckPremium1, int inDeckPremium2, CardCollectionInfo.Premium prem, bool isFull, bool isBatchDismantleMode)
        //{
        //    BepInExLoader.log.LogMessage(cardID);
        //}
        [HarmonyPrefix]
        public void Open_Post(int cardID, int inDeckNormal, int inDeckPremium1, int inDeckPremium2, CardCollectionInfo.Premium prem, bool isFull, bool isBatchDismantleMode, int regulationID)
        {
            BepInExLoader.log.LogMessage(cardID);
        }

        public static void SetData_Post(CardBase __instance, CardBaseData data, int regulationID = -1)
        {
            BepInExLoader.log.LogMessage(__instance.GetComponentInChildren<YgomSystem.UI.SelectionButton>());
        }
    }
}
