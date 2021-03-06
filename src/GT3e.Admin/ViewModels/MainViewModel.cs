using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Syncfusion.SfSkinManager;

namespace GT3e.Admin.ViewModels;

public class MainViewModel : ObservableRecipient
{
  private string selectedTheme;
  private string statusMessage = "Started";
  private int selectedTabIndex;

  public MainViewModel()
  {
    this.SetInitialTheme();
    this.Initialise();
  }

  public ConsoleViewModel Console { get; } = new();
  public CustomSkinsViewModel CustomSkins { get; } = new();
  public DriverStatsViewModel DriverStats { get; } = new();
  public LogViewModel Log { get; } = new();
  public PendingVerificationTestsViewModel PendingVerificationTests { get; } = new();
  public StewardControlCentreViewModel StewardControlCentre { get; } = new();

  public List<string> Themes { get; } = new()
                                        {
                                          "Blend",
                                          "Material Dark",
                                          "Office 2019 Dark Gray",
                                          "Office 365",
                                          "Saffron"
                                        };

  public string SelectedTheme
  {
    get => this.selectedTheme;
    set
    {
      this.SetProperty(ref this.selectedTheme, value);
      this.HandleThemeChanged();
    }
  }

  public int SelectedTabIndex
  {
    get => this.selectedTabIndex;
    set
    {
      this.SetProperty(ref this.selectedTabIndex, value);
      this.Console.ConsoleVisibility = value == 4? Visibility.Collapsed: Visibility.Visible;
    }
  }

  public string StatusMessage
  {
    get => this.statusMessage;
    set => this.SetProperty(ref this.statusMessage, value);
  }

  private void HandleThemeChanged()
  {
    var theme = this.SelectedTheme.Replace(" ", "");
    SettingsProvider.SaveSettings(new UserSettings
                                  {
                                    Theme = this.SelectedTheme
                                  });
    var result =
      MessageBox.Show(
        $"The theme will be applied when the application next starts.{Environment.NewLine}{Environment.NewLine}Do you want to exit now?",
        "Theme Change",
        MessageBoxButton.YesNo);

    if(result == MessageBoxResult.Yes)
    {
      Application.Current.Shutdown(0);
    }
  }

  private void Initialise()
  {
    if(!Directory.Exists(PathProvider.AppDocumentsFolderPath))
    {
      Directory.CreateDirectory(PathProvider.AppDocumentsFolderPath);
    }

    if(!Directory.Exists(PathProvider.AppDocumentsDownloadFolderPath))
    {
      Directory.CreateDirectory(PathProvider.AppDocumentsDownloadFolderPath);
    }
  }

  private void SetInitialTheme()
  {
    var settings = SettingsProvider.GetUserSettings();
    this.selectedTheme = settings.Theme;
    SfSkinManager.SetTheme(Application.Current.MainWindow,
      new Theme(this.SelectedTheme.Replace(" ", "")));
  }
}