using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidCapERP.Dto.ErrorLogs;

namespace MidCapERP.BusinessLogic.Interface
{
    public  interface  IErrorLogsBL
    {
        public Task<IEnumerable<ErrorLogsResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<ErrorLogsResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<ErrorLogsRequestDto> GetById(int Id, CancellationToken cancellationToken);
    }
}
