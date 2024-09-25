using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repository.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Services
{
    public class LoanServices : ILoanService
    {
        private readonly PeerlandingContext _peerlandingComtext;
        public LoanServices(PeerlandingContext peerlandingContext)
        {
            _peerlandingComtext = peerlandingContext;
        }
        public async Task<string> CreateLoan(ReqLoanDto loan)
        {
            var newLoan = new MstLoans
            {
                BorrowerId = loan.BorrowerId,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                Duration = loan.Duration,
            };

            await _peerlandingComtext.MstLoans.AddAsync(newLoan);
            await _peerlandingComtext.SaveChangesAsync();

            return newLoan.BorrowerId;
        }

        public Task<List<ResGetLoanDto>> GetAllLoan(string s)
        {
            var loans = _peerlandingComtext.MstLoans
                .Include(x => x.User)
                .Where(x => s == null || x.Status == s)
                .OrderByDescending(x=> x.CreatedAt)
                .Select(x => new ResGetLoanDto
            {
                LoanId = x.Id,
                BorrowerName = x.User.Name,
                InterestRate = x.InterestRate,
                Duration = x.Duration,
                Status = x.Status,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToListAsync();

            return loans;
        }

        public Task<ResUpdateLoanDto> UpdateLoan(ReqUpdateLoanDto loan, string id)
        {
           var loanToUpdate = _peerlandingComtext.MstLoans.SingleOrDefault(x => x.Id == id);
            if (loanToUpdate == null)
            {
                throw new Exception("Loan not found!");
            }
            loanToUpdate.Status = loan.Status;
            _peerlandingComtext.MstLoans.Update(loanToUpdate);
            _peerlandingComtext.SaveChanges();
            return Task.FromResult( new ResUpdateLoanDto
            {
                Message = "Loan updated successfully!",
                Status = loanToUpdate.Status
            });
        }
    }
}
