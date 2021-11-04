﻿using DataLayer.EfClasses;
using System;
using System.Collections.Generic;

namespace ServiceLayer.DatabaseServices.Concrete
{
    public static class SpecialBook
    {
        public static Book CreateSpecialBook()
        {
            var book4 = new Book
            {
                Title = "Quantum Networking",
                Description = "Entangled quantum networking provides faster-than-light data communications",
                PublishedOn = new DateTime(2057, 1, 1),
                Price = 220,
                Tags = new List<Tag> { new Tag { TagId = "Quantum Entanglement" } }
            };
            book4.AuthorsLink = new List<BookAuthor>
                {new BookAuthor {Author = new Author {Name = "Future Person"}, Book = book4}};
            book4.Reviews = new List<Review>
            {
                new Review
                {
                    VoterName = "Jon P Smith", NumStars = 5,
                    Comment = "I look forward to reading this book, if I am still alive!"
                },
                new Review
                {
                    VoterName = "Albert Einstein", NumStars = 5, Comment = "I write this book if I was still alive!"
                }
            };
            book4.Promotion = new PriceOffer { NewPrice = 219, PromotionalText = "Save $1 if you order 40 years ahead!" };

            return book4;
        }
    }
}
