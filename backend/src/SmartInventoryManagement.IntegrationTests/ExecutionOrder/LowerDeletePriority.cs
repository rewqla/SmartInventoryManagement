using Xunit.Abstractions;
using Xunit.Sdk;

namespace SmartInventoryManagement.IntegrationTests.ExecutionOrder;

public class LowerDeletePriority : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        return  testCases.OrderBy(testCase => testCase.TestMethod.Method.Name.Contains("Delete")
            ? 0
            : 1);
    }
}