namespace RefactoredApi.Utilities;

public sealed class AsyncDelayer : IAsyncDelayer
{
   public async Task<Int32> Delay(Int32 milliseconds, CancellationToken token)
   {
      await Task.Delay(milliseconds, token);

      return milliseconds;
   }
}
