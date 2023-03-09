namespace Isu.Extra.Exceptions;

public class LessonException : System.Exception
{
    public LessonException() { }
    public LessonException(string message)
        : base(message) { }
}