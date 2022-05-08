using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace YGOTranslate
{
    public class CardSetting
    {
        public CardSetting(int gameId,int id,string eng,string cn,string cnDesc)
        {
            this.gameId = gameId;
            this.id = id;
            this.eng = eng; 
            this.cn = cn;
            this.cnDesc = cnDesc;
        }
        public int gameId;
        public int id;
        public string eng;
        public string cn;
        public string engDesc;
        public string cnDesc;
    }

    public static class Data
    {
        public static List<CardSetting> cards = new List<CardSetting>();

        static string file;
        public async static void Setup(string dir)
        {          
            if (!File.Exists(dir))
            {
                BepInExLoader.log.LogMessage("Dir"+ Path.GetFullPath(dir)+ " not found");
                return;
            }

            file = dir;

            string str = "";
            using(var reader = new StreamReader(dir))
            {
                int i = 0;
                while ((str = reader.ReadLine()) != null)
                {

                    CardSetting c;
                    var keys = str.Split(',');
                    if (keys.Length == 5)
                    {
                        c = new CardSetting(Int32.Parse(keys[0]), Int32.Parse(keys[1]), keys[2].ToLower(), keys[3], keys[4]);
                    }
                    else
                    {
                        var engLength = keys.Length - 4;
                        c = new CardSetting(Int32.Parse(keys[0]), Int32.Parse(keys[1]), "", keys[3 + engLength - 1], keys[4 + engLength - 1]);
                        var finalEng = "";
                        for (int j = 0; j < engLength; j++)
                            finalEng += keys[j + 2];

                        c.eng = finalEng.ToLower();
                    }

                    if (c.eng != string.Empty)
                        cards.Add(c);
                    ++i;

                    await Task.Yield();
                }

                reader.Close();
            }
        }

        public static CardSetting FindById(int gameId)
        {
            if (cards.FindIndex((c) => c.gameId == gameId) !=-1)
            {
                int index = cards.FindIndex((c) => c.gameId == gameId);
                return cards.Find((c) => c.gameId == gameId);
            }

            return null;
        }

        public static CardSetting FindByName(string nm,int gameId=-1)
        {
            nm = nm.Replace(",", string.Empty);
            nm = nm.ToLower();
            if (cards.FindIndex((c) => c.eng == nm) != -1)
            {
                //BepInExLoader.log.LogMessage("Founded!");
                var index = cards.FindIndex((c) => c.eng == nm);
                cards[index].gameId = gameId;
                return cards.Find((c) => c.eng == nm);
            }
            return null;
        }
    
        public async static void PatchGameId()
        {
            string data = "";
            FileStream fs = new FileStream(Path.GetDirectoryName(file) + "/modified.csv", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            foreach (var card in cards)
            {
                data = card.gameId.ToString() + "," + card.id.ToString() + "," + card.eng + "," + card.cn + "," + card.cnDesc;
                BepInExLoader.log.LogMessage(data);
                sw.WriteLine(data);
                await Task.Yield();
            }
            sw.Close();
            fs.Close();
        }
    }
}
