using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Services.DataBase;
using ILogger = Serilog.ILogger;

namespace ToDo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private IDbService _dbService;
        private ILogger _logger;

        public ToDoController(IDbService dbService, ILogger logger)
        {
            _dbService = dbService;
            _logger = logger;
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
                _logger.Error(ex, "Error occurred when performing Create API method. @{deal}", deal);
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
