namespace MyCalendarApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnBookShiftClicked(object sender, EventArgs e)
    {
        // 1. Request Runtime permission from the device
        var status = await Permissions.RequestAsync<Permissions.CalendarWrite>();
        if (status == PermissionStatus.Granted)
        {
            DateTime chosenDate = ShiftDatePicker.Date;
            string email = "your_shared_group@gmail.com"; // Your target Google Calendar email
            
            BookShiftOnAndroidDevice(email, chosenDate);
            
            await DisplayAlert("Success", $"Shift booked for {chosenDate.ToShortDateString()}", "OK");
        }
        else
        {
            await DisplayAlert("Permission Denied", "Cannot write to calendar without permissions.", "OK");
        }
    }

    private void BookShiftOnAndroidDevice(string sharedCalendarEmail, DateTime selectedDate)
    {
#if ANDROID
        var context = Android.App.Application.Context;
        long calendarId = -1;

        // Query the local Android Calendar Provider for the specific email account
        string[] projection = { Android.Provider.CalendarContract.Calendars.InterfaceConsts.Id };
        string selection = $"{Android.Provider.CalendarContract.Calendars.InterfaceConsts.AccountName} = ?";
        string[] selectionArgs = { sharedCalendarEmail };

        using (var cursor = context.ContentResolver.Query(
            Android.Provider.CalendarContract.Calendars.ContentUri, projection, selection, selectionArgs, null))
        {
            if (cursor != null && cursor.MoveToFirst())
            {
                calendarId = cursor.GetLong(0);
            }
        }

        if (calendarId == -1) return; // Calendar not found on this device

        // Set up the shift timeframe (e.g. 08:00 to 16:00 local time)
        var startTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 8, 0, 0, DateTimeKind.Local);
        var endTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 16, 0, 0, DateTimeKind.Local);

        long startMillis = ((DateTimeOffset)startTime).ToUnixTimeMilliseconds();
        long endMillis = ((DateTimeOffset)endTime).ToUnixTimeMilliseconds();

        // Pack the parameters into native Android ContentValues
        Android.Content.ContentValues values = new Android.Content.ContentValues();
        values.Put(Android.Provider.CalendarContract.Events.InterfaceConsts.CalendarId, calendarId);
        values.Put(Android.Provider.CalendarContract.Events.InterfaceConsts.Title, "Work Shift");
        values.Put(Android.Provider.CalendarContract.Events.InterfaceConsts.Description, "Injected via MAUI App.");
        values.Put(Android.Provider.CalendarContract.Events.InterfaceConsts.Dtstart, startMillis);
        values.Put(Android.Provider.CalendarContract.Events.InterfaceConsts.Dtend, endMillis);
        values.Put(Android.Provider.CalendarContract.Events.InterfaceConsts.EventTimezone, "Europe/Helsinki");

        // Fire directly into the device database
        context.ContentResolver.Insert(Android.Provider.CalendarContract.Events.ContentUri, values);
#endif
    }
}
