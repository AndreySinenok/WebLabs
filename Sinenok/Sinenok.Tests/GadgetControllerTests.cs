using Sinenok.Domain.Entities.Models;
using Sinenok.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sinenok.API.Data;
using Sinenok.API.Controllers;

namespace Sinenok.Tests
{
    public class GadgetControllerTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly IWebHostEnvironment _environment;
        public GadgetControllerTests()
        {
            _environment = Substitute.For<IWebHostEnvironment>();

            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below). 
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite,  including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data 
            using var context = new AppDbContext(_contextOptions);

            context.Database.EnsureCreated();

            var categories = new Category[]
               {
            new Category {Name="", NormalizedName="phones"},
            new Category {Name="", NormalizedName="consoles"}
               };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var gadgets = new List<Gadget>
                {
                    new Gadget { Name="", Description="", Price=0,
                        Category=categories.FirstOrDefault(c => c.NormalizedName == "phones") },
                    new Gadget { Name="", Description="", Price=0,
                        Category=categories.FirstOrDefault(c => c.NormalizedName == "phones") },
                    new Gadget { Name="", Description="", Price=0,
                        Category=categories.FirstOrDefault(c => c.NormalizedName == "consoles") },
                    new Gadget { Name="", Description="", Price=0,
                        Category=categories.FirstOrDefault(c => c.NormalizedName == "consoles") },
                    new Gadget { Name="", Description="", Price=0,
                        Category=categories.FirstOrDefault(c => c.NormalizedName == "consoles") }
                };

            context.AddRange(gadgets);
            context.SaveChanges();
        }
        public void Dispose() => _connection?.Dispose();
        AppDbContext CreateContext() => new AppDbContext(_contextOptions);

        // �������� ������� �� ��������� 
        [Fact]
        public async void ControllerFiltersCategory()
        {
            // arrange 
            using var context = CreateContext();
            var category = context.Categories.First();

            var controller = new GadgetsController(context, _environment);

            // act 
            var response = await controller.GetGadgets(category.NormalizedName);
            ResponseData<ListModel<Gadget>> responseData = response.Value;
            var dishesList = responseData.Data.Items; // ���������� ������ �������� 

            //assert 
            Assert.True(dishesList.All(d => d.CategoryId == category.Id));
        }

        // �������� �������� ���������� ������� 
        // ������ �������� - ������ �������� 
        // ������ �������� - ��������� ���������� ������� (��� �������, ��� ����� �������� 5) 
        [Theory]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        public async void ControllerReturnsCorrectPagesCount(int size, int qty)
        {
            using var context = CreateContext();
            var controller = new GadgetsController(context, _environment);

            // act 
            var response = await controller.GetGadgets(null, 1, size);
            ResponseData<ListModel<Gadget>> responseData = response.Value;
            var totalPages = responseData.Data.TotalPages; // ���������� ���������� �������

            //assert 
            Assert.Equal(qty, totalPages); // ���������� ������� ��������� 
        }

        [Fact]
        public async void ControllerReturnsCorrectPage()
        {
            using var context = CreateContext();
            var controller = new GadgetsController(context, _environment);
            // ��� ������� �������� 3 � ����� ���������� �������� 5 
            // �� 2-� �������� ������ ���� 2 ������� 
            int itemsInPage = 2;
            // ������ ������ �� ������ �������� 
            Gadget firstItem = context.Gadgets.ToArray()[3];

            // act 
            // �������� ������ 2-� �������� 
            var response = await controller.GetGadgets(null, 2);
            ResponseData<ListModel<Gadget>> responseData = response.Value;
            var gadgetsList = responseData.Data.Items; // ���������� ������ �������� 
            var currentPage = responseData.Data.CurrentPage; // ���������� ����� ������� ��������

            //assert 
            Assert.Equal(2, currentPage);// ����� �������� ��������� 
            Assert.Equal(2, gadgetsList.Count); // ���������� �������� �� �������� ����� 2
            Assert.Equal(firstItem.Id, gadgetsList[0].Id); // 1-� ������ � ������ ����������
        }
    }
}