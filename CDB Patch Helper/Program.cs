const string path = "./files/";

SqliteHelper targetCdb = new SqliteHelper($"{path}target.cdb");
SqliteHelper engCdb = new SqliteHelper($"{path}cards-en.cdb");

var engList = engCdb.GetCards();
var targetList = targetCdb.GetCards();

engList.DistinctBy(c => c.Id);
targetList.DistinctBy((c) => c.Id);
targetList.DistinctBy((c) => c.Name);

var csv = $"{path}data.csv";
FileInfo fi = new FileInfo(csv);
if (!fi.Directory.Exists)
    fi.Directory.Create();

FileStream fs = new FileStream(csv, FileMode.Create, FileAccess.Write);
StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
string data = $"遊戲id,實卡id,Eng,卡名,敘述";
sw.WriteLine(data);

int i = 0;
foreach (var d in targetList)
{
    var engCard = engList.Find(c=>c.Id==d.Id);
    var engName = engCard == null ? "" : engCard.Name;

    var name = d.Name;
    var desc = d.Desc.Replace("\n", "\\n").Replace("\r", "");
    data = $"-1,{d.Id},{engName},{name},{desc}";
    // data = data.Replace("\"", "\"\"");
    sw.WriteLine(data);

    ++i;
}
sw.Close();
fs.Close();
