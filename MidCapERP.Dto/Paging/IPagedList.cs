namespace MidCapERP.Dto.Paging
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// Page index
        /// </summary>
        public int RecordsToSkip { get; }

        /// <summary>
        /// Page size
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Total count
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages { get; }
    }
}