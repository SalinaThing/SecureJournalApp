
namespace SecureJournal.Data.Services
{
    /// <summary>
    /// Service to manage the currently selected date in the calendar.
    /// Other components can subscribe to <see cref="SelectedDateChanged"/> 
    /// to update UI or perform actions when the date changes.
    /// </summary>
    public class CalendarService
    {

        /// Event triggered whenever <see cref="SelectedDate"/> is updated.
        public event Action? SelectedDateChanged;

        /// The currently selected date. Defaults to today's date.
        public DateTime SelectedDate { get; private set; } = DateTime.Now.Date;

        /// Sets a new selected date and notifies all subscribers.
        /// <param name="date">The new date to select.</param>
        public void SetSelectedDate(DateTime date)
        {
            SelectedDate = date.Date;
            SelectedDateChanged?.Invoke();
        }
    }
}
