namespace AppointmentScheduler.Domain.Requests {
    public class UpdateProfileRequest {
        public string Fullname { get; set; }
        public DateOnly? BirthDate { get; set; }
        public char Gender { get; set; }
    }
}
