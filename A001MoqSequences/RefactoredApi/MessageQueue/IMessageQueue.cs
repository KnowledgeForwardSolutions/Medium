namespace RefactoredApi.MessageQueue;

public interface IMessageQueue
{
   Task<Boolean> PublishEventAsync(UserEvent userEvent);
}
