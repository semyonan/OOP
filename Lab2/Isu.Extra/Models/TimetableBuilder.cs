using Isu.Extra.Entities;

namespace Isu.Extra.Models;

public class TimetableBuilder
{
    private readonly List<DayTimetable> _dayTimetables;

    public TimetableBuilder()
    {
        _dayTimetables = new List<DayTimetable>();

        for (int i = 0; i < 14; ++i)
        {
            _dayTimetables.Add(new DayTimetable());
        }
    }

    public List<DayTimetable> DayTimetables => _dayTimetables;

    public TimetableBuilder AddDayLectures(Timetable.Day day, Timetable.Week week, List<Lecture> lecturesToAdd)
    {
        _dayTimetables[(int)day + ((int)week * 7)].AddRangeOfLectures(lecturesToAdd);

        return this;
    }

    public TimetableBuilder AddDayPractices(Timetable.Day day, Timetable.Week week, List<Practice> practicesToAdd)
    {
        _dayTimetables[(int)day + ((int)week * 7)].AddRangeOfPractices(practicesToAdd);

        return this;
    }

    public TimetableBuilder AddLessonsFromTimetable(Timetable? timetable)
    {
        if (timetable != null)
        {
            for (int day = 0; day < this._dayTimetables.Count; ++day)
            {
                _dayTimetables[day].AddRange(timetable.DayTimetables[day]);
            }
        }

        return this;
    }

    public Timetable Build()
    {
        return new Timetable(this);
    }
}