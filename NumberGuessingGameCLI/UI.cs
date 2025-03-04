﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGuessingGameCLI
{
    public static class UI
    {
        public static void ShowWelcomeMessage()
        {
            Console.WriteLine("\n🎮 Welcome to the Number Guessing Game!");
            Console.WriteLine("Try to guess the number between 1 and 100.");
        }

        public static int GetUserGuess()
        {
            Console.Write("\nEnter your guess: ");
            int guess;

            while (!int.TryParse(Console.ReadLine(), out guess) || guess < 1 || guess > 100)
            {
                Console.Write("❌ Invalid input. Enter a number between 1 and 100: ");
            }

            return guess;
        }

        public static void ShowHint(int guess, int secretNumber)
        {
            if (guess < secretNumber)
                Console.WriteLine("🔼 Try a higher number.");
            else
                Console.WriteLine("🔽 Try a lower number.");
        }

        public static void ShowWinMessage(int attempts, string timeTaken)
        {
            Console.WriteLine($"\n🎉 Congratulations! You guessed the number in {attempts} attempts.");
            Console.WriteLine($"⏱️ Time taken: {timeTaken}");
        }

        public static void ShowLoseMessage(int secretNumber)
        {
            Console.WriteLine($"\n❌ You ran out of attempts! The number was {secretNumber}.");
        }

        public static bool AskToPlayAgain()
        {
            Console.Write("\nDo you want to play again? (yes/no): ");
            return Console.ReadLine()?.Trim().ToLower() == "yes";
        }
    }
}
