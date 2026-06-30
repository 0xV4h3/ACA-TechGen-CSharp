namespace RuleEngine;

public interface IEntity
{
    int Id { get; }
    string EntityType { get; }
}

public class FootballPlayer(int id, string fullName, int age, string position, int shirtNumber) : IEntity
{
    public int Id { get; } = id;
    public string EntityType => "FootballPlayer";
    public string FullName { get; } = fullName;
    public int Age { get; } = age;
    public string Position { get; } = position;
    public int ShirtNumber { get; } = shirtNumber;
}

public class FootballClub(int id, string clubName, string city, int foundedYear, int squadSize) : IEntity
{
    public int Id { get; } = id;
    public string EntityType => "FootballClub";
    public string ClubName { get; } = clubName;
    public string City { get; } = city;
    public int FoundedYear { get; } = foundedYear;
    public int SquadSize { get; } = squadSize;
}