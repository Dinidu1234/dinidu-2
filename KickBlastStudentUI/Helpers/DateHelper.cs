namespace KickBlastStudentUI.Helpers;

public static class DateHelper
{
    public static DateTime GetSecondSaturday(DateTime month)
    {
        var firstDay = new DateTime(month.Year, month.Month, 1);
        var offset = ((int)DayOfWeek.Saturday - (int)firstDay.DayOfWeek + 7) % 7;
        var firstSaturday = firstDay.AddDays(offset);
        return firstSaturday.AddDays(7);
    }
}
