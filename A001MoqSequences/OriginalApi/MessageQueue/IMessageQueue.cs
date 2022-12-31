namespace OriginalApi.MessageQueue;

public interface IMessageQueue
{
   Task<Boolean> PublishEventAsync(UserEvent userEvent);
}
