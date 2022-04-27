using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acc.Lib.Messages;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

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

    AccLog.Events.Subscribe(this.HandleAccLogEvent);
  }

  public ObservableCollection<AccEvent> FilteredEvents { get; } = new();

  public AccEvent SelectedEvent
  {
    get => this.selectedEvent;
    set
    {
      this.SetProperty(ref this.selectedEvent, value);
      this.SelectedEventJson = this.SelectedEvent.Json;
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
    set
    {
      this.SetProperty(ref this.includeAccidentEvents, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeBestPersonalLapEvents
  {
    get => this.includeBestPersonalLapEvents;
    set
    {
      this.SetProperty(ref this.includeBestPersonalLapEvents, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeBestSessionLapEvents
  {
    get => this.includeBestSessionLapEvents;
    set
    {
      this.SetProperty(ref this.includeBestSessionLapEvents, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeBroadcastingEvents
  {
    get => this.includeBroadcastingEvents;
    set
    {
      this.SetProperty(ref this.includeBroadcastingEvents, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeEntryListUpdates
  {
    get => this.includeEntryListUpdates;
    set
    {
      this.SetProperty(ref this.includeEntryListUpdates, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeGreenFlagEvents
  {
    get => this.includeGreenFlagEvents;
    set
    {
      this.SetProperty(ref this.includeGreenFlagEvents, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeLapCompletedEvents
  {
    get => this.includeLapCompletedEvents;
    set
    {
      this.SetProperty(ref this.includeLapCompletedEvents, value);
      this.ApplyFilter();
    }
  }

  public bool IncludePenaltyCommEvents
  {
    get => this.includePenaltyCommEvents;
    set
    {
      this.SetProperty(ref this.includePenaltyCommEvents, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeRealtimeCarUpdates
  {
    get => this.includeRealtimeCarUpdates;
    set
    {
      this.SetProperty(ref this.includeRealtimeCarUpdates, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeRealtimeUpdates
  {
    get => this.includeRealtimeUpdates;
    set
    {
      this.SetProperty(ref this.includeRealtimeUpdates, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeSessionOverEvents
  {
    get => this.includeSessionOverEvents;
    set
    {
      this.SetProperty(ref this.includeSessionOverEvents, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeTrackDataUpdates
  {
    get => this.includeTrackDataUpdates;
    set
    {
      this.SetProperty(ref this.includeTrackDataUpdates, value);
      this.ApplyFilter();
    }
  }

  public bool IncludeUntypedEvents
  {
    get => this.includeUntypedEvents;
    set
    {
      this.SetProperty(ref this.includeUntypedEvents, value);
      this.ApplyFilter();
    }
  }

  private void ApplyFilter()
  {
    this.FilteredEvents.Clear();
    foreach(var accEvent in this.events.OrderByDescending(e => e.Timestamp))
    {
      switch(accEvent.MessageType)
      {
        case InboundMessageTypes.RealtimeUpdate:
          if(this.IncludeRealtimeUpdates)
          {
            this.FilteredEvents.Add(accEvent);
          }

          break;
        case InboundMessageTypes.RealtimeCarUpdate:
          if(this.IncludeRealtimeCarUpdates)
          {
            this.FilteredEvents.Add(accEvent);
          }

          break;
        case InboundMessageTypes.EntryList:
        case InboundMessageTypes.EntryListCar:
          if(this.IncludeEntryListUpdates)
          {
            this.FilteredEvents.Add(accEvent);
          }

          break;
        case InboundMessageTypes.TrackData:
          if(this.IncludeTrackDataUpdates)
          {
            this.FilteredEvents.Add(accEvent);
          }

          break;
        case InboundMessageTypes.BroadcastingEvent:
          if(this.IncludeBroadcastingEvents)
          {
            switch(accEvent.BroadcastingEventType)
            {
              case BroadcastingEventType.None:
                if(this.IncludeUntypedEvents)
                {
                  this.FilteredEvents.Add(accEvent);
                }

                break;
              case BroadcastingEventType.GreenFlag:
                if(this.IncludeGreenFlagEvents)
                {
                  this.FilteredEvents.Add(accEvent);
                }

                break;
              case BroadcastingEventType.SessionOver:
                if(this.IncludeSessionOverEvents)
                {
                  this.FilteredEvents.Add(accEvent);
                }

                break;
              case BroadcastingEventType.PenaltyCommMsg:
                if(this.IncludePenaltyCommEvents)
                {
                  this.FilteredEvents.Add(accEvent);
                }

                break;
              case BroadcastingEventType.Accident:
                if(this.IncludeAccidentEvents)
                {
                  this.FilteredEvents.Add(accEvent);
                }

                break;
              case BroadcastingEventType.LapCompleted:
                if(this.IncludeLapCompletedEvents)
                {
                  this.FilteredEvents.Add(accEvent);
                }

                break;
              case BroadcastingEventType.BestSessionLap:
                if(this.IncludeBestSessionLapEvents)
                {
                  this.FilteredEvents.Add(accEvent);
                }

                break;
              case BroadcastingEventType.BestPersonalLap:
                if(this.IncludeBestPersonalLapEvents)
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
    this.ApplyFilter();
  }
}