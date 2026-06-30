namespace RuleEngine;

static class Program
{
    static void Main(string[] args)
    {
        var engine = new RuleEngine();
        RegisterRules(engine);

        IEntity[] entities =
        {
            new FootballPlayer(1, "Lionel Messi", 38, "GOAT", 10),
            new FootballPlayer(2, "", 16, "Goalkeeper", 1),
            new FootballClub(100, "FC Barcelona", "Barcelona", 1899, 26),
            new FootballClub(101, "", "", 1800, -5)
        };


        Console.WriteLine("=== Fail-Fast Mode ===");
        foreach (var entity in entities)
        {
            try
            {
                engine.ValidateFailFast(entity);
                Console.WriteLine($"{entity.EntityType} #{entity.Id} passed validation.");
            }
            catch (RuleViolationException ex)
            {
                Console.WriteLine($"{entity.EntityType} #{entity.Id} failed: [{ex.RuleName}] {ex.Message}");
            }
            catch (EntityValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        Console.WriteLine();

        Console.WriteLine("=== Collect-All Mode ===");
        foreach (var entity in entities)
        {
            try
            {
                engine.ValidateCollectAll(entity);
                Console.WriteLine($"{entity.EntityType} #{entity.Id} passed validation.");
            }
            catch (EntityValidationException ex)
            {
                Console.WriteLine(ex.Message);
                foreach (var v in ex.Violations)
                {
                    Console.WriteLine($"- [{v.RuleName}] {v.Message}");
                }
            }
        }
    }

    private static void RegisterRules(RuleEngine engine)
    {
        engine.AddRule(new Rule(
            "Player full name required",
            "FootballPlayer",
            entity =>
            {
                var p = (FootballPlayer)entity;
                if (string.IsNullOrWhiteSpace(p.FullName))
                    throw new RuleViolationException("Player full name required", "Player name must not be empty.");
            }));

        engine.AddRule(new Rule(
            "Player must be adult",
            "FootballPlayer",
            entity =>
            {
                var p = (FootballPlayer)entity;
                if (p.Age < 18)
                    throw new RuleViolationException("Player must be adult", "Player must be at least 18 years old.");
            }));

        engine.AddRule(new Rule(
            "Valid player position",
            "FootballPlayer",
            entity =>
            {
                var p = (FootballPlayer)entity;
                var validPositions = new[] { "Goalkeeper", "Defender", "Midfielder", "Forward", "GOAT" };
                if (Array.IndexOf(validPositions, p.Position) < 0)
                    throw new RuleViolationException("Valid player position", "Player position is not recognized.");
            }));

        engine.AddRule(new Rule(
            "Club name required",
            "FootballClub",
            entity =>
            {
                var c = (FootballClub)entity;
                if (string.IsNullOrWhiteSpace(c.ClubName))
                    throw new RuleViolationException("Club name required", "Club name must not be empty.");
            }));

        engine.AddRule(new Rule(
            "Club founded year valid",
            "FootballClub",
            entity =>
            {
                var c = (FootballClub)entity;
                if (c.FoundedYear < 1850 || c.FoundedYear > DateTime.Now.Year)
                    throw new RuleViolationException("Club founded year valid", "Founded year is not valid.");
            }));

        engine.AddRule(new Rule(
            "Club squad size valid",
            "FootballClub",
            entity =>
            {
                var c = (FootballClub)entity;
                if (c.SquadSize < 11)
                    throw new RuleViolationException("Club squad size valid", "Squad size should be at least 11.");
            }));
    }
}