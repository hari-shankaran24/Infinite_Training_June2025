using System;

namespace Code5
{
    class Books
    {
        public string BookName { get; set; }
        public string AuthorName { get; set; }

        public Books(string bookName, string authorName)
        {
            BookName = bookName;
            AuthorName = authorName;
        }

        public void Display()
        {
            Console.WriteLine($"Book: {BookName}, Author: {AuthorName}");
        }
    }

    class BookShelf
    {
        private Books[] bookArray = new Books[5];

        public Books this[int index]
        {
            get { return bookArray[index]; }
            set { bookArray[index] = value; }
        }
    }

    class BookShelves
    {
        static void Main(string[] args)
        {
            BookShelf shelf = new BookShelf();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"\nEnter details for Book {i + 1}:");

                Console.Write("Book Name: ");
                string bookName = Console.ReadLine();

                Console.Write("Author Name: ");
                string authorName = Console.ReadLine();

                shelf[i] = new Books(bookName, authorName);
            }

            Console.WriteLine("\nBooks on the Shelf:\n");

            for (int i = 0; i < 5; i++)
            {
                shelf[i].Display();
            }

            Console.ReadLine();
        }
    }
}
