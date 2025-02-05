using Application.DTOS;
using Core.Entities;
using System.Text.Json;

namespace Infrastructure
{
    public static class GovernorateCityInitializer
    {
        public static async Task AddDateSeeding(ApplicationDBContext context)
        {

            await SeedGovernorates(context);
            await SeedCities(context);
        }

        private static void ClearExistingData(ApplicationDBContext context)
        {
            context.Governorates.RemoveRange(context.Governorates);
            context.Cities.RemoveRange(context.Cities);
            context.SaveChanges();
        }

        private static async Task SeedGovernorates(ApplicationDBContext context)
        {
            if (!context.Governorates.Any())
            {
                var governoratesData = await File.ReadAllTextAsync("../Infrastructure/DataSeeding/Governorate.json");
                var governoratesObjects = JsonSerializer.Deserialize<List<Governorate>>(governoratesData);
                if (governoratesObjects?.Count() > 0)
                {
                    context.Governorates.AddRange(governoratesObjects);
                    context.SaveChanges();
                }
            }
        }

        private static async Task SeedCities(ApplicationDBContext context)
        {
            if (!context.Cities.Any())
            {
                var citiesData = await File.ReadAllTextAsync("../Infrastructure/DataSeeding/Cities.json");
                var citiesObjects = JsonSerializer.Deserialize<List<CityDataDTO>>(citiesData);
                if (citiesObjects?.Count() > 0)
                {
                    foreach (var cityData in citiesObjects)
                    {
                        var cities = cityData.Cities.Select(cityName => new City
                        {
                            GovernorateID = cityData.GovernorateID,
                            Name = cityName
                        });

                        context.Cities.AddRange(cities);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
