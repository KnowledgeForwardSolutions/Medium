namespace RefactoredApi.BackgroundServices;

public interface IOutboxProcessor
{
   Task ProcessOutboxItemsAsync(CancellationToken token);
}
