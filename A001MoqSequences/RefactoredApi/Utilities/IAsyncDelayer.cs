namespace RefactoredApi.Utilities;

public interface IAsyncDelayer
{
   Task<Int32> Delay(Int32 milliseconds, CancellationToken token);
}
