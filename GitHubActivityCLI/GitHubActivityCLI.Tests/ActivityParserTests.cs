using GitHubActivityCLI;
using NUnit.Framework;
using System.Collections.Generic;

namespace GitHubActivityCLI.Tests
{
    [TestFixture]
    public class ActivityParserTests
    {
        [Test]
        public void ParseAndFormatActivity_ShouldReturnFormattedOutput_WhenValidJsonIsProvided()
        {
            string json = "[{ \"type\": \"PushEvent\", \"repo\": { \"name\": \"user/repo1\" } }, " +
                          "{ \"type\": \"IssuesEvent\", \"repo\": { \"name\": \"user/repo2\" } }]";

            List<string> result = ActivityParser.ParseAndDisplayActivity(json);

            Assert.That(result, Has.Count.EqualTo(2)); // Only activities, no header
            Assert.That(result[0], Is.EqualTo("▶ Pushed commits → user/repo1"));
            Assert.That(result[1], Is.EqualTo("▶ Opened or commented on an issue → user/repo2"));
        }


        [Test]
        public void ParseAndFormatActivity_ShouldFilterByEventType_WhenEventTypeIsProvided()
        {
            string json = "[{ \"type\": \"PushEvent\", \"repo\": { \"name\": \"user/repo1\" } }, " +
                          "{ \"type\": \"IssuesEvent\", \"repo\": { \"name\": \"user/repo2\" } }]";

            List<string> result = ActivityParser.ParseAndDisplayActivity(json, "PushEvent");

            Assert.That(result, Has.Count.EqualTo(1)); // Only 1 filtered activity
            Assert.That(result[0], Is.EqualTo("▶ Pushed commits → user/repo1"));
        }

        [Test]
        public void ParseAndFormatActivity_ShouldReturnEmptyList_WhenJsonHasNoActivity()
        {
            string json = "[]";
            List<string> result = ActivityParser.ParseAndDisplayActivity(json);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ParseAndFormatActivity_ShouldReturnEmptyList_WhenJsonIsInvalid()
        {
            string invalidJson = "INVALID_JSON";
            List<string> result = ActivityParser.ParseAndDisplayActivity(invalidJson);

            Assert.That(result, Is.Empty);
        }
    }
}