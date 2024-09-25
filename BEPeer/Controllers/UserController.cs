using System.Text;
using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BEPeer.Controllers
{
    [Route("rest/v1/user/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userservices;
        public UserController(IUserServices userServices)
        {
            _userservices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> Register(ReqRegisterUserDto register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Any())
                        .Select(x =>
                        new {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();
                    var errorMessage = new StringBuilder("Validate errors occured!");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }
                var res = await _userservices.Register(register);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "User Registered!",
                    Data = res,
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email already useddd!")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null,
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            };
        }

        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<IActionResult> AddUser(ReqRegisterUserDto register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Any())
                        .Select(x =>
                        new {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();
                    var errorMessage = new StringBuilder("Validate errors occured!");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }
                var res = await _userservices.Register(register);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "User Registered!",
                    Data = res,
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email already useddd!")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null,
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            };
        }


        [HttpPost]
        public async Task<IActionResult> Login(ReqLoginUserDto login)
        {
            try
            {
                var res = await _userservices.Login(login);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User Logged in!",
                    Data = res,
                });

        
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email or password is Wrong!")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null,
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            };
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userservices.GetAllUsers();
                return Ok(new ResBaseDto<List<ResUserDto>>
                {
                    Success = true,
                    Message ="Success gathered All user data!",
                    Data  = users
                });
            } catch(Exception ex)
                {
                if (ex.Message == "Email or Password is Wrong!")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null,
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });

            }
        }

        [HttpPut]
        //[Authorize(Roles ="admin")]
        public async Task<IActionResult> UpdatebyAdmin([FromQuery] string id, ReqUpdateAdminDto reqUpdate)
        {
            try
            {
                var res = await _userservices.UpdateUserbyAdmin(reqUpdate, id);

                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User Updated!",
                    Data = res,
                });
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
        }


        [HttpDelete]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            try
            {
                var response = await _userservices.Delete(id);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User berhasil di delete",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "User not found")
                {
                    return BadRequest(new ResBaseDto<string>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });

            }

        }
    }
}
