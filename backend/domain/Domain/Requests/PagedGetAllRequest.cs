namespace AppointmentScheduler.Domain.Requests;

public class PagedGetAllRequest
{
    public int Offset { get; set; }
    public int Count { get; set; }
    public string? By { get; set;}
}