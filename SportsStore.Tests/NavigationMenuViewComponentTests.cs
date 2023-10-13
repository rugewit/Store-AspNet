using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void CanSelectCategories()
        {
            // Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            }).AsQueryable<Product>());

            var target = new NavigationMenuViewComponent(mock.Object);

            // Act = get the set of categories
            var results = ((target.Invoke() as ViewViewComponentResult)
                .ViewData.Model
                as IEnumerable<string>)
                .ToArray();
            // Assert
            var expected = new[]{ "Apples", "Oranges", "Plums" };
            Assert.True(Enumerable.SequenceEqual(expected, results));
        }

        [Fact]
        public void IndicatesSelectedCategory()
        {
            // Arrange
            var categoryToSelect = "Apples";

            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 4, Name = "P2", Category = "Oranges"},
            }).AsQueryable<Product>());

            var target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            // Action
            var result = (string) (target.Invoke() as 
                ViewViewComponentResult).ViewData["SelectedCategory"];

            // Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
