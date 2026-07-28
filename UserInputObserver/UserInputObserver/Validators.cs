namespace UserInputObserver;

public class IntValidator(int threshold) : UserInputValidator
{
    public int Threshold { get; set; } = threshold;

    protected override Result<string> Validate(string input)
    {
        if (!int.TryParse(input, out int number))
            return Result<string>.Fail($"[IntValidator] '{input}' is not a valid integer.");

        if (number > Threshold)
            return Result<string>.Fail($"[IntValidator] Inputted number {number} exceeds the threshold of {Threshold}.");

        return Result<string>.Ok(input);
    }
}

public class PredicateValidator(Predicate<string> predicate, string errorMessage = "Input failed predicate validation.")
    : UserInputValidator
{
    private readonly Predicate<string> _predicate = predicate;
    private readonly string _errorMessage = errorMessage;
    
    protected override Result<string> Validate(string input)
    {
        if (!_predicate(input))
            return Result<string>.Fail($"[PredicateValidator] {_errorMessage}");

        return Result<string>.Ok(input);
    }
}