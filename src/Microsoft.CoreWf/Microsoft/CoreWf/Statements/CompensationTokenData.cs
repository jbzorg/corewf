// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CoreWf.Runtime;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.CoreWf.Statements
{
    [Fx.Tag.XamlVisible(false)]
    [DataContract]
    internal class CompensationTokenData
    {
        internal CompensationTokenData(long compensationId, long parentCompensationId)
        {
            this.CompensationId = compensationId;
            this.ParentCompensationId = parentCompensationId;
            this.BookmarkTable = new BookmarkTable();
            this.ExecutionTracker = new ExecutionTracker();
            this.CompensationState = CompensationState.Creating;
        }

        [DataMember(EmitDefaultValue = false)]
        internal long CompensationId
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        internal long ParentCompensationId
        {
            get;
            set;
        }

        [DataMember]
        internal BookmarkTable BookmarkTable
        {
            get;
            set;
        }

        [DataMember]
        internal ExecutionTracker ExecutionTracker
        {
            get;
            set;
        }

        [DefaultValue(CompensationState.Active)]
        [DataMember(EmitDefaultValue = false)]
        internal CompensationState CompensationState
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        internal string DisplayName
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        internal bool IsTokenValidInSecondaryRoot
        {
            get;
            set;
        }

        internal void RemoveBookmark(NativeActivityContext context, CompensationBookmarkName bookmarkName)
        {
            Bookmark bookmark = this.BookmarkTable[bookmarkName];

            if (bookmark != null)
            {
                context.RemoveBookmark(bookmark);
                this.BookmarkTable[bookmarkName] = null;
            }
        }
    }
}
