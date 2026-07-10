namespace GenericSpecifications.Models;

public class LoanApplication(string id, int creditScore, decimal income, int employmentYears, bool hasBankruptcy)
{
    public string Id { get; } = id;
    public int CreditScore { get; } = creditScore;
    public decimal Income { get; } = income;
    public int EmploymentYears { get; } = employmentYears;
    public bool HasBankruptcy { get; } = hasBankruptcy;
}