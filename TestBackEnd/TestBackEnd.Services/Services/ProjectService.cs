using AutoMapper;
using TestBackEnd.Domain.DTOs.Project;
using TestBackEnd.Application.Interfaces;
using TestBackEnd.Domain.Entities;
using TestBackEnd.Domain.Interfaces;
using TestBackEnd.Domain.Enums;
using TestBackEnd.Infrastructure;
using System.Transactions;
using TaskEntity = TestBackEnd.Domain.Entities.Task;
using ClosedXML.Excel;
using System.Reflection;
using TestBackEnd.Domain.DTOs;

namespace TestBackEnd.Application.Services
{
    /// <summary>
    /// Servicio con las operaciones sobre los proyectos
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly UserSessionService _userSessionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="userSessionService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public ProjectService(UserSessionService userSessionService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userSessionService = userSessionService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Obtiene los proyectos segun el usuario en sesión
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectDTO>> List()
        {
            var userSession = _userSessionService.GetUserFromSession();
            var listProjects = await _unitOfWork.ProjectRepository.GetAll();
            if (!userSession.UserRoles.Contains(Rols.administrator.GetDisplayName())) 
            {
                var task = await _unitOfWork.TaskRepository.GetByFunction(x =>x.UserId == userSession.UserID);
                return _mapper.Map<List<ProjectDTO>>(listProjects.Where(x => task.Select(x =>x.ProjectId).Contains(x.Id)));
            }               
            return _mapper.Map<List<ProjectDTO>>(listProjects);
        }


        /// <summary>
        /// Obtiene un proyecto por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProjectDTO> Get(Guid id)
        {
            return _mapper.Map<ProjectDTO>(await _unitOfWork.ProjectRepository.FindById(id));
        }

        /// <summary>
        /// Crea un proyecto y asigna una tarea si el usuario es lider
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ProjectDTO> Create(ProjectDTO data)
        {
            var userSession = _userSessionService.GetUserFromSession();

            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    Project project = await _unitOfWork.ProjectRepository.Create(_mapper.Map<Project>(data));
                    if (userSession != null && userSession.UserRoles.Contains(Rols.LeaderProject.GetDisplayName()))
                    {
                        var task = new TaskEntity 
                        {
                            Name = "Tarea de revisión",
                            Description = "Tarea creada automaticamente",
                            ProjectId = project.Id,
                            Type = (int)TypeTask.Review,
                            State = (int)State.process,
                            StartDate = DateTime.Now,
                            UserId = userSession.UserID
                        };
                        await _unitOfWork.TaskRepository.Create(task);
                    }
                    transaction.Complete();
                    return _mapper.Map<ProjectDTO>(project);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// Actualiza un proyecto
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ProjectDTO> Update(ProjectDTO data)
        {
            return _mapper.Map<ProjectDTO>(await _unitOfWork.ProjectRepository.Update(_mapper.Map<Project>(data)));
        }

        /// <summary>
        /// Inactiva un proyecto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> Delete(Guid id)
        {
            Project project = await _unitOfWork.ProjectRepository.FindById(id);
            if (project == null)
                return new { message = "No se enconto el projecto.", correct = false };

            project.State = (int)State.InActive;
            await _unitOfWork.ProjectRepository.Update(project);
            return new { correct = true };
        }

        public async Task<FileDTO> ExcelReport()
        {
            var result = (await _unitOfWork.ProjectRepository.GetAll(x =>x.Tasks)).ToList();
            if (!result.Any())
                return null;

            List<ReporExcelProjectDTO> data = new List<ReporExcelProjectDTO>();

            foreach (var item in result)
            {
                data.Add(new ReporExcelProjectDTO
                {
                    Name = item.Name,
                    Description = item.Description,
                    State = item.State,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    ReviewCount = item.Tasks.Where(x =>x.Type == (int)TypeTask.Review).Count(),
                    DevelopCount = item.Tasks.Where(x => x.Type == (int)TypeTask.Develop).Count(),
                    TestCount = item.Tasks.Where(x => x.Type == (int)TypeTask.Test).Count(),
                    BugCount = item.Tasks.Where(x => x.Type == (int)TypeTask.Bug).Count(),
                });

            }

            var headers = GetCellsSearch(data.FirstOrDefault().GetType());
            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet workSheet = data.ToWorksheet(headers, $"Reporte de proyectos", 1, 2);
                //.Style.Font.Bold = true;

                for (byte i = 1; i <= headers.Count; i++)
                {
                    for (int j = 2; j <= data.Count() + 2; j++)
                    {
                        workSheet.Cell(j, i).Style.Border.BottomBorderColor = XLColor.Black;
                        workSheet.Cell(j, i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        workSheet.Cell(j, i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        workSheet.Cell(j, i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        workSheet.Cell(j, i).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        workSheet.Cell(j, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                    workSheet.Cell(2, i).Style.Fill.BackgroundColor = XLColor.FromHtml("#438eb9");
                    workSheet.Cell(2, i).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                }
                workSheet.Columns().AdjustToContents();
                workBook.AddWorksheet(workSheet);
                MemoryStream output = new MemoryStream();
                workBook.SaveAs(output);
                string nombreArchivo = $"Reporte de proyectos {DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                return new FileDTO { File = output.ToArray(), NameFile = nombreArchivo };
            }
        }

        private static List<ExcelCell> GetCellsSearch(Type t)
        {
            List<ExcelCell> cells = new List<ExcelCell>();
            var dateStyle = new ExcelStyle { FormatStyle = FormatStyle.DateTime };
            foreach (PropertyInfo pi in t.GetProperties().Where(x => x.CustomAttributes.Any()))
            {
                cells.Add(new ExcelCell(pi.CustomAttributes.FirstOrDefault().ConstructorArguments.FirstOrDefault().Value.ToString(),
                    pi.Name,
                    pi.CustomAttributes.Count() > 1 ? dateStyle : null));
            }
            return cells;
        }
    }
}
