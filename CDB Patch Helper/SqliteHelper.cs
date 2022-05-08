using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SQLite;
using System.Data.SqlClient;

public class SqliteHelper
{
    public string path;

    public SqliteHelper(string path)
    {
        this.path = path;
    }

    public List<Card> GetCards()
    {
        List<Card> result = new List<Card>();

        try
        {
            var connPath = "Data Source=" + path + ";";
            using (var connection = new SQLiteConnection(connPath))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @" select * from texts;";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Card(-1,Int32.Parse(reader["id"].ToString()), reader["name"].ToString(), reader["desc"].ToString()));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return result;
    }
}