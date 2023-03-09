using Isu.Exception;

namespace Isu.Models;

public class CourseNumber
{
    private const uint MaxCourseNumber = 4;
    private const uint MinCourseNumber = 1;
    public CourseNumber(uint number)
    {
        if (number is < MinCourseNumber or > MaxCourseNumber)
        {
            throw new IsuException("Invalid course number");
        }

        Number = number;
    }

    public uint Number { get; }
    public override int GetHashCode()
    {
        return Number.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is CourseNumber courseNumber)
        {
            return courseNumber.Number == Number;
        }

        return false;
    }
}