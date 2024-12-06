using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Presentation.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppointmentScheduler.Presentation.Models;

public class AppointmentModel
{
    
    [JsonPropertyName("id")]
    public uint? Id {  get; set; }

    [JsonPropertyName("at")]
    [Required(ErrorMessage = "Chọn ngày đặt lịch hẹn")]
    public DateTime? AtTime { get; set; }

    [Required(ErrorMessage = "Chọn hồ sơ")]
    [JsonPropertyName("profile")]
    public uint? Profile { get; set; } = null;

    [Required(ErrorMessage = "Chọn bác sĩ")]
    [JsonPropertyName("doctor")]
    public uint? Doctor { get; set; } = null;

    [Required(ErrorMessage = "Chọn trạng thái")]
    [JsonPropertyName("state")]
    public EAppointmentState State { get; set; }

    [JsonPropertyName("number")]
    public uint? Number { get; set; }

}