// Copyright (c) e5. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CoreWf;
using System;

namespace NumberGuessConsoleApp
{
    class NumberGuessActivity : NativeActivity
    {
        InArgument<int> MaxNumber = new InArgument<int>(0);
        OutArgument<int> Turns = new OutArgument<int>();

        Variable<int> Guess = new Variable<int>("Guess", 0);
        Variable<int> Target = new Variable<int>("Target", 0);

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddImplementationVariable(Guess);
            metadata.AddImplementationVariable(Target);

            var maxNumber = new RuntimeArgument("MaxNumber", typeof(int), ArgumentDirection.In, true);
            metadata.Bind(MaxNumber, maxNumber);
            metadata.AddArgument(maxNumber);

            var turns = new RuntimeArgument("Turns", typeof(int), ArgumentDirection.Out, true);
            metadata.Bind(Turns, turns);
            metadata.AddArgument(turns);
        }

        protected override void Execute(NativeActivityContext context)
        {
            Target.Set(context, new Random().Next(1, MaxNumber.Get(context) + 1));
            context.CreateBookmark("EnterGuess", new BookmarkCallback(BookmarkCallback), BookmarkOptions.MultipleResume);
        }

        protected override bool CanInduceIdle => true;

        void BookmarkCallback(NativeActivityContext context, Bookmark bookmark, object bookmarkData)
        {
            int localGuess = (int)bookmarkData, localTarget = Target.Get(context);
            Guess.Set(context, localGuess);
            Turns.Set(context, Turns.Get(context) + 1);

            if (localGuess != localTarget)
            {
                if (localGuess < localTarget) Console.WriteLine("Your guess is too low.");
                else Console.WriteLine("Your guess is too high.");
            }
            else context.RemoveBookmark(bookmark.Name);
        }
    }
}
