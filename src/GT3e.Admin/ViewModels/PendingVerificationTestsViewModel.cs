using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GT3e.Acc;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GT3e.Admin.ViewModels;

public class PendingVerificationTestsViewModel : ObservableObject
{
    private RaceSessionViewModel currentRaceSession;
    private bool isLoadEnabled;
    private VerificationTestPackageInfo? selectedTest;
    private Visibility statsPanelVisibility;

    public PendingVerificationTestsViewModel()
    {
        this.RefreshCommand = new RelayCommand(this.HandleRefreshCommand);
        this.LoadTestCommand = new AsyncRelayCommand(this.HandleLoadTestCommand);
        this.PendingTests = new ObservableCollection<VerificationTestPackageInfo>();
        this.StatsPanelVisibility = Visibility.Hidden;
        this.HandleRefreshCommand();

        // var raceSession = AccDataProvider.LoadRaceSession(
        //     @"C:\Users\contr\AppData\Local\GT3e.Admin\Downloads\VerificationTestPackages\76561197992680661\76561197992680661.json")!;
        // this.CurrentRaceSession = new RaceSessionViewModel(raceSession);
    }

    public IAsyncRelayCommand LoadTestCommand { get; }

    public ObservableCollection<VerificationTestPackageInfo> PendingTests { get; }
    public ICommand RefreshCommand { get; }

    public RaceSessionViewModel CurrentRaceSession
    {
        get => this.currentRaceSession;
        set => this.SetProperty(ref this.currentRaceSession, value);
    }

    public bool IsLoadEnabled
    {
        get => this.isLoadEnabled;
        set => this.SetProperty(ref this.isLoadEnabled, value);
    }

    public VerificationTestPackageInfo? SelectedTest
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
            Path.Combine(PathProvider.AccSavedReplaysFolderPath, $"{this.SelectedTest.Name}.rpy");
        File.Copy(replayFilePath, destinationFilePath, true);
        ConsoleLog.Write("Replay has been copied as a Saved replay in ACC ready for you to review.");
    }

    private void HandleRefreshCommand()
    {
        var pendingTests = StorageProvider.GetPendingVerificationTests();
        this.PendingTests.Clear();
        foreach(var pendingTest in pendingTests)
        {
            this.PendingTests.Add(pendingTest);
        }
    }
}