using System;

namespace BaseService.Models;

public class ApplicationNotification(string message, bool isError) : Message
{
    public string Message { get; set; } = message;
    public bool IsError { get; set; } = isError;
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}
