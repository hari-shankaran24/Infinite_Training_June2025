using System;

namespace Code6
{
    class Books
    {
        public string BookName;
        public string AuthorName;
        public Books(string bookName, string authorName)
        {
            BookName = bookName;
            AuthorName = authorName;
        }
        public void Display()
        {
            Console.WriteLine("Book Name: " + BookName);
            Console.WriteLine("Author Name: " + AuthorName);
        }
    }
    class BookShelf
    {
        public Books[] shelf = new Books[5];
        public Books this[int index]
        {
            get { return shelf[index]; }
            set { shelf[index] = value; }
        }

        public void DisplayBooks()
        {
            for (int i = 0; i < shelf.Length; i++)  
            {
                if (shelf[i] != null)
                {
                    Console.WriteLine($"\nSlot {i + 1}:");
                    shelf[i].Display();
                }
                else
                {
                    Console.WriteLine($"Slot {i + 1}: [Empty]");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BookShelf shelf = new BookShelf();


            Console.WriteLine("Enter the details of 5 books:");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"\nBook {i + 1}:");
                Console.Write("Enter the Book Name: ");
                string bookName = Console.ReadLine();
                Console.Write("Enter the Author Name: ");
                string authorName = Console.ReadLine();
                shelf[i] = new Books(bookName, authorName);
            }

            shelf.DisplayBooks();

            Console.ReadLine(); 
        }
    }
}
