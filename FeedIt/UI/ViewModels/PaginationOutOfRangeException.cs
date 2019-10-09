using System;

namespace FeedIt.UI.ViewModels
{
    public class PaginationOutOfRangeException : Exception
    {
        public bool Negative { get; }
        public PaginationOutOfRangeException(bool negative)
        {
            Negative = negative;
        }
    }
}