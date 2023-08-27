class Program
{
    const int KEY_INDEX_GAME_ID = 0;
    const int KEY_INDEX_ID = 1;

    const string path = "./files/";
    static List<Card> targetList = new();

    static void Main(string[] args)
    {
        SqliteHelper targetCdb = new SqliteHelper($"{path}target.cdb");

        targetList = targetCdb.GetCards();
        targetList.DistinctBy((c) => c.Id);
        targetList.DistinctBy((c) => c.Name);

        var csv = $"{path}data.csv";
        FileInfo fi = new FileInfo(csv);
        if (!fi.Directory?.Exists ?? false)
            fi.Directory?.Create();

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
            var name = d.Name;
            var desc = d.Desc.Replace("\n", "\\n").Replace("\r", "");
            data = $"{d.Id},{name},{desc}";
            // data = data.Replace("\"", "\"\"");
            sw.WriteLine(data);

            ++i;
        }
        sw.Close();
        fs.Close();
    }
}

