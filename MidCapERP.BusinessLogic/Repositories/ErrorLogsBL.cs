using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.ErrorLogs;

namespace MidCapERP.BusinessLogic.Repositories
{
    public  class ErrorLogsBL : IErrorLogsBL
    {

        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;

        public ErrorLogsBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ErrorLogsResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ErrorLogsDA.GetAll(cancellationToken);
            var DataToReturn = _mapper.Map<List<ErrorLogsResponseDto>>(data.ToList());
            return DataToReturn;
        }

        public async Task<ErrorLogsResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ErrorLogsDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Category not found");
            }
            return _mapper.Map<ErrorLogsResponseDto>(data);
        }

        public async Task<ErrorLogsRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ErrorLogsDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Category not found");
            }
            return _mapper.Map<ErrorLogsRequestDto>(data);
        }

    }
}
