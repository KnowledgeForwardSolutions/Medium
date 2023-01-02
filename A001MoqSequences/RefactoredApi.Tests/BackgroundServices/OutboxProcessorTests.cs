namespace RefactoredApi.Tests.BackgroundServices;

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

      var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
      var messageQueueMock = new Mock<IMessageQueue>(MockBehavior.Strict);
      var delayerMock = new Mock<IAsyncDelayer>(MockBehavior.Strict);

      var sequence = new MockSequence();

      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_emptyBatch);
      delayerMock.InSequence(sequence)
         .Setup(x => x.Delay(_outboxSettings.PollingInterval, token))
         .Callback(() => cancellationTokenSource.Cancel())
         .ReturnsAsync((Int32 interval, CancellationToken token) => interval);

      var sut = new OutboxProcessor(
         repositoryMock.Object,
         messageQueueMock.Object,
         delayerMock.Object,
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

      var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
      var messageQueueMock = new Mock<IMessageQueue>(MockBehavior.Strict);
      var delayerMock = new Mock<IAsyncDelayer>(MockBehavior.Strict);

      var sequence = new MockSequence();

      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_outboxEvents[0..2].ToList());
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[0]))
         .ReturnsAsync(true);
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[1]))
         .ReturnsAsync(true);
      repositoryMock.InSequence(sequence)
         .Setup(x => x.UpdateLastProcessedEventIdAsync(_outboxEvents[1].EventId))
         .ReturnsAsync(true);
      delayerMock.InSequence(sequence)
         .Setup(x => x.Delay(_outboxSettings.PollingInterval, token))
         .Callback(() => cancellationTokenSource.Cancel())
         .ReturnsAsync((Int32 interval, CancellationToken token) => interval);

      var sut = new OutboxProcessor(
         repositoryMock.Object,
         messageQueueMock.Object,
         delayerMock.Object,
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

      var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
      var messageQueueMock = new Mock<IMessageQueue>(MockBehavior.Strict);
      var delayerMock = new Mock<IAsyncDelayer>(MockBehavior.Strict);

      var sequence = new MockSequence();

      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_outboxEvents[0..3].ToList());
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[0]))
         .ReturnsAsync(true);
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[1]))
         .ReturnsAsync(true);
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[2]))
         .ReturnsAsync(true);
      repositoryMock.InSequence(sequence)
         .Setup(x => x.UpdateLastProcessedEventIdAsync(_outboxEvents[2].EventId))
         .ReturnsAsync(true);
      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_emptyBatch);
      delayerMock.InSequence(sequence)
         .Setup(x => x.Delay(_outboxSettings.PollingInterval, token))
         .Callback(() => cancellationTokenSource.Cancel())
         .ReturnsAsync((Int32 interval, CancellationToken token) => interval);

      var sut = new OutboxProcessor(
         repositoryMock.Object,
         messageQueueMock.Object,
         delayerMock.Object,
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

      var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
      var messageQueueMock = new Mock<IMessageQueue>(MockBehavior.Strict);
      var delayerMock = new Mock<IAsyncDelayer>(MockBehavior.Strict);

      var sequence = new MockSequence();

      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .Callback(() => cancellationTokenSource.Cancel())
         .ReturnsAsync(_outboxEvents[0..3].ToList());

      var sut = new OutboxProcessor(
         repositoryMock.Object,
         messageQueueMock.Object,
         delayerMock.Object,
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

      var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
      var messageQueueMock = new Mock<IMessageQueue>(MockBehavior.Strict);
      var delayerMock = new Mock<IAsyncDelayer>(MockBehavior.Strict);

      var sequence = new MockSequence();

      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_outboxEvents[0..3].ToList());
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[0]))
         .ReturnsAsync(true);
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[1]))
         .Callback(() => cancellationTokenSource.Cancel())
         .ReturnsAsync(true);

      var sut = new OutboxProcessor(
         repositoryMock.Object,
         messageQueueMock.Object,
         delayerMock.Object,
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

      var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
      var messageQueueMock = new Mock<IMessageQueue>(MockBehavior.Strict);
      var delayerMock = new Mock<IAsyncDelayer>(MockBehavior.Strict);

      var sequence = new MockSequence();

      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_outboxEvents[0..3].ToList());
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[0]))
         .ReturnsAsync(true);
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[1]))
         .ReturnsAsync(true);
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[2]))
         .ReturnsAsync(true);
      repositoryMock.InSequence(sequence)
         .Setup(x => x.UpdateLastProcessedEventIdAsync(_outboxEvents[2].EventId))
         .Callback(() => cancellationTokenSource.Cancel())
         .ReturnsAsync(true);

      var sut = new OutboxProcessor(
         repositoryMock.Object,
         messageQueueMock.Object,
         delayerMock.Object,
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

      var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
      var messageQueueMock = new Mock<IMessageQueue>(MockBehavior.Strict);
      var delayerMock = new Mock<IAsyncDelayer>(MockBehavior.Strict);

      var sequence = new MockSequence();

      // First pass, empty batch.
      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_emptyBatch);
      delayerMock.InSequence(sequence)
         .Setup(x => x.Delay(_outboxSettings.PollingInterval, token))
         .ReturnsAsync((Int32 interval, CancellationToken token) => interval);
      // Second pass, full batch.
      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_outboxEvents[0..3].ToList());
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[0]))
         .ReturnsAsync(true);
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[1]))
         .ReturnsAsync(true);
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[2]))
         .ReturnsAsync(true);
      repositoryMock.InSequence(sequence)
         .Setup(x => x.UpdateLastProcessedEventIdAsync(_outboxEvents[2].EventId))
         .ReturnsAsync(true);
      // Third pass, partial batch.
      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_outboxEvents[3..4].ToList());
      messageQueueMock.InSequence(sequence)
         .Setup(x => x.PublishEventAsync(_outboxEvents[3]))
         .ReturnsAsync(true);
      repositoryMock.InSequence(sequence)
         .Setup(x => x.UpdateLastProcessedEventIdAsync(_outboxEvents[3].EventId))
         .ReturnsAsync(true);
      delayerMock.InSequence(sequence)
         .Setup(x => x.Delay(_outboxSettings.PollingInterval, token))
         .ReturnsAsync((Int32 interval, CancellationToken token) => interval);
      // Fourth pass, empty batch.
      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_emptyBatch);
      delayerMock.InSequence(sequence)
         .Setup(x => x.Delay(_outboxSettings.PollingInterval, token))
         .ReturnsAsync((Int32 interval, CancellationToken token) => interval);
      // Fifth pass, empty batch.
      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .ReturnsAsync(_emptyBatch);
      delayerMock.InSequence(sequence)
         .Setup(x => x.Delay(_outboxSettings.PollingInterval, token))
         .ReturnsAsync((Int32 interval, CancellationToken token) => interval);
      // Sixth pass, cancel and exit.
      repositoryMock.InSequence(sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize))
         .Callback(() => cancellationTokenSource.Cancel())
         .ReturnsAsync(_outboxEvents[4..7].ToList());

      var sut = new OutboxProcessor(
         repositoryMock.Object,
         messageQueueMock.Object,
         delayerMock.Object,
         _outboxSettings);

      // Act.
      await sut.ProcessOutboxItemsAsync(token);
   }
}
