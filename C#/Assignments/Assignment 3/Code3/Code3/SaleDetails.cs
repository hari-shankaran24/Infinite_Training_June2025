using System;

class Saledetails
{
    public static int SalesNo;
    public static int ProductNo;
    public static int Qty;
    public static double Price;
    public static string DateOfSale;
    public static double TotalAmount;

    public static void Sales()
    {
        TotalAmount = Qty * Price;
    }

    public static void ShowData()
    {
        Console.WriteLine("\n--- Sale Details ---");
        Console.WriteLine($"Sales No      : {SalesNo}");
        Console.WriteLine($"Product No    : {ProductNo}");
        Console.WriteLine($"Quantity      : {Qty}");
        Console.WriteLine($"Price         : {Price}");
        Console.WriteLine($"Date of Sale  : {DateOfSale}");
        Console.WriteLine($"Total Amount  : {TotalAmount}");
    }
}

class Sales_program

{
    static void Main()
    {
        Console.Write("Enter Sales No: ");
        Saledetails.SalesNo = int.Parse(Console.ReadLine());

        Console.Write("Enter Product No: ");
        Saledetails.ProductNo = int.Parse(Console.ReadLine());

        Console.Write("Enter Quantity: ");
        Saledetails.Qty = int.Parse(Console.ReadLine());

        Console.Write("Enter Price per Unit : ");
        Saledetails.Price = double.Parse(Console.ReadLine());

        Console.Write("Enter Date of Sale: ");
        Saledetails.DateOfSale = Console.ReadLine();

        Saledetails.Sales();
        Saledetails.ShowData();

        Console.ReadLine();
    }
}
