using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.DataAccess.Repositories
{
    public class OTPLoginDA : IOTPLoginDA
    {
        private readonly ISqlRepository<OTPLogin> _loginToken;

        public OTPLoginDA(ISqlRepository<OTPLogin> loginToken)
        {
            _loginToken = loginToken;
        }

        public async Task<IQueryable<OTPLogin>> GetAll(CancellationToken cancellationToken)
        {
            return await _loginToken.GetAsync(cancellationToken);
        }

        public async Task<OTPLogin> CreateLoginToken(OTPLogin model, CancellationToken cancellationToken)
        {
            return await _loginToken.InsertAsync(model, cancellationToken);
        }

        public async Task<OTPLogin> UpdateLoginToken(OTPLogin model, CancellationToken cancellationToken)
        {
            return await _loginToken.UpdateAsync(model, cancellationToken);
        }
    }
}
