#pragma warning disable NUnit1032

using NUnit.Framework;
using SafeVaultAPI.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class TestInputValidation
{
    private UserController? controller;

    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string?> {
            {"ConnectionStrings:DefaultConnection", "dummy"}
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        controller = new UserController(config);
    }

    // 🔒 SQL Injection Test
    [Test]
    public void TestForSQLInjection()
    {
        var result = controller!.Login(new SafeVaultAPI.Models.User {
            Username = "' OR 1=1 --",
            Password = "test123"
        });

        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    // 🔒 XSS Test
    [Test]
    public void TestForXSS()
    {
        var result = controller!.Login(new SafeVaultAPI.Models.User {
            Username = "<script>alert(1)</script>",
            Password = "test123"
        });

        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    // 🔐 Invalid Login Test
    [Test]
    public void TestInvalidLogin()
    {
        var result = controller!.Login(new SafeVaultAPI.Models.User {
            Username = "wrong",
            Password = "wrong"
        });

        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    // 🚫 Unauthorized Access (non-admin trying admin)
    [Test]
    public void TestUnauthorizedAccess()
    {
        var result = controller!.AdminDashboard("user");

        Assert.That(result, Is.InstanceOf<ForbidResult>());
    }

    // ✅ Admin Access
    [Test]
    public void TestAdminAccess()
    {
        var result = controller!.AdminDashboard("admin");

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
}