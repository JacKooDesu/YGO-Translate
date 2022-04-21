# YGO-Translate
YGO MD 中文字典查找插件。

## 安裝方法
0. 下載並安裝[BepInEx Unity IL2CPP](https://builds.bepinex.dev/projects/bepinex_be)
1. [下載](https://github.com/JacKooDesu/YGO-Translate/releases)插件
2. 解壓縮後將Build資料夾內所有檔案拉到MD根目錄 `( ~\SteamLibrary\steamapps\common\Yu-Gi-Oh!  Master Duel )`
3. 執行遊戲

## 注意事項
- ~~遊戲語言需先調整成 ***英文*** 才能正常翻譯~~ v1.2 後不再依賴卡名查詢
- 如果使用Excel或其他可開啟試算表的軟體開啟 `YGOTranslate\data.csv` ， ***不要*** 存檔，可能會導致字典檔無法被讀取
- 如果想取代部分詞彙，請使用文字編輯器 (ex: `記事本` 、 `vs code` 、 `notepad++`)

## 原理
- 修改遊戲內 `GetName()`、`GetDesc()` 、 `GetRubyName()` 三個方法，依照卡牌內部ID取得中文翻譯回傳
