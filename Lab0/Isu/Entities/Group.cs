using Isu.Exception;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private const int MaxStudentNumber = 25;

    private readonly List<Student> _listOfStudents;
    public Group(GroupName groupName)
    {
        GroupName = groupName;
        _listOfStudents = new List<Student>();
    }

    public GroupName GroupName { get; }
    public IReadOnlyList<Student> ListOfStudents => _listOfStudents;

    public Student AddStudent(Student student)
    {
        if (_listOfStudents.Contains(student))
        {
            throw new IsuException("Student is already in this group");
        }

        if (_listOfStudents.Count == MaxStudentNumber)
        {
            throw new IsuException("The maximum number of students has been reached");
        }

        _listOfStudents.Add(student);

        return student;
    }

    public void RemoveStudent(Student student)
    {
        if (!_listOfStudents.Contains(student))
        {
            throw new IsuException("Group doesn't contain this student");
        }

        _listOfStudents.Remove(student);
    }
}