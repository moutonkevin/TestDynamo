namespace Homedish.Events.Contracts
{
    public interface IListener
    {
        IListener WithChannel<TEvent>() where TEvent : Event;
    }
}
