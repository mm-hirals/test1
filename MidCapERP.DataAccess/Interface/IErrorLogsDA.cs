using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public  interface  IErrorLogsDA
    {
        public Task<IQueryable<ErrorLogs>> GetAll(CancellationToken cancellationToken);

        public Task<ErrorLogs> GetById(int Id, CancellationToken cancellationToken);

    }
}
