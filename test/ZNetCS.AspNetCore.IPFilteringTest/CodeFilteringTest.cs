﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeFilteringTest.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The filtering test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.IPFilteringTest;

#region Usings

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ZNetCS.AspNetCore.IPFiltering;

#endregion

/// <summary>
/// The filtering test.
/// </summary>
[TestClass]
public class CodeFilteringTest
{
    #region Public Methods

    /// <summary>
    /// The code allow on whitelist test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowForwardedForOnWhitelistTest()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "192.168.0.10-192.168.10.20" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Forwarded-For", "192.168.0.10, 192.168.0.9, 192.168.0.8");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow no IP test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowNoIPTest()
    {
        using var factory = WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { DefaultBlockLevel = DefaultBlockLevel.None });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The allow all test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPAllTest()
    {
        using var factory = WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { DefaultBlockLevel = DefaultBlockLevel.None });

        // Act
        HttpResponseMessage response = await factory.Server.CreateRequest("/").SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow not on blacklist 1 test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPNotOnBlacklist1Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    DefaultBlockLevel = DefaultBlockLevel.None,
                    Blacklist = new List<string> { "192.168.0.0/255.255.255.0" }
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.10.1");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow not on blacklist 2 test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPNotOnBlacklist2Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    DefaultBlockLevel = DefaultBlockLevel.None,
                    Blacklist = new List<string> { "192.168.0.10-192.168.10.20" }
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.9");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow not on blacklist 3 test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPNotOnBlacklist3Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    DefaultBlockLevel = DefaultBlockLevel.None,
                    Blacklist = new List<string> { "fe80::/10" }
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "::1");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow not on blacklist 4 test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPNotOnBlacklist4Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    DefaultBlockLevel = DefaultBlockLevel.None,
                    Blacklist = new List<string> { "192.168.0.1" }
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.2");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow on whitelist 1 test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPOnWhitelist1Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "192.168.0.0/255.255.255.0" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.34");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow on whitelist 2 test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPOnWhitelist2Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "192.168.0.10-192.168.10.20" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.3.45");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow on whitelist 3 test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPOnWhitelist3Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "fe80::/10" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "fe80::d503:4ee:3882:c586");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The code allow on whitelist 4 test.
    /// </summary>
    [TestMethod]
    public async Task CodeAllowRealIPOnWhitelist4Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "192.168.0.1" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.1");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
    }

    /// <summary>
    /// The block all forbidden test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockAllForbiddenTest()
    {
        using var factory = WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { HttpStatusCode = HttpStatusCode.Forbidden });

        // Act
        HttpResponseMessage response = await factory.Server.CreateRequest("/").SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode, "StatusCode != Forbidden");
    }

    /// <summary>
    /// The block all not found test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockAllNotFoundTest()
    {
        using var factory = WebApplicationFactoryHelper.CreateCodeFactory();

        // Act
        HttpResponseMessage response = await factory.Server.CreateRequest("/").SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block not on whitelist test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockForwarderForNotOnWhitelistTest()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "192.168.0.10-192.168.10.20" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Forwarded-For", "192.168.0.9, 192.168.0.10, 192.168.0.11");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block no IP test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockNoIPTest()
    {
        using var factory = WebApplicationFactoryHelper.CreateCodeFactory();

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != NotFound");
    }

    /// <summary>
    /// The code block not on whitelist 1 test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPNotOnWhitelist1Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "192.168.0.0/255.255.255.0" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.10.1");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block not on whitelist 2 test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPNotOnWhitelist2Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "192.168.0.10-192.168.10.20" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.9");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block not on whitelist 3 test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPNotOnWhitelist3Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "fe80::/10" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "::1");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block not on whitelist 4 test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPNotOnWhitelist4Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(new IPFilteringOptions { Whitelist = new List<string> { "192.168.0.1" } });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.2");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block on blacklist 1 test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPOnBlacklist1Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    DefaultBlockLevel = DefaultBlockLevel.None,
                    Blacklist = new List<string> { "192.168.0.0/255.255.255.0" }
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.10");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block on blacklist 2 test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPOnBlacklist2Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    DefaultBlockLevel = DefaultBlockLevel.None,
                    Blacklist = new List<string> { "192.168.0.10-192.168.10.20" }
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.11");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block on blacklist 3 test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPOnBlacklist3Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    DefaultBlockLevel = DefaultBlockLevel.None,
                    Blacklist = new List<string> { "fe80::/10" }
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "fe80::d503:4ee:3882:c586");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block on blacklist 4 test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPOnBlacklist4Test()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    DefaultBlockLevel = DefaultBlockLevel.None,
                    Blacklist = new List<string> { "192.168.0.1" }
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.1");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    /// <summary>
    /// The code block on whitelist but blacklisted test.
    /// </summary>
    [TestMethod]
    public async Task CodeBlockRealIPOnWhitelistBlacklistTest()
    {
        using var factory =
            WebApplicationFactoryHelper.CreateCodeFactory(
                new IPFilteringOptions
                {
                    Whitelist = new List<string> { "192.168.0.10-192.168.10.20" },
                    Blacklist = new List<string> { "192.168.0.100-192.168.0.150" },
                });

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        request.AddHeader("X-Real-IP", "192.168.0.120");

        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "StatusCode != Not Found");
    }

    #endregion
}