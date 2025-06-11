namespace Exceptions
{
    public class InvalidReviewLengthException: Exception
    {
        public InvalidReviewLengthException() : base("Review too long!") { }
    }

    public class ReviewLengthShortException: Exception
    {
        public ReviewLengthShortException() : base("Review too short!") { }
    }
}
