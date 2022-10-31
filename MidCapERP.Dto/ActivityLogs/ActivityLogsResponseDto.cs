using System.ComponentModel;

namespace MidCapERP.Dto.ActivityLogs
{
    public class ActivityLogsResponseDto
    {
        public int ActivityLogID { get; set; }
        public int SubjectTypeId { get; set; }
        public string SubjectId { get; set; } = default!;

        [DisplayName("Description")]
        public string Description { get; set; } = default!;

        [DisplayName("Action")]
        public string Action { get; set; } = default!;

        public int CreatedBy { get; set; }

        [DisplayName("Activity Date")]
        public DateTime CreatedDate { get; set; }

        public DateTime CreatedUTCDate { get; set; }

        [DisplayName("Activity By")]
        public string CreatedByName { get; set; }

        [DisplayName("Activity Date")]
        public string CreatedDateFormat => CreatedDate.ToLongDateString();

        /// <summary>
        /// Remarks : Do not change the method Name or Properties. Check the PagedList.cs to get referance of the method
        /// </summary>
        /// <param name="orderbyColumn">Order by column name</param>
        /// <returns>actual database column name</returns>
        public string MapOrderBy(string orderbyColumn)
        {
            switch (orderbyColumn)
            {
                case "createdDateFormat":
                    return "CreatedDate";

                default:
                    return orderbyColumn;
            };
        }
    }
}