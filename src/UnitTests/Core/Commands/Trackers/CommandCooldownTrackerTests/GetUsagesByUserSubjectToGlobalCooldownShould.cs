using System;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Events;
using FluentAssertions;
using Xunit;

namespace UnitTests.Core.Commands.Trackers.CommandCooldownTrackerTests
{
    public class GetUsagesByUserSubjectToGlobalCooldownShould
    {
        [Fact]
        public void ReturnEmptyCollection_GivenNoCommandsUsed()
        {
            var tracker = new CommandCooldownTracker(new CommandHandlerSettings());

            var usages = tracker.GetUsagesByUserSubjectToGlobalCooldown("brendan", DateTimeOffset.UtcNow);

            usages.Should().BeEmpty();
        }

        [Fact]
        public void ReturnSingleItem_GivenOneRecentCommandUsage()
        {
            var tracker = new CommandCooldownTracker(new CommandHandlerSettings {GlobalCommandCooldown = 1});
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            tracker.RecordUsage(new CommandUsage("brendan", currentTime, null));

            var usages = tracker.GetUsagesByUserSubjectToGlobalCooldown("brendan", currentTime);

            usages.Should().HaveCount(1);
        }

        [Fact]
        public void ReturnEmptyCollection_GivenOneOldCommandUsage()
        {
            var tracker = new CommandCooldownTracker(new CommandHandlerSettings {GlobalCommandCooldown = 1});
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            tracker.RecordUsage(new CommandUsage("brendan", currentTime.AddSeconds(-2), null));

            var usages = tracker.GetUsagesByUserSubjectToGlobalCooldown("brendan", currentTime);

            usages.Should().BeEmpty();
        }
    }
}
