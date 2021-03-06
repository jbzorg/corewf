﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.CoreWf;
using Microsoft.CoreWf.Expressions;
using Microsoft.CoreWf.Statements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Samples
{
    public class WFAppExtension : IDisposable
    {
        private AutoResetEvent _completedEvent;
        private Exception _terminationException;
        private WorkflowApplication _wfApp;

        [Fact]
        public void RunTest()
        {
            Sequence workflow = new Sequence
            {
                Activities =
                {
                    new InvokeMyWorkflowExtensionActivity()
                }
            };

            _completedEvent = new AutoResetEvent(false);
            MyWorkflowExtension myExtension = new MyWorkflowExtension();

            WorkflowApplication wfApp = new WorkflowApplication(workflow);
            wfApp.Completed = delegate (WorkflowApplicationCompletedEventArgs e)
            {
                _terminationException = e.TerminationException;
                _completedEvent.Set();
            };
            wfApp.Extensions.Add(myExtension);

            Assert.True(!myExtension.ExtensionMethodInvoked);

            wfApp.Run();

            _completedEvent.WaitOne(TimeSpan.FromSeconds(2));

            Assert.True(myExtension.ExtensionMethodInvoked);
        }

        public void Dispose()
        {
        }
    }

    public class MyWorkflowExtension
    {
        public bool ExtensionMethodInvoked
        {
            get;
            private set;
        }

        public void ExtensionMethod()
        {
            ExtensionMethodInvoked = true;
            return;
        }
    }

    public class InvokeMyWorkflowExtensionActivity : NativeActivity
    {
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
        }

        protected override void Execute(NativeActivityContext context)
        {
            MyWorkflowExtension myExtension = context.GetExtension<MyWorkflowExtension>();
            if (myExtension != null)
            {
                myExtension.ExtensionMethod();
            }
        }
    }
}
