using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ProjectBlack.CreditRequests.Dtos;
using System.Threading.Tasks;
using ProjectBlack.Authorization;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.UI;
using ProjectBlack.Authorization.Users;

namespace ProjectBlack.CreditRequests
{
    [AbpAuthorize]
    public class CreditRequestAppService : AsyncCrudAppService<CreditRequest, CreditRequestDto, long>
    {
        private readonly UserManager _userManager;

        public CreditRequestAppService(
            IRepository<CreditRequest, long> repository,
            UserManager userManager)
            : base(repository)
        {
            _userManager = userManager;
        }

        public override async Task<CreditRequestDto> CreateAsync(CreditRequestDto input)
        {
            if (input.IngresoMensual >= 1500)
                input.Estado = CreditStatus.Aprobado.ToString();
            else
                input.Estado = CreditStatus.Pendiente.ToString();

            return await base.CreateAsync(input);
        }

        public override async Task<PagedResultDto<CreditRequestDto>> GetAllAsync(PagedAndSortedResultRequestDto input)
        {
            if (PermissionChecker.IsGranted(PermissionNames.Pages_Analyst))
            {
                var result = await base.GetAllAsync(input);

                foreach (var dto in result.Items)
                {
                    if (dto.CreatorUserId.HasValue)
                    {
                        var user = _userManager.Users.FirstOrDefault(u => u.Id == dto.CreatorUserId.Value);
                        dto.CreatorUserName = user != null ? $"{user.Name} {user.Surname}" : string.Empty;
                    }
                }

                return result;
            }

            var userId = AbpSession.UserId;
            if (userId == null)
                throw new UserFriendlyException("Usuario no autenticado.");

            var query = Repository.GetAll()
                .Where(x => x.CreatorUserId == userId.Value);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.CreationTime)
                .PageBy(input)
                .ToListAsync();

            var dtos = ObjectMapper.Map<List<CreditRequestDto>>(items);

            foreach (var dto in dtos)
            {
                dto.CreatorUserName = _userManager.Users
                    .Where(u => u.Id == dto.CreatorUserId)
                    .Select(u => u.Name + " " + u.Surname)
                    .FirstOrDefault();
            }

            return new PagedResultDto<CreditRequestDto>(totalCount, dtos);
        }

        [AbpAuthorize(PermissionNames.Pages_Analyst)]
        public async Task Approve(long id)
        {
            var credito = await Repository.GetAsync(id);

            if (credito.CreatorUserId == AbpSession.UserId)
                throw new UserFriendlyException("No puedes aprobar tu propia solicitud.");

            credito.Estado = CreditStatus.Aprobado;
            await Repository.UpdateAsync(credito);
        }

        [AbpAuthorize(PermissionNames.Pages_Analyst)]
        public async Task Reject(long id)
        {
            var entity = await Repository.GetAsync(id);
            entity.Estado = CreditStatus.Rechazado;
            await Repository.UpdateAsync(entity);
        }

        [AbpAuthorize(PermissionNames.Pages_Analyst)]
        public async Task<ListResultDto<CreditRequestDto>> GetByEstadoAsync(string estado)
        {
            var items = await Repository.GetAll()
                .Where(x => x.Estado.ToString() == estado)
                .ToListAsync();

            var dtos = ObjectMapper.Map<List<CreditRequestDto>>(items);

            foreach (var dto in dtos)
            {
                dto.CreatorUserName = _userManager.Users
                    .Where(u => u.Id == dto.CreatorUserId)
                    .Select(u => u.Name + " " + u.Surname)
                    .FirstOrDefault();
            }

            return new ListResultDto<CreditRequestDto>(dtos);
        }
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var entity = await Repository.GetAsync(input.Id);

            if (entity.CreatorUserId != AbpSession.UserId)
                throw new UserFriendlyException("No puedes eliminar una solicitud que no te pertenece.");

            if (entity.Estado != CreditStatus.Pendiente)
                throw new UserFriendlyException("Solo puedes eliminar solicitudes en estado 'Pendiente'.");

            await base.DeleteAsync(input);
        }

    }
}
