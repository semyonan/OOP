using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Timetable
{
    private readonly List<DayTimetable> _dayTimetables;

    public Timetable(TimetableBuilder builder)
    {
        _dayTimetables = new List<DayTimetable>(builder.DayTimetables);
    }

    public enum Day
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }

    public enum Week
    {
        Odd,
        Even,
    }

    public List<DayTimetable> DayTimetables => _dayTimetables;

    public bool LessonCrossingCheck(Timetable t1)
    {
        for (int day = 0; day < this._dayTimetables.Count; ++day)
        {
            if (!_dayTimetables[day].DayLessonCrossingCheck(t1._dayTimetables[day].LessonsList))
            {
                return false;
            }
        }

        return true;
    }
}