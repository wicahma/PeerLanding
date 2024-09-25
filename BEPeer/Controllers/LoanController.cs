using System.Text;
using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services.Interfaces;
using DAL.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BEPeer.Controllers
{

    [Route("rest/v1/loan/[action]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanservices;
        public LoanController(ILoanService loanService)
        {
            _loanservices = loanService;
        }
        [HttpPost]
        public async Task<IActionResult> NewLoan(ReqLoanDto loan)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x =>
                        new
                        {
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

                var res= await _loanservices.CreateLoan(loan);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "Loan Created!",
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

        [HttpPut]
        public async Task<IActionResult> UpdateLoan(ReqUpdateLoanDto loan, [FromQuery] string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x =>
                        new
                        {
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

                var res = await _loanservices.UpdateLoan(loan, id);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = res.Message,
                    Data = res.Status,
                });
            } catch (Exception ex)
            {
                if (ex.Message == "Loan not found!")
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
                    Data = null,
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> getLoan([FromQuery] string status)
        {
            try
            {
                var res = await _loanservices.GetAllLoan(status);

                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "All Loan data loaded succesfully!",
                    Data = res,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
        }
    }
}
