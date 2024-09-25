using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Req;
using DAL.DTO.Res;

namespace DAL.Repository.Services.Interfaces
{
    public interface ILoanService
    {
        Task<string> CreateLoan(ReqLoanDto loan);
        Task<ResUpdateLoanDto> UpdateLoan(ReqUpdateLoanDto loan, string id);

        Task<List<ResGetLoanDto>> GetAllLoan(string status);
    }
}
