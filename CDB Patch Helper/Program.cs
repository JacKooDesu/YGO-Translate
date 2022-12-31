class Program
{
    const int KEY_INDEX_GAME_ID = 0;
    const int KEY_INDEX_ID = 1;

    const string path = "./files/";
    static List<Card> engList;
    static List<Card> targetList;

    static void Main(string[] args)
    {
        SqliteHelper targetCdb = new SqliteHelper($"{path}target.cdb");
        SqliteHelper engCdb = new SqliteHelper($"{path}cards-en.cdb");

        engList = engCdb.GetCards();
        targetList = targetCdb.GetCards();

        engList.DistinctBy(c => c.Id);
        targetList.DistinctBy((c) => c.Id);
        targetList.DistinctBy((c) => c.Name);

        var csv = $"{path}data.csv";
        FileInfo fi = new FileInfo(csv);
        if (!fi.Directory.Exists)
            fi.Directory.Create();
        else
            Modify(fi, csv);

        CreateCsv(fi, csv);
    }

    static void CreateCsv(FileInfo fi, string csv)
    {
        FileStream fs = new FileStream(csv, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
        string data = "";
        //sw.WriteLine(data);

        int i = 0;
        foreach (var d in targetList)
        {
            var engCard = engList.Find(c => c.Id == d.Id);
            var engName = engCard == null ? "" : engCard.Name;

            var name = d.Name;
            var desc = d.Desc.Replace("\n", "\\n").Replace("\r", "");
            data = $"{d.GameId},{d.Id},{engName},{name},{desc}";
            // data = data.Replace("\"", "\"\"");
            sw.WriteLine(data);

            ++i;
        }
        sw.Close();
        fs.Close();
    }

    static void Modify(FileInfo fi, string csv)
    {
        using (var reader = new StreamReader(csv))
        {
            string str = "";
            int i = 0;
            while ((str = reader.ReadLine()) != null)
            {
                var keys = str.Split(',');
                int gameId = Int32.Parse(keys[KEY_INDEX_GAME_ID]);
                int id = Int32.Parse(keys[KEY_INDEX_ID]);

                if (engList.Exists(c => c.Id == id))
                {
                    string oldName = string.Empty;
                    for(int j = 2; j < keys.Length-2; ++j)
                    {
                        oldName += keys[j];
                        oldName += j != keys.Length - 3 ? ',' : string.Empty;
                    }

                    if(gameId!=-1)
                        engList.Find(c => c.Id == id).Name = String.IsNullOrEmpty(oldName) ? "Name_Missing" : oldName;
                }
                // if cn id exist, only update name/desc
                if (targetList.Exists(c => c.Id == id))
                {
                    targetList.Find(c => c.Id == id).GameId = gameId;
                }
                ++i;
            }
            reader.Close();
        }
    }
}

