namespace RefactoredApi.BackgroundServices;

public class OutboxProcessor : IOutboxProcessor
{
   private readonly IRepository _repository;
   private readonly IMessageQueue _messageQueue;
   private readonly IAsyncDelayer _delayer;
   private readonly OutboxSettings _settings;

   public OutboxProcessor(
      IRepository repository,
      IMessageQueue messageQueue,
      IAsyncDelayer delayer,
      OutboxSettings settings)
   {
      _repository = repository;
      _messageQueue = messageQueue;
      _delayer = delayer;
      _settings = settings;
   }

   public async Task ProcessOutboxItemsAsync(CancellationToken token)
   {
      while (!token.IsCancellationRequested)
      {
         var batch = await _repository.GetEventsAsync(_settings.BatchSize);
         await PublishItemsAsync(batch, token);

         if (batch.Count != _settings.BatchSize
            && !token.IsCancellationRequested)
         {
            await _delayer.Delay(_settings.PollingInterval, token);
         }
      }
   }

   private async Task PublishItemsAsync(
      IReadOnlyList<UserEvent> batch,
      CancellationToken token)
   {
      Int64 lastProcessedId = default;
      foreach (var item in batch)
      {
         if (token.IsCancellationRequested)
         {
            return;
         }

         await _messageQueue.PublishEventAsync(item);
         lastProcessedId = item.EventId;
      }

      if (batch.Count > 0)
      {
         await _repository.UpdateLastProcessedEventIdAsync(lastProcessedId);
      }
   }
}
