using MidCapERP.Dto.DataGrid;
using System.Linq.Dynamic.Core;

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
        /// <param name="source"></param>
        /// <param name="dataTableFilterDto"></param>
        public PagedList(IQueryable<T> source, DataTableFilterDto dataTableFilterDto)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / dataTableFilterDto.PageSize;

            if (TotalCount % dataTableFilterDto.PageSize > 0)
                TotalPages++;

            Length = dataTableFilterDto.PageSize;
            RecordsToSkip = dataTableFilterDto.Start;

            if (dataTableFilterDto.Order != null && dataTableFilterDto.Order.Any())
            {
                string sortBy = dataTableFilterDto.Columns[dataTableFilterDto.Order[0].ColumnPosition].Data;
                string sortDirection = dataTableFilterDto.Order[0].Direction.ToLower();
                source = source.OrderBy(sortBy + " " + sortDirection);
            }

            AddRange(source.Skip(dataTableFilterDto.Start).Take(dataTableFilterDto.PageSize).ToList());
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