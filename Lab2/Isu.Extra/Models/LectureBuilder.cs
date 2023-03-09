using Isu.Extra.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class LectureBuilder
{
    private string? _name;
    private Stream<TimetabledGroup>? _groupsStream;
    private Stream<JgofsGroup>? _jgofsGroupStream;
    private TimeInterval? _timeInterval;
    private ClassroomNumber? _classroomNumber;
    private Teacher? _teacher;

    public LectureBuilder Reset()
    {
        _name = null;
        _groupsStream = null;
        _jgofsGroupStream = null;
        _teacher = null;
        _classroomNumber = null;
        _timeInterval = null;

        return this;
    }

    public LectureBuilder Name(string name)
    {
        _name = name;

        return this;
    }

    public LectureBuilder TimeInterval(TimeInterval timeInterval)
    {
        _timeInterval = timeInterval;

        return this;
    }

    public LectureBuilder ClassroomNumber(ClassroomNumber classroomNumber)
    {
        _classroomNumber = classroomNumber;

        return this;
    }

    public LectureBuilder Teacher(Teacher teacher)
    {
        _teacher = teacher;

        return this;
    }

    public LectureBuilder GroupStream(Stream<TimetabledGroup>? groupStream)
    {
        _groupsStream = groupStream;

        return this;
    }

    public LectureBuilder GroupStream(Stream<JgofsGroup>? jgofsGroupStream)
    {
        _jgofsGroupStream = jgofsGroupStream;

        return this;
    }

    public Lecture Build()
    {
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

        return new Lecture(_name, _timeInterval, _classroomNumber, _teacher, _groupsStream, _jgofsGroupStream);
    }
}