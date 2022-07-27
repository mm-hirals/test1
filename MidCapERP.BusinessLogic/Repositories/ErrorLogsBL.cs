using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.Dto.ErrorLogs;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ErrorLogsBL : IErrorLogsBL
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
            var dataToReturn = _mapper.Map<List<ErrorLogsResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<ErrorLogsResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ErrorLogsDA.GetById(Id, cancellationToken);
            return _mapper.Map<ErrorLogsResponseDto>(data);
        }

        public async Task<ErrorLogsRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ErrorLogsDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("ErrorLogs not found");
            }
            return _mapper.Map<ErrorLogsRequestDto>(data);
        }
    }
}