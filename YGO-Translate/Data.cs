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
        public CardSetting(int id,string eng,string cn,string cnDesc)
        {
            this.id = id;
            this.eng = eng; 
            this.cn = cn;
            this.cnDesc = cnDesc;
        }
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
                    if (i != 0)
                    {
                        var keys =str.Split(',');
                        if (keys.Length == 4)
                        {
                            cards.Add(new CardSetting(Int32.Parse(keys[0]),  keys[1], keys[2], keys[3]));
                        } 
                        else
                        {
                            var engLength = keys.Length - 3;
                            var c = new CardSetting(Int32.Parse(keys[0]), "", keys[2+engLength-1], keys[3 + engLength - 1]);
                            var finalEng = "";
                            for (int j = 0; j < engLength; j++)
                                finalEng += keys[j + 1];
                            
                            c.eng = finalEng;

                            cards.Add(c);
                        }
                    }
                    ++i;

                    await Task.Yield();
                }

                reader.Close();
            }
        }

        public static CardSetting FindById(int id)
        {
            if (cards.FindIndex((c) => c.id == id) !=-1)
            {
                int index = cards.FindIndex((c) => c.id == id);
                cards[index].id = id;
                return cards.Find((c) => c.id == id);
            }

            return null;
        }

        public static CardSetting FindByName(string nm,int id=-1)
        {
            nm = nm.Replace(",", string.Empty);
            if (cards.FindIndex((c) => c.eng == nm) != -1)
            {
                //BepInExLoader.log.LogMessage("Founded!");
                var index = cards.FindIndex((c) => c.eng == nm);
                cards[index].id = id;
                return cards.Find((c) => c.eng == nm);
            }
            return null;
        }
    
        public async static void SaveModifiedCards()
        {
            string data = "";
            FileStream fs = new FileStream(Path.GetDirectoryName(file) + "/modified.csv", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            foreach (var card in cards)
            {
                data = card.id.ToString() + ",," + card.cn + "," + card.cnDesc;
                BepInExLoader.log.LogMessage(data);
                sw.WriteLine(data);
                await Task.Yield();
            }
            sw.Close();
            fs.Close();
        }
    }
}
