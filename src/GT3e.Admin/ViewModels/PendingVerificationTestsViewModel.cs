using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acc.Lib;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GT3e.Admin.ViewModels;

public class PendingVerificationTestsViewModel : ObservableObject
{
    private bool canReject;
    private RaceSessionViewModel currentRaceSession;
    private bool isLoadEnabled;
    private string rejectionReason;
    private VerificationTestPackageInfo selectedTest;
    private Visibility statsPanelVisibility;

    public PendingVerificationTestsViewModel()
    {
        this.RefreshCommand = new AsyncRelayCommand(this.HandleRefreshCommand);
        this.LoadTestCommand = new AsyncRelayCommand(this.HandleLoadTestCommand);
        this.AcceptCommand = new AsyncRelayCommand(this.HandleAcceptCommand);
        this.RejectCommand = new AsyncRelayCommand(this.HandleRejectCommand);
        this.PendingTests = new ObservableCollection<VerificationTestPackageInfo>();
        this.StatsPanelVisibility = Visibility.Collapsed;
        // this.HandleRefreshCommand().GetAwaiter().GetResult();
    }

    public IAsyncRelayCommand AcceptCommand { get; }
    public IAsyncRelayCommand LoadTestCommand { get; }
    public ObservableCollection<VerificationTestPackageInfo> PendingTests { get; }
    public IAsyncRelayCommand RefreshCommand { get; }
    public IAsyncRelayCommand RejectCommand { get; }
    public bool CanReject { get => this.canReject; set => this.SetProperty(ref this.canReject, value); }
    public RaceSessionViewModel CurrentRaceSession
    {
        get => this.currentRaceSession;
        set
        {
            this.SetProperty(ref this.currentRaceSession, value);
            this.StatsPanelVisibility = value != null? Visibility.Visible: Visibility.Hidden;
        }
    }

    public bool IsLoadEnabled
    {
        get => this.isLoadEnabled;
        set => this.SetProperty(ref this.isLoadEnabled, value);
    }

    public string RejectionReason
    {
        get => this.rejectionReason;
        set
        {
            this.SetProperty(ref this.rejectionReason, value);
            this.CanReject = !string.IsNullOrWhiteSpace(value);
        }
    }

    public VerificationTestPackageInfo SelectedTest
    {
        get => this.selectedTest;
        set
        {
            this.SetProperty(ref this.selectedTest, value);
            this.IsLoadEnabled = value != null;
        }
    }

    public Visibility StatsPanelVisibility
    {
        get => this.statsPanelVisibility;
        set => this.SetProperty(ref this.statsPanelVisibility, value);
    }

    private Task HandleAcceptCommand()
    {
        return this.SaveStats();
    }

    private async Task HandleLoadTestCommand()
    {
        var packageDownloadFilePath = await StorageProvider.GetPendingVerificationTest(this.selectedTest);
        FilePackager.UnpackToSameLocation(packageDownloadFilePath);
        ConsoleLog.Write("Loading session stats...");
        var raceSessionFilePath = packageDownloadFilePath.Replace("zip", "json");
        var raceSession = AccDataProvider.LoadRaceSession(raceSessionFilePath);
        if(raceSession != null)
        {
            this.CurrentRaceSession = new RaceSessionViewModel(raceSession);
            this.StatsPanelVisibility = Visibility.Visible;
        }
        else
        {
            this.StatsPanelVisibility = Visibility.Hidden;
        }

        ConsoleLog.Write("Copying replay to ACC...");
        var replayFilePath = packageDownloadFilePath.Replace("zip", "rpy");
        var destinationFilePath =
            Path.Combine(AccPathProvider.SavedReplaysFolderPath, $"{this.SelectedTest.Name}.rpy");
        File.Copy(replayFilePath, destinationFilePath, true);
        ConsoleLog.Write("Replay has been copied as a Saved replay in ACC ready for you to review.");
    }

    private async Task HandleRefreshCommand()
    {
        var pendingTests = await StorageProvider.GetPendingVerificationTests();
        this.PendingTests.Clear();
        foreach(var pendingTest in pendingTests)
        {
            this.PendingTests.Add(pendingTest);
        }

        if(!this.PendingTests.Any())
        {
            return;
        }
        this.SelectedTest = this.PendingTests[0];
    }

    private Task HandleRejectCommand()
    {
        return this.SaveStats(true);
    }

    private async Task SaveStats(bool rejected = false)
    {
        if(this.SelectedTest != null)
        {
            var driverStats = await StorageProvider.GetDriverStats(this.SelectedTest.Name) ?? new DriverStats
            {
                SteamId = this.SelectedTest.Name,
                FirstName = this.CurrentRaceSession.DriverFirstName,
                LastName = this.CurrentRaceSession.DriverLastName
            };

            driverStats.AddVerificationTestAttempt(new VerificationTestAttempt
            {
                AverageLapTime = this.CurrentRaceSession.AverageLapTime,
                FastestLapTime = this.CurrentRaceSession.FastestLapTime,
                FinishPosition = this.CurrentRaceSession.FinishPosition,
                InvalidLaps = this.CurrentRaceSession.InvalidLaps,
                Rejected = rejected,
                RejectionReason = this.RejectionReason,
                ReviewDate = DateTime.Now,
                TotalLaps = this.CurrentRaceSession.TotalLaps
            });

            await StorageProvider.UploadDriverStats(driverStats);
            await StorageProvider.DeletePendingVerificationTest(this.SelectedTest);
            this.CurrentRaceSession = null;
            await this.HandleRefreshCommand();
        }
    }
}