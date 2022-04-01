using System.Collections.ObjectModel;
using System.Windows.Input;
using GT3e.Admin.Models;
using GT3e.Admin.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GT3e.Admin.ViewModels;

public class PendingVerificationTestsViewModel : ObservableObject
{
    public PendingVerificationTestsViewModel()
    {
        this.RefreshCommand = new RelayCommand(this.HandleRefreshCommand);
        this.PendingTests = new ObservableCollection<VerificationTestPackageInfo>();
        this.HandleRefreshCommand();
    }

    public ObservableCollection<VerificationTestPackageInfo> PendingTests { get; set; }
    private void HandleRefreshCommand()
    {
        var pendingTests = StorageProvider.GetPendingVerificationTests();
        this.PendingTests.Clear();
        foreach(var pendingTest in pendingTests)
        {
            this.PendingTests.Add(pendingTest);
        }
    }
    public ICommand RefreshCommand { get; }
}