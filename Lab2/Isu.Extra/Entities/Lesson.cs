using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public abstract class Lesson
{
    protected static readonly TimeSpan TwoAcademicHours = new (0, 90, 0);

    protected Lesson(
        string name,
        TimeInterval timeInterval,
        ClassroomNumber classroomNumber,
        Teacher teacher)
    {
        if (string.IsNullOrEmpty(name)
            || (timeInterval.End - timeInterval.Start != TwoAcademicHours))
        {
            throw new LessonException("Invalid argoments");
        }

        Name = name;
        TimeInterval = timeInterval;
        ClassroomNumber = classroomNumber;
        Teacher = teacher;
    }

    public string Name { get; }
    public TimeInterval TimeInterval { get; }
    public ClassroomNumber ClassroomNumber { get; }
    public Teacher Teacher { get; }

    public int CompareTo(object? obj)
    {
        if (obj is not Lesson lesson)
        {
            throw new LessonException("Invalid argument");
        }

        return TimeInterval.CompareTo(lesson.TimeInterval);
    }
}