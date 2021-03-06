using Model.Domain.Models;
using Model.Domain.Entities;
using Model.Domain.Responses;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Model.Domain.Interfaces
{
    public interface IRepositoryUser
    {
        Task<User> AddUser(User user);
        Task<List<User>> GetAll();
        Task<List<UserResponse>> FindByFilters(FilterRequestModel requestFilter);
        Task<UserResponse> DisableUser(UserRequestUpdateModel requestDisableUser);
    }
}
