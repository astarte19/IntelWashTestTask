using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntelWash.Model
{
    public static class TestData
    {
        public static void InsertTestData(IServiceProvider serviceProvider)
        {
             using (var context = new ApplicationContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>()))
            {
                if (context.Products.Any())
                {
                    return; 
                }

                context.Products.AddRange(
                    new Product { Name = "Macbook pro 16", Price = 99000, Id = 1 },
                    new Product { Name = "iPhone 13 Pro Max", Price = 120000, Id = 2 },
                    new Product { Name = "iPad Pro 13", Price = 180000, Id = 3 },
                    new Product { Name = "Apple watch", Price = 45000, Id = 4 }
                );

                context.SalesPoints.AddRange(
                    new SalesPoint
                    {
                        Name = "Re:Store",
                        ProvidedProducts = new List<ProvidedProduct>
                        {
                                new ProvidedProduct { ProductId = 2, ProductQuantity = 21 },
                                new ProvidedProduct { ProductId = 3, ProductQuantity = 15 },
                                new ProvidedProduct { ProductId = 1, ProductQuantity = 12 },
                                new ProvidedProduct { ProductId = 4, ProductQuantity = 14 }
                        }
                    },
                    new SalesPoint
                    {
                        Name = "OZON",
                        ProvidedProducts = new List<ProvidedProduct>
                        {
                                new ProvidedProduct { ProductId = 1, ProductQuantity = 5 },
                                new ProvidedProduct { ProductId = 3, ProductQuantity = 12 },
                                new ProvidedProduct { ProductId = 2, ProductQuantity = 16 }
                        }
                    },
                    new SalesPoint
                    {
                        Name = "Mvideo",
                        ProvidedProducts = new List<ProvidedProduct>
                        {
                            new ProvidedProduct { ProductId = 1, ProductQuantity = 51 },
                            new ProvidedProduct { ProductId = 3, ProductQuantity = 22 },
                            new ProvidedProduct { ProductId = 2, ProductQuantity = 19 }
                        }
                    }
                );

                context.Buyers.AddRange
                    (
                        new Buyer { Name = "Елена", SalesId = new List<SalesId> { new SalesId { SaleId = 1 } } },
                        new Buyer { Name = "Ольга", SalesId = null }
                        
                    );

                context.Sales.AddRange
                    (
                        new Sale 
                        { 
                            BuyerId = 1, 
                            Date = DateTime.Now, 
                            Time = DateTime.Now, 
                            SalesPointId = 1, 
                            SalesData = new List<SaleData> 
                            { 
                                new SaleData(context) { ProductId = 1, ProductQuantity = 2 },
                                new SaleData(context) {ProductId = 3, ProductQuantity = 10 }
                            } 
                        },
                        new Sale
                        {
                            BuyerId = null,
                            Date = DateTime.Now,
                            Time = DateTime.Now,
                            SalesPointId = 2,
                            SalesData = new List<SaleData>
                            {
                                new SaleData(context) { ProductId = 2, ProductQuantity = 1 },
                                new SaleData(context) {ProductId = 1, ProductQuantity = 3 }
                            }
                        }
                    );

                context.SaveChanges();
            }
        }
        }
        
    }
