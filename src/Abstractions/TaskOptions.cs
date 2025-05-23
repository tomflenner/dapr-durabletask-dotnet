﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;

namespace Microsoft.DurableTask;

/// <summary>
/// Options that can be used to control the behavior of orchestrator task execution.
/// </summary>
public record TaskOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskOptions"/> class.
    /// </summary>
    /// <param name="retry">The task retry options.</param>
    public TaskOptions(TaskRetryOptions? retry = null)
    {
        this.Retry = retry;
    }

    /// <summary>
    /// Gets the task retry options.
    /// </summary>
    public TaskRetryOptions? Retry { get; init; }

    /// <summary>
    /// Returns a new <see cref="TaskOptions" /> from the provided <see cref="RetryPolicy" />.
    /// </summary>
    /// <param name="policy">The policy to convert from.</param>
    /// <returns>A <see cref="TaskOptions" /> built from the policy.</returns>
    public static TaskOptions FromRetryPolicy(RetryPolicy policy)
    {
        return new TaskOptions(policy);
    }

    /// <summary>
    /// Returns a new <see cref="TaskOptions" /> from the provided <see cref="AsyncRetryHandler" />.
    /// </summary>
    /// <param name="handler">The handler to convert from.</param>
    /// <returns>A <see cref="TaskOptions" /> built from the handler.</returns>
    public static TaskOptions FromRetryHandler(AsyncRetryHandler handler)
    {
        return new TaskOptions(handler);
    }

    /// <summary>
    /// Returns a new <see cref="TaskOptions" /> from the provided <see cref="RetryHandler" />.
    /// </summary>
    /// <param name="handler">The handler to convert from.</param>
    /// <returns>A <see cref="TaskOptions" /> built from the handler.</returns>
    public static TaskOptions FromRetryHandler(RetryHandler handler)
    {
        return new TaskOptions(handler);
    }

    /// <summary>
    /// Returns a new <see cref="SubOrchestrationOptions" /> with the provided instance ID. This can be used when
    /// starting a new sub-orchestration to specify the instance ID.
    /// </summary>
    /// <param name="instanceId">The instance ID to use.</param>
    /// <returns>A new <see cref="SubOrchestrationOptions" />.</returns>
    public SubOrchestrationOptions WithInstanceId(string instanceId)
    {
        return new SubOrchestrationOptions(this, instanceId);
    }
}

/// <summary>
/// Options that can be used to control the behavior of orchestrator task execution. This derived type can be used to
/// supply extra options for orchestrations.
/// </summary>
public record SubOrchestrationOptions : TaskOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubOrchestrationOptions"/> class.
    /// </summary>
    /// <param name="retry">The task retry options.</param>
    /// <param name="instanceId">The orchestration instance ID.</param>
    public SubOrchestrationOptions(TaskRetryOptions? retry = null, string? instanceId = null)
        : base(retry)
    {
        this.InstanceId = instanceId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubOrchestrationOptions"/> class.
    /// </summary>
    /// <param name="options">The task options to wrap.</param>
    /// <param name="instanceId">The orchestration instance ID.</param>
    public SubOrchestrationOptions(TaskOptions options, string? instanceId = null)
        : base(options)
    {
        this.InstanceId = instanceId;
        if (instanceId is null && options is SubOrchestrationOptions derived)
        {
            this.InstanceId = derived.InstanceId;
        }
    }

    /// <summary>
    /// Gets the orchestration instance ID.
    /// </summary>
    public string? InstanceId { get; init; }
}

/// <summary>
/// Options for submitting new orchestrations via the client.
/// </summary>
public record StartOrchestrationOptions
{
    /// <summary>
    /// Options for submitting new orchestrations via the client.
    /// </summary>
    /// <param name="InstanceId">
    /// The unique ID of the orchestration instance to schedule. If not specified, a new GUID value is used.
    /// </param>
    /// <param name="StartAt">
    /// The time when the orchestration instance should start executing. If not specified or if a date-time in the past
    /// is specified, the orchestration instance will be scheduled immediately.
    /// </param>
    public StartOrchestrationOptions(string? InstanceId = null, DateTimeOffset? StartAt = null)
    {
        this.InstanceId = InstanceId;
        this.StartAt = StartAt;
    }

    /// <summary>
    /// Gets the tags to associate with the orchestration instance.
    /// </summary>
    public IReadOnlyDictionary<string, string> Tags { get; init; } = ImmutableDictionary.Create<string, string>();

    /// <summary>
    /// The unique ID of the orchestration instance to schedule. If not specified, a new GUID value is used.
    /// </summary>
    public string? InstanceId { get; init; }

    /// <summary>
    /// The time when the orchestration instance should start executing. If not specified or if a date-time in the past
    /// is specified, the orchestration instance will be scheduled immediately.
    /// </summary>
    public DateTimeOffset? StartAt { get; init; }

    public void Deconstruct(out string? InstanceId, out DateTimeOffset? StartAt)
    {
        InstanceId = this.InstanceId;
        StartAt = this.StartAt;
    }
}
