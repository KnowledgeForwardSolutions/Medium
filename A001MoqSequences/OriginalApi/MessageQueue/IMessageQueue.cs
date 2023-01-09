namespace OriginalApi.MessageQueue;

public interface IMessageQueue
{
   Task<Int64> PublishEventAsync(UserEvent userEvent);
}
