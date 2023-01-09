namespace OriginalApi.BackgroundServices;

public sealed class OutboxProcessorService : BackgroundService
{
   private readonly IRepository _repository;
   private readonly IMessageQueue _messageQueue;
   private readonly OutboxSettings _settings;

   public OutboxProcessorService(
      IRepository repository, 
      IMessageQueue messageQueue,
      OutboxSettings settings)
   {
      _repository = repository;
      _messageQueue = messageQueue;
      _settings = settings;
   }

   protected override async Task ExecuteAsync(CancellationToken token)
   {
      while(!token.IsCancellationRequested)
      {
         var batch = await _repository.GetEventsAsync(_settings.BatchSize);
         await PublishItemsAsync(batch, token);

         if (batch.Count != _settings.BatchSize 
            && !token.IsCancellationRequested)
         {
            await Task.Delay(_settings.PollingInterval, token);
         }
      }
   }

   private async Task PublishItemsAsync(
      IReadOnlyList<UserEvent> batch,
      CancellationToken token)
   {
      Int64 lastProcessedId = default;
      foreach(var item in batch)
      {
         if (token.IsCancellationRequested)
         {
            return;
         }

         await _messageQueue.PublishEventAsync(item);
         lastProcessedId = item.EventId;
      }

      if (batch.Count > 0 && !token.IsCancellationRequested)
      {
         await _repository.UpdateLastProcessedEventIdAsync(lastProcessedId);
      }
   }
}
