using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Services.DataBase;

namespace ToDo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private IDbService _dbService;

        public ToDoController(IDbService dbService)
        {
            _dbService = dbService;
        }

        // GET: todo/todo
        [HttpGet]
        public ActionResult Index()
        {
            var deals = _dbService.GetAll<DataAccess.Entities.Deal>().ToList();         
            return new JsonResult(deals);
        }

        // POST: todo/Create
        [HttpPost()]
        public async Task<ActionResult<DataAccess.Entities.Deal>> Create([FromBody] DataAccess.Entities.Deal deal)
        {
            try
            {
                var updatedDeal = await _dbService.CreateOrUpdateAsync(deal);
                return new JsonResult(updatedDeal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // DELETE: todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var dealToDelete = await _dbService.GetAll<DataAccess.Entities.Deal>(d => d.Id == id).FirstOrDefaultAsync();
            
            if(dealToDelete == null)
            {
                return NotFound();
            }
            
            try
            {
                bool deleted = await _dbService.DeleteAsync(dealToDelete);
                return new JsonResult(deleted);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
//[ApiController]
//[Route("[controller]")]
//public class WeatherForecastController : ControllerBase
//{
//    private static readonly string[] Summaries = new[]
//    {
//        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//    };

//    private readonly ILogger<WeatherForecastController> _logger;

//    public WeatherForecastController(ILogger<WeatherForecastController> logger)
//    {
//        _logger = logger;
//    }

//    [HttpGet(Name = "GetWeatherForecast")]
//    public IEnumerable<WeatherForecast> Get()
//    {
//        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//        {
//            Date = DateTime.Now.AddDays(index),
//            TemperatureC = Random.Shared.Next(-20, 55),
//            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//        })
//        .ToArray();
//    }
//}
