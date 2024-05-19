﻿// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Identities
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="UserIntegrationEventsController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Identities.EventsWebApis.Controllers;

using Dapr;

using Hexalith.Application.Events;
using Hexalith.Application.Projections;
using Hexalith.Application.States;
using Hexalith.Infrastructure.WebApis.Controllers;
using Hexalith.Identities.Domain.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class IdentitiesPubSubController.
/// Implements the <see cref="EventIntegrationController" />.
/// </summary>
/// <seealso cref="EventIntegrationController" />
/// <remarks>
/// Initializes a new instance of the <see cref="UserIntegrationEventsController"/> class.
/// </remarks>
/// <param name="eventProcessor">The event processor.</param>
/// <param name="projectionProcessor">The projection processor.</param>
/// <param name="hostEnvironment">The host environment.</param>
/// <param name="logger">The logger.</param>
[ApiController]
public abstract class UserIntegrationEventsController(
    IIntegrationEventProcessor eventProcessor,
    IProjectionUpdateProcessor projectionProcessor,
    IHostEnvironment hostEnvironment,
    ILogger logger) : EventIntegrationController(eventProcessor, projectionProcessor, hostEnvironment, logger)
{
    /// <summary>
    /// Handle aggregate external reference events as an asynchronous operation.
    /// </summary>
    /// <param name="eventState">State of the event.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [UserEventsBusTopic]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("/handle-customer-events")]
    public async Task<ActionResult> HandleUserEventsAsync(EventState eventState)
         => await HandleEventAsync(
                eventState,
                IdentitiesDomainHelper.UserAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);
}