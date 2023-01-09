namespace RefactoredApi.Tests.SequenceBuilder.BackgroundServices;

public class OutboxProcessorTests
{
   private readonly OutboxSettings _outboxSettings = new()
   {
      BatchSize = 3,
      PollingInterval = 30000
   };

   private readonly UserEvent[] _outboxEvents = Enumerable.Range(1, 10)
      .Select(x => new UserEvent(x, $"Details for user event {x}"))
      .ToArray();

   private readonly IReadOnlyList<UserEvent> _emptyBatch = new List<UserEvent>();

   [Fact]
   public async void ProcessOutboxItemsAsync_PublishesNoItemsAndDelays_WhenEmptyBatchEncountered()
   {
      // Arrange.
      var cancellationTokenSource = new CancellationTokenSource();
      var token = cancellationTokenSource.Token;

      var (repository, messageQueue, delayer) = new OutboxProcessorTestSequenceBuilder(_outboxSettings, token)
         .SetupGetEventsAsync(_emptyBatch)
         .SetupDelayAsync(() => cancellationTokenSource.Cancel())
         .Build();

      var sut = new OutboxProcessor(
         repository,
         messageQueue,
         delayer,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);

      // Assert - assert not needed because of MockBehavior.Strict
   }

   [Fact]
   public async void ProcessOutboxItemsAsync_PublishesBatchItemsAndDelays_WhenPartialBatchEncountered()
   {
      // Arrange.
      var cancellationTokenSource = new CancellationTokenSource();
      var token = cancellationTokenSource.Token;

      var (repository, messageQueue, delayer) = new OutboxProcessorTestSequenceBuilder(_outboxSettings, token)
         .SetupGetEventsAsync(_outboxEvents[0..2].ToList())
         .SetupPublishEventAsync(_outboxEvents[0])
         .SetupPublishEventAsync(_outboxEvents[1])
         .SetupUpdateLastProcessedEventIdAsync(_outboxEvents[1].EventId)
         .SetupDelayAsync(() => cancellationTokenSource.Cancel())
         .Build();

      var sut = new OutboxProcessor(
         repository,
         messageQueue,
         delayer,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);
   }

   [Fact]
   public async void ProcessOutboxItemsAsync_PublishesBatchItemsAndDoesNotDelay_WhenFullBatchEncountered()
   {
      // Arrange.
      var cancellationTokenSource = new CancellationTokenSource();
      var token = cancellationTokenSource.Token;

      var (repository, messageQueue, delayer) = new OutboxProcessorTestSequenceBuilder(_outboxSettings, token)
         .SetupGetEventsAsync(_outboxEvents[0..3].ToList())
         .SetupPublishEventAsync(_outboxEvents[0])
         .SetupPublishEventAsync(_outboxEvents[1])
         .SetupPublishEventAsync(_outboxEvents[2])
         .SetupUpdateLastProcessedEventIdAsync(_outboxEvents[2].EventId)
         .SetupGetEventsAsync(_emptyBatch)
         .SetupDelayAsync(() => cancellationTokenSource.Cancel())
         .Build();

      var sut = new OutboxProcessor(
         repository,
         messageQueue,
         delayer,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);
   }

   [Fact]
   public async void ProcessOutboxItemsAsync_DoesNotPublishAndExits_WhenCancelledDuringGetEvents()
   {
      // Arrange.
      var cancellationTokenSource = new CancellationTokenSource();
      var token = cancellationTokenSource.Token;

      var (repository, messageQueue, delayer) = new OutboxProcessorTestSequenceBuilder(_outboxSettings, token)
         .SetupGetEventsAsync(
            _outboxEvents[0..3].ToList(), 
            () => cancellationTokenSource.Cancel())
         .Build();

      var sut = new OutboxProcessor(
         repository,
         messageQueue,
         delayer,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);
   }

   [Fact]
   public async void ProcessOutboxItemsAsync_PublishesPartialBatchAndExits_WhenCancelledDuringPublish()
   {
      // Arrange.
      var cancellationTokenSource = new CancellationTokenSource();
      var token = cancellationTokenSource.Token;

      var (repository, messageQueue, delayer) = new OutboxProcessorTestSequenceBuilder(_outboxSettings, token)
         .SetupGetEventsAsync(_outboxEvents[0..3].ToList())
         .SetupPublishEventAsync(_outboxEvents[0])
         .SetupPublishEventAsync(
            _outboxEvents[1],
            () => cancellationTokenSource.Cancel())
        .Build();

      var sut = new OutboxProcessor(
         repository,
         messageQueue,
         delayer,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);
   }

   [Fact]
   public async void ProcessOutboxItemsAsync_ShouldNotUpdateLastProcessedItemOrWait_WhenCancelledDuringPublishingLastEventInBatch()
   {
      // Arrange.
      var cancellationTokenSource = new CancellationTokenSource();
      var token = cancellationTokenSource.Token;

      var (repository, messageQueue, delayer) = new OutboxProcessorTestSequenceBuilder(_outboxSettings, token)
         .SetupGetEventsAsync(_outboxEvents[0..2].ToList())
         .SetupPublishEventAsync(_outboxEvents[0])
         .SetupPublishEventAsync(
            _outboxEvents[1],
            () => cancellationTokenSource.Cancel())
        .Build();

      var sut = new OutboxProcessor(
         repository,
         messageQueue,
         delayer,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);
   }

   [Fact]
   public async void ProcessOutboxItemsAsync_PublishesEntireBatchThenUpdatesLastProcessedItemAndExits_WhenCancelledDuringUpdateLastProcessed()
   {
      // Arrange.
      var cancellationTokenSource = new CancellationTokenSource();
      var token = cancellationTokenSource.Token;

      var (repository, messageQueue, delayer) = new OutboxProcessorTestSequenceBuilder(_outboxSettings, token)
         .SetupGetEventsAsync(_outboxEvents[0..3].ToList())
         .SetupPublishEventAsync(_outboxEvents[0])
         .SetupPublishEventAsync(_outboxEvents[1])
         .SetupPublishEventAsync(_outboxEvents[2])
         .SetupUpdateLastProcessedEventIdAsync(
            _outboxEvents[2].EventId,
            () => cancellationTokenSource.Cancel())
         .Build();

      var sut = new OutboxProcessor(
         repository,
         messageQueue,
         delayer,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);
   }

   [Fact]
   public async void ProcessOutboxItemsAsync_PublishesMultipleBatches_WhenHandlingLargeEventStream()
   {
      // Arrange.
      var cancellationTokenSource = new CancellationTokenSource();
      var token = cancellationTokenSource.Token;

      var (repository, messageQueue, delayer) = new OutboxProcessorTestSequenceBuilder(_outboxSettings, token)
         // First pass, empty batch.
         .SetupGetEventsAsync(_emptyBatch)
         .SetupDelayAsync()
         // Second pass, full batch.
         .SetupGetEventsAsync(_outboxEvents[0..3].ToList())
         .SetupPublishEventAsync(_outboxEvents[0])
         .SetupPublishEventAsync(_outboxEvents[1])
         .SetupPublishEventAsync(_outboxEvents[2])
         .SetupUpdateLastProcessedEventIdAsync(_outboxEvents[2].EventId)
         // Third pass, partial batch.
         .SetupGetEventsAsync(_outboxEvents[3..4].ToList())
         .SetupPublishEventAsync(_outboxEvents[3])
         .SetupUpdateLastProcessedEventIdAsync(_outboxEvents[3].EventId)
         .SetupDelayAsync()
         // Fourth pass, empty batch.
         .SetupGetEventsAsync(_emptyBatch)
         .SetupDelayAsync()
         // Fifth pass, empty batch.
         .SetupGetEventsAsync(_emptyBatch)
         .SetupDelayAsync()
         // Sixth pass, cancel and exit.
         .SetupGetEventsAsync(
            _outboxEvents[4..7].ToList(),
            () => cancellationTokenSource.Cancel())
         .Build();

      var sut = new OutboxProcessor(
         repository,
         messageQueue,
         delayer,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);
   }
}
