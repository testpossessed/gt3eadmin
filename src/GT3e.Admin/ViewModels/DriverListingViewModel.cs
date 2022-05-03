using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GT3e.Admin.ViewModels;

public class DriverListingViewModel : ObservableObject
{
    private int carIndex;
    private byte carModel;
    private string displayName;
    private float distanceIntoLap;
    private float gap;
    private string gapText;
    private int position;
    private int positionInClass;
    private int positionOnTrack;
    private int raceNumber;
    private float speedKmh;

    public int CarIndex
    {
        get => this.carIndex;
        set => this.SetProperty(ref this.carIndex, value);
    }

    public byte CarModel
    {
        get => this.carModel;
        set => this.SetProperty(ref this.carModel, value);
    }

    public string DisplayName
    {
        get => this.displayName;
        set => this.SetProperty(ref this.displayName, value);
    }

    public float DistanceIntoLap
    {
        get => this.distanceIntoLap;
        set => this.SetProperty(ref this.distanceIntoLap, value);
    }

    public float Gap
    {
        get => this.gap;
        set => this.SetProperty(ref this.gap, value);
    }

    public string GapText
    {
        get => this.gapText;
        set => this.SetProperty(ref this.gapText, value);
    }

    public int Position
    {
        get => this.position;
        set => this.SetProperty(ref this.position, value);
    }

    public int PositionInClass
    {
        get => this.positionInClass;
        set => this.SetProperty(ref this.positionInClass, value);
    }

    public int PositionOnTrack
    {
        get => this.positionOnTrack;
        set => this.SetProperty(ref this.positionOnTrack, value);
    }

    public int RaceNumber
    {
        get => this.raceNumber;
        set => this.SetProperty(ref this.raceNumber, value);
    }

    public float SpeedKmh
    {
        get => this.speedKmh;
        set
        {
            this.SetProperty(ref this.speedKmh, value);
            this.GapText = this.SpeedKmh < 10? "---": $"{this.Gap / this.SpeedKmh * Constants.GapFactor:F}";
        }
    }
}