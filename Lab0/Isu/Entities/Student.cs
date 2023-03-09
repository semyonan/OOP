using Isu.Exception;

namespace Isu.Entities;

public class Student
{
    public Student(string name, int id, Group group)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new IsuException("Invalid  StudentName");
        }

        Name = name;
        Id = id;
        Group = group;
    }

    public string Name { get; }
    public int Id { get; }
    public Group Group { get; private set; }

    public void GroupChange(Group newGroup)
    {
        newGroup.AddStudent(this);
        Group = newGroup;
        Group.RemoveStudent(this);
    }
}