using DataLayer.EfClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServiceLayer.DatabaseServices.Concrete
{
    public static  class BookJsonLoader
    {
        public static IEnumerable<Book> LoadBooks(string fileDir, string fileSearchString)
        {
            var filePath = GetJsonFilePath(fileDir, fileSearchString);
            var jsonDecoded = JsonConvert.DeserializeObject<ICollection<BookInfoJson>>(File.ReadAllText(filePath));

            var authDict = new Dictionary<string, Author>();
            var tagDict = new Dictionary<string, Tag>();

            foreach (var bookInfoJson in jsonDecoded)
            {
                foreach (var author in bookInfoJson.authors)
                {
                    if (!authDict.ContainsKey(author))
                    {
                        authDict[author] = new Author { Name = author };
                    }
                }
                foreach (var category in bookInfoJson.categories)
                {
                    if (!tagDict.ContainsKey(category))
                    {
                        tagDict[category] = new Tag { TagId = category };
                    }
                }
            }
            return jsonDecoded.Select(x => CreateBookWithRefs(x, authDict, tagDict));
        }

        private static Book CreateBookWithRefs(BookInfoJson x, Dictionary<string, Author> authDict, Dictionary<string, Tag> tagDict)
        {
            var book = new Book
            {
                Title = x.title,
                Description = x.description,
                PublishedOn = DecodePublishDate(x.publishedDate),
                Publisher = x.publisher,
                Price = (decimal)(x.saleInfoListPriceAmount ?? -1),
                ImageUrl = x.imageLinksThumbnail
            };

            byte i = 0;
            book.AuthorsLink = new List<BookAuthor>();
            foreach (var author in x.authors)
            {
                book.AuthorsLink.Add(new BookAuthor { Book = book, Author = authDict[author], Order = i++ });
            }
            book.Tags = new List<Tag>();
            foreach (var category in x.categories)
            {
                book.Tags.Add(tagDict[category]);
            }
            if (x.averageRating != null)
            {
                book.Reviews = CalculateReviewsToMatch((double)x.averageRating, (int)x.ratingsCount);
            }
            return book;
        }

        private static ICollection<Review> CalculateReviewsToMatch(double averageRating, int ratingsCount)
        {
            var reviews = new List<Review>();
            var currentAve = averageRating;
            for (int i = 0; i < ratingsCount; i++)
            {
                reviews.Add(new Review
                {
                    VoterName = "anonymous",
                    NumStars = (int)(currentAve > averageRating ? Math.Truncate(averageRating) : Math.Ceiling(averageRating))
                });
                currentAve = reviews.Average(x => x.NumStars);
            }
            return reviews;
        }

        private static DateTime DecodePublishDate(string publishedDate)
        {
            var split = publishedDate.Split('-');
            switch (split.Length)
            {
                case 1:
                    return new DateTime(int.Parse(split[0]), 1, 1);
                case 2:
                    return new DateTime(int.Parse(split[0]), int.Parse(split[1]), 1);
                case 3:
                    return new DateTime(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
               
            }
            throw new InvalidOperationException($"The json publishDate failed to decode: string was {publishedDate}");
        }

        private static string GetJsonFilePath(string fileDir, string fileSearchString)
        {
            var fileList = Directory.GetFiles(fileDir, fileSearchString);

            if (fileList.Length == 0)
            {
                throw new FileNotFoundException($"Could not find a file with the search name of {fileSearchString} in directory {fileDir}");
            }
            return fileList.ToList().OrderBy(x => x).Last();
        }
    }
}
