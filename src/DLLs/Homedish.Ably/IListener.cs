using System;
using Homedish.Events.Contracts;

namespace Homedish.Ably
{
    public interface IListener
    {
        IListener WithChannel<TEvent>() where TEvent : Event;
    }
}
