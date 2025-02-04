﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoredContext dbContext)
        {

            if (!dbContext.ProductBrands.Any()) {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

                if (Brands?.Count > 0)
                {
                    foreach (var brand in Brands)
                        await dbContext.Set<ProductBrand>().AddAsync(brand);
                    await dbContext.SaveChangesAsync();
                } 
            }

            if (!dbContext.ProductCategories.Any())
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductCategory>>(TypesData);

                if (types?.Count > 0)
                {
                    foreach (var type in types)
                        await dbContext.Set<ProductCategory>().AddAsync(type);
                    await dbContext.SaveChangesAsync();
                }

            }

            if (!dbContext.Products.Any()) {
                var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                        await dbContext.Set<Product>().AddAsync(product);
                    await dbContext.SaveChangesAsync();
                }
            }

        }

    }
}
