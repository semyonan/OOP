using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Lecture : Lesson, IComparable
{
    public Lecture(
        string name,
        TimeInterval timeInterval,
        ClassroomNumber classroomNumber,
        Teacher teacher,
        Stream<TimetabledGroup>? groupsStream,
        Stream<JgofsGroup>? jgofsGroupsStream)
        : base(name, timeInterval, classroomNumber, teacher)
    {
        if (((groupsStream != null) && (jgofsGroupsStream != null))
            || ((groupsStream == null) && (jgofsGroupsStream == null)))
        {
            throw new LessonException("One of groupStream and jgofsStream must be assigned");
        }

        GroupsStream = groupsStream;
        JgofsStream = jgofsGroupsStream;
    }

    public Stream<JgofsGroup>? JgofsStream { get; }
    public Stream<TimetabledGroup>? GroupsStream { get; }
}