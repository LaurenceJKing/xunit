namespace Xunit.Abstractions
{
    /// <summary>
    /// Represents the top of a stack frame, typically taken from an exception or failure information.
    /// </summary>
    public interface IStackFrameInfo
    {
        /// <summary>
        /// Gets the filename of the stack frame. May be <c>null</c> if the stack frame is not known.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Returns <c>true</c> if this is an empty stack frame.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets the line number of the stack frame. May be 0 if the stack frame is not known.
        /// </summary>
        int LineNumber { get; }
    }
}
