using Microsoft.EntityFrameworkCore;

namespace TestPens.Extensions
{
    public class PaginatedInfo
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedInfo(int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => PageIndex + 1 < TotalPages;
    }
}
