using System;
using Dapper;
using SWOF.Api.Models.Enums;

namespace SWOF.Api.Models
{
[Table("Schedules")]
public class Schedule
{
public DateTime Date { get; set; }

public Shift Shift { get; set; }

public int EngineerId { get; set; }
}
}
