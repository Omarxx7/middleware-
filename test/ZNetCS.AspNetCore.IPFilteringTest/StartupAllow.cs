﻿// -----------------------------------------------------------------------
// <copyright file="StartupAllow.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ZNetCS.AspNetCore.IPFilteringTest;

#region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

/// <summary>
/// The startup class.
/// </summary>
public class StartupAllow
{
    #region Fields

    private readonly IConfiguration configuration;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupAllow"/> class.
    /// </summary>
    /// <param name="configuration">
    /// The configuration interface.
    /// </param>
    public StartupAllow(IConfiguration configuration)
        => this.configuration = configuration;

    #endregion

    #region Public Methods

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">
    /// The application builder.
    /// </param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseIPFiltering();
        app.Run(async context => { await context.Response.WriteAsync("Hello World"); });
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIPFiltering(this.configuration.GetSection("IPFiltering"));
    }

    #endregion
}