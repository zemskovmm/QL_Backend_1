using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using Npgsql;
using QuartierLatin.CityMapper;

await using var db = new NpgsqlConnection("Server=127.0.0.1;User id=postgres;password=postgres;database=ql-old");
db.Open();
await using var trx = await db.BeginTransactionAsync();
await using var loadCityCommand = new NpgsqlCommand(@"
    SELECT ""Cities"".""CityId"", ""Cities"".""Key"", ""Cities"".""ParentCityId"" FROM ""Cities""
    JOIN ""CatalogObjects"" CO ON ""Cities"".""CityId"" = CO.""CityId"";
", db, trx);

await using var loadHousingInfo = new NpgsqlCommand(@"
    SELECT ""CityId"", ""Key""  FROM ""CatalogObjects"" where ""CatalogCategoryId"" = 3;
", db, trx);

// Reading cities
await using var reader = await loadCityCommand.ExecuteReaderAsync();
await using var readerStream = reader.ToEnumerableStream<OldCity>(x => new()
{
    Id = x.GetInt32("CityId"),
    Name = x.GetString("Key"),
    ParentId = x["ParentCityId"] is DBNull ? 0 : x.GetInt32("ParentCityId")
});

var cities = await readerStream.ToListAsync();

// Reading housings
await using var housingReader = await loadHousingInfo.ExecuteReaderAsync();
await using var housingReaderStream = housingReader.ToEnumerableStream<Housings>(x => new()
{
    Name = x.GetString("Key"),
    CityId = x.GetInt32("CityId")
});

var housings = await housingReaderStream.ToListAsync();

var result = new Dump { Cities  = cities, Housings = housings };
File.WriteAllText("dump.json", JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));

public record OldCity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ParentId { get; set; }
}
public record Housings
{
    public string Name { get; set; }
    
    public int CityId { get; set; }
}
public record Dump
{
    public List<OldCity> Cities { get; set; }
    public List<Housings> Housings { get; set; }
}