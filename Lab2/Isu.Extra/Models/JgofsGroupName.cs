using Isu.Extra.Entities;
using Isu.Extra.Exception;

namespace Isu.Extra.Models;

public class JgofsGroupName : IEquatable<JgofsGroupName>
{
    private const int MinJgofsGroupNameLength = 6;
    private const int MaxJgofsGroupNameLength = 7;

    private char[] _streamId = { '1', '2', '3', '4', '5', '6' };
    private char[] _groupId = { '1', '2', '3', '4', '5', '6' };

    public JgofsGroupName(string name, Faculty faculty)
    {
        if (string.IsNullOrEmpty(name)
            || (name.Length is > MaxJgofsGroupNameLength or < MinJgofsGroupNameLength)
            || ((name.Length == MaxJgofsGroupNameLength)
                && (faculty.JgofsNames.Contains(new JgofsName(name.Substring(0, 3))) is false))
            || ((name.Length == MinJgofsGroupNameLength)
                && (faculty.JgofsNames.Contains(new JgofsName(name.Substring(0, 2))) is false))
            || (name[^4] != ' ')
            || (_streamId.Contains(name[^3]) is false)
            || (name[^2] != '.')
            || (_groupId.Contains(name[^1]) is false))
        {
            throw new JgofsException("Invalid JgofsGroupName");
        }

        Name = name;
        Stream = name[^3] - '0';
        Group = name[^1] - '0';
        Faculty = faculty;

        if (name.Length == MaxJgofsGroupNameLength)
        {
            JgofsName = new JgofsName(name.Substring(0, 3));
        }
        else
        {
            JgofsName = new JgofsName(name.Substring(0, 2));
        }
    }

    public string Name { get; }
    public JgofsName JgofsName { get; }
    public int Stream { get; }
    public int Group { get; }
    public Faculty Faculty { get; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, JgofsName, Stream, Group, Faculty);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((JgofsGroupName)obj);
    }

    public bool Equals(JgofsGroupName? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && JgofsName.Equals(other.JgofsName) && Stream == other.Stream && Group == other.Group && Faculty.Equals(other.Faculty);
    }
}