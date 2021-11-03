﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstEfCoreApp
{
    internal class Commands
    {
        internal static bool WipeCreateSeed(bool onlyIfNoDatabase)
        {
            using (var db = new AppDbContext())
            {
                if (onlyIfNoDatabase && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                {
                    return false;
                }
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                if (!db.Books.Any())
                {
                    WriteTestData(db);
                    Console.WriteLine("Seeded database");
                }
            }
            return true;
        }

        internal static void ChangeWebUrl()
        {
            Console.WriteLine("New Quantum Networking WebUrl > ");
            var newWebUrl = Console.ReadLine();

            using var db = new AppDbContext();

            var singleBook = db.Books.Include(book => book.Author)
                .Single(book => book.Title == "Quantum Networking");

            singleBook.Author.WebUrl = newWebUrl;
            db.SaveChanges();
            Console.WriteLine("... SaveChanges Called.");
        }

        internal static void ListAllWithLogs()
        {
            var logs = new List<string>();
            using (var db = new AppDbContext())
            {
                var serviceProvider = db.GetInfrastructure();
                var loggerFactory = (ILoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));
                loggerFactory.AddProvider(new MyLogProvider(logs));

                var books = db.Books.AsNoTracking().Include(book => book.Author);
                foreach (var entity in books)
                {
                    var webUrl = entity.Author.WebUrl == null
                        ? "- no web url given -"
                        : entity.Author.WebUrl;
                    Console.WriteLine($"{entity.Title} by {entity.Author.Name}");
                    Console.WriteLine($"Published on {entity.PublishedOn:dd:MMM:yyyy}. {webUrl}");
                }
            }
            Console.WriteLine("------ LOGS --- ");
            foreach (var log in logs)
            {
                Console.WriteLine(log);
            }
        }

        internal static void ChangeWebUrlWithLogs()
        {
            var logs = new List<string>();
            Console.WriteLine("New Quantum Networking WebUrl >");
            var newWebUrl = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                var serviceProvider = db.GetInfrastructure();
                var loggerFactory = (ILoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));
                loggerFactory.AddProvider(new MyLogProvider(logs));

                var singleBook = db.Books.Include(book => book.Author)
                    .Single(b => b.Title == "Quantum Networking");
                singleBook.Author.WebUrl = newWebUrl;
                db.SaveChanges();
                Console.WriteLine("... SaveChanges called.");
            }
        }

        internal static void ListAll()
        {
            using (var db = new AppDbContext())
            {
                var books = db.Books.AsNoTracking().Include(book=> book.Author);
                foreach (var book in books)
                {
                    var webUrl = book.Author.WebUrl ?? "- no web url given -";
                    Console.WriteLine($"{ book.Title} byte { book.Author.Name}");
                    Console.WriteLine($"     Published on  ${book.PublishedOn:dd-MMM-yyyy}. {webUrl}");
                }
            }
        }

        private static void WriteTestData(AppDbContext db)
        {
            var martinFowler = new Author
            {
                Name = "Martin Fowler",
                WebUrl = "http://martinfowler.com/"
            };
            var books = new List<Book>
            {
                new Book
                {
                    Title = "Refactoring",
                    Description = "Improving the design of existing code",
                    PublishedOn = new DateTime(1999, 7, 8),
                    Author = martinFowler
                },
                new Book
                {
                    Title = "Patterns of Enterprise Application Architecture",
                    Description = "Written in direct response to the stiff challenges",
                    PublishedOn = new DateTime(2002, 11, 15),
                    Author = martinFowler
                },
                new Book
                {
                    Title = "Domain-Driven Design",
                    Description = "Linking business needs to software design",
                    PublishedOn = new DateTime(2003, 8, 30),
                    Author = new Author {Name = "Eric Evans", WebUrl = "http://domainlanguage.com/"}
                },
                new Book
                {
                    Title = "Quantum Networking",
                    Description = "Entangled quantum networking provides faster-than-light data communications",
                    PublishedOn = new DateTime(2057, 1, 1),
                    Author = new Author {Name = "Future Person"}
                }
            };

            db.Books.AddRange(books);
            db.SaveChanges();
        }
    }
}