namespace C4Net.Framework.Core.IoC
{
    /// <summary>
    /// Enumeration for the inversion instances expiration.
    /// </summary>
    public enum IoCExpiration
    {
        /// <summary>
        /// The instance never expires.
        /// </summary>
        Never,
        /// <summary>
        /// The instance expires at a given date time.
        /// </summary>
        DateTime,

        /// <summary>
        /// The instance expires when it's idle too much time.
        /// </summary>
        Idle
    }
}
