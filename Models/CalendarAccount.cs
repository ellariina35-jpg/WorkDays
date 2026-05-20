namespace WorkDays.Models
{
    /// <summary>
    /// Represents a calendar account on the device
    /// </summary>
    public class CalendarAccount
    {
        /// <summary>
        /// Unique identifier for the calendar (varies by platform)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Display name of the calendar
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Email address associated with the calendar
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Type of calendar account (e.g., "google", "outlook", "exchange")
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// Calendar color if available
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Whether the calendar is read-only
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Owner/account name for the calendar
        /// </summary>
        public string AccountName { get; set; }

        public override string ToString()
        {
            // Display friendly name with email if available
            if (!string.IsNullOrEmpty(Email))
                return $"{DisplayName} ({Email})";
            return DisplayName;
        }
    }
}
