// Copyright (c) e5. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CoreWf;
using Microsoft.CoreWf.Statements;
using System;

namespace NumberGuessConsoleApp
{
    /// <summary>
    /// example from https://code.msdn.microsoft.com/windowsapps/Windows-Workflow-deed2cd5 for WF on Core 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var app = CreateWorkflow();
            WorkflowApplication
        }

        static Activity CreateWorkflow()
        {
            var workflow = new Sequence();
            workflow.Activities.Add(new NumberGuessActivity(100));
            return workflow;
        }
    }
}