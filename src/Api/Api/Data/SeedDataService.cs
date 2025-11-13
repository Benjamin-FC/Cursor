using Api.Domain;

namespace Api.Data;

public static class SeedDataService
{
    public static void SeedData(AppDbContext context)
    {
        if (context.Contacts.Any())
        {
            return; // Data already seeded
        }

        var contacts = new List<Contact>();
        var random = new Random(42); // Fixed seed for consistent data
        
        var firstNames = new[] { "John", "Jane", "Michael", "Sarah", "David", "Emily", "Robert", "Lisa", "James", "Mary" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
        var companies = new[] { "Tech Corp", "Digital Solutions", "Innovation Inc", "Global Systems", "Future Technologies", "Smart Solutions", "Advanced Systems", "Modern Tech", "Digital Innovations", "Tech Solutions" };
        var cities = new[] { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose" };
        var states = new[] { "NY", "CA", "IL", "TX", "AZ", "PA", "TX", "CA", "TX", "CA" };
        
        var contactCount = 153;
        
        for (int i = 1; i <= contactCount; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var company = companies[random.Next(companies.Length)];
            var city = cities[random.Next(cities.Length)];
            var state = states[random.Next(states.Length)];
            
            contacts.Add(new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com",
                Phone = $"555-{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                Company = company,
                AddressLine1 = $"{random.Next(100, 999)} Main St",
                AddressLine2 = random.Next(0, 2) == 1 ? $"Apt {random.Next(1, 999)}" : null,
                City = city,
                State = state,
                PostalCode = $"{random.Next(10000, 99999)}",
                Country = "USA",
                IsActive = random.Next(0, 10) > 1, // 90% active
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 365)),
                UpdatedAt = DateTimeOffset.UtcNow.AddDays(-random.Next(0, 30))
            });
        }
        
        context.Contacts.AddRange(contacts);
        context.SaveChanges();
    }
}

