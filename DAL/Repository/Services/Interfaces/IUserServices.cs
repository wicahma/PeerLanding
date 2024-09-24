using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface IUserServices
    {
        Task<string> Register(ReqRegisterUserDto register); 
        Task<ResLoginDto> Login(ReqLoginUserDto login);
        Task<List<ResUserDto>> GetAllUsers();
        Task<ResUpdateDto> UpdateUserbyAdmin(ReqUpdateAdminDto reqUpdate, string id);
        Task<string> Delete(string id);
    }
}