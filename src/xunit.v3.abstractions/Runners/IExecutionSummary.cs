namespace Xunit
{
    /// <summary>
    /// Collects execution totals for a group of test cases.
    /// </summary>
    public interface IExecutionSummary
    {
        /// <summary>
        /// Gets or set the total number of tests run.
        /// </summary>
        int Total { get; set; }

        /// <summary>
        /// Gets or sets the number of failed tests.
        /// </summary>
        int Failed { get; set; }

        /// <summary>
        /// Gets or sets the number of skipped tests.
        /// </summary>
        int Skipped { get; set; }

        /// <summary>
        /// Gets or sets the total execution time for the tests.
        /// </summary>
        decimal Time { get; set; }

        /// <summary>
        /// Gets or sets the total errors (i.e., cleanup failures) for the tests.
        /// </summary>
        int Errors { get; set; }
    }
}
