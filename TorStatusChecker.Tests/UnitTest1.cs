using System.Reactive.Linq;

namespace TorStatusChecker.Tests
{
    public class TorStatusCheckerTests
    {
        [Fact()]
        [Trait("Category", "Acceptance")]
        public async Task There_should_be_some_issues()
        {
            var sut = new Checker(new HttpClientFactory());
            var result = await sut.Issues.FirstAsync();
            var issues = result.Where(issue => !issue.Resolved);
            Assert.True(issues.Any());
        }
    }
}