// Copyright (c) e5. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CoreWf;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberGuessConsoleApp
{
    class NumberGuessActivity : NativeActivity
    {
        InArgument<int> MaxNumber;
        OutArgument<int> Turns;


        int maxNumber;
        Variable<int> Guess = new Variable<int>("Guess", 0);
        Variable<int> Target;

        public NumberGuessActivity(int MaxNumber)
        {
            maxNumber = MaxNumber;
            Target = new Variable<int>("Target", new Random().Next(1, maxNumber + 1));
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddImplementationVariable(Guess);
            metadata.AddImplementationVariable(Target);
        }

        protected override void Execute(NativeActivityContext context)
        {
            context.CreateBookmark("EnterGuess", new BookmarkCallback(BookmarkCallback), BookmarkOptions.MultipleResume);
        }

        protected override bool CanInduceIdle => true;

        void BookmarkCallback(NativeActivityContext context, Bookmark bookmark, object bookmarkData)
        {
            int localGuess, localTarget = Target.Get(context);
            Console.WriteLine($"Please enter a number between 1 and {maxNumber}");
            while (!int.TryParse(Console.ReadLine(), out localGuess)) Console.WriteLine("Try again");
            Guess.Set(context, localGuess);
            turns.Set(context, turns.Get(context) + 1);

            if (localGuess != localTarget)
            {
                if (localGuess < localTarget) Console.WriteLine("Your guess is too low.");
                else Console.WriteLine("Your guess is too high.");
            }
            else context.RemoveBookmark(bookmark.Name);
        }
    }
}
