// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using DurableTask.Core.Serializing;
using CoreJsonDataConverter = DurableTask.Core.Serializing.JsonDataConverter;

namespace Dapr.DurableTask.Worker.Shims;

/// <summary>
/// A shim to go from <see cref="DataConverter" /> to <see cref="CoreJsonDataConverter" />.
/// </summary>
sealed class JsonDataConverterShim : CoreJsonDataConverter
{
    readonly DataConverter innerConverter;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDataConverterShim"/> class.
    /// </summary>
    /// <param name="innerConverter">The converter to wrap.</param>
    public JsonDataConverterShim(DataConverter innerConverter)
    {
        this.innerConverter = Check.NotNull(innerConverter);
    }

    /// <inheritdoc/>
    public override string Serialize(object value)
        => this.innerConverter.Serialize(value);

    /// <inheritdoc/>
    public override string Serialize(object value, bool formatted)
        => this.Serialize(value);

    /// <inheritdoc/>
    public override object Deserialize(string data, Type objectType)
        => this.innerConverter.Deserialize(data, objectType);
}
