using AutoMapper;
using TestBackEnd.Domain.DTOs.Task;
using TestBackEnd.Application.Interfaces;
using TestBackEnd.Domain.Interfaces;
using TaskEntity = TestBackEnd.Domain.Entities.Task;
using TestBackEnd.Domain.Enums;
using TestBackEnd.Infrastructure;

namespace TestBackEnd.Application.Services
{
    /// <summary>
    /// Servicio con las operaciones sobre las tareas
    /// </summary>
    public class TaskService : IGenericService<TaskDTO>
    {
        private readonly UserSessionService _userSessionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructir de la clase 
        /// </summary>
        /// <param name="userSessionService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public TaskService(UserSessionService userSessionService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userSessionService = userSessionService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Obtiene las tareas segun el usuario en sesión
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskDTO>> List()
        {
            var userSession = _userSessionService.GetUserFromSession();
            var listTaks = await _unitOfWork.TaskRepository.GetAll();
            if (!userSession.UserRoles.Contains(Rols.administrator.GetDisplayName()))
                return _mapper.Map<List<TaskDTO>>(listTaks.Where(x => x.UserId == userSession.UserID).ToList());

            return _mapper.Map<List<TaskDTO>>(listTaks);
        }

        /// <summary>
        /// Obtiene una tareas por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskDTO> Get(Guid id)
        {
            return _mapper.Map<TaskDTO>(await _unitOfWork.TaskRepository.FindById(id));
        }

        /// <summary>
        /// Crea una tarea
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<TaskDTO> Create(TaskDTO data)
        {
            var a = _mapper.Map<TaskEntity>(data);

            return _mapper.Map<TaskDTO>(await _unitOfWork.TaskRepository.Create(_mapper.Map<TaskEntity>(data)));
        }

        public Task<TaskDTO> Update(TaskDTO data)
        {
            throw new NotImplementedException();
        }

        public Task<object> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
