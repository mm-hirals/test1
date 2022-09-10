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
    public class LoginTokenDA : ILoginTokenDA
    {
        private readonly ISqlRepository<LoginToken> _loginToken;

        public LoginTokenDA(ISqlRepository<LoginToken> loginToken)
        {
            _loginToken = loginToken;
        }

        public async Task<IQueryable<LoginToken>> GetAll(CancellationToken cancellationToken)
        {
            return await _loginToken.GetAsync(cancellationToken);
        }

        public async Task<LoginToken> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _loginToken.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<LoginToken> CreateLoginToken(LoginToken model, CancellationToken cancellationToken)
        {
            return await _loginToken.InsertAsync(model, cancellationToken);
        }

        public async Task<LoginToken> UpdateLoginToken(LoginToken model, CancellationToken cancellationToken)
        {
            return await _loginToken.UpdateAsync(model, cancellationToken);
        }
    }
}
