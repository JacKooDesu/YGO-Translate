# YGO-Translate
YGO MD 中文字典查找插件。

## 安裝方法
0. 下載並安裝 [BepInEx Unity IL2CPP](https://builds.bepinex.dev/projects/bepinex_be)
1. [下載](https://github.com/JacKooDesu/YGO-Translate/releases) 插件
2. 解壓縮後將Build資料夾內所有檔案拉到MD根目錄 `( ~\SteamLibrary\steamapps\common\Yu-Gi-Oh!  Master Duel )`
3. 執行遊戲

## 設定

- 路徑 - `~\Yu-Gi-Oh! Master Duel\BepInEx\config\com.jackoo.YGOTranslate.cfg` 
- dataPath - 翻譯檔路徑
- switchKey - 開關按鍵，使用 `Ctrl`+`[value]`，預設 `F9`
- TMP_fallback - 是否使用 `TMP Fallback` 功能
- tmpKey - TMP Fallback 資源載入按鍵，`Ctrl`+`[value]`，預設 `F12`

## 注意事項
- ~~遊戲語言需先調整成 ***英文*** 才能正常翻譯~~ v1.2 後不再依賴卡名查詢
- 如果使用Excel或其他可開啟試算表的軟體開啟 `YGOTranslate\data.csv` ， ***不要*** 存檔，可能會導致字典檔無法被讀取
- 如果想取代部分詞彙，請使用文字編輯器 (ex: `記事本` 、 `vs code` 、 `notepad++`)
- 使用TMP Fallback功能時，請確認 `YGOTranslate\font` 檔案存在且合法

## 原理
- 修改遊戲內 `GetName()`、`GetDesc()` 、 `GetRubyName()` 三個方法，依照卡牌內部ID取得中文翻譯回傳

## 已知問題
- ~~`TMP Fallback` 目前的 `Font Asset` 是使用 [Noto Sans](https://fonts.google.com/noto/specimen/Noto+Sans+TC) 生成，沒有包含字元 `‧` 導致功能開啟還是會缺字~~
- 改用 [Chiron sans](https://chiron-fonts.github.io/) 還是有部分缺字

## 雜項
- 使用 [XUnity.AutoTranslator](https://github.com/bbepis/XUnity.AutoTranslator) - XUnity.Common 並修改部分 `TMP Fallback` 腳本
- 此插件 ***沒有修改遊戲檔案、沒有修改連線相關函式*** ，但如果怕被鎖還是別用
