﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FestivalZnanostiApi.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int Status { get; set; }

    public string Type { get; set; }

    public int VisitorsCount { get; set; }

    public string Equipment { get; set; }

    public string Summary { get; set; }

    public int LocationId { get; set; }

    public int? SubmitterId { get; set; }

    public int FestivalYearId { get; set; }

    [JsonIgnore]
    public virtual FestivalYear FestivalYear { get; set; }

    [JsonIgnore]
    public virtual ICollection<Lecturer> Lecturer { get; set; } = new List<Lecturer>();


    [JsonIgnore]
    public virtual Location Location { get; set; }


    [JsonIgnore]
    public virtual Account Submitter { get; set; }

    [JsonIgnore]
    public virtual ICollection<ParticipantsAge> ParticipantsAge { get; set; } = new List<ParticipantsAge>();

    [JsonIgnore]
    public virtual ICollection<TimeSlot> TimeSlot { get; set; } = new List<TimeSlot>();
}