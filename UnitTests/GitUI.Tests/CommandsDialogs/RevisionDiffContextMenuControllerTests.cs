﻿using FluentAssertions;
using GitUI.CommandsDialogs;
using GitUIPluginInterfaces;
using NUnit.Framework;

namespace GitUITests.CommandsDialogs
{
    [TestFixture]
    public class RevisionDiffContextMenuControllerTests
    {
        private FileStatusListContextMenuController _revisionDiffContextMenuController;

        [SetUp]
        public void Setup()
        {
            _revisionDiffContextMenuController = new FileStatusListContextMenuController();
        }

        [Test]
        public void BrowseDiff_SuppressDiffToLocalWhenNoSelectedRevision()
        {
            var selectionInfo = new ContextMenuDiffToolInfo();
            _revisionDiffContextMenuController.ShouldShowMenuFirstToSelected(selectionInfo).Should().BeFalse();
            _revisionDiffContextMenuController.ShouldShowMenuFirstToLocal(selectionInfo).Should().BeFalse();
            _revisionDiffContextMenuController.ShouldShowMenuSelectedToLocal(selectionInfo).Should().BeFalse();
        }

        [Test]
        public void BrowseDiff_SuppressDiffToLocalWhenNoLocalExists()
        {
            var rev = new GitRevision(ObjectId.Random());
            var selectionInfo = new ContextMenuDiffToolInfo(selectedRevision: rev, localExists: false);
            _revisionDiffContextMenuController.ShouldShowMenuFirstToSelected(selectionInfo).Should().BeTrue();
            _revisionDiffContextMenuController.ShouldShowMenuFirstToLocal(selectionInfo).Should().BeFalse();
            _revisionDiffContextMenuController.ShouldShowMenuSelectedToLocal(selectionInfo).Should().BeFalse();
        }

        [Test]
        public void BrowseDiff_ShowContextDiffToolForWorkTree()
        {
            var rev = new GitRevision(ObjectId.WorkTreeId);
            var selectionInfo = new ContextMenuDiffToolInfo(selectedRevision: rev);
            _revisionDiffContextMenuController.ShouldShowMenuFirstToSelected(selectionInfo).Should().BeTrue();
            _revisionDiffContextMenuController.ShouldShowMenuFirstToLocal(selectionInfo).Should().BeTrue();
            _revisionDiffContextMenuController.ShouldShowMenuSelectedToLocal(selectionInfo).Should().BeFalse();
        }

        [Test]
        public void BrowseDiff_ShowContextDiffToolForWorkTreeParent()
        {
            var rev = new GitRevision(ObjectId.Random());
            var selectionInfo = new ContextMenuDiffToolInfo(selectedRevision: rev, selectedItemParentRevs: new[] { ObjectId.WorkTreeId });
            _revisionDiffContextMenuController.ShouldShowMenuFirstToSelected(selectionInfo).Should().BeTrue();
            _revisionDiffContextMenuController.ShouldShowMenuFirstToLocal(selectionInfo).Should().BeFalse();
            _revisionDiffContextMenuController.ShouldShowMenuSelectedToLocal(selectionInfo).Should().BeTrue();
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void BrowseDiff_ShowContextDiffToolForDeletedAndNew(bool d, bool n)
        {
            var rev = new GitRevision(ObjectId.Random());
            var selectionInfo = new ContextMenuDiffToolInfo(selectedRevision: rev, allAreDeleted: d, allAreNew: n);
            _revisionDiffContextMenuController.ShouldShowMenuFirstToSelected(selectionInfo).Should().BeTrue();
            _revisionDiffContextMenuController.ShouldShowMenuFirstToLocal(selectionInfo).Should().BeTrue();
            _revisionDiffContextMenuController.ShouldShowMenuSelectedToLocal(selectionInfo).Should().Be(!d);
        }
    }
}
