namespace RefactoredApi.BackgroundServices;

public class OutboxProcessorService : BackgroundService
{
   private readonly IOutboxProcessor _outboxProcessor;

   public OutboxProcessorService(IOutboxProcessor outboxProcessor)
      => _outboxProcessor = outboxProcessor;

   protected override async Task ExecuteAsync(CancellationToken token)
      => await _outboxProcessor.ProcessOutboxItemsAsync(token);
}
