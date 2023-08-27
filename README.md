# YGO-Translate
YGO MD 中文字典查找插件。

## 2023.08 後更

小弟我最近比較沒時間開遊戲、更新遊戲，有使用插件的玩家可以查看 `~\BepInEx\LogOutput.log` ，並善用 Issue 提交功能，讓我可以直接用 API 更新。

## 安裝方法
0. 下載並安裝 [BepInEx Unity IL2CPP](https://builds.bepinex.dev/projects/bepinex_be)
1. [下載](https://github.com/JacKooDesu/YGO-Translate/releases) 插件
2. 解壓縮後將Build資料夾內所有檔案拉到MD根目錄 `( ~\SteamLibrary\steamapps\common\Yu-Gi-Oh!  Master Duel )`
3. 執行遊戲

## 設定

- 路徑 - `~\Yu-Gi-Oh! Master Duel\BepInEx\config\com.jackoo.YGOTranslate.cfg` 
- dataPath - 翻譯檔路徑
- idPairPath - Password-GameID 檔案路徑
- switchKey - 開關按鍵，使用 `Ctrl`+`[value]`，預設 `F9`
- TMP_fallback - 是否使用 `TMP Fallback` 功能
- ~~tmpKey - TMP Fallback 資源載入按鍵，`Ctrl`+`[value]`，預設 `F12`~~ 版本1.5後會自動加載
- copy_enable - 開啟卡名複製功能 (原文基本上可使用，翻譯過後的卡名需要展開滿版卡片說明才能複製)，預設為`false`
- copyKey - 複製卡名按鍵，`Ctrl`+`Shift`+`[value]`，預設 `c`

## 注意事項
- ~~遊戲語言需先調整成 ***英文*** 才能正常翻譯~~ v1.2 後不再依賴卡名查詢
- 如果使用Excel或其他可開啟試算表的軟體開啟 `YGOTranslate\data.csv` ， ***不要*** 存檔，可能會導致字典檔無法被讀取
- 如果想取代部分詞彙，請使用文字編輯器 (ex: `記事本` 、 `vs code` 、 `notepad++`)
- 使用TMP Fallback功能時，請確認 `YGOTranslate\font` 檔案存在且合法

## 原理
- 修改遊戲內 `GetName()`、`GetDesc()` 、 `GetRubyName()` 三個方法，依照卡牌內部ID取得中文翻譯回傳

## 已知問題
- ~~`TMP Fallback` 目前的 `Font Asset` 是使用 [Noto Sans](https://fonts.google.com/noto/specimen/Noto+Sans+TC) 生成，沒有包含字元 `‧` 導致功能開啟還是會缺字~~
- 改用 [Chiron sans](https://chiron-fonts.github.io/)，若還有缺字可以至 [Issue](https://github.com/JacKooDesu/YGO-Translate/issues) 內回報，會盡快更新TMP Asset
- 卡面文字目前還是會有嚴重缺字
- 靈擺效果與怪獸效果不會分開顯示

## 雜項
- 使用 [XUnity.Common](https://github.com/bbepis/XUnity.AutoTranslator/tree/master/src/XUnity.Common) 並修改部分 `TMP Fallback` 腳本
- 此插件 ***沒有修改遊戲檔案、沒有修改連線相關函式*** ，但如果怕被鎖還是別用
