﻿namespace AppointmentScheduler.Domain.Requests.Create
{
    public class CreateProfileRequest
    {
        public uint? PatientId { get; set; }
        public string Fullname { get; set; }
        public DateOnly? BirthDate { get; set; }
        public char Gender { get; set; }
    }
}
