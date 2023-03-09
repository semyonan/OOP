using System.Diagnostics;
using Isu.Entities;
using Isu.Exception;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var isu = new IsuService();

        Group group1 = isu.AddGroup(new GroupName("M3106"));
        Group group2 = isu.AddGroup(new GroupName("M3107"));

        Student student = isu.AddStudent(group1, "Семенова Анна");

        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group2, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");

        isu.ChangeStudentGroup(student, group2);

        Assert.True(student.Group.GroupName.Equals(new GroupName("M3107")));
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var isu = new IsuService();

        Group group1 = isu.AddGroup(new GroupName("M3106"));

        Student student = isu.AddStudent(group1, "Семенова Анна");

        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group1, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");
        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group1, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");
        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group1, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");
        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group1, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");
        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group1, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");
        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group1, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");
        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group1, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");
        isu.AddStudent(group1, "Соболев Олег");
        isu.AddStudent(group1, "Сидоров Андрей");
        isu.AddStudent(group1, "Иванов Иван");
        Assert.Throws<IsuException>(() => isu.AddStudent(group1, "Сидоров Андрей"));
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        var isu = new IsuService();

        Assert.Throws<IsuException>(() => isu.AddGroup(new GroupName("M31O6")));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var isu = new IsuService();

        Group group1 = isu.AddGroup(new GroupName("M3106"));
        Group group2 = isu.AddGroup(new GroupName("M3107"));

        Student student = isu.AddStudent(group1, "Семенова Анна");
        Assert.True(student.Group.GroupName.Equals(new GroupName("M3106")));

        isu.ChangeStudentGroup(student, group2);
        Assert.True(student.Group.GroupName.Equals(new GroupName("M3107")));
    }
}