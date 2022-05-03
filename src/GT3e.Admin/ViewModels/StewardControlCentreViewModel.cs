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
    private bool canEditConnection;
    private CompositeDisposable compositeDisposable;
    private string connectionId;
    private Visibility connectionSettingsVisibility;
    private bool connectToLocalInstance;
    private bool enableLogging;
    private string host;
    private bool isConnected;
    private bool isStarted;
    private string password;
    private string port;
    private Visibility sessionDetailsVisibility;
    private float trackDistance;
    private string trackDistanceKm;
    private string trackDistanceMiles;
    private string trackName;

    public StewardControlCentreViewModel()
    {
        this.StartCommand = new AsyncRelayCommand(this.HandleStartCommand, this.CanStart);
        this.StopCommand = new AsyncRelayCommand(this.HandleStopCommand, this.CanStop);
        this.ConnectToLocalInstance = true;
        this.CanEditConnection = true;
        this.ConnectionSettingsVisibility = Visibility.Collapsed;
        this.SessionDetailsVisibility = Visibility.Collapsed;

        this.Host = "10.0.0.166";
        this.Port = "9000";
        this.Password = "asd";

        this.PropertyChanged += this.HandlePropertyChanged;
    }

    public ObservableCollection<DriverListingViewModel> Drivers { get; } = new();

    public IAsyncRelayCommand StartCommand { get; }

    public IAsyncRelayCommand StopCommand { get; }

    public bool CanEditConnection
    {
        get => this.canEditConnection;
        set => this.SetProperty(ref this.canEditConnection, value);
    }

    public string ConnectionId
    {
        get => this.connectionId;
        set => this.SetProperty(ref this.connectionId, value);
    }

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

    public bool EnableLogging
    {
        get => this.enableLogging;
        set => this.SetProperty(ref this.enableLogging, value);
    }

    public string Host
    {
        get => this.host;
        set => this.SetProperty(ref this.host, value);
    }

    public bool IsConnected
    {
        get => this.isConnected;
        set => this.SetProperty(ref this.isConnected, value);
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

    public Visibility SessionDetailsVisibility
    {
        get => this.sessionDetailsVisibility;
        set => this.SetProperty(ref this.sessionDetailsVisibility, value);
    }

    public string TrackDistanceKm
    {
        get => this.trackDistanceKm;
        set => this.SetProperty(ref this.trackDistanceKm, value);
    }

    public string TrackDistanceMiles
    {
        get => this.trackDistanceMiles;
        set => this.SetProperty(ref this.trackDistanceMiles, value);
    }

    public string TrackName
    {
        get => this.trackName;
        set => this.SetProperty(ref this.trackName, value);
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
        this.LogAccEvent(message, InboundMessageType.BroadcastingEvent);
        this.LogMessage(message.ToString());
    }

    private void HandleEntryListUpdates(EntryListUpdate message)
    {
        this.LogAccEvent(message, InboundMessageType.EntryListCar);
        this.LogMessage(message.ToString());
        this.UpdateDriverListings(message);
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
    {
        this.UpdateCommandState();
    }

    private void HandleRealTimeCarUpdates(RealtimeCarUpdate message)
    {
        this.LogAccEvent(message, InboundMessageType.RealtimeCarUpdate);
        this.LogMessage(message.ToString());
        this.UpdateDriverListings(message);
    }

    private void HandleRealTimeUpdates(RealtimeUpdate message)
    {
        this.LogAccEvent(message, InboundMessageType.RealtimeUpdate);
        this.LogMessage(message.ToString());
    }

    private async Task HandleStartCommand()
    {
        this.isStarted = true;
        this.CanEditConnection = false;
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
        this.CanEditConnection = true;
        this.UpdateCommandState();
        await this.accConnection.ShutdownAsync();
        this.compositeDisposable.Dispose();
    }

    private void HandleTrackDataUpdates(TrackDataUpdate message)
    {
        this.LogAccEvent(message, InboundMessageType.TrackData);
        this.LogMessage(message.ToString());
        this.ConnectionId = message.ConnectionIdentifier;
        this.TrackName = message.TrackName;
        this.trackDistance = message.TrackMeters;
        this.TrackDistanceKm = $"{this.trackDistance / 1000:F} KM";
        this.TrackDistanceMiles = $"{this.trackDistance * Constants.MetersToMilesFactor:F} Miles";
        this.SessionDetailsVisibility = Visibility.Visible;
    }

    private bool HasConnectionSettings()
    {
        return this.ConnectToLocalInstance || (!string.IsNullOrWhiteSpace(this.Host)
                                               && !string.IsNullOrWhiteSpace(this.Port));
    }

    private void LogAccEvent(object message, InboundMessageType messageType)
    {
        if(!this.EnableLogging)
        {
            return;
        }

        AccLog.Log(new AccEvent
                   {
                       MessageType = messageType,
                       Message = message
                   });
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
        this.Drivers.Add(new DriverListingViewModel
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
        driverListing.PositionInClass = message.CupPosition;
        driverListing.DistanceIntoLap = message.SplinePosition;
        driverListing.PositionOnTrack = message.TrackPosition;
        driverListing.SpeedKmh = message.Kmh;

        if(driverListing.SpeedKmh < 1)
        {
            return;
        }

        var carInFront =
            this.Drivers.OrderBy(e => e.Position).LastOrDefault(e => e.Position < driverListing.Position);
        if(carInFront == null)
        {
            driverListing.Gap = 0;
            driverListing.GapText = "0";
            return;
        }

        var gapDistance = carInFront.DistanceIntoLap - driverListing.DistanceIntoLap;
        while(gapDistance > 1f)
        {
            gapDistance -= 1f;
        }

        driverListing.Gap = gapDistance * this.trackDistance;

    }
}