using System;

namespace Code5
{
    class InsufficientBalanceException : ApplicationException
    {
        public double AttemptedAmount { get; set; }

        public InsufficientBalanceException(string message, double amount) : base(message)
        {
            AttemptedAmount = amount;
        }
    }

    class Account
    {
        double balance = 10000;

        public void Deposit(double amount)
        {
            balance += amount;
        }

        public void Withdraw(double amount)
        {
            if (amount > balance)
                throw new InsufficientBalanceException("Insufficient funds.", amount);
            balance -= amount;
        }

        public void ShowBalance()
        {
            Console.WriteLine("Balance: " + balance);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Account acc = new Account();

            Console.WriteLine("Enter transaction type (D/W):");
            char t = char.Parse(Console.ReadLine());

            Console.WriteLine("Enter amount:");
            double amt = double.Parse(Console.ReadLine());

            try
            {
                if (t == 'D' || t == 'd')
                    acc.Deposit(amt);
                else if (t == 'W' || t == 'w')
                    acc.Withdraw(amt);

                acc.ShowBalance();
            }
            catch (InsufficientBalanceException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine($"Attempted Amount: {e.AttemptedAmount}");
            }

            Console.ReadLine();
        }
    }
}
