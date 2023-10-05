namespace ServiceModule.Public;
public record AddServiceRequestDto(string Name, decimal Price, string Currency, int ValidFromWeekDayNumber, int ValidToWeekDayNumber);
