﻿using WebTimetable.Contracts.Models;

namespace WebTimetable.Contracts.Responses;

public class ScheduleResponse
{
    public List<LessonItem> Schedule { get; init; }
}