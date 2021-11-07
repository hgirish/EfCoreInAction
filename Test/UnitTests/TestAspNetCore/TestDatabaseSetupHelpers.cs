using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.EfCode;
using ServiceLayer.DatabaseServices;
using Test.TestHelpers;
using TestSupport.EfHelpers;
using TestSupport.Helpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.TestAspNetCore
{
    public class  TestDatabaseSetupHelpers
    {
        [Fact]
        public async Task TestSeedDatabaseIfNoBooksAsyncEmptyDatabase()
        {
            var options = this.CreateUniqueClassOptions<EfCoreContext>();

            using (var context = new EfCoreContext(options))
            {
                context.Database.EnsureClean();

                var callingAssemblyPath = TestData.GetCallingAssemblyTopLevelDir();

                var wwwRootDir = Path.GetFullPath(Path.Combine(callingAssemblyPath, "..\\BookApp\\wwwroot"));

                await context.SeedDatabaseIfNoBooksAsync(wwwRootDir);

                context.Books.Count().ShouldEqual(54);
            }

        }

        [Fact]
        public async Task TestSeedDatabaseIfNoBooksAsyncBooksAlreadyThere()
        {
            var options = SqliteInMemory.CreateOptions<EfCoreContext>();

            using (var context = new EfCoreContext(options))
            {
                context.Database.EnsureCreated();
                context.SeedDatabaseFourBooks();

                var callingAssemblyPath = TestData.GetCallingAssemblyTopLevelDir();
                var wwwRootDir = Path.GetFullPath(Path.Combine(callingAssemblyPath, "..\\BookApp\\wwwroot"));

                await context.SeedDatabaseIfNoBooksAsync(wwwRootDir);

                context.Books.Count().ShouldEqual(4);

            }
        }
    }
}
