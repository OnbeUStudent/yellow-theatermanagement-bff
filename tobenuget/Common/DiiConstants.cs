namespace DiiCommon
{
    public static class DiiConstants
    {
        public static class TemporarilyHardCoded
        {
            public const long TheaterId = 1;
            public const string TheaterCode = "boulevard";
        }

        public static class Theaters
        {
            public const int BookingDangerAlertWindowSizeInMonths = 4;
            public const int BookingWarningAlertWindowSizeInMonths = 2;
            public const int BookingInfoAlertWindowSizeInMonths = 6;

            public const int BookingWindowSizeInMonths =
                BookingDangerAlertWindowSizeInMonths
                + BookingWarningAlertWindowSizeInMonths
                + BookingInfoAlertWindowSizeInMonths;

            public const int SnackPackOrderingDangerAlertWindowSizeInMonths = 2;
            public const int SnackPackOrderingWarningAlertWindowSizeInMonths = 2;
            public const int SnackPackOrderingInfoAlertWindowSizeInMonths = 3;

            public const int SnackPackOrderingWindowSizeInMonths =
                SnackPackOrderingDangerAlertWindowSizeInMonths
                + SnackPackOrderingWarningAlertWindowSizeInMonths
                + SnackPackOrderingInfoAlertWindowSizeInMonths;
        }
        public static class Consumers
        {
            public const int TicketPurchaseWindowSizeInMonths = 3;
        }
    }
}
