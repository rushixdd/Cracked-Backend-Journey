namespace NumberGuessingGameCLI
{
    public class Difficulty
    {
        public int SelectDifficulty()
        {
            Console.WriteLine("\nChoose Difficulty Level:");
            Console.WriteLine("1. Easy (10 attempts)");
            Console.WriteLine("2. Medium (5 attempts)");
            Console.WriteLine("3. Hard (3 attempts)");

            string choice = Console.ReadLine();
            return choice switch
            {
                "1" => 10,
                "2" => 5,
                "3" => 3,
                _ => 5
            };
        }
    }
}
