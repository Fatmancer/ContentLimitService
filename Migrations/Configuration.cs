namespace ContentLimitService.Migrations
{
    using ContentLimitService.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ContentLimitServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ContentLimitServiceContext context)
        {
            var seedItems = new List<Item>
            {
                new Item() { Id = 1, Name = "TV", Value = 350F, CategoryId = 1, Category = "Electronics"},
                new Item() { Id = 2, Name = "Steak", Value = 8.50F, CategoryId = 3, Category = "Kitchen" },
                new Item() { Id = 3, Name = "Sweater", Value = 45F, CategoryId = 2, Category = "Clothing" },
                new Item() { Id = 4, Name = "Kitchen Sink", Value = 350F, CategoryId = 3, Category = "Kitchen" },
                new Item() { Id = 5, Name = "Awesome Hat", Value = 1000F, CategoryId = 2, Category = "Clothing" },
                new Item() { Id = 6, Name = "Fridge", Value = 800F, CategoryId = 3, Category = "Kitchen" },
                new Item() { Id = 7, Name = "Nintendo Switch", Value = 600F, CategoryId = 1, Category = "Electronics" },
                new Item() { Id = 8, Name = "Table", Value = 150F, CategoryId = 4, Category = "Misc" }
            };
            seedItems.ForEach(c => context.Items.AddOrUpdate(x => x.Id, c));

            var seedCategories = new List<Category>
            {
                new Category() { Id = 1, Name = "Electronics", Items = new Collection<Item>() },
                new Category() { Id = 2, Name = "Clothing", Items = new Collection<Item>() },
                new Category() { Id = 3, Name = "Kitchen", Items = new Collection<Item>() },
                new Category() { Id = 4, Name = "Misc", Items = new Collection<Item>() }
            };

            foreach (Item i in seedItems)
            {
                seedCategories[i.CategoryId - 1].Items.Add(i);
            }
            seedCategories.ForEach(c => context.Categories.AddOrUpdate(x => x.Id, c));
        }
    }
}
