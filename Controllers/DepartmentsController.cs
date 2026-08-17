using System.Threading;
using AutoMapper;
using DWQueueAPI.Data.Entities;
using DWQueueAPI.DTOs.DepartmenDTOs;
using DWQueueAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;



namespace DWQueueAPI.Controllers

{
    [Route("api/[controller]")]

    [ApiController]

    [Authorize]

    public class DepartmentsController : ControllerBase

    {
        private readonly DepartmentService _departmentService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "AllDepartments"; 

        public DepartmentsController(DepartmentService departmentService, IMapper mapper , IDistributedCache cache)
        {
            _departmentService = departmentService;
            _mapper = mapper;
            _cache = cache;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            //try
            //{
                Console.WriteLine("🔄 >>> [Redis Cache] در حال بررسی موجود بودن دپارتمان‌ها در ردیس...");


                var cachedData = await _cache.GetStringAsync(CacheKey, cancellationToken);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    Console.WriteLine("🚀 >>> [Redis Cache] دیتای دپارتمان‌ها با سرعت نور از رِدیس خوانده شد!");
                    var responseDto = JsonSerializer.Deserialize<IEnumerable<DepartmentResponseDto>>(cachedData);
                    return Ok(responseDto);
                }
                // ۲. اگر در ردیس نبود، از سرویس اصلی و دیتابیس می‌خوانیم
                Console.WriteLine("⚠️ >>> [Redis Cache] دیتا در ردیس نبود. رفتن سراغ دیتابیس...");
                var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
                var response = _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);


                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // کل کش ۱۰ دقیقه عمر کند
                    SlidingExpiration = TimeSpan.FromMinutes(2) // اگر ۲ دقیقه درخواستی نیامد پاک شود
                };

                var serializedData = JsonSerializer.Serialize(response);
                await _cache.SetStringAsync(CacheKey, serializedData, cacheOptions, cancellationToken);
                Console.WriteLine("💾 >>> [Redis Cache] دیتای جدید با موفقیت در ردیس ذخیره شد.");
            //var response = departments.Select(d => new DepartmentResponseDto

            //{

            //    DepartmentID = d.DepartmentID,

            //    DepartmentName = d.DepartmentName

            //}).ToList();

            return Ok(response);
            //}
            //catch (Exception ex)
            //{
              //  return BadRequest(ex.Message);
            //}
        }


        [HttpGet("{id}")]

        public async Task<IActionResult> GetByID(int id , CancellationToken cancellationToken)
        {
            //try
            //{
                var department = await _departmentService.GetDepartmentByIDAsync(id , cancellationToken);
                var response = _mapper.Map<DepartmentResponseDto>(department);

                if (department == null)
                    return NotFound("Department not found");
                
                //var response = new DepartmentResponseDto
                //{
                //    DepartmentID = department.DepartmentID,
                //    DepartmentName = department.DepartmentName
                //};

                return Ok(response);
            //}

            //catch (Exception ex)
            //{
                //return BadRequest(ex.Message);
            //}
        }





        [HttpPost]

        public async Task<IActionResult> Create(CreateDepartmentDto createDepartment , CancellationToken cancellationToken)
        {
            //try
            //{
                var department = _mapper.Map<Departments>(createDepartment);
                //Departments department = new Departments
                //{
                //    DepartmentName = createDepartment.DepartmentName
                //};

                await _departmentService.AddDepartmentAsync(department, cancellationToken);

                await _cache.RemoveAsync(CacheKey, cancellationToken);
                Console.WriteLine("🗑️ >>> [Redis Cache] به دلیل ساخت دپارتمان جدید، کش قبلی باطل و پاک شد.");

                return Ok("Department created successfully");
                //}
                    //catch (Exception ex)
                //{
                //var innerError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                //return BadRequest(innerError);

            //}

        }



        [HttpPut(nameof(Update))]
        public async Task<IActionResult> Update(UpdateDepartmetDto updateDepartment, CancellationToken cancellationToken)
        {
            //try
            //{
                if (updateDepartment.DepartmentID == null)
                    return BadRequest("ID in URL does not match ID in body.");
                //Departments department = new Departments
                //{
                //    DepartmentID = updateDepartment.DepartmentID,
                //    DepartmentName = updateDepartment.DepartmentName
                //};

                var department = _mapper.Map<Departments>(updateDepartment);
                await _departmentService.UpdateDepartmentAsync(department, cancellationToken);
                // ⚠️ [Cache Invalidation] چون دیتا آپدیت شده، کش قبلی ردیس را باطل می‌کنیم
                await _cache.RemoveAsync(CacheKey, cancellationToken);
                Console.WriteLine("🗑️ >>> [Redis Cache] به دلیل ویرایش دپارتمان، کش قبلی باطل و پاک شد.");
                return Ok("Department updated successfully");
            //}
            //catch (Exception ex)
            //{
                //return BadRequest(ex.Message);
            //}
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            //try
            //{
                await _departmentService.DeleteDepartmentAsync(id, cancellationToken);

                // ⚠️ [Cache Invalidation] چون دیتا حذف شده، کش قبلی ردیس را باطل می‌کنیم
                await _cache.RemoveAsync(CacheKey, cancellationToken);
                Console.WriteLine("🗑️ >>> [Redis Cache] به دلیل حذف دپارتمان، کش قبلی باطل و پاک شد.");


                return Ok("Department deleted successfully");
            //}
            //catch (Exception ex)
            //{
                //return BadRequest(ex.Message);
            //}
        }
    }



}