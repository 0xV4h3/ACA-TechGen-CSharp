namespace UserInputObserver;

class Program
{
    static void Main(string[] args)
    {
        var tracker = new UserTracker();
        
        tracker.Subscribe(new ConsoleUserInputLogger());
        tracker.Subscribe(new IntValidator(100));
        tracker.Subscribe(new PredicateValidator(
            input => input.Length >= 3, 
            "Input string is too short. Minimum length is 3 characters."
        ));
        
        ProcessInput(tracker, "200");
        ProcessInput(tracker, "hi");
        ProcessInput(tracker, "1");


        static void ProcessInput(UserTracker tracker, string input)
        {
            List<Result<string>> results = tracker.NotifyUserInput(input);
            List<Result<string>> failures = [];
            foreach (var result in results)
            {
                if(!result.Success)
                    failures.Add(result);
            }
            
            if (failures.Count > 0)
            {
                Console.WriteLine("Validation Status: FAILED");
                foreach (var failure in failures)
                    Console.WriteLine($" -> {failure.ErrorMessage}");
            }
            else
                Console.WriteLine("Validation Status: SUCCESS");
        }
    }
}
