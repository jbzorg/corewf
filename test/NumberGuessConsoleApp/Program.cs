// Copyright (c) e5. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CoreWf;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NumberGuessConsoleApp
{
    /// <summary>
    /// example from https://code.msdn.microsoft.com/windowsapps/Windows-Workflow-deed2cd5 for WF on Core 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var syncEvent = new AutoResetEvent(false);
            var idleEvent = new AutoResetEvent(false);
            var target = 100;

            var wfApp = new WorkflowApplication(new NumberGuessActivity(), new Dictionary<string, object> { ["MaxNumber"] = target });

            wfApp.Completed = (WorkflowApplicationCompletedEventArgs e) =>
            {
                int Turns = Convert.ToInt32(e.Outputs["Turns"]);
                Console.WriteLine($"Congratulations, you guessed the number in {Turns} turns.");
                syncEvent.Set();
            };

            wfApp.Aborted = (WorkflowApplicationAbortedEventArgs e) =>
            {
                Console.WriteLine(e.Reason);
                syncEvent.Set();
            };

            wfApp.OnUnhandledException = (WorkflowApplicationUnhandledExceptionEventArgs e) =>
            {
                Console.WriteLine(e.UnhandledException);
                return UnhandledExceptionAction.Terminate;
            };

            wfApp.Idle = (WorkflowApplicationIdleEventArgs e) => idleEvent.Set();

            wfApp.Run();

            // Loop until the workflow completes.
            Console.WriteLine($"Please enter a number between 1 and {target}");
            WaitHandle[] handles = { syncEvent, idleEvent };
            while (WaitHandle.WaitAny(handles) != 0)
            {
                // Gather the user input and resume the bookmark.
                bool validEntry = false;
                while (!validEntry)
                {
                    int Guess;
                    if (!Int32.TryParse(Console.ReadLine(), out Guess)) Console.WriteLine("Please enter an integer.");
                    else
                    {
                        validEntry = true;
                        wfApp.ResumeBookmark("EnterGuess", Guess);
                    }
                }
            }
        }
    }
}