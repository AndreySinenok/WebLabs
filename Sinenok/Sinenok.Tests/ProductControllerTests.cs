﻿using Sinenok.UI.Controllers;
using Sinenok.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sinenok.Domain;
using Sinenok.Domain.Entities.Models;
using Sinenok.Domain.Entities;

namespace Sinenok.Tests
{
    public class ProductControllerTests
    {
        IProductService _productService;
        ICategoryService _categoryService;
        public ProductControllerTests()
        {
            SetupData();
        }
        // Список категорий сохраняется во ViewData 
        [Fact]
        public async void IndexPutsCategoriesToViewData()
        {
            //arrange 
            var controller = new ProductController(_productService, _categoryService);

            //act 
            var response = await controller.Index(null);

            //assert 
            var view = Assert.IsType<ViewResult>(response);
            var categories = Assert.IsType<List<Category>>(view.ViewData["categories"]);
            Assert.Equal(3, categories.Count);
            Assert.Equal("Все", view.ViewData["currentCategory"]);

        }
        // Имя текущей категории сохраняется во ViewData 
        [Fact]
        public async void IndexSetsCorrectCurrentCategory()
        {
            //arrange 
            var categories = await _categoryService.GetCategoryListAsync();
            var currentCategory = categories.Data[0];
            var controller = new ProductController(_productService, _categoryService);

            //act 
            var response = await controller.Index(currentCategory.NormalizedName);

            //assert 
            var view = Assert.IsType<ViewResult>(response);

            Assert.Equal(currentCategory.Name, view.ViewData["currentCategory"]);
        }
        // В случае ошибки возвращается NotFoundObjectResult 
        [Fact]
        public async void IndexReturnsNotFound()
        {
            //arrange         
            string errorMessage = "Test error";
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Success = false;
            categoriesResponse.ErrorMessage = errorMessage;

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse))
             ;
            var controller = new ProductController(_productService, _categoryService);

            //act 
            var response = await controller.Index(null);

            //assert 
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(errorMessage, result.Value.ToString());
        }
        // Настройка имитации ICategoryService и IProductService 
        void SetupData()
        {
            _categoryService = Substitute.For<ICategoryService>();
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Data = new List<Category>
        {
            new Category {Id=1, Name="Телефоны", NormalizedName="phones"},
            new Category {Id=2, Name="Игровые приставки", NormalizedName="consoles"},
            new Category {Id=3, Name="Умные часы", NormalizedName="watches"},
        };

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse))
             ;

            _productService = Substitute.For<IProductService>();

            var gadgets = new List<Gadget>
        {
            new Gadget {Id = 1 },
            new Gadget { Id = 2 },
            new Gadget { Id = 3 },
            new Gadget { Id = 4 },
            new Gadget { Id = 5 }
        };

            var productResponse = new ResponseData<ListModel<Gadget>>();
            productResponse.Data = new ListModel<Gadget> { Items = gadgets };
            _productService.GetProductListAsync(Arg.Any<string?>(), Arg.Any<int>())
                .Returns(productResponse);
        }
    }
}