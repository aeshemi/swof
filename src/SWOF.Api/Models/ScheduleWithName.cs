using System;
using SWOF.Api.Models.Enums;

namespace SWOF.Api.Models
{
public class ScheduleWithName
{
public DateTime Date { get; set; }

public Shift Shift { get; set; }

public string Name { get; set; }
}
}
