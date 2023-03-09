using Isu.Extra.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class DayTimetable
{
    private readonly List<Lesson> _lessonsList;

    public DayTimetable()
    {
        _lessonsList = new List<Lesson>();
    }

    public DayTimetable(List<Lesson> lessons)
    {
        _lessonsList = new List<Lesson>(lessons);
    }

    public List<Lesson> LessonsList => new List<Lesson>(_lessonsList);

    public static bool LessonInListCrossingCheck<T>(List<T> lessons)
    {
        if (lessons is List<Lesson> lessonsToCheck)
        {
            for (int i = 0; i < lessonsToCheck.Count - 1; ++i)
            {
                if (lessonsToCheck[i].TimeInterval.End > lessonsToCheck[i + 1].TimeInterval.Start)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool DayLessonCrossingCheck<T>(List<T> lessonToCheck)
    {
        if (lessonToCheck is List<Lesson> l1)
        {
            int iter1 = 0;
            int iter2 = 0;
            while ((iter1 < l1.Count) && (iter2 < this._lessonsList.Count))
            {
                if (l1[iter1].TimeInterval.End < this._lessonsList[iter2].TimeInterval.Start)
                {
                    iter1++;
                }
                else if (l1[iter1].TimeInterval.Start > this._lessonsList[iter2].TimeInterval.End)
                {
                    iter2++;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void AddRangeOfPractices(List<Practice> lessonsToAdd)
    {
        lessonsToAdd.Sort();

        if (!LessonInListCrossingCheck(lessonsToAdd))
        {
            throw new TimetableException("Lessons in list can't cross");
        }

        if (!DayLessonCrossingCheck(lessonsToAdd))
        {
            throw new TimetableException("Lessons can't cross");
        }

        _lessonsList.AddRange(lessonsToAdd);
        _lessonsList.Sort();
    }

    public void AddRangeOfLectures(List<Lecture> lessonsToAdd)
    {
        lessonsToAdd.Sort();

        if (!LessonInListCrossingCheck(lessonsToAdd))
        {
            throw new TimetableException("Lessons in list can't cross");
        }

        if (!DayLessonCrossingCheck(lessonsToAdd))
        {
            throw new TimetableException("Lessons can't cross");
        }

        _lessonsList.AddRange(lessonsToAdd);
        _lessonsList.Sort();
    }

    public void AddRange(DayTimetable anotherDayTimetable)
    {
        if (!DayLessonCrossingCheck(anotherDayTimetable.LessonsList))
        {
            throw new TimetableException("Lessons can't cross");
        }

        _lessonsList.AddRange(anotherDayTimetable.LessonsList);
        _lessonsList.Sort();
    }
}