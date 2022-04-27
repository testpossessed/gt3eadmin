using System;
using Acc.Lib.Messages;
using Newtonsoft.Json;

namespace GT3e.Admin.Models;

public class AccEvent
{
  public AccEvent()
  {
    this.Timestamp = DateTime.Now;
  }

  public DateTime Timestamp { get; }
  public BroadcastingEventType BroadcastingEventType { get; set; }
  public object Message { get; set; }
  public InboundMessageTypes MessageType { get; set; }
  public string Json  => JsonConvert.SerializeObject(Message, Formatting.Indented);
}