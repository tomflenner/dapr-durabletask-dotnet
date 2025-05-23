﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Microsoft.DurableTask.Analyzers.Tests.Verifiers;

public static partial class CSharpAnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic()"/>
    public static DiagnosticResult Diagnostic()
    {
        return CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic();
    }

    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic(string)"/>
    public static DiagnosticResult Diagnostic(string diagnosticId)
    {
        return CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(diagnosticId);
    }

    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic(DiagnosticDescriptor)"/>
    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
    {
        return CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(descriptor);
    }

    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
    public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new Test
        {
            TestCode = source,
        };

        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }

    public class Test : CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
    {
    }
}
