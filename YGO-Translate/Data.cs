using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using Mono.Data.Sqlite;

namespace YGOTranslate
{
    public class CardSetting
    {
        public CardSetting(int gameId, string cn, string cnDesc)
        {
            this.gameId = gameId;
            this.cn = cn;
            this.cnDesc = cnDesc;
        }
        public int gameId;
        public string cn;
        public string cnDesc;
    }

    public static class Data
    {
        public static List<CardSetting> cards = new List<CardSetting>();
        public static List<int> invalidIds = new List<int>();

        static string file;
        public async static void Setup(string cdb, string idPair)
        {
            if (!File.Exists(cdb))
            {
                BepInExLoader.log.LogMessage("Dir" + Path.GetFullPath(cdb) + " not found");
                return;
            }

            file = cdb;

            string str = "";
            Dictionary<uint, List<int>> idPairDict = new Dictionary<uint, List<int>>();
            using (var reader = new StreamReader(idPair))
            {
                while ((str = reader.ReadLine()) != null)
                {
                    var keys = str.Split(',');

                    if (keys.Length != 2 ||
                        !Int32.TryParse(keys[0], out var id) ||
                        !UInt32.TryParse(keys[1], out var pwd))
                    {
                        BepInExLoader.log.LogMessage("id-pwd-pair: `" + str + "` is invalid!!");
                        continue;
                    }

                    if (idPairDict.ContainsKey(pwd))
                        idPairDict[pwd].Add(id);
                    else
                        idPairDict.Add(pwd, new List<int> { id });

                    await Task.Yield();
                }

                reader.Close();
            }

            using (var reader = new StreamReader(cdb))
            {
                while ((str = reader.ReadLine()) != null)
                {
                    var keys = str.Split(',');

                    if (keys.Length != 3 ||
                        !UInt32.TryParse(keys[0], out var pwd) ||
                        !idPairDict.TryGetValue(pwd, out var ids))
                    {
                        // BepInExLoader.log.LogMessage("cdb: `" + str + "` is invalid!!");
                        continue;
                    }

                    var name = keys[1];
                    var desc = keys[2];

                    cards.AddRange(ids.Select(
                        id => new CardSetting(id, name, desc)));
                    await Task.Yield();
                }

                reader.Close();
            }

        }

        public static CardSetting FindById(int gameId)
        {
            if (cards.FindIndex((c) => c.gameId == gameId) != -1)
            {
                int index = cards.FindIndex((c) => c.gameId == gameId);
                return cards.Find((c) => c.gameId == gameId);
            }

            return null;
        }

        public static void LogInvalid(string name, int id)
        {
            if (!invalidIds.Contains(id))
            {
                BepInExLoader.log.LogWarning("Card `" + name + "` not found! / id = " + id.ToString());
                Data.invalidIds.Add(id);
            }
        }
    }
}
