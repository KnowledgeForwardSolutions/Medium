namespace RefactoredApi.Tests.SequenceBuilder.BackgroundServices;

public class OutboxProcessorTestSequenceBuilder
{
   private readonly Mock<IRepository> _repositoryMock = new(MockBehavior.Strict);
   private readonly Mock<IMessageQueue> _messageQueueMock = new(MockBehavior.Strict);
   private readonly Mock<IAsyncDelayer> _delayerMock = new(MockBehavior.Strict);

   private readonly MockSequence _sequence = new();

   private readonly OutboxSettings _outboxSettings = default!;
   private readonly CancellationToken _token;

   public OutboxProcessorTestSequenceBuilder(
      OutboxSettings settings,
      CancellationToken token)
   {
      _outboxSettings = settings;
      _token = token;
   }

   public (MockSequence, IRepository, IMessageQueue, IAsyncDelayer) Build()
      => (_sequence, _repositoryMock.Object, _messageQueueMock.Object, _delayerMock.Object);

   public OutboxProcessorTestSequenceBuilder SetupDelayAsync(Action? callback = null)
   {
      var setup = _delayerMock.InSequence(_sequence)
         .Setup(x => x.Delay(_outboxSettings.PollingInterval, _token));
      if (callback != null)
      {
         setup.Callback(() => callback());
      }
      setup.ReturnsAsync((Int32 interval, CancellationToken token) => interval);

      return this;
   }

   public OutboxProcessorTestSequenceBuilder SetupGetEventsAsync(
      IReadOnlyList<UserEvent> events,
      Action? callback = null)
   {
      var setup = _repositoryMock.InSequence(_sequence)
         .Setup(x => x.GetEventsAsync(_outboxSettings.BatchSize));
      if (callback != null)
      {
         setup.Callback(() => callback());
      }
      setup.ReturnsAsync(events);

      return this;
   }

   public OutboxProcessorTestSequenceBuilder SetupPublishEventAsync(
      UserEvent userEvent,
      Action? callback = null)
   {
      var setup = _messageQueueMock.InSequence(_sequence)
         .Setup(x => x.PublishEventAsync(userEvent));
      if (callback != null)
      {
         setup.Callback(() => callback());
      }
      setup.ReturnsAsync(true);

      return this;
   }

   public OutboxProcessorTestSequenceBuilder SetupUpdateLastProcessedEventIdAsync(
      Int64 eventId,
      Action? callback = null)
   {
      var setup = _repositoryMock.InSequence(_sequence)
         .Setup(x => x.UpdateLastProcessedEventIdAsync(eventId));
      if (callback != null)
      {
         setup.Callback(() => callback());
      }
      setup.ReturnsAsync(true);

      return this;
   }
}
