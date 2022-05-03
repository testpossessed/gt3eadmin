using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Acc.Lib.Messages;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GT3e.Admin.ViewModels;

public class LogViewModel : ObservableObject
{
    private readonly List<AccEvent> events = new();
    private bool includeAccidentEvents;
    private bool includeBestPersonalLapEvents;
    private bool includeBestSessionLapEvents;
    private bool includeBroadcastingEvents;
    private bool includeEntryListUpdates;
    private bool includeGreenFlagEvents;
    private bool includeLapCompletedEvents;
    private bool includePenaltyCommEvents;
    private bool includeRealtimeCarUpdates;
    private bool includeRealtimeUpdates;
    private bool includeSessionOverEvents;
    private bool includeTrackDataUpdates;
    private bool includeUntypedEvents;
    private AccEvent selectedEvent;
    private string selectedEventJson;

    public LogViewModel()
    {
        this.IncludeBroadcastingEvents = true;
        this.IncludeEntryListUpdates = true;
        this.IncludeRealtimeUpdates = true;
        this.IncludeRealtimeCarUpdates = true;
        this.IncludeTrackDataUpdates = true;
        this.IncludeRealtimeCarUpdates = true;
        this.IncludeAccidentEvents = true;
        this.IncludeBestPersonalLapEvents = true;
        this.IncludeBestSessionLapEvents = true;
        this.IncludeGreenFlagEvents = true;
        this.IncludePenaltyCommEvents = true;
        this.IncludePenaltyCommEvents = true;
        this.IncludeLapCompletedEvents = true;
        this.IncludeSessionOverEvents = true;
        this.IncludeUntypedEvents = true;

        this.LoadCommand = new RelayCommand(this.HandleLoadCommand);

        AccLog.Events.Subscribe(this.HandleAccLogEvent);
    }

    private void HandleLoadCommand()
    {
        this.ApplyFilter();
    }

    public ICommand LoadCommand { get;  }

    public ObservableCollection<AccEvent> FilteredEvents { get; } = new();

    public AccEvent SelectedEvent
    {
        get => this.selectedEvent;
        set
        {
            this.SetProperty(ref this.selectedEvent, value);
            if (this.SelectedEvent != null)
            {
                this.SelectedEventJson = this.SelectedEvent.Json;
            }
        }
    }

    public string SelectedEventJson
    {
        get => this.selectedEventJson;
        set => this.SetProperty(ref this.selectedEventJson, value);
    }

    public bool IncludeAccidentEvents
    {
        get => this.includeAccidentEvents;
        set => this.SetProperty(ref this.includeAccidentEvents, value);
    }

    public bool IncludeBestPersonalLapEvents
    {
        get => this.includeBestPersonalLapEvents;
        set => this.SetProperty(ref this.includeBestPersonalLapEvents, value);
    }

    public bool IncludeBestSessionLapEvents
    {
        get => this.includeBestSessionLapEvents;
        set => this.SetProperty(ref this.includeBestSessionLapEvents, value);
    }

    public bool IncludeBroadcastingEvents
    {
        get => this.includeBroadcastingEvents;
        set => this.SetProperty(ref this.includeBroadcastingEvents, value);
    }

    public bool IncludeEntryListUpdates
    {
        get => this.includeEntryListUpdates;
        set => this.SetProperty(ref this.includeEntryListUpdates, value);
    }

    public bool IncludeGreenFlagEvents
    {
        get => this.includeGreenFlagEvents;
        set => this.SetProperty(ref this.includeGreenFlagEvents, value);
    }

    public bool IncludeLapCompletedEvents
    {
        get => this.includeLapCompletedEvents;
        set => this.SetProperty(ref this.includeLapCompletedEvents, value);
    }

    public bool IncludePenaltyCommEvents
    {
        get => this.includePenaltyCommEvents;
        set => this.SetProperty(ref this.includePenaltyCommEvents, value);
    }

    public bool IncludeRealtimeCarUpdates
    {
        get => this.includeRealtimeCarUpdates;
        set => this.SetProperty(ref this.includeRealtimeCarUpdates, value);
    }

    public bool IncludeRealtimeUpdates
    {
        get => this.includeRealtimeUpdates;
        set => this.SetProperty(ref this.includeRealtimeUpdates, value);
    }

    public bool IncludeSessionOverEvents
    {
        get => this.includeSessionOverEvents;
        set => this.SetProperty(ref this.includeSessionOverEvents, value);
    }

    public bool IncludeTrackDataUpdates
    {
        get => this.includeTrackDataUpdates;
        set => this.SetProperty(ref this.includeTrackDataUpdates, value);
    }

    public bool IncludeUntypedEvents
    {
        get => this.includeUntypedEvents;
        set => this.SetProperty(ref this.includeUntypedEvents, value);
    }

    private void ApplyFilter()
    {
        this.SelectedEvent = null;
        this.SelectedEventJson = string.Empty;
        this.FilteredEvents.Clear();
        foreach (var accEvent in this.events.OrderByDescending(e => e.Timestamp))
        {
            switch (accEvent.MessageType)
            {
                case InboundMessageType.RealtimeUpdate:
                    if (this.IncludeRealtimeUpdates)
                    {
                        this.FilteredEvents.Add(accEvent);
                    }

                    break;
                case InboundMessageType.RealtimeCarUpdate:
                    if (this.IncludeRealtimeCarUpdates)
                    {
                        this.FilteredEvents.Add(accEvent);
                    }

                    break;
                case InboundMessageType.EntryList:
                case InboundMessageType.EntryListCar:
                    if (this.IncludeEntryListUpdates)
                    {
                        this.FilteredEvents.Add(accEvent);
                    }

                    break;
                case InboundMessageType.TrackData:
                    if (this.IncludeTrackDataUpdates)
                    {
                        this.FilteredEvents.Add(accEvent);
                    }

                    break;
                case InboundMessageType.BroadcastingEvent:
                    if (this.IncludeBroadcastingEvents)
                    {
                        switch (accEvent.BroadcastingEventType)
                        {
                            case BroadcastingEventType.None:
                                if (this.IncludeUntypedEvents)
                                {
                                    this.FilteredEvents.Add(accEvent);
                                }

                                break;
                            case BroadcastingEventType.GreenFlag:
                                if (this.IncludeGreenFlagEvents)
                                {
                                    this.FilteredEvents.Add(accEvent);
                                }

                                break;
                            case BroadcastingEventType.SessionOver:
                                if (this.IncludeSessionOverEvents)
                                {
                                    this.FilteredEvents.Add(accEvent);
                                }

                                break;
                            case BroadcastingEventType.PenaltyCommMsg:
                                if (this.IncludePenaltyCommEvents)
                                {
                                    this.FilteredEvents.Add(accEvent);
                                }

                                break;
                            case BroadcastingEventType.Accident:
                                if (this.IncludeAccidentEvents)
                                {
                                    this.FilteredEvents.Add(accEvent);
                                }

                                break;
                            case BroadcastingEventType.LapCompleted:
                                if (this.IncludeLapCompletedEvents)
                                {
                                    this.FilteredEvents.Add(accEvent);
                                }

                                break;
                            case BroadcastingEventType.BestSessionLap:
                                if (this.IncludeBestSessionLapEvents)
                                {
                                    this.FilteredEvents.Add(accEvent);
                                }

                                break;
                            case BroadcastingEventType.BestPersonalLap:
                                if (this.IncludeBestPersonalLapEvents)
                                {
                                    this.FilteredEvents.Add(accEvent);
                                }

                                break;
                        }
                    }

                    break;
            }
        }
    }

    private void HandleAccLogEvent(AccEvent accEvent)
    {
        this.events.Add(accEvent);
    }
}