﻿using GVFS.FunctionalTests.Category;
using NUnit.Framework;

namespace GVFS.FunctionalTests.Tests.GitCommands
{
    [TestFixture]
    [Category(CategoryConstants.GitCommands)]
    public class RebaseConflictTests : GitRepoTests
    {
        public RebaseConflictTests() : base(enlistmentPerTest: true)
        {
        }

        [TestCase]
        public void RebaseConflict()
        {
            this.ValidateGitCommand("checkout " + GitRepoTests.ConflictTargetBranch);
            this.RunGitCommand("rebase " + GitRepoTests.ConflictSourceBranch);
            this.FilesShouldMatchAfterConflict();
        }

        [TestCase]
        public void RebaseConflict_ThenAbort()
        {
            this.ValidateGitCommand("checkout " + GitRepoTests.ConflictTargetBranch);
            this.RunGitCommand("rebase " + GitRepoTests.ConflictSourceBranch);
            this.ValidateGitCommand("rebase --abort");
            this.FilesShouldMatchCheckoutOfTargetBranch();
        }

        [TestCase]
        public void RebaseConflict_ThenSkip()
        {
            this.ValidateGitCommand("checkout " + GitRepoTests.ConflictTargetBranch);
            this.RunGitCommand("rebase " + GitRepoTests.ConflictSourceBranch);
            this.ValidateGitCommand("rebase --skip");
            this.FilesShouldMatchCheckoutOfSourceBranch();
        }

        [TestCase]
        public void RebaseConflict_AddThenContinue()
        {
            this.ValidateGitCommand("checkout " + GitRepoTests.ConflictTargetBranch);
            this.RunGitCommand("rebase " + GitRepoTests.ConflictSourceBranch);
            this.ValidateGitCommand("add .");
            this.ValidateGitCommand("rebase --continue");
            this.FilesShouldMatchAfterConflict();
        }

        [TestCase]
        public void RebaseMultipleCommits()
        {
            string sourceCommit = "FunctionalTests/20170403_rebase_multiple_source";
            string targetCommit = "FunctionalTests/20170403_rebase_multiple_onto";

            this.ControlGitRepo.Fetch(sourceCommit);
            this.ControlGitRepo.Fetch(targetCommit);

            this.ValidateGitCommand("checkout " + sourceCommit);
            this.RunGitCommand("rebase origin/" + targetCommit);
            this.ValidateGitCommand("rebase --abort");
        }

        protected override void CreateEnlistment()
        {
            base.CreateEnlistment();
            this.ControlGitRepo.Fetch(GitRepoTests.ConflictSourceBranch);
            this.ControlGitRepo.Fetch(GitRepoTests.ConflictTargetBranch);
            this.ValidateGitCommand("checkout " + GitRepoTests.ConflictSourceBranch);
            this.ValidateGitCommand("checkout " + GitRepoTests.ConflictTargetBranch);
        }
    }
}
