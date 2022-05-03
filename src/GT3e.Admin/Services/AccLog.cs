using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GT3e.Admin.Models;

namespace GT3e.Admin.Services;

internal static class AccLog
{
    private static readonly Subject<AccEvent> eventsSubject = new();
    internal static IObservable<AccEvent> Events => eventsSubject.AsObservable();

    internal static void Log(AccEvent @event)
    {
        eventsSubject.OnNext(@event);
    }
}