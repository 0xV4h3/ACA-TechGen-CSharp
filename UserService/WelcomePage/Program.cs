using System.Globalization;
using WelcomePage.Data;
using WelcomePage.DTOs;
using WelcomePage.Repositories;
using WelcomePage.Services;

namespace WelcomePage;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new AppDbContext())
        {
            db.EnsureSchema();
            
        }
    }
}