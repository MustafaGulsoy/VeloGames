using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;


namespace VeloGames
{



    public class Book
    {

        public string title { get; set; }
        public string author { get; set; }
        public string ISBN { get; set; }
        public int copyCount { get; set; }
        public List<Book> bookOnLean { get; set; }
        public bool expired { get; set; }
        public string takenDate { get; set; }
        public string dropOffDate { get; set; }


        public Book(string title, string author, string takenDate, string dropOffDate, int copyCount, bool expired)
        {
            this.title = title;
            this.author = author;
            this.takenDate = takenDate;
            this.dropOffDate = dropOffDate;
            this.expired = expired;
            this.ISBN = Guid.NewGuid().ToString();
            this.copyCount = copyCount;
        }

    }
    public static class Library
    {
        static List<Book> libraryData = new List<Book>();

        static string path = "datas.json";
        static void Main()
        {



            Console.WriteLine("Hello, World!");

            //string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            //File.WriteAllText("veri.json", jsonString);
            Console.WriteLine("Hello, World!");

            SetUp();
            Loop();


            //Console.WriteLine("Girdiğiniz sayı: " + sayi);

        }

        static List<Book> GetAllBooks()
        {
            List<Book> curentData;
            if (File.Exists(path))
            {
                string jsonVeri = File.ReadAllText(path);
                curentData = JsonSerializer.Deserialize<List<Book>>(jsonVeri);
                foreach (var book in curentData)
                {
                    ShowBookData(book);

                }
            }
            else
            {
                curentData = new List<Book>();
            }




            return new List<Book>();

        }
        static List<Book> GetBooks(string authorName = "", string title = "")
        {
            List<Book> wantedBooks = new List<Book>();
            List<Book> curentData;
            if (File.Exists(path))
            {
                string jsonVeri = File.ReadAllText(path);
                curentData = JsonSerializer.Deserialize<List<Book>>(jsonVeri);

                if (authorName != "")
                    wantedBooks = curentData.FindAll(book => book.author == authorName);

                if (title != "")
                    wantedBooks = curentData.FindAll(book => book.author == authorName);

                if (title != "" && authorName != "")
                    wantedBooks = curentData.FindAll(book => book.author == authorName && book.title == title);


            }

            if (wantedBooks.Count == 0)
            {
                Console.WriteLine("Arama yaptığınız bilgilerle eslesen bir kitap bulunamadı.");

                return null;
            }
            return wantedBooks;
        }





        static void AddBook()
        {
            bool bookSaved = false;
            int copyCount = 1;



            Console.WriteLine("Lütfen eklemek istediginiz kitap baslığını giriniz.");
            string title = Console.ReadLine();
            Console.WriteLine("Lütfen eklemek istediginiz kitabın yazarını giriniz.");
            string author = Console.ReadLine();
            string takenDate = DateTime.Now.ToString();
            string dropOffDate = DateTime.Now.AddDays(2).ToString();




            List<Book> currentData;
            if (File.Exists(path))
            {
                string jsonVeri = File.ReadAllText(path);
                currentData = JsonSerializer.Deserialize<List<Book>>(jsonVeri);
            }
            else
            {
                currentData = new List<Book>();
            }


            foreach (Book book in currentData)
            {
                if (book.title == title && book.author == author)
                {
                    book.copyCount++;
                    copyCount = book.copyCount;
                    bookSaved = true;

                }
            }

            currentData.Add(new Book(title, author, "", "", copyCount, false));


            string yeniJsonVeri = JsonSerializer.Serialize(currentData, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(path, yeniJsonVeri);

            Console.WriteLine("Veri başarıyla eklendi.");


        }

        static Book ReturnBook(string ISBN)
        {
            Book wantedBook;
            List<Book> currentData = new List<Book>();
            if (File.Exists(path))
            {
                string jsonVeri = File.ReadAllText(path);
                currentData = JsonSerializer.Deserialize<List<Book>>(jsonVeri);
            }
            else
            {
                Console.WriteLine("Aranan dosya bulunmamaktadır.");
                return null;
            }
            wantedBook = currentData.FirstOrDefault<Book>(book => book.ISBN == ISBN);
            if (wantedBook != null)
                Console.WriteLine("Aranan Kitaba ait bir kayıt bulunamadı.");
            wantedBook.dropOffDate = DateTime.Now.ToString();

            return wantedBook;



        }

        static Book BorrowingBooks(string ISBN)
        {


            Book wantedBook;
            List<Book> currentData = new List<Book>();
            if (File.Exists(path))
            {
                string jsonVeri = File.ReadAllText(path);
                currentData = JsonSerializer.Deserialize<List<Book>>(jsonVeri);
            }
            else
            {
                Console.WriteLine("Aranan dosya bulunmamaktadır.");
                return null;
            }

            wantedBook = currentData.FirstOrDefault(book => book.ISBN == ISBN);
            if (wantedBook == null)
            {
                Console.WriteLine("Aranan Kitaba ait bir kayıt bulunamadı.");
                return null;
            }
            wantedBook.takenDate = DateTime.Now.ToString();
            foreach (Book book in GetBooks(wantedBook.author, wantedBook.title))
            {
                if (book.copyCount <= 0)
                {
                    Console.WriteLine("Aranan Kitaba şuan başka bir okuyucu tarafından okunuyor, lutfen daha sonra tekrar deneyiniz");
                    break;
                }

                book.copyCount--;



            };


            string yeniJsonVeri = JsonSerializer.Serialize(currentData, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(path, yeniJsonVeri);



            ShowBookData(wantedBook);
            return wantedBook;


        }
        static void GetInforAboutxpiredBooks()
        {

            TimeSpan fark; ;
            Book wantedBook;
            List<Book> currentData = new List<Book>();
            if (File.Exists(path))
            {
                string jsonVeri = File.ReadAllText(path);
                currentData = JsonSerializer.Deserialize<List<Book>>(jsonVeri);

                foreach (var item in currentData)
                {
                    if (item.expired == true)
                        ShowBookData(item);


                }
            }
        }
        static void SetUp()
        {


        }

        static void Loop()
        {
            int chose;
            while (true)
            {


                Console.WriteLine("" +
                    "\n 1 - Kütüphaneye yeni bir kitap ekle." +
                    "\n 2 - Kütüphanedeki tüm kitapların listesini görüntüleyin." +
                    "\n 3 - Bir kitabı başlığına veya yazarına göre arayın" +
                    "\n 4 - Bir kitap ödünç alın" +
                    "\n 5 - Bir kitabı iade edin " +
                    "\n 6 - Süresi geçmiş kitaplarla ilgili bilgileri görüntüleyin" +
                    "\n Lütfen Yapmak istediğiniz işlemi seçiniz.");
                chose = Convert.ToInt32(Console.ReadLine());
                CheckExpired();

                switch (chose)
                {

                    case 1:
                        AddBook();
                        break;
                    case 2:
                        GetAllBooks();
                        break;

                    case 3:

                        GetBooks("test", "test");
                        break;
                    case 4:
                        Console.WriteLine("Lutfen odunc almak istediginiz kitabin ISBN'sini giriniz ");
                        BorrowingBooks(Console.ReadLine());
                        break;

                    case 5:
                        ReturnBook("test");
                        break;

                    case 6:
                        GetInforAboutxpiredBooks();
                        break;

                    default:
                        Console.WriteLine("Gecersiz islem yaptınız Lütfen tekrar deneyin");
                        break;
                }

            }
        }

        static void CheckExpired()
        {
            TimeSpan fark; ;
            Book wantedBook;
            List<Book> currentData = new List<Book>();
            if (File.Exists(path))
            {
                string jsonVeri = File.ReadAllText(path);
                currentData = JsonSerializer.Deserialize<List<Book>>(jsonVeri);

                foreach (var item in currentData)
                {

                    if (item.takenDate != "")
                    {
                        fark = DateTime.Parse(item.takenDate).AddDays(2) - DateTime.Now;
                        if (fark.TotalDays >= 2)
                            item.expired = true;
                    }
                }

            }

            string yeniJsonVeri = JsonSerializer.Serialize(currentData, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(path, yeniJsonVeri);




        }
        static void ShowBookData(Book book)
        {
            Console.WriteLine("Book ISBN: " + book.ISBN);
            Console.WriteLine("Book title: " + book.title);
            Console.WriteLine("Book author: " + book.author);
            Console.WriteLine("Book takenDate: " + book.takenDate);
            Console.WriteLine("Book dropOffDate: " + book.dropOffDate);
            Console.WriteLine("Book expired: " + book.expired);
            Console.WriteLine("Book copyCount: " + book.copyCount);
            Console.WriteLine("------------------------");

        }
    }



}
