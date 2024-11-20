namespace AppointmentScheduler.Domain.Requests.Update
{
    public class UpdateProfileRequest
    {
        public string Fullname { get; set; }
        public DateOnly? BirthDate { get; set; }
        public char Gender { get; set; }
    }
}
