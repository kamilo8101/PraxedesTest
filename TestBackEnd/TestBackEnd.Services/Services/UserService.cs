using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestBackEnd.Application.Interfaces;
using TestBackEnd.Domain.DTOs;
using TestBackEnd.Domain.Enums;
using TestBackEnd.Domain.Interfaces;
using TestBackEnd.Infrastructure;
using Z.EntityFramework.Plus;

namespace TestBackEnd.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserSessionService _userSessionService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, UserSessionService userSessionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _userSessionService = userSessionService;
        }

        public Task<List<SelectListDTO>> SelectList()
        {
            throw new NotImplementedException();
        }

        public async Task<List<SelectListDTO>> SelectListTask(int type)
        {
            IList<IdentityUser>users;
            IEnumerable<string> taks;
            switch (type)
            {
                case (int)TypeTask.Develop:
                case (int)TypeTask.Bug:
                    users = await _userManager.GetUsersInRoleAsync(Rols.developer.GetDisplayName());
                    taks = (await _unitOfWork.TaskRepository.GetByFunction(x => users.Select(y => y.Id).Contains(x.UserId) && new int[] { (int)State.pending, (int)State.process}.Contains(x.State))).GroupBy(x =>x.UserId).Where(g => g.Count() >= 3).Select(x => x.Key);
                    return users.Where(x => !taks.Contains(x.Id)).Select(x => new SelectListDTO
                    {
                        Id = x.Id,
                        Value = x.UserName
                    }).ToList();
                case (int)TypeTask.Test:
                    users = await _userManager.GetUsersInRoleAsync(Rols.Tester.GetDisplayName());
                    taks = (await _unitOfWork.TaskRepository.GetByFunction(x => users.Select(y => y.Id).Contains(x.UserId) && new int[] { (int)State.pending, (int)State.process}.Contains(x.State))).GroupBy(x => x.UserId).Select(x => x.Key);
                    return users.Where(x => !taks.Contains(x.Id)).Select(x => new SelectListDTO
                    {
                        Id = x.Id,
                        Value = x.UserName
                    }).ToList();

                default:
                    return null;
            }
        }
    }
}
