// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.DurableTask.Client;

/// <summary>
/// Default builder for <see cref="IDurableTaskClientBuilder" />.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DefaultDurableTaskClientBuilder"/> class.
/// </remarks>
public class DefaultDurableTaskClientBuilder : IDurableTaskClientBuilder
{
    Type? buildTarget;

    /// <summary>
    /// Default builder for <see cref="IDurableTaskClientBuilder" />.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DefaultDurableTaskClientBuilder"/> class.
    /// </remarks>
    /// <param name="name">The name of the builder.</param>
    /// <param name="services">The service collection.</param>
    public DefaultDurableTaskClientBuilder(string? name, IServiceCollection services)
    {
        this.Name = name ?? Options.DefaultName;
        this.Services = services;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public IServiceCollection Services { get; }

    /// <inheritdoc/>
    public Type? BuildTarget
    {
        get
        {
            return this.buildTarget;
        }
        set
        {
            if (!IsValidBuildTarget(value))
            {
                throw new ArgumentException(
                    $"Type must be non-abstract and a subclass of {typeof(DurableTaskClient)}", nameof(value));
            }

            this.buildTarget = value;
        }
    }

    /// <inheritdoc/>
    public DurableTaskClient Build(IServiceProvider serviceProvider)
    {
        if (this.buildTarget is null)
        {
            throw new InvalidOperationException(
                "No valid DurableTask client target was registered. Ensure a valid client has been configured via"
                + " 'UseBuildTarget(Type target)'. An example of a valid client is '.UseGrpc()'.");
        }

        return (DurableTaskClient)ActivatorUtilities.CreateInstance(serviceProvider, this.buildTarget, this.Name);
    }

    static bool IsValidBuildTarget(Type? type)
    {
        if (type is null)
        {
            return true; // we will let you set this back to null.
        }

        return type.IsSubclassOf(typeof(DurableTaskClient)) && !type.IsAbstract;
    }
}
