using Isu.Extra.Exception;

namespace Isu.Extra.Models;

public class JgofsName : IEquatable<JgofsName>
{
    private const int MinJgofsNameLength = 2;
    private const int MaxJgofsNameLength = 3;

    public JgofsName(string name)
    {
        if (string.IsNullOrEmpty(name)
            || (name.Length is > MaxJgofsNameLength or < MinJgofsNameLength))
        {
            throw new JgofsException("Invalid JgofsName");
        }

        Name = name;
    }

    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }

    public bool Equals(JgofsName? other)
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
        return Equals((JgofsName)obj);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}