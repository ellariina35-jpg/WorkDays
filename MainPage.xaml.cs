namespace WorkDays
{
    public class CalendarInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        private List<string> timeSlots = new();
        private List<CalendarInfo> calendars = new();

        public MainPage()
        {
            InitializeComponent();
            InitializeTimeSlots();
            InitializeCalendars();
        }

        private void InitializeTimeSlots()
        {
            // Generate time slots from 08:00 to 21:00
            for (int hour = 8; hour <= 21; hour++)
            {
                timeSlots.Add($"{hour:D2}:00");
            }

            // Populate pickers
            foreach (var time in timeSlots)
            {
                StartTimePicker.Items.Add(time);
                EndTimePicker.Items.Add(time);
            }

            // Set default selections
            StartTimePicker.SelectedIndex = 0; // 08:00
            EndTimePicker.SelectedIndex = 1;   // 09:00
        }

        private async void InitializeCalendars()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.CalendarRead>();
                if (status == PermissionStatus.Granted)
                {
                    // Add default calendars
                    calendars.Add(new CalendarInfo { Name = "Work Calendar", Id = "work" });
                    calendars.Add(new CalendarInfo { Name = "Personal Calendar", Id = "personal" });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load calendars: {ex.Message}", "OK");
            }

            // Populate calendar picker
            foreach (var cal in calendars)
            {
                CalendarPicker.Items.Add(cal.Name);
            }

            if (calendars.Count > 0)
            {
                CalendarPicker.SelectedIndex = 0;
            }
        }

        private async void OnBookShiftClicked(object sender, EventArgs e)
        {
            // Validate selections
            if (ShiftDatePicker.Date == default(DateTime))
            {
                await DisplayAlertAsync("Validation Error", "Please select a date", "OK");
                return;
            }

            if (StartTimePicker.SelectedIndex == -1)
            {
                await DisplayAlertAsync("Validation Error", "Please select a start time", "OK");
                return;
            }

            if (EndTimePicker.SelectedIndex == -1)
            {
                await DisplayAlertAsync("Validation Error", "Please select an end time", "OK");
                return;
            }

            string calendarEmail = CalendarEmailEntry.Text?.Trim();
            CalendarInfo selectedCalendar = null;

            if (CalendarPicker.SelectedIndex >= 0 && CalendarPicker.SelectedIndex < calendars.Count)
            {
                selectedCalendar = calendars[CalendarPicker.SelectedIndex];
            }
            else if (string.IsNullOrEmpty(calendarEmail))
            {
                await DisplayAlertAsync("Validation Error", "Please select a calendar or enter an email address", "OK");
                return;
            }

            // Request calendar write permission
            var status = await Permissions.RequestAsync<Permissions.CalendarWrite>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlertAsync("Permission Denied", "Calendar write permission is required", "OK");
                return;
            }

            try
            {
                DateTime shiftDate = ShiftDatePicker.Date ?? DateTime.Today;
                string startTime = timeSlots[StartTimePicker.SelectedIndex];
                string endTime = timeSlots[EndTimePicker.SelectedIndex];

                // Parse times
                var startTimeSpan = TimeSpan.Parse(startTime);
                var endTimeSpan = TimeSpan.Parse(endTime);

                // Create calendar event
                var startDateTime = shiftDate.Add(startTimeSpan);
                var endDateTime = shiftDate.Add(endTimeSpan);

                // Validate end time is after start time
                if (endDateTime <= startDateTime)
                {
                    endDateTime = endDateTime.AddDays(1);
                }

                string targetCalendar = selectedCalendar?.Name ?? calendarEmail;

                // Create event details
                string eventSummary = "Work Shift";
                string eventDescription = $"Work shift from {startTime} to {endTime}";

                // Attempt to create event (platform-specific implementation needed)
                await CreateCalendarEvent(startDateTime, endDateTime, eventSummary, eventDescription, targetCalendar);

                await DisplayAlertAsync(
                    "Success",
                    $"Shift booked for {shiftDate:MMMM dd, yyyy}\n" +
                    $"Time: {startTime} - {endTime}\n" +
                    $"Calendar: {targetCalendar}",
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Failed to book shift: {ex.Message}", "OK");
            }
        }

        private async Task CreateCalendarEvent(DateTime startDateTime, DateTime endDateTime,
            string summary, string description, string calendarName)
        {
            // TODO: Implement platform-specific calendar event creation
            // For Android, this would use CalendarContract
            // For iOS, this would use EventKit

            // Placeholder implementation
#if __ANDROID__
            // Android implementation would go here
            await Task.Delay(100);
#elif __IOS__
            // iOS implementation would go here
            await Task.Delay(100);
#else
            await Task.Delay(100);
#endif
        }
    }
}
