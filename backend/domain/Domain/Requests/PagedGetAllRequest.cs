namespace AppointmentScheduler.Domain.Requests;

public class PagedGetAllRequest
{
    public int Offset { get; set; } = 0;
    public int Count { get; set; } = int.MaxValue;
    public string By { get; set; }
}