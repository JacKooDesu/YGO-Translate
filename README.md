# YGO-Translate
YGO MD 中文字典查找插件。

## 安裝方法
0. 下載並安裝[BepInEx Unity IL2CPP](https://builds.bepinex.dev/projects/bepinex_be)
1. [下載](https://github.com/JacKooDesu/YGO-Translate/releases)插件
2. 解壓縮後將Build資料夾內所有檔案拉到MD根目錄 `( ~\SteamLibrary\steamapps\common\Yu-Gi-Oh!  Master Duel )`
3. 執行遊戲

## 注意事項
- 遊戲語言需先調整成 ***英文*** 才能正常翻譯

## 原理
- 修改遊戲內 `GetName()`、`GetDesc()` 兩個方法，使用卡片名稱進入字典檔中查詢對應的中文回傳

