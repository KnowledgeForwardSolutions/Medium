namespace RefactoredApi.Repository;

public interface IRepository
{
   Task<IReadOnlyList<UserEvent>> GetEventsAsync(Int32 batchSize);

   Task<Boolean> UpdateLastProcessedEventIdAsync(Int64 eventId);
}
