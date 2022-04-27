using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GT3e.Admin.ViewModels;

public class CustomSkinsViewModel : ObservableObject
{
  private RaceSessionViewModel currentRaceSession;
  private bool isDownloadEnabled;
  private CustomSkinInfo? selectedSkin;

  public CustomSkinsViewModel()
  {
    this.RefreshCommand = new AsyncRelayCommand(this.HandleRefreshCommand);
    this.DownloadCommand = new AsyncRelayCommand(this.HandleDownloadCommand);
    this.Skins = new ObservableCollection<CustomSkinInfo>();
    // this.HandleRefreshCommand()
    //     .GetAwaiter()
    //     .GetResult();
  }

  public IAsyncRelayCommand DownloadCommand { get; }
  public IAsyncRelayCommand RefreshCommand { get; }
  public ObservableCollection<CustomSkinInfo> Skins { get; }
  public RaceSessionViewModel CurrentRaceSession
  {
    get => this.currentRaceSession;
    set => this.SetProperty(ref this.currentRaceSession, value);
  }

  public bool IsDownloadEnabled
  {
    get => this.isDownloadEnabled;
    set => this.SetProperty(ref this.isDownloadEnabled, value);
  }

  public CustomSkinInfo? SelectedSkin
  {
    get => this.selectedSkin;
    set
    {
      this.SetProperty(ref this.selectedSkin, value);
      this.IsDownloadEnabled = value != null;
    }
  }

  private async Task HandleDownloadCommand()
  {
    await StorageProvider.DownloadCustomSkin(this.selectedSkin);
  }

  private async Task HandleRefreshCommand()
  {
    var customSkins = await StorageProvider.GetCustomSkins();
    this.Skins.Clear();
    foreach(var customSkin in customSkins)
    {
      this.Skins.Add(customSkin);
    }
  }
}