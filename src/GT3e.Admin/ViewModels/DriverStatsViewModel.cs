using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GT3e.Admin.ViewModels;

public class DriverStatsViewModel : ObservableObject
{
    private DriverStats selectedStats;
    private Visibility statsPanelVisibility;

    public DriverStatsViewModel()
    {
        this.RefreshCommand = new AsyncRelayCommand(this.HandleRefreshCommand);
        this.DriverStatsList = new ObservableCollection<DriverStats>();
        this.StatsPanelVisibility = Visibility.Collapsed;
        this.HandleRefreshCommand();
    }

    public ObservableCollection<DriverStats> DriverStatsList { get; }
    public IAsyncRelayCommand RefreshCommand { get; }

    public DriverStats SelectedStats
    {
        get => this.selectedStats;
        set
        {
            this.SetProperty(ref this.selectedStats, value);
            this.StatsPanelVisibility = value != null ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public Visibility StatsPanelVisibility
    {
        get => this.statsPanelVisibility;
        set => this.SetProperty(ref this.statsPanelVisibility, value);
    }

    private async Task HandleRefreshCommand()
    {
        var driverStatsList = await StorageProvider.GetDriverStats();
        this.DriverStatsList.Clear();
        foreach(var driverStats in driverStatsList)
        {
            this.DriverStatsList.Add(driverStats);
        }

        if(!this.DriverStatsList.Any())
        {
            return;
        }
        this.SelectedStats = this.DriverStatsList[0];
    }
}