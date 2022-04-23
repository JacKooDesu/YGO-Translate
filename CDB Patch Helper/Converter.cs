//using System.Runtime.InteropServices;
//using System.Data;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Mono.Data.Sqlite;
//using System.IO;

//public class Converter : MonoBehaviour
//{
//    string dbPath = "URI=file:Assets/DBs/";
//    public string fromDbName, zhDbName;

//    public Dictionary<string, Card> fromCards = new Dictionary<string, Card>();
//    public Dictionary<string, Card> zhCards = new Dictionary<string, Card>();

//    public class Card
//    {
//        public int id;
//        public string name;
//        public string desc;
//        public Card(string name, string desc)
//        {
//            this.name = name;
//            this.desc = desc;
//        }
//    }
//    [TextArea(2, 10)]
//    public string cnChars;
//    List<string> cnCharList = new List<string>();

//    void Start()
//    {
//        fromCards = ReadCards($"{dbPath}{fromDbName}");
//        zhCards = ReadCards($"{dbPath}{zhDbName}");

//        // StartCoroutine(Export());
//        // StartCoroutine(GetAllChineseChar());
//        StartCoroutine(ExportCsv(zhCards));

//    }

//    Dictionary<string, Card> ReadCards(string dbName)
//    {
//        Dictionary<string, Card> result = new Dictionary<string, Card>();
//        using (var connection = new SqliteConnection(dbName))
//        {
//            connection.Open();
//            using (var command = connection.CreateCommand())
//            {
//                command.CommandText = "SELECT * FROM texts;";

//                using (IDataReader reader = command.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        result.Add(
//                            reader["id"].ToString(),
//                            new Card(reader["name"].ToString(), reader["desc"].ToString()));
//                    }
//                }
//            }
//            connection.Close();
//        }

//        return result;
//    }

//    [ContextMenu("輸出")]
//    IEnumerator Export()
//    {
//        var sb  = new System.Text.StringBuilder();
//        foreach (var d in zhCards)
//        {
//            if (!fromCards.ContainsKey(d.Key))
//                continue;

//            var card = fromCards[d.Key];

//            // var name = $"{card.name}={SystemChineseConverter.ToTraditional(d.Value.name)}\n";
//            // var desc = $"{card.desc.Replace("\n", "\\n").Replace("\r", "\\n")}={SystemChineseConverter.ToTraditional(d.Value.desc).Replace("\n", "\\n").Replace("\r", "\\n")}\n";

//            sb.AppendLine($"-1={name}={SystemChineseConverter.ToTraditional(d.Value.name)},{card.desc}={SystemChineseConverter.ToTraditional(d.Value.desc).Replace("\n", "\\n").Replace("\r", "\\n")}");

//            yield return null;
//        }

//        File.WriteAllText($"{Application.dataPath}/DBs/Result.txt", sb.ToString());
//        print("Complete");
//    }

//    IEnumerator ExportCsv(Dictionary<string, Card> target)
//    {
//        var path = $"{Application.dataPath}/DBs/Data.csv";
//        FileInfo fi = new FileInfo(path);
//        if (!fi.Directory.Exists)
//            fi.Directory.Create();

//        FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
//        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
//        string data = $"id,Eng,卡名,敘述";
//        sw.WriteLine(data);

//        int i=0;
//        foreach (var d in target)
//        {
//            var eng = fromCards[d.Key].name;
//            var name = $"{SystemChineseConverter.ToTraditional(d.Value.name)}";
//            var desc = $"{SystemChineseConverter.ToTraditional(d.Value.desc).Replace("\n", "\\n").Replace("\r", "")}";
//            data = $"-1,{eng},{name},{desc}";
//            data = data.Replace("\"", "\"\"");
//            sw.WriteLine(data);

//            ++i;
//            yield return null;
//        }
//        sw.Close();
//        fs.Close();
//        print("Complete");
//    }

//    IEnumerator GetAllChineseChar()
//    {
//        foreach (var d in zhCards)
//        {
//            var name = $"{SystemChineseConverter.ToTraditional(d.Value.name)}";
//            var desc = $"{SystemChineseConverter.ToTraditional(d.Value.desc).Replace("\n", "").Replace("\r", "")}";

//            var tempList = new List<string>();
//            foreach (char c in name)
//            {
//                tempList.Add(c.ToString());
//            }
//            foreach (char c in desc)
//            {
//                tempList.Add(c.ToString());
//            }

//            cnCharList.AddRange(tempList);

//            cnCharList = cnCharList.Distinct().ToList();

//            yield return null;
//        }
//        print(cnCharList.Count);

//        string str = "";
//        foreach (var c in cnCharList)
//        {
//            str += c;
//            yield return null;
//        }


//        File.WriteAllText($"{Application.dataPath}/DBs/Texts.txt", str);
//    }
//}

//public static class SystemChineseConverter
//{
//    internal const int LOCALE_SYSTEM_DEFAULT = 0x0800;
//    internal const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
//    internal const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

//    /// <summary> 
//    /// 使用OS的kernel.dll做為簡繁轉換工具，只要有裝OS就可以使用，不用額外引用dll，但只能做逐字轉換，無法進行詞意的轉換 
//    /// <para>所以無法將電腦轉成計算機</para> 
//    /// </summary> 
//    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
//    internal static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

//    /// <summary> 
//    /// 繁體轉簡體 
//    /// </summary> 
//    /// <param name="pSource">要轉換的繁體字：體</param> 
//    /// <returns>轉換後的簡體字：体</returns> 
//    public static string ToSimplified(string pSource)
//    {
//        string tTarget = new string(' ', pSource.Length);
//        int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, pSource, pSource.Length, tTarget, pSource.Length);
//        return tTarget;
//    }

//    /// <summary> 
//    /// 簡體轉繁體 
//    /// </summary> 
//    /// <param name="pSource">要轉換的繁體字：体</param> 
//    /// <returns>轉換後的簡體字：體</returns> 
//    public static string ToTraditional(string pSource)
//    {
//        string tTarget = new string(' ', pSource.Length);
//        int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, pSource, pSource.Length, tTarget, pSource.Length);
//        return tTarget;
//    }
//}
