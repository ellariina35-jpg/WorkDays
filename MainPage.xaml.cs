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
        DateTime chosenDate = ShiftDatePicker.Date;
        string email = "your_shared_group@gmail.com"; // Provide target email here

        BookShiftOnAndroidDevice(email, chosenDate);

        await DisplayAlert("Success", $"Shift booked for {chosenDate.ToShortDateString()}", "OK");
    }
}

private void OnCounterClicked(object? sender, EventArgs e)
{
    count++;

    if (count == 1)
        CounterBtn.Text = $"Clicked {count} time";
    else
        CounterBtn.Text = $"Clicked {count} times";

    SemanticScreenReader.Announce(CounterBtn.Text);
}
    }
}
