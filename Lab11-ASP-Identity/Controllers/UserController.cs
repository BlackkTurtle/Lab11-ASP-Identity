using System.Security.Claims;
using Lab11_ASP_Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public UserController(ILogger<UserController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost("/signin/{password}")]
        public async Task<ActionResult> LogInUserAsync([FromBody] User fullentity,string password)
        {
            try
            {
                var entity = await userManager.FindByNameAsync(fullentity.UserName);
                if (entity == null)
                {
                    return BadRequest("User does not exist");
                }
                var result=await signInManager.PasswordSignInAsync(entity, password,false,false);
                if (result.Succeeded)
                {
                    return Ok("Logged In");
                }
                else
                {
                    return BadRequest("Wrong Password");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
        [HttpPost("LogOut")]
        public async Task<ActionResult<User>> LogOutAsync()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Ok("Logged Out");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
        [HttpPost("{password}")]
        public async Task<ActionResult> PostUserAsync([FromBody] User fullentity,string password)
        {
            try
            {
                if (fullentity == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт є некоректним");
                }
                var entity = new User()
                {
                    Name = fullentity.Name,
                    UserName=fullentity.UserName
                };
                var result=userManager.CreateAsync(entity,password).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    userManager.AddClaimAsync(entity,new Claim(ClaimTypes.Role,"Administrator")).GetAwaiter().GetResult();
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
        [HttpPut("/updateuser")]
        public async Task<ActionResult> UpdateTable1Async([FromBody] User updatedentity)
        {
            try
            {
                if (updatedentity == null)
                {
                    _logger.LogInformation($"Empty JSON received from the client");
                    return BadRequest("object is null");
                }

                var entity = await userManager.FindByNameAsync(updatedentity.UserName);
                if (entity == null)
                {
                    _logger.LogInformation($"username: {updatedentity.UserName} was not found in the database");
                    return NotFound();
                }
                entity = new User
                {
                    Name = updatedentity.Name,
                };
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction failed! Something went wrong in method - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred.");
            }
        }

        [HttpDelete("{uname}")]
        public async Task<ActionResult> DeleteTable1ByIdAsync(string uname)
        {
            try
            {
                var entity = await userManager.FindByNameAsync(uname);
                if (entity == null)
                {
                    _logger.LogInformation($"Id: {uname}, не був знайдейний у базі даних");
                    return NotFound();
                }
                await userManager.DeleteAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
    }
}
