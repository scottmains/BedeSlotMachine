using BedeSlotMachine;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {

        decimal depositAmount = 0;
        // Prompt player for deposit amount
        while (depositAmount == 0)
        {
            Console.WriteLine("Enter deposit amount:");
            depositAmount = CheckDepositAmount(depositAmount);
        }
      
        // Initialize balance and win amount
        decimal balance = depositAmount;
        decimal winAmount = 0;

        // Create a list of the symbol object, inputting name, coefficient and probability
        List<Symbol> symbols = new List<Symbol>
        {
        new Symbol("A", 0.4m, 0.45),
        new Symbol("B", 0.6m, 0.35),
        new Symbol("P", 0.8m, 0.15),
        new Symbol("*", 0.0m, 0.05)
        };

        // Slot machine loop
        while (balance > 0)
        {
            // Prompt player for stake amount
           var stakeAmount = CheckStakeAmount(balance);

           //if stake amount is 0(either inputted 0 or less or more than balance amount) then go back to start
           if (stakeAmount == 0)
            {
                continue;
            } 


            var spinResults = SimulateSpin(symbols);

            Console.WriteLine("Spin results:");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(spinResults[i][j] + " ");
                }
                Console.WriteLine();
            }

           var coeffSum = checkWinningLines(spinResults, symbols);

            if (coeffSum > 0)
            {
                winAmount = coeffSum * 3 * stakeAmount;
                balance += winAmount;

                Console.WriteLine("Congratulations! You've won " + winAmount);
                Console.WriteLine("Current Balance:" + balance);
            }
            else
            {
                winAmount = 0;
                // Update balance
                balance -= stakeAmount;
                balance += winAmount;

                // Display current balance
                Console.WriteLine("Your current balance is: " + balance);
               
            }
        }

        Console.WriteLine("Sorry, you're out of money. Thanks for playing!");
    }

    private static decimal CheckDepositAmount(decimal depositAmount)
    {
        
        while (!decimal.TryParse(Console.ReadLine(), out depositAmount))
        {
            Console.WriteLine("Please enter a valid deposit amount:");
            return 0;
        }
        if (depositAmount <= 0)
        {
            Console.WriteLine("Deposit amount must be greater than zero.");
            return 0; // or any other action you want to take
        }

        return depositAmount;
    }

    //This function checks if the symbols on each line are the same.
    private static decimal checkWinningLines(string[][] spinResults, List<Symbol> symbols)
    {
        decimal coeffSum = 0;
        for (int i = 0; i < 4; i++)
        {
            //iterate through the rows and check if the first, second or third symbol are the same
            if (spinResults[i][0] == spinResults[i][1] && spinResults[i][1] == spinResults[i][2])
            {
                //select this symbol from the symbol list 
                var symbol = symbols.FirstOrDefault(s => s.Name == spinResults[i][0] || s.Name == "*");
                if (symbol != null)
                {
                    //add the symbols coeff to the coeff sum
                    coeffSum += symbol.Coefficient;
                }
            }
        }
        return coeffSum;
    }
           
    // randomly selects symbols from the list of symbols.
    private static string[][] SimulateSpin(List<Symbol> symbols)
    {
        Random rnd = new Random();
        string[][] spinResults = new string[4][];
        //iterate through rows
        for (int i = 0; i < 4; i++)
        {
            spinResults[i] = new string[3];
            //iterate through columns
            for (int j = 0; j < 3; j++)
            {
                //returns a random floating point
                double probability = rnd.NextDouble();
                double cumulativeProbability = 0;
                foreach (var symbol in symbols)
                {
                    //keep track of total probability of all symbols
                    cumulativeProbability += symbol.Probability;

                    //if random probability is less than the cumulative, this symbol will be chosen
                    if (probability < cumulativeProbability)
                    {
                        spinResults[i][j] = symbol.Name;
                        break;
                    }
                }
            }
        }
        return spinResults;
    }

    private static decimal CheckStakeAmount(decimal balance)
    {
        Console.WriteLine("Enter stake amount:");
        decimal stakeAmount = decimal.Parse(Console.ReadLine());

        // Check if stake amount is greater than balance or greater than 0
        if (stakeAmount <= 0 || stakeAmount > balance)
        {
            Console.WriteLine("Stake amount must be greater than zero and less than the current balance.");
            return 0; 
        }

        return stakeAmount;
    }
}
