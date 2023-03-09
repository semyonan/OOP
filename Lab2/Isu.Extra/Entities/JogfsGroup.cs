using Isu.Extra.Exception;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class JgofsGroup : ITimetabled
{
    private const int MaxStudentNumber = 25;

    private readonly List<TimetabledStudent> _listOfStudents;

    public JgofsGroup(JgofsGroupName groupName, Stream<JgofsGroup> stream)
    {
        GroupName = groupName;
        Stream = stream;
        Timetable = null;
        _listOfStudents = new List<TimetabledStudent>();
    }

    public JgofsGroupName GroupName { get; }
    public Stream<JgofsGroup> Stream { get; }
    public Timetable? Timetable { get; private set; }
    public IReadOnlyList<TimetabledStudent> ListOfStudents => _listOfStudents;
    public int Vacancy => MaxStudentNumber - _listOfStudents.Count;
    public void AddTimetable(Timetable timetable)
    {
        Timetable = timetable;
    }

    public TimetabledStudent AddStudent(TimetabledStudent student)
    {
        if (_listOfStudents.Contains(student))
        {
            throw new JgofsException("Student is already in this group");
        }

        if (_listOfStudents.Count == MaxStudentNumber)
        {
            throw new JgofsException("The maximum number of students has been reached");
        }

        _listOfStudents.Add(student);

        return student;
    }

    public void RemoveStudent(TimetabledStudent student)
    {
        if (!_listOfStudents.Contains(student))
        {
            throw new JgofsException("Group doesn't contain this student");
        }

        _listOfStudents.Remove(student);
    }
}