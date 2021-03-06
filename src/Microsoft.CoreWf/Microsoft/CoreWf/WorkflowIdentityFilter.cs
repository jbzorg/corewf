// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.CoreWf
{
    public enum WorkflowIdentityFilter
    {
        Exact = 0,
        Any = 1,
        AnyRevision = 2
    }

    internal static class WorkflowIdentityFilterExtensions
    {
        public static bool IsValid(this WorkflowIdentityFilter value)
        {
            return (int)value >= (int)WorkflowIdentityFilter.Exact && (int)value <= (int)WorkflowIdentityFilter.AnyRevision;
        }
    }
}
