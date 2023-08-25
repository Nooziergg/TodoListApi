namespace ToDoList.Utils
{
    public static class DateIntervalUtils
    {
        public static bool IsValidDateInterval(DateTime? startDate, DateTime? endDate)
        {
            bool isValidInterval = true;

            if (BothDatesAreProvided(startDate, endDate))
            {
                isValidInterval = StartDateIsBeforeOrEqualToEndDate(startDate.Value, endDate.Value);
            }

            return isValidInterval;
        }

        private static bool BothDatesAreProvided(DateTime? startDate, DateTime? endDate)
        {
            return startDate.HasValue && endDate.HasValue;
        }

        private static bool StartDateIsBeforeOrEqualToEndDate(DateTime startDate, DateTime endDate)
        {
            return startDate <= endDate;
        }
    }

}
