using System;

namespace Csharp_Code3_24_06_25
{//-------------------------------Bank Details Program-----------------------------------------------------
    class Accounts
    {
        protected int AccountNo;
        protected string CustomerName;
        protected string AccountType;
        protected char TransactionType;
        protected double amount;
        protected double Balance;
        int[] marks = new int[5];

        public Accounts(int accNo, string custName, string accType, char transactionType, double amt)
        {
            AccountNo = accNo;
            CustomerName = custName;
            AccountType = accType;
            TransactionType = transactionType;
            amount = amt;
            Balance = 10000;

            BalanceUpdate();
        }

        public void BalanceUpdate()
        {
            if (TransactionType == 'D' || TransactionType == 'd')
            {
                Credit(amount);
            }
            else if (TransactionType == 'W' || TransactionType == 'w')
            {
                Debit(amount);
            }
            else
            {
                Console.WriteLine("Invalid Transaction Type");
            }
        }

        public void Credit(double amount)
        {
            Balance += amount;
        }

        public void Debit(double amount)
        {
            if (amount > Balance)
            {
                Console.WriteLine("Insufficient Balance");
            }
            else
            {
                Balance -= amount;
            }
        }

        public void Showdata()
        {
            Console.WriteLine("\nAccount Details");
            Console.WriteLine($"Account Number      : {AccountNo}");
            Console.WriteLine($"Account Holder Name : {CustomerName}");
            Console.WriteLine($"Account Type        : {AccountType}");
            Console.WriteLine($"Transaction Type    : {TransactionType}");
            Console.WriteLine($"Amount              : {amount}");
            Console.WriteLine($"Account Balance     : {Balance}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the Account number: ");
            int accNo = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter your Name: ");
            string custName = Console.ReadLine();

            Console.WriteLine("Enter the Account Type Savings/Current: ");
            string accType = Console.ReadLine();

            Console.WriteLine("Enter the Transaction Type D/W for Deposit/Withdrawal: ");
            char transType = char.Parse(Console.ReadLine());

            Console.WriteLine("Enter the amount: ");
            double amt = double.Parse(Console.ReadLine());

            Accounts userAcc = new Accounts(accNo, custName, accType, transType, amt);
            userAcc.Showdata();
            Console.ReadLine();
        }
    }
}
