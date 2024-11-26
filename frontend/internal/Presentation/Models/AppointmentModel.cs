﻿using AppointmentScheduler.Presentation.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppointmentScheduler.Presentation.Models;

public class AppointmentModel : BaseModel
{
    //[Required(ErrorMessage = "Chọn ngày đặt lịch hẹn")]
    //public DateTime AtTime { get => base.AtTime; set => base.AtTime = value; }

    //[Required(ErrorMessage = "Chọn hồ sơ")]
    //public uint? ProfileId { get => base.ProfileId; set => base.ProfileId = value; }

    //[Required(ErrorMessage = "Chọn bác sĩ")]
    //public uint DoctorId { get => base.DoctorId; set => base.DoctorId = value; }

    //[Required(ErrorMessage = "Chọn trạng thái")]
    //   public EAppointmentState? State { get => (EAppointmentState?)base.State; set => base.State = (uint?)value; }


    [Required(ErrorMessage = "Chọn ngày đặt lịch hẹn")]
    public DateTime? AtTime { get; set; }

    [Required(ErrorMessage = "Chọn hồ sơ")]
    public uint? ProfileId { get; set; }

    [Required(ErrorMessage = "Chọn bác sĩ")]
    public uint? DoctorId { get; set; }

    [Required(ErrorMessage = "Chọn trạng thái")]
    public EAppointmentState State { get; set; }


    public uint? Number { get; set; }

}