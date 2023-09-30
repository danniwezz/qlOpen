
namespace Shared.Core;
public class IdGenerator
{
    private static readonly IdGen.IdGenerator _generator = new(new Random().Next() % 1024);
    public static long NewId() => _generator.CreateId();
}