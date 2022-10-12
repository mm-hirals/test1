using MidCapERP.Dto.DataGrid;
using System.Linq.Dynamic.Core;
using System.Reflection;

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
                string sortBy = dataTableFilterDto.Columns[dataTableFilterDto.Order[0].column].data;
                string sortDirection = dataTableFilterDto.Order[0].dir.ToLower();
                source = source.OrderBy(ApplyOrder<T>(sortBy) + " " + sortDirection);
            }

            AddRange(source.Skip(dataTableFilterDto.Start).Take(dataTableFilterDto.PageSize).ToList());
        }

        private string ApplyOrder<T>(string property)
        {
            Type type = typeof(T);
            var method = type.GetMethod("MapOrderBy", BindingFlags.Instance | BindingFlags.Public);
            if (method != null)
            {
                object classInstance = Activator.CreateInstance(type, null);
                return Convert.ToString(method.Invoke(classInstance, new object[] { property }));
            }
            return property;
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