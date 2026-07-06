namespace GenericSpecifications.Models;

public class Candidate(string id, int experienceYears, decimal salaryAsk, bool isRemoteFriendly, List<string> skills)
{
    public string Id { get; } = id;
    public int ExperienceYears { get; } = experienceYears;
    public decimal SalaryAsk { get; } = salaryAsk;
    public bool IsRemoteFriendly { get; } = isRemoteFriendly;
    public List<string> Skills { get; } = skills;
}