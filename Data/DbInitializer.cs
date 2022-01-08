using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pascu_Serban_Proiect.Models;

namespace Pascu_Serban_Proiect.Data
{
    public class DbInitializer
    {
        public static void Initialize(ToyShopContext context)
        {
            context.Database.EnsureCreated();

            if (context.Toys.Any())
            {
                return;
            }

            var toys = new Toy[]
            {
                new Toy{Name="Catan - jocul de baza", Description="Catan este numita insula pe care tu si prietenii tai ati descoperit-o. Sunteti primii locuitori. Sunt construite asezari, apoi drumuri. Materiile prime (resursele) ajung de la sate la orase prin intermediul comertului.", Price=Decimal.Parse("110")},
                new Toy{Name="Barbie, Descopera culoarea - Tinute stralucitoare", Description="Papusile Barbie Color Reveal asigura o experienta de despachetare plina de surprize, sapte la numar! Este garantata o papusa care poarta o tinuta stralucitoare, dar aspectul fiecarei papusi ramane un mister pana cand este dezvaluit.", Price=Decimal.Parse("120")},
                new Toy{Name="Catelusul vorbitor cu etape de dezvoltare, limba romana", Description="Copiii vor rade, vor invata si se vor juca cu noul Catelus Vorbitor cu continut educativ pe etape de dezvoltare!", Price=Decimal.Parse("170")},
                new Toy{Name="Jucarie din plus Olaf, 20 cm", Description="Jucarie de plus Regatul de Gheata Olaf, 20 cm", Price=Decimal.Parse("30")}
            };
            foreach(Toy t in toys)
            {
                context.Toys.Add(t);
            }
            context.SaveChanges();

            var customers = new Customer[]
            {
                new Customer{CustomerID=100 ,Name="Oprea Diana", BirthDate=DateTime.Parse("1980-03-06")},
                new Customer{CustomerID=150 ,Name="Pustea Gabriel", BirthDate=DateTime.Parse("1976-04-12")},
                new Customer{CustomerID=200 ,Name="Avram Lavinia", BirthDate=DateTime.Parse("1982-08-10")},
                new Customer{CustomerID=250 ,Name="Marinescu Silviu", BirthDate=DateTime.Parse("1995-07-01")},
            };
            foreach(Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();

            var workers = new Worker[]
            {
                new Worker{WorkerID=10 ,Name="Popescu Angela", BirthDate=DateTime.Parse("1969-07-02")},
                new Worker{WorkerID=15 ,Name="Porumb Dragos", BirthDate=DateTime.Parse("1970-05-11")},
                new Worker{WorkerID=20 ,Name="Gheorghescu Viviana", BirthDate=DateTime.Parse("1988-02-06")},
                new Worker{WorkerID=25 ,Name="Chis Alexandru", BirthDate=DateTime.Parse("1991-08-02")},
            };
            foreach (Worker w in workers)
            {
                context.Workers.Add(w);
            }
            context.SaveChanges();

            var orders = new Order[]
            {
                new Order{CustomerID= 250, ToyID= 3,WorkerID= 10,OrderDate=DateTime.Parse("02-05-2021")},
                new Order{CustomerID= 150, ToyID= 1,WorkerID= 25,OrderDate=DateTime.Parse("10-04-2021")},
                new Order{CustomerID= 200,ToyID= 2,WorkerID= 25,OrderDate=DateTime.Parse("08-09-2021")},
                new Order{CustomerID= 200,ToyID= 4,WorkerID= 20,OrderDate=DateTime.Parse("10-08-2021")}
            };
            foreach (Order o in orders)
            {
                context.Orders.Add(o);
            }
            context.SaveChanges();

            var brands = new Brand[]
            {
                new Brand{BrandName="Kosmos", Adress="Str. Aviatorilor, nr. 14, Alba Iulia" },
                new Brand{BrandName="Barbie", Adress="Str. Victoriei, nr. 3, Bucuresti" },
                new Brand{BrandName="Fisher Price", Adress="Str. George Enescu, nr. 26, Sibiu" },
                new Brand{BrandName="Disney", Adress="Str. Lavandei, nr. 11, Cluj-Napoca" }
            };
            foreach(Brand b in brands)
            {
                context.Brands.Add(b);
            }
            context.SaveChanges();

            var brandedtoys = new BrandedToy[]
            {
                new BrandedToy
                {
                    ToyID = toys.Single(c => c.Name == "Catan - jocul de baza").ID,
                    BrandID = brands.Single(i => i.BrandName == "Kosmos").ID
                },
                new BrandedToy
                {
                    ToyID = toys.Single(c => c.Name == "Barbie, Descopera culoarea - Tinute stralucitoare").ID,
                    BrandID = brands.Single(i => i.BrandName == "Barbie").ID
                },
                new BrandedToy
                {
                    ToyID = toys.Single(c => c.Name == "Catelusul vorbitor cu etape de dezvoltare, limba romana").ID,
                    BrandID = brands.Single(i => i.BrandName == "Fisher Price").ID
                },
                new BrandedToy
                {
                    ToyID = toys.Single(c => c.Name == "Jucarie din plus Olaf, 20 cm").ID,
                    BrandID = brands.Single(i => i.BrandName == "Disney").ID
                },
            };
            foreach(BrandedToy bt in brandedtoys)
            {
                context.BrandedToys.Add(bt);
            }
            context.SaveChanges();
        }
    }
}
