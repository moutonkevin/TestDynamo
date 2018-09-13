namespace Homedish.Events.Contracts
{
    public interface IListener
    {
        IListener StartListening<TEvent, THandler>(EventConfiguration configuration = null) 
            where TEvent : Event 
            where THandler : IHandler<TEvent>;

        void StopListening<TEvent>()
            where TEvent : Event;
    }
}
