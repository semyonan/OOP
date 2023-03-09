using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class ClassroomNumber : IEquatable<ClassroomNumber>
{
    private const int MinBuildingAndFloorNumber = 1;
    private const int MaxBuildingAndFloorNumber = 4;
    public ClassroomNumber(uint number)
    {
        if ((number / 1000 < MinBuildingAndFloorNumber)
            || (number / 1000 > MaxBuildingAndFloorNumber)
            || ((number % 1000) / 100 < MinBuildingAndFloorNumber)
            || ((number % 1000) / 100 > MaxBuildingAndFloorNumber))
        {
            throw new LessonException("Invalid classroom number");
        }

        Number = number;
    }

    public uint Number { get; }

    public bool Equals(ClassroomNumber? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClassroomNumber)obj);
    }

    public override int GetHashCode()
    {
        return (int)Number;
    }
}