namespace Business_logic.Exceptions
{
    public class InvalidGoalStartDateException: Exception
    {
        public InvalidGoalStartDateException(): base("Goal's start date must be before goal's end date!") { }
    }

    public class InvalidGoalEndDateException: Exception
    {
        public InvalidGoalEndDateException(): base("Goal's end date cannot be in the past!") { }
    }

    public class InvalidReadingGoalException : Exception
    {
        public InvalidReadingGoalException(int readingGoal) : base($"Reading goal cannot be {readingGoal}. Please enter a number greater tha 0!") { }
    }
}
