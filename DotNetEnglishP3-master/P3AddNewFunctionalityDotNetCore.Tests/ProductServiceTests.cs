
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Data;
using Moq;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace P3AddNewFunctionalityDotNetCore.Tests

{
    public class ProductServiceTests
    {

        static List<string> ValidityResult(ProductViewModel product)
        {
            var mockCart = Mock.Of<ICart>();
            var mockProductRepository = Mock.Of<IProductRepository>();
            var mockOrderRepository = Mock.Of<IOrderRepository>();
            var mockLocalizer = Mock.Of<IStringLocalizer<ProductService>>();

            var productService = new ProductService(
                mockCart,
                mockProductRepository,
                mockOrderRepository,
                mockLocalizer
            );
          return  productService.CheckProductModelErrors(product);
        }

        [Fact]
        public void MissingDataTest()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("en");
            var product = new ProductViewModel
            {
                Name = "",
                Price = "",
                Stock = "",
                Description = "Valid description",
                Details = "Valid details"
            };

            //Act
            var result = ValidityResult(product);

            //Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("Please enter a name", result);
            Assert.Contains("Please enter a price", result);
            Assert.Contains("Please enter a stock value", result);

        }

        [Fact]
        public void NegativePriceAndStockTest()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("en");
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "-100",
                Stock = "-5",
                Description = "Valid description",
                Details = "Valid details"
            };

            //Act
            var result = ValidityResult(product);

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("The stock must be a positive integer", result);
            Assert.Contains("The price must be a positive number", result);
        }

        [Fact]
        public void InvalidPriceAndStockTest()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("en");
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "e5",
                Stock = "abc",
                Description = "Valid description",
                Details = "Valid details"
            };

            //Act
            var result = ValidityResult(product);

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("The stock must be a positive integer", result);
            Assert.Contains("The price must be a positive number", result);
        }

        [Fact]
        public void ValidProductTest()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Phone",
                Price = "100",
                Stock = "5",
                Description = "Valid description",
                Details = "Valid details"
            };

            //Act
            var result = ValidityResult(product);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CommaSeparatorTest()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("en");
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "100,5",
                Stock = "5,5",
                Description = "Valid description",
                Details = "Valid details"
            };

            //Act
            var result = ValidityResult(product);

            //Assert
            Assert.Single(result);
            Assert.Contains("The stock must be a positive integer", result);
        }

        [Fact]
        public void DotSeparatorTest()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("en");
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "100.5",
                Stock = "5.5",
                Description = "Valid description",
                Details = "Valid details"
            };

            //Act
            var result = ValidityResult(product);

            //Assert
            Assert.Single(result);
            Assert.Contains("The stock must be a positive integer", result);
        }            
             
        [Fact]
        public void MissingDataInFrenchTest()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "",
                Price = "",
                Stock = "",
                Description = "",
                Details = ""
            };

            //Act
            var result = ValidityResult(product);

            //Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("Veuillez saisir un nom", result);
            Assert.Contains("Veuillez saisir un prix", result);
            Assert.Contains("Veuillez saisir une quantité", result);          
        }

        [Fact]
        public void InvalidDataInFrenchTest()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "price",
                Stock = "-10.5",
                Description = "",
                Details = ""
            };

            //Act
            var result = ValidityResult(product);

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("Le stock doit être un entier positif", result);
            Assert.Contains("Le prix doit être un nombre positif", result);
        }
    }
}
        
 
