using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Input = BepInEx.IL2CPP.UnityEngine.Input;
using HarmonyLib;
using TMPro;
using XUnity.Common.Constants;
using System.IO;
using XUnity.Common.Logging;
using XUnity.Common.Utilities;

namespace YGOTranslate
{
    public class FontHelper : MonoBehaviour
    {
        public static GameObject instance;
        public static UnityEngine.Object fallbackFont;

        public static bool hasTryLoad = false;

        public FontHelper(IntPtr ptr) : base(ptr)
        {
            BepInExLoader.log.LogMessage("FontHelper Component Constructor!");
        }

        [HarmonyPostfix]
        public static void Update()
        {
            if (hasTryLoad)
                return;

            hasTryLoad = true;
            if (fallbackFont == null)
            {
                fallbackFont = LoadFont();
                LoadFallbackFont();
            }

            BepInExLoader.log.LogMessage("Changed tmpro font");
            Event.current.Use();
        }

        static UnityEngine.Object LoadFont()
        {
            UnityEngine.Object font = null;
            var path = "YGOTranslate\\font";

            if (!File.Exists(path))
            {
                BepInExLoader.log.LogMessage("Attempting to load TextMesh Pro font from internal Resources API.");

                font = Resources.Load("font");
            }
            else
            {
                AssetBundle bundle = null;
                if (UnityTypes.AssetBundle_Methods.LoadFromFile != null)
                {
                    bundle = (AssetBundle)UnityTypes.AssetBundle_Methods.LoadFromFile.Invoke(null, new object[] { path });
                }
                else if (UnityTypes.AssetBundle_Methods.CreateFromFile != null)
                {
                    bundle = (AssetBundle)UnityTypes.AssetBundle_Methods.CreateFromFile.Invoke(null, new object[] { path });
                }
                else
                {
                    BepInExLoader.log.LogError("Not found asset bundle load method while loading font: " + path);
                    return null;
                }

                if (bundle == null)
                {
                    BepInExLoader.log.LogError("Asset bundle load failed!");
                    return null;
                }

                if (UnityTypes.TMP_FontAsset != null)
                {
#if MANAGED
                    var assets = (UnityEngine.Object[])UnityTypes.AssetBundle_Methods.LoadAllAssets.Invoke( bundle, new object[] { UnityTypes.TMP_FontAsset.UnityType } );
#else
                    var assets = (UnhollowerBaseLib.Il2CppReferenceArray<UnityEngine.Object>)UnityTypes.AssetBundle_Methods.LoadAllAssets.Invoke(bundle, new object[] { UnityTypes.TMP_FontAsset.UnityType });
#endif
                    font = assets?.FirstOrDefault();
                }
                else if (UnityTypes.AssetBundle_Methods.LoadAll != null)
                {
#if MANAGED
                    var assets = (UnityEngine.Object[])UnityTypes.AssetBundle_Methods.LoadAll.Invoke( bundle, new object[] { UnityTypes.TMP_FontAsset.UnityType } );
#else
                    var assets = (UnhollowerBaseLib.Il2CppReferenceArray<UnityEngine.Object>)UnityTypes.AssetBundle_Methods.LoadAll.Invoke(bundle, new object[] { UnityTypes.TMP_FontAsset.UnityType });
#endif
                    font = assets?.FirstOrDefault();
                }
            }

            if (font != null)
            {
                var versionProperty = UnityTypes.TMP_FontAsset_Properties.Version;
                var version = (string)versionProperty?.Get(font) ?? "Unknown";
                BepInExLoader.log.LogMessage(font.name);

                GameObject.DontDestroyOnLoad(font);
            }
            else
            {
                BepInExLoader.log.LogError("Cannot not find the TMP font asset!");
            }

            return font;

            //try
            //{

            //    var asbundle = AssetBundle.LoadFromFile(System.IO.Path.GetFullPath(path));
            //    // BepInExLoader.log.LogMessage(asbundle.LoadAsset<TMP_FontAsset>("NotoSansCJKtc-Medium SDF"));
            //    var customTmp = new TMP_FontAsset();
            //    // customTMP.material = new Material(asbundle.LoadAsset("TMP_SDF") as Shader);
            //    // customTMP.
            //    // instance.GetComponent<TextMeshProUGUI>().font
            //    asbundle.Unload(false);
            //}
            //catch (Exception ex)
            //{
            //    BepInExLoader.log.LogError(ex);
            //}
        }

        private static void LoadFallbackFont()
        {
            try
            {
                if (UnityTypes.TMP_Settings_Properties.FallbackFontAssets == null)
                {
                    BepInExLoader.log.LogWarning("Cannot use fallback font because it is not supported in this version.");
                    return;
                }

                if (fallbackFont == null)
                {
                    BepInExLoader.log.LogWarning("Could not load fallback font for TextMesh Pro");
                    return;
                }

#if MANAGED
               var fallbacks = (IList)UnityTypes.TMP_Settings_Properties.FallbackFontAssets.Get( null );
#else
                var fallbacksObj = (Il2CppSystem.Object)UnityTypes.TMP_Settings_Properties.FallbackFontAssets.Get(null);
                var fallbacks = fallbacksObj.TryCast<Il2CppSystem.Collections.IList>();
#endif
                fallbacks.Add(fallbackFont);

                BepInExLoader.log.LogMessage($"Loaded fallback font for TextMesh Pro: " + fallbackFont);
            }
            catch (Exception e)
            {
                BepInExLoader.log.LogError("An error occurred while trying to load fallback font for TextMesh Pro.");
            }
        }

    }

}
