using Shared.Core;

namespace ServiceModule.Core;

public class Service
{

    public static Service Create(string name, decimal price, string currency, int validFromWeekDayNumber, int validToWeekDayNumber)
    {
        return new Service
        {
            Id = IdGenerator.NewId(),
            Name = name,
            Price = price,
            Currency = currency,
            ValidFromWeekDayNumber = validFromWeekDayNumber,
            ValidToWeekDayNumber = validToWeekDayNumber
        };
    }

    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
    public int ValidFromWeekDayNumber { get; set; }
    public int ValidToWeekDayNumber { get; set; }
}
