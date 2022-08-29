﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Role;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class RoleBL : IRoleBL
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWorkDA _unitOfWorkDA;
        private readonly CurrentUser _currentUser;
        private readonly IMapper _mapper;

        public RoleBL(IUnitOfWorkDA unitOfWorkDA, CurrentUser currentUser, IMapper mapper, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _currentUser = currentUser;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<IList<ApplicationRole>> GetAllRoles(CancellationToken cancellationToken)
        {
            var getUser = await GetAllRoleData(cancellationToken);
            var rolesByTenant = getUser.Where(x => x.TenantId == _currentUser.TenantId).ToList();
            return rolesByTenant;
        }

        public async Task<JsonRepsonse<RoleResponseDto>> GetFilterRoleData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var roleAllData = await GetAllRoleData(cancellationToken);
            var role = from x in roleAllData
                       where x.TenantId == _currentUser.TenantId
                       select new RoleResponseDto
                       {
                           Id = x.Id,
                           Name = x.Name,
                           NormalizedName = x.NormalizedName,
                           TenantId = x.TenantId
                       };
            var roleData = new PagedList<RoleResponseDto>(role, dataTableFilterDto);
            return new JsonRepsonse<RoleResponseDto>(dataTableFilterDto.Draw, roleData.TotalCount, roleData.TotalCount, roleData);
        }

        public async Task<RoleRequestDto> CreateRole(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            ApplicationRole addRoleData = new ApplicationRole();

            addRoleData.Name = roleRequestDto.Name;
            addRoleData.TenantId = _currentUser.TenantId;
            var insertedRole = await _unitOfWorkDA.RoleDA.CreateRole(addRoleData);
            return _mapper.Map<RoleRequestDto>(insertedRole);
        }

        public async Task<RoleRequestDto> UpdateRole(RoleRequestDto model, CancellationToken cancellationToken)
        {
            var data = await _roleManager.FindByIdAsync(model.Id);
            data.Name = model.Name;
            var updateRole = await _unitOfWorkDA.RoleDA.UpdateRole(data);
            return _mapper.Map<RoleRequestDto>(updateRole);
        }

        #region Private Method

        private async Task<IQueryable<ApplicationRole>> GetAllRoleData(CancellationToken cancellationToken)
        {
            var getAllRole = await _unitOfWorkDA.RoleDA.GetRoles(cancellationToken);
            if (getAllRole == null)
            {
                throw new Exception("Role data not found");
            }
            return getAllRole;
        }

        #endregion Private Method
    }
}