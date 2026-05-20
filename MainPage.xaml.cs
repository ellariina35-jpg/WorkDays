namespace WorkDays
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
private async void OnBookShiftClicked(object sender, EventArgs e)
{
    // Trigger runtime permission validation
    var status = await Permissions.RequestAsync<Permissions.CalendarWrite>();
    if (status == PermissionStatus.Granted)
    {
        DateTime chosenDate = ShiftDatePicker.Date ?? DateTime.Today;
        if (chosenDate == default(DateTime))
        {
            await DisplayAlert("Error", "Please select a valid date", "OK");
            return;
        }

        string email = "your_shared_group@gmail.com"; // Provide target email here

        // TODO: Implement BookShiftOnAndroidDevice to add event to calendar
        // For now, just show confirmation
        await DisplayAlert("Success", $"Shift booked for {chosenDate.ToShortDateString()}", "OK");
    }
}
    }
}
