using Microsoft.EntityFrameworkCore;
using Library.Pg.Data;
using Library.Pg.Models;

namespace Library.Pg;

class Program
{
    static async Task Main(string[] args)
    {
        using (var db = new LibraryContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

    }
}