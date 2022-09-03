namespace UserService.Integration;

public interface IPublisher
{
    void PublishToMessageQueue(string exchange,string integrationEvent, object data);
}