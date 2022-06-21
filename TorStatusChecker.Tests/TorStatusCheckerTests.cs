using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Xunit.Abstractions;

namespace TorStatusChecker.Tests
{
    public class TorStatusCheckerTests
    {
        private readonly ITestOutputHelper output;

        public TorStatusCheckerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        [Trait("Category", "Acceptance")]
        public async Task There_should_be_some_issues()
        {
            var sut = new TorNetwork(new HttpClientFactory());
            var result = await sut.Issues.ToList();
            var issues = result.Where(issue => !issue.Resolved);
            Assert.True(issues.Any());
        }

        [Fact]
        [Trait("Category", "Acceptance")]
        public async Task Interval()
        {
            var reporter = new TorNetwork(new HttpClientFactory());
            var newThreadScheduler = new NewThreadScheduler();
            var checker = new StatusChecker(reporter, TimeSpan.FromSeconds(5),  newThreadScheduler);
            checker.Issues.Subscribe(issues =>
            {
                issues.ToList().ForEach(issue => output.WriteLine(issue.ToString()));
            });
            await Task.Delay(12000);
        }
    }
}