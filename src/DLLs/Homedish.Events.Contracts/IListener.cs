namespace Homedish.Events.Contracts
{
    public interface IListener
    {
        IListener ListenTo<TEvent>() where TEvent : Event;
    }
}
