namespace GT3e.Acc.Messages;

public enum OutboundMessageTypes : byte
{
    RegisterCommandApplication = 1,
    UnregisterCommandApplication = 9,

    RequestEntryList = 10,
    RequestTrackData = 11,

    ChangeHudPage = 49,
    ChangeFocus = 50,
    InstantReplayRequest = 51,

    PlayManualReplayHighlight = 52, // TODO, but planned
    SaveManualReplayHighlight = 60  // TODO, but planned: saving manual replays gives distributed clients the possibility to see the play the same replay
}