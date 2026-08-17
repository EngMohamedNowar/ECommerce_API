using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ECommerce.Infrastructure.Persistence.Seeding.Data
{
    public static class JsonSeeder
    {
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task SeedIfEmpty<Tentity, Tmodel>(
            DbSet<Tentity> dbSet,
            string fileName,
            Func<Tmodel,Tentity> map,
            CancellationToken ct = default
            ) where Tentity : BaseEntity
        {
            if (await dbSet.AnyAsync(ct))
                return;
            var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Seeding", "Data", fileName); // catch Path file
            if (!File.Exists(filePath)) return; //check 
            await using var stream = File.OpenRead(filePath); // read file data
            var models = await JsonSerializer.DeserializeAsync<List<Tmodel>>(stream, options, ct);// convert file from Json to object data
            if (models is null || models.Count == 0) return;
            await dbSet.AddRangeAsync(models.Select(map),ct);
        }
    }
}
