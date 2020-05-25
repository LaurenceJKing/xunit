namespace Xunit
{
    /// <summary>
    /// Collects execution totals for a group of test cases.
    /// </summary>
    public class ExecutionSummary : IExecutionSummary
    {
        /// <inheritdoc />
        public int Total { get; set; }

        /// <inheritdoc />
        public int Failed { get; set; }

        /// <inheritdoc />
        public int Skipped { get; set; }

        /// <inheritdoc />
        public decimal Time { get; set; }

        /// <inheritdoc />
        public int Errors { get; set; }
    }
}
