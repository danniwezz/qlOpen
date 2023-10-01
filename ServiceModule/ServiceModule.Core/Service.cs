using Shared.Core;

namespace ServiceModule.Core;

public class Service
{

    public static Service Create(string name, decimal price, string currency)
    {
        return new Service
        {
            Id = IdGenerator.NewId(),
            Name = name,
            Price = price,
            Currency = currency
        };
    }

    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
}
