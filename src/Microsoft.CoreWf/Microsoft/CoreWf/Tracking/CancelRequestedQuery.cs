﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.CoreWf.Tracking
{
    public sealed class CancelRequestedQuery : TrackingQuery
    {
        public CancelRequestedQuery()
        {
            this.ActivityName = "*";
            this.ChildActivityName = "*";
        }

        public string ActivityName { get; set; }
        public string ChildActivityName { get; set; }
    }
}
