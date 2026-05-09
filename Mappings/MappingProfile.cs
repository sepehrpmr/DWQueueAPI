using AutoMapper;
using DWQueueAPI.Data.Entities;
using DWQueueAPI.DTOs.DepartmenDTOs; 
using DWQueueAPI.DTOs.EmployeeDTOs;
using DWQueueAPI.DTOs.ProjectDTOs;

namespace DWQueueAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employees, EmployeeResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeID));

            CreateMap<CreateEmployeeDto, Employees>();
            CreateMap<UpdateEmployeeDto, Employees>();





            CreateMap<Departments, DepartmentResponseDto>();

            CreateMap<CreateDepartmentDto, Departments>();
            CreateMap<UpdateDepartmetDto, Departments>();





            CreateMap<Projects, ProjectResponseDto>();


            CreateMap<CreateProjectDto, Projects>();
            CreateMap<UpdateProjectDto, Projects>();


        }
    }
}
