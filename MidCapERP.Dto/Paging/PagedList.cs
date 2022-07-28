namespace MidCapERP.Dto.Paging
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="recordsToSkip">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IEnumerable<T> source, int recordsToSkip, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            Length = pageSize;
            RecordsToSkip = recordsToSkip;
            AddRange(source.Skip(recordsToSkip).Take(pageSize).ToList());
        }

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