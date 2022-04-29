using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows;
using Acc.Lib;
using Acc.Lib.Messages;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GT3e.Admin.ViewModels;

public class StewardControlCentreViewModel : ObservableObject
{
  private AccConnection accConnection;
  private CompositeDisposable compositeDisposable;
  private Visibility connectionSettingsVisibility;
  private bool connectToLocalInstance;
  private string host;
  private bool isStarted;
  private string password;
  private string port;

  public StewardControlCentreViewModel()
  {
    this.StartCommand = new AsyncRelayCommand(this.HandleStartCommand, this.CanStart);
    this.StopCommand = new AsyncRelayCommand(this.HandleStopCommand, this.CanStop);
    this.ConnectToLocalInstance = false;

    this.Host = "10.0.0.166";
    this.Port = "9000";
    this.Password = "asd";

    this.PropertyChanged += this.HandlePropertyChanged;
  }

  public ObservableCollection<DriverListing> Drivers { get; } = new();

  public IAsyncRelayCommand StartCommand { get; }

  public IAsyncRelayCommand StopCommand { get; }

  public Visibility ConnectionSettingsVisibility
  {
    get => this.connectionSettingsVisibility;
    set => this.SetProperty(ref this.connectionSettingsVisibility, value);
  }

  public bool ConnectToLocalInstance
  {
    get => this.connectToLocalInstance;
    set
    {
      this.SetProperty(ref this.connectToLocalInstance, value);
      this.ConnectionSettingsVisibility =
        this.ConnectToLocalInstance? Visibility.Collapsed: Visibility.Visible;
    }
  }

  public string Host
  {
    get => this.host;
    set => this.SetProperty(ref this.host, value);
  }

  public string Password
  {
    get => this.password;
    set => this.SetProperty(ref this.password, value);
  }

  public string Port
  {
    get => this.port;
    set => this.SetProperty(ref this.port, value);
  }

  private bool CanStart()
  {
    return !this.isStarted && this.HasConnectionSettings();
  }

  private bool CanStop()
  {
    return this.isStarted;
  }

  private void HandleAccError(Exception exception)
  {
    this.LogMessage(exception.Message);
  }

  private void HandleBroadcastingEvents(BroadcastingEvent message)
  {
    AccLog.Log(new AccEvent
               {
                 MessageType = InboundMessageTypes.BroadcastingEvent,
                 BroadcastingEventType = message.BroadcastingEventType,
                 Message = message
               });
    this.LogMessage(message.ToString());
  }

  private void HandleEntryListUpdates(EntryListUpdate message)
  {
    AccLog.Log(new AccEvent
               {
                 MessageType = InboundMessageTypes.EntryListCar,
                 Message = message
               });
    this.LogMessage(message.ToString());
    this.UpdateDriverListings(message);
  }

  private void HandlePropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
  {
    this.UpdateCommandState();
  }

  private void HandleRealTimeCarUpdates(RealtimeCarUpdate message)
  {
    AccLog.Log(new AccEvent
               {
                 MessageType = InboundMessageTypes.RealtimeCarUpdate,
                 Message = message
               });
    this.LogMessage(message.ToString());
    this.UpdateDriverListings(message);
  }

  private void HandleRealTimeUpdates(RealtimeUpdate message)
  {
    AccLog.Log(new AccEvent
               {
                 MessageType = InboundMessageTypes.RealtimeUpdate,
                 Message = message
               });
    this.LogMessage(message.ToString());
  }

  private async Task HandleStartCommand()
  {
    this.isStarted = true;
    var accHost = this.Host;
    var accPort = this.Port;
    var accPassword = this.Password;

    this.UpdateCommandState();
    if(this.ConnectToLocalInstance)
    {
      accHost = "127.0.0.1";
      var broadcastSettings = AccLocalConfigProvider.GetBroadcastingSettings();
      if(broadcastSettings != null)
      {
        accPort = broadcastSettings.UpdListenerPort.ToString();
        accPassword = broadcastSettings.ConnectionPassword;
      }
    }

    LogWriter.Info($"Monitoring {accHost}:{accPort}");
    await this.MonitorAcc(accHost, accPort, accPassword);
  }

  private async Task HandleStopCommand()
  {
    this.isStarted = false;
    this.UpdateCommandState();
    await this.accConnection.ShutdownAsync();
    this.compositeDisposable.Dispose();
  }

  private void HandleTrackDataUpdates(TrackDataUpdate message)
  {
    AccLog.Log(new AccEvent
               {
                 MessageType = InboundMessageTypes.TrackData,
                 Message = message
               });
    this.LogMessage(message.ToString());
  }

  private bool HasConnectionSettings()
  {
    return this.ConnectToLocalInstance || !string.IsNullOrWhiteSpace(this.Host)
           && !string.IsNullOrWhiteSpace(this.Port);
  }

  private void LogMessage(string message)
  {
    LogWriter.Info(message);
    ConsoleLog.Write(message);
  }

  private async Task MonitorAcc(string hostName, string port, string connectionPassword)
  {
    this.compositeDisposable = new CompositeDisposable();

    var portNumber = int.Parse(port);
    this.accConnection = new AccConnection(hostName,
      portNumber,
      "GT3e Admin",
      connectionPassword,
      null,
      500,
      this.LogMessage);

    this.compositeDisposable.Add(
      this.accConnection.BroadcastingEvents.Subscribe(this.HandleBroadcastingEvents,
        this.HandleAccError));
    this.compositeDisposable.Add(
      this.accConnection.EntryListUpdates.Subscribe(this.HandleEntryListUpdates,
        this.HandleAccError));
    this.compositeDisposable.Add(
      this.accConnection.RealTimeUpdates.Subscribe(this.HandleRealTimeUpdates,
        this.HandleAccError));
    this.compositeDisposable.Add(
      this.accConnection.RealTimeCarUpdates.Subscribe(this.HandleRealTimeCarUpdates,
        this.HandleAccError));
    this.compositeDisposable.Add(
      this.accConnection.TrackDataUpdates.Subscribe(this.HandleTrackDataUpdates,
        this.HandleAccError));

    await this.accConnection.Connect();
  }

  private void UpdateCommandState()
  {
    this.StartCommand.NotifyCanExecuteChanged();
    this.StopCommand.NotifyCanExecuteChanged();
  }

  private void UpdateDriverListings(EntryListUpdate message)
  {
    this.Drivers.Add(new DriverListing
                     {
                       CarModel = message.CarInfo.CarModelType,
                       CarIndex = message.CarInfo.CarIndex,
                       DisplayName = message.CarInfo.GetCurrentDisplayName(),
                       Position = message.CarInfo.CarIndex + 1,
                       RaceNumber = message.CarInfo.RaceNumber
                     });
  }

  private void UpdateDriverListings(RealtimeCarUpdate message)
  {
    var driverListing = this.Drivers.FirstOrDefault(e => e.CarIndex == message.CarIndex);
    if(driverListing == null)
    {
      return;
    }

    driverListing.Position = message.Position;
  }
}