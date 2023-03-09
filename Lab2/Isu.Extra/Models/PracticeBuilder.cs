using Isu.Extra.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class PracticeBuilder
{
    private string? _name;
    private ITimetabled? _group;
    private TimeInterval? _timeInterval;
    private ClassroomNumber? _classroomNumber;
    private Teacher? _teacher;

    public PracticeBuilder Reset()
    {
        _name = null;
        _group = null;
        _teacher = null;
        _classroomNumber = null;
        _timeInterval = null;

        return this;
    }

    public PracticeBuilder Name(string name)
    {
        _name = name;

        return this;
    }

    public PracticeBuilder TimeInterval(TimeInterval timeInterval)
    {
        _timeInterval = timeInterval;

        return this;
    }

    public PracticeBuilder ClassroomNumber(ClassroomNumber classroomNumber)
    {
        _classroomNumber = classroomNumber;

        return this;
    }

    public PracticeBuilder Teacher(Teacher teacher)
    {
        _teacher = teacher;

        return this;
    }

    public PracticeBuilder Group(ITimetabled group)
    {
        _group = group;

        return this;
    }

    public Practice Build()
    {
        if (_group == null)
        {
            throw new LessonException("Group or JgofsGroup must be assigned");
        }

        if (_name == null)
        {
            throw new LessonException("Name of Lesson must be assigned");
        }

        if (_classroomNumber == null)
        {
            throw new LessonException("Classroom of Lesson must be assigned");
        }

        if (_timeInterval == null)
        {
            throw new LessonException("Time Interval of Lesson must be assigned");
        }

        if (_teacher == null)
        {
            throw new LessonException("Teacher of Lesson must be assigned");
        }

        return new Practice(_name, _timeInterval, _classroomNumber, _teacher, _group);
    }
}