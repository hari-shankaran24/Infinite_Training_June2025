using System;
//Question 2. Create a Class called Products with Productid, Product Name, Price. Accept 10 Products, sort them based on the price, and display the sorted Products

namespace Code_Challenge_2
{
    public class Products
    {
        public int ProductId;
        public string ProductName;
        public double Price;
    }

    class Products_Sort
    {
        static void Main()
        {
            Products[] product = new Products[10];

            for (int i = 0; i < 10; i++)
            {
                product[i] = new Products();
                Console.WriteLine($"Product {i + 1}:");
                Console.Write("ID: ");
                product[i].ProductId = int.Parse(Console.ReadLine());

                Console.Write("Name: ");
                product[i].ProductName = Console.ReadLine();

                Console.Write("Price: ");
                product[i].Price = double.Parse(Console.ReadLine());
            }

            Array.Sort(product, (a, b) => a.Price.CompareTo(b.Price));

            Console.WriteLine("\nProducts Sorted by Price:");
            foreach (var p in product)
            {
                Console.WriteLine($"ID: {p.ProductId}, Name: {p.ProductName}, Price: {p.Price}");
            }

            Console.ReadLine();
        }
    }
}
