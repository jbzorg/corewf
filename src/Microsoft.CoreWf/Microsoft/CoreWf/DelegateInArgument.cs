// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.CoreWf
{
    public abstract class DelegateInArgument : DelegateArgument
    {
        internal DelegateInArgument()
            : base()
        {
            this.Direction = ArgumentDirection.In;
        }
    }

    public sealed class DelegateInArgument<T> : DelegateInArgument
    {
        public DelegateInArgument()
            : base()
        {
        }

        public DelegateInArgument(string name)
            : base()
        {
            this.Name = name;
        }

        protected override Type TypeCore
        {
            get
            {
                return typeof(T);
            }
        }

        // Soft-Link: This method is referenced through reflection by
        // ExpressionUtilities.TryRewriteLambdaExpression.  Update that
        // file if the signature changes.
        public new T Get(ActivityContext context)
        {
            if (context == null)
            {
                throw Microsoft.CoreWf.Internals.FxTrace.Exception.ArgumentNull("context");
            }

            return context.GetValue<T>((LocationReference)this);
        }

        public void Set(ActivityContext context, T value)
        {
            if (context == null)
            {
                throw Microsoft.CoreWf.Internals.FxTrace.Exception.ArgumentNull("context");
            }

            context.SetValue((LocationReference)this, value);
        }

        internal override Location CreateLocation()
        {
            return new Location<T>();
        }
    }
}
