﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.PowerFx.Core.Functions;
using Microsoft.PowerFx.Core.IR;
using Microsoft.PowerFx.Core.Tests;
using Microsoft.PowerFx.Core.Texl.Builtins;
using Microsoft.PowerFx.Functions;
using Microsoft.PowerFx.Types;
using Xunit;

namespace Microsoft.PowerFx.Interpreter.Tests
{
    public class PatchFunctionTests : PowerFxTest
    {
        private readonly ParserOptions _opts = new ParserOptions { AllowsSideEffects = true };

        [Theory]
        [InlineData(typeof(PatchImpl))]
        public async Task CheckArgsTestAsync(Type type)
        {
            var expressionError = new ExpressionError()
            {
                Kind = ErrorKind.ReadOnlyValue,
                Severity = ErrorSeverity.Critical,
                Message = "Something went wrong"
            };

            FormulaValue[] args = new[]
            {
                FormulaValue.NewError(expressionError)
            };

            BasicServiceProvider innerServices = new BasicServiceProvider();

            innerServices.AddService(Features.PowerFxV1);

            var function = Activator.CreateInstance(type) as IFunctionInvoker;
            var invokeInfo = new FunctionInvokeInfo
            {
                 Args = args,
                 IRContext = IRContext.NotInSource(FormulaType.Unknown)
            };

            var result = await function.InvokeAsync(invokeInfo, CancellationToken.None);

            Assert.IsType<ErrorValue>(result);
        }
    }
}
