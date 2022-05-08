using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Card
{
    public Card(int gameId,int id, string name, string desc)
    {
        GameId = gameId;
        Id = id;
        Name = name;
        Desc = desc;
    }

    public int GameId { get; set; }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
}

