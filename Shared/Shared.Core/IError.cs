namespace Shared.Core;

public interface IError
{
	Dictionary<string, object?> Data { get; }
	string Message { get; }
	string Type { get; }
}