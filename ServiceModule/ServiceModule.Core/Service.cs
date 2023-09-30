namespace ServiceModule;

public class Service
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
}
