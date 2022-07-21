using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidCapERP.Dto.SubjectTypes;

namespace MidCapERP.BusinessLogic.Interface
{
    public  interface  ISubjectTypesBL
    {
        public Task<IEnumerable<SubjectTypesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<SubjectTypesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<SubjectTypesRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<SubjectTypesRequestDto> CreateSubjectTypes(SubjectTypesRequestDto model, CancellationToken cancellationToken);

        public Task<SubjectTypesRequestDto> UpdateSubjectTypes(int Id, SubjectTypesRequestDto model, CancellationToken cancellationToken);

        public Task<SubjectTypesRequestDto> DeleteSubjectTypes(int Id, CancellationToken cancellationToken);
    }
}
