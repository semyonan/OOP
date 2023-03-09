using Isu.Entities;
using Isu.Exception;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private const int MaxIsuId = 999999;

    private readonly List<Group> _groupList = new List<Group>();
    private readonly List<Student> _studentList = new List<Student>();
    private int _isuId = 100000;

    public Group AddGroup(GroupName name)
    {
        if (GroupAlreadyExists(name))
        {
            throw new IsuException("Group with this name already exists");
        }

        var group = new Group(name);
        _groupList.Add(group);

        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        Group? currentGroup = FindGroup(group.GroupName);

        if (currentGroup == null)
        {
            throw new IsuException("Group doesn't exists");
        }

        if (_isuId > MaxIsuId)
        {
            throw new IsuException("Maximum student ID has been reached");
        }

        Student student = currentGroup.AddStudent(new Student(name, _isuId, currentGroup));
        _isuId++;

        _studentList.Add(student);

        return student;
    }

    public Student GetStudent(int id)
    {
        Student? student = FindStudent(id);

        if (student == null)
        {
            throw new IsuException("Student with this ID doesn't exists");
        }

        return student;
    }

    public Student? FindStudent(int id)
    {
        return _studentList.FirstOrDefault(x => x.Id == id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        Group? currentGroup = FindGroup(groupName);

        if (currentGroup == null)
        {
            throw new IsuException("Group doesn't exists");
        }

        return currentGroup.ListOfStudents.ToList();
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        List<Group> selectedGroups = FindGroups(courseNumber);

        if (!selectedGroups.Any())
        {
            throw new IsuException("Groups with this course number doesn't exists");
        }

        return selectedGroups.SelectMany(g => g.ListOfStudents).ToList();
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _groupList.FirstOrDefault(x => x.GroupName.Equals(groupName));
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _groupList.Where(g => g.GroupName.CourseNumber.Equals(courseNumber)).ToList();
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        Group? currentGroup = FindGroup(newGroup.GroupName);

        if (currentGroup == null)
        {
            throw new IsuException("Group doesn't exists");
        }

        Student? currentStudent = FindStudent(student.Id);

        if (currentStudent == null)
        {
            throw new IsuException("Student doesn't exists");
        }

        currentStudent.GroupChange(currentGroup);
    }

    private bool GroupAlreadyExists(GroupName groupName) => _groupList.Exists(x => x.GroupName.Equals(groupName));
}