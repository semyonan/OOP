using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Teacher : IEquatable<Teacher>
{
    public Teacher(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new LessonException("Invalid  Teacher name");
        }

        Name = name;
    }

    public string Name { get; }

    public bool Equals(Teacher? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Teacher)obj);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}