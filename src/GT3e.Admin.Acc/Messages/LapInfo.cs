namespace GT3e.Acc.Messages;

public class LapInfo
{
    internal void UpdateFromReader(BinaryReader binaryReader)
    {
        this.LapTimeMs = binaryReader.ReadInt32();
        this.CarIndex = binaryReader.ReadUInt16();
        this.DriverIndex = binaryReader.ReadUInt16();
        var splitCount = binaryReader.ReadByte();
        for(var i = 0; i < splitCount; i++)
        {
            this.Splits.Add(binaryReader.ReadInt32());
        }

        this.IsInvalid = binaryReader.ReadByte() > 0;
        this.IsValidForBest = binaryReader.ReadByte() > 0;

        var isOutLap = binaryReader.ReadByte() > 0;
        var isInLap = binaryReader.ReadByte() > 0;

        if(isOutLap)
        {
            this.LapType = LapType.Outlap;
        }
        else if(isInLap)
        {
            this.LapType = LapType.Inlap;
        }
        else
        {
            this.LapType = LapType.Regular;
        }

        // Now it's possible that this is "no" lap that doesn't even include a 
        // first split, we can detect this by comparing with int32.Max
        while(this.Splits.Count < 3)
        {
            this.Splits.Add(null);
        }

        // "null" entries are Int32.Max, in the C# world we can replace this to null
        for(var i = 0; i < this.Splits.Count; i++)
        {
            if(this.Splits[i] == int.MaxValue)
            {
                this.Splits[i] = null;
            }
        }

        if(this.LapTimeMs == int.MaxValue)
        {
            this.LapTimeMs = null;
        }
    }

    public int? LapTimeMs { get; set; }
    public List<int?> Splits { get; } = new();
    public ushort CarIndex { get; internal set; }
    public ushort DriverIndex { get; internal set; }
    public bool IsInvalid { get; internal set; }
    public bool IsValidForBest { get; internal set; }
    public LapType LapType { get; internal set; }

    public override string ToString()
    {
        return $"{this.LapTimeMs,5}|{string.Join("|", this.Splits)}";
    }
}