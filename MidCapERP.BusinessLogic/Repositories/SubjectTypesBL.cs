using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.SubjectTypes;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class SubjectTypesBL : ISubjectTypesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;

        public SubjectTypesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectTypesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<SubjectTypesResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<SubjectTypesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.SubjectTypesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("SubjectTypes not found");
            }
            return _mapper.Map<SubjectTypesResponseDto>(data);
        }

        public async Task<SubjectTypesRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.SubjectTypesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("SubjectType not found");
            }
            return _mapper.Map<SubjectTypesRequestDto>(data);
        }

        public async Task<SubjectTypesRequestDto> CreateSubjectTypes(SubjectTypesRequestDto model, CancellationToken cancellationToken)
        {
            var SubjectTypesToInsert = _mapper.Map<SubjectTypes>(model);
            SubjectTypesToInsert.TenantId = 1;
            SubjectTypesToInsert.IsDeleted = true;
            SubjectTypesToInsert.CreatedDate = DateTime.Now;
            SubjectTypesToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.SubjectTypesDA.CreateSubjectTypes(SubjectTypesToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<SubjectTypesRequestDto>(data);
            return _mappedUser;
        }

        public async Task<SubjectTypesRequestDto> UpdateSubjectTypes(int Id, SubjectTypesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await _unitOfWorkDA.SubjectTypesDA.GetById(Id, cancellationToken);
            if (oldData == null)
            {
                throw new Exception("SubjectType not found");
            }
            oldData.Comments = model.Comments;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            oldData.IsDeleted = model.IsDeleted;

            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.SubjectTypesDA.UpdateSubjectTypes(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<SubjectTypesRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(SubjectTypesRequestDto model, SubjectTypes oldData)
        {
            oldData.SubjectTypeName = model.SubjectTypeName;
        }

        public async Task<SubjectTypesRequestDto> DeleteSubjectTypes(int Id, CancellationToken cancellationToken)
        {
            var SubjectTypesToUpdate = await _unitOfWorkDA.SubjectTypesDA.GetById(Id, cancellationToken);
            if (SubjectTypesToUpdate == null)
            {
                throw new Exception("SubjectType not found");
            }
            SubjectTypesToUpdate.IsDeleted = true;
            SubjectTypesToUpdate.UpdatedDate = DateTime.Now;
            SubjectTypesToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.SubjectTypesDA.UpdateSubjectTypes(Id, SubjectTypesToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<SubjectTypesRequestDto>(data);
            return _mappedUser;
        }
    }
}