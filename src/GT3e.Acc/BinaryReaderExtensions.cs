using System.Text;
using GT3e.Acc.Messages;

namespace GT3e.Acc;

internal static class BinaryReaderExtensions
{
    internal static BroadcastingEvent ReadBroadcastingEvent(this BinaryReader reader)
    {
        return new BroadcastingEvent
        {
            BroadcastingEventType = (BroadcastingEventType) reader.ReadByte(),
            Message = reader.ReadStr(),
            TimeMs = reader.ReadInt32(),
            CarId = reader.ReadInt32()
        };
    }

    internal static LapInfo ReadLap(this BinaryReader binaryReader)
    {
        var lap = new LapInfo();
        lap.UpdateFromReader(binaryReader);
        return lap;
    }

    internal static string ReadStr(this BinaryReader binaryReader)
    {
        var length = binaryReader.ReadUInt16();
        var bytes = binaryReader.ReadBytes(length);
        return Encoding.UTF8.GetString(bytes);
    }

    internal static void UpdateCarInfo(this BinaryReader reader, CarInfo carInfo)
    {
        carInfo.CarModelType = reader.ReadByte();
        carInfo.TeamName = reader.ReadStr();
        carInfo.RaceNumber = reader.ReadInt32();
        carInfo.CupCategory = reader.ReadByte();
        carInfo.CurrentDriverIndex = reader.ReadByte();
        carInfo.Nationality = (Nationality) reader.ReadUInt16();

        var driversOnCarCount = reader.ReadByte();
        for(var di = 0; di < driversOnCarCount; di++)
        {
            var driverInfo = new DriverInfo
            {
                FirstName = reader.ReadStr(),
                LastName = reader.ReadStr(),
                ShortName = reader.ReadStr(),
                Category = (DriverCategory) reader.ReadByte(),
                Nationality = (Nationality) reader.ReadUInt16()
            };

            carInfo.AddDriver(driverInfo);
        }
    }

    internal static RealtimeUpdate ReadRealtimeUpdate(this BinaryReader reader)
    {
        var realtimeUpdate = new RealtimeUpdate
        {
            EventIndex = (int) reader.ReadUInt16(),
            SessionIndex = (int) reader.ReadUInt16(),
            SessionType = (RaceSessionType) reader.ReadByte(),
            Phase = (SessionPhase) reader.ReadByte()
        };
        var sessionTime = reader.ReadSingle();
        realtimeUpdate.SessionTime = TimeSpan.FromMilliseconds(sessionTime);
        var sessionEndTime = reader.ReadSingle();
        realtimeUpdate.SessionEndTime = TimeSpan.FromMilliseconds(sessionEndTime);

        realtimeUpdate.FocusedCarIndex = reader.ReadInt32();
        realtimeUpdate.ActiveCameraSet = reader.ReadStr();
        realtimeUpdate.ActiveCamera = reader.ReadStr();
        realtimeUpdate.CurrentHudPage = reader.ReadStr();

        realtimeUpdate.IsReplayPlaying = reader.ReadByte() > 0;
        if(realtimeUpdate.IsReplayPlaying)
        {
            realtimeUpdate.ReplaySessionTime = reader.ReadSingle();
            realtimeUpdate.ReplayRemainingTime = reader.ReadSingle();
        }

        realtimeUpdate.TimeOfDay = TimeSpan.FromMilliseconds(reader.ReadSingle());
        realtimeUpdate.AmbientTemp = reader.ReadByte();
        realtimeUpdate.TrackTemp = reader.ReadByte();
        realtimeUpdate.Clouds = reader.ReadByte() / 10.0f;
        realtimeUpdate.RainLevel = reader.ReadByte() / 10.0f;
        realtimeUpdate.Wetness = reader.ReadByte() / 10.0f;

        realtimeUpdate.BestSessionLap = reader.ReadLap();
        return realtimeUpdate;
    }

    internal static RealtimeCarUpdate ReadRealtimeCarUpdate(this BinaryReader reader)
    {
        var update = new RealtimeCarUpdate
        {
            CarIndex = reader.ReadUInt16(),
            DriverIndex = reader.ReadUInt16(),
            DriverCount = reader.ReadByte(),
            Gear = reader.ReadByte() - 2,
            WorldPosX = reader.ReadSingle(),
            WorldPosY = reader.ReadSingle(),
            Yaw = reader.ReadSingle(),
            CarLocation = (CarLocation)reader.ReadByte(),
            Kmh = reader.ReadUInt16(),
            Position = reader.ReadUInt16(),
            CupPosition = reader.ReadUInt16(),
            TrackPosition = reader.ReadUInt16(),
            SplinePosition = reader.ReadSingle(),
            Laps = reader.ReadUInt16(),
            Delta = reader.ReadInt32(),
            BestSessionLap = reader.ReadLap(),
            LastLap = reader.ReadLap(),
            CurrentLap = reader.ReadLap()
        };

        return update;
    }

    internal static TrackDataUpdate ReadTrackDataUpdate(this BinaryReader reader, string connectionIdentifier)
    {
        var update = new TrackDataUpdate
        {
            ConnectionIdentifier = connectionIdentifier,
            TrackName = reader.ReadStr(),
            TrackId = reader.ReadInt32(),
            TrackMeters = reader.ReadInt32(),
            CameraSets = new Dictionary<string, List<string>>()
        };

        var cameraSetCount = reader.ReadByte();
        for(var camSet = 0; camSet < cameraSetCount; camSet++)
        {
            var camSetName = reader.ReadStr();
            update.CameraSets.Add(camSetName, new List<string>());

            var cameraCount = reader.ReadByte();
            for(var cam = 0; cam < cameraCount; cam++)
            {
                var cameraName = reader.ReadStr();
                update.CameraSets[camSetName]
                         .Add(cameraName);
            }
        }

        var hudPages = new List<string>();
        var hudPagesCount = reader.ReadByte();
        for(var i = 0; i < hudPagesCount; i++)
        {
            hudPages.Add(reader.ReadStr());
        }

        update.HudPages = hudPages;

        return update;
    }

    internal static void WriteString(this BinaryWriter binaryWriter, string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        binaryWriter.Write(Convert.ToUInt16(bytes.Length));
        binaryWriter.Write(bytes);
    }
}