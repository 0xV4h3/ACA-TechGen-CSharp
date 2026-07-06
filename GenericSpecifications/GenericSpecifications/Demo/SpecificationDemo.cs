using GenericSpecifications.Specification;
using GenericSpecifications.Models;

namespace GenericSpecifications.Demo;

public static class SpecificationDemo
{
    public static void Run()
    {
        RunProductFiltering();
        RunLoanApproval();
        RunJobScreening();
        RunOrderRouting();
    }

    private static void RunProductFiltering()
    {
        var products = new List<Product>
        {
            new("Keyboard", "Electronics", 80m, 3),
            new("Monitor", "Electronics", 95m, 8),
            new("Mouse", "Electronics", 25m, 0),
            new("Laptop", "Electronics", 1200m, 5),
            new("Desk", "Furniture", 210m, 2)
        };

        ISpecification<Product> inStock = new InStockSpecification();
        ISpecification<Product> electronics = new CategorySpecification("Electronics");
        ISpecification<Product> affordable = new MaxPriceSpecification(100m);

        var promoEligible = Specifications.AllOf(inStock, electronics, affordable);
        var restockCandidates = new OutOfStockSpecification().And(electronics);
        var premium = electronics.And(new MinPriceSpecification(500m));

        Console.WriteLine("-- E-commerce product filtering");
        Console.WriteLine("Promo eligible:");
        foreach (var p in products.Where(promoEligible))
        {
            Console.WriteLine($" - {p.Name} (${FormatDecimal(p.Price)}, stock={p.Stock})");
        }

        Console.WriteLine("Restock candidates:");
        foreach (var p in products.Where(restockCandidates))
        {
            Console.WriteLine($" - {p.Name} (stock={p.Stock})");
        }

        var firstPremium = products.FirstOrDefault(premium);
        Console.WriteLine("Premium electronics (first match):");
        if (firstPremium != null)
        {
            Console.WriteLine($" - {firstPremium.Name} (${FormatDecimal(firstPremium.Price)})");
        }

        var affordableElectronicsCount = products.Count(inStock.And(electronics).And(affordable));
        Console.WriteLine($"Affordable electronics count: {affordableElectronicsCount}");
    }

    private static void RunLoanApproval()
    {
        var applications = new List<LoanApplication>
        {
            new("A-01", 720, 4200m, 5, false),
            new("A-02", 680, 2800m, 4, false),
            new("A-03", 590, 3100m, 7, false),
            new("A-04", 710, 1900m, 3, false),
            new("A-05", 640, 3500m, 2, true),
            new("A-06", 670, 3400m, 2, false)
        };

        var approvedSpec = Specifications.AllOf<LoanApplication>(
            Specifications.Create<LoanApplication>(a => a.CreditScore >= 700),
            Specifications.Create<LoanApplication>(a => a.Income >= 4000m),
            Specifications.Create<LoanApplication>(a => a.EmploymentYears >= 2),
            Specifications.Create<LoanApplication>(a => !a.HasBankruptcy)
        );

        var rejectedSpec = Specifications.AnyOf<LoanApplication>(
            Specifications.Create<LoanApplication>(a => a.CreditScore < 620),
            Specifications.Create<LoanApplication>(a => a.Income < 3000m),
            Specifications.Create<LoanApplication>(a => a.HasBankruptcy)
        );

        Console.WriteLine("-- Loan approval rules");
        foreach (var app in applications)
        {
            string result;
            if (approvedSpec.IsSatisfiedBy(app))
                result = "APPROVED";
            else if (rejectedSpec.IsSatisfiedBy(app))
                result = "REJECTED";
            else
                result = "MANUAL REVIEW";

            Console.WriteLine($" {app.Id}: score={app.CreditScore}, income={FormatDecimal(app.Income)} -> {result}");
        }
    }

    private static void RunJobScreening()
    {
        var candidates = new List<Candidate>
        {
            new("C-01", 6, 4500m, true, ["C#", ".NET", "SQL"]),
            new("C-02", 2, 5200m, false, ["Java", "Spring"]),
            new("C-03", 4, 4100m, true, ["C#", "Azure"]),
            new("C-04", 8, 3800m, false, ["C#", ".NET", "Docker"]),
            new("C-05", 12, 6000m, true, [".NET", "Kubernetes"])
        };

        var expRange = new PropertyInRangeSpecification<Candidate, int>(c => c.ExperienceYears, 3, 20);
        var salaryMax = Specifications.Create<Candidate>(c => c.SalaryAsk <= 5000m);

        var csharpSkill = new HasAnySpecification<Candidate, string>(
            c => c.Skills,
            new PropertyContainsSpecification<string>(s => s, "C#")
        );

        var dotnetSkill = new HasAnySpecification<Candidate, string>(
            c => c.Skills,
            new PropertyContainsSpecification<string>(s => s, ".NET")
        );

        var shortlist = Specifications.AllOf(expRange, salaryMax, dotnetSkill);

        var remoteFriendly = Specifications.Create<Candidate>(c => c.IsRemoteFriendly);
        var backup = Specifications
            .AnyOf(csharpSkill, dotnetSkill)
            .And(remoteFriendly)
            .And(salaryMax)
            .And(shortlist.Not());

        Console.WriteLine("-- Job candidate screening");
        Console.WriteLine("Shortlist:");
        foreach (var c in candidates.Where(shortlist))
        {
            Console.WriteLine($" - {c.Id}: {c.ExperienceYears}y, skills=[{Join(c.Skills)}], ask={FormatDecimal(c.SalaryAsk)}");
        }

        Console.WriteLine("Backup pool:");
        foreach (var c in candidates.Where(backup))
        {
            Console.WriteLine($" - {c.Id}: Remote, skills=[{Join(c.Skills)}]");
        }
    }

    private static void RunOrderRouting()
    {
        var orders = new List<ShipmentOrder>
        {
            new("O-100", 1.2m, false, false, "Local"),
            new("O-101", 18.5m, false, true, "Local"),
            new("O-102", 0.8m, true, false, "International"),
            new("O-103", 25m, true, true, "International"),
            new("O-104", 4.5m, false, false, "Regional")
        };

        var airExpress = Specifications.AllOf<ShipmentOrder>(
            Specifications.Create<ShipmentOrder>(o => o.IsExpress),
            Specifications.Create<ShipmentOrder>(o => !o.IsFragile),
            Specifications.Create<ShipmentOrder>(o => o.WeightKg <= 5m),
            Specifications.Create<ShipmentOrder>(o => EqualsIgnoreCase(o.Zone, "Local"))
        );

        var freight = Specifications.AnyOf<ShipmentOrder>(
            Specifications.Create<ShipmentOrder>(o => o.WeightKg >= 15m),
            Specifications.Create<ShipmentOrder>(o => o.IsFragile),
            Specifications.Create<ShipmentOrder>(o => EqualsIgnoreCase(o.Zone, "International"))
        );

        var standard = Specifications.AllOf<ShipmentOrder>(
            Specifications.Create<ShipmentOrder>(o => o.WeightKg < 15m),
            Specifications.Create<ShipmentOrder>(o => !o.IsFragile)
        );

        var routes = new List<RouteRule>
        {
            new("AIR-EXPRESS", airExpress),
            new("FREIGHT", freight),
            new("STANDARD", standard)
        };

        Console.WriteLine("-- Order fulfillment routing");
        foreach (var order in orders)
        {
            string route = "CUSTOM";

            foreach (var rule in routes)
            {
                if (rule.Specification.IsSatisfiedBy(order))
                {
                    route = rule.RouteName;
                    break;
                }
            }

            Console.WriteLine($" {order.Id}: {FormatDecimal(order.WeightKg)}kg, fragile={order.IsFragile}, express={order.IsExpress}, zone={order.Zone} -> {route}");
        }
    }

    private static string Join(IReadOnlyList<string> items)
    {
        if (items == null || items.Count == 0) return string.Empty;

        var sb = new System.Text.StringBuilder();
        sb.Append(items[0]);

        for (int i = 1; i < items.Count; i++)
        {
            sb.Append(", ");
            sb.Append(items[i]);
        }

        return sb.ToString();
    }

    private static string FormatDecimal(decimal value)
    {
        if (decimal.Truncate(value) == value)
            return ((int)value).ToString();

        return value.ToString("0.##");
    }

    private static bool EqualsIgnoreCase(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    private class RouteRule(string routeName, ISpecification<ShipmentOrder> specification)
    {
        public string RouteName { get; } = routeName;
        public ISpecification<ShipmentOrder> Specification { get; } = specification;
    }
}