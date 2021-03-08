using Model.Domain.Models;
using Model.Domain.Entities;
using System.Threading.Tasks;
using Model.Domain.Responses;
using System.Collections.Generic;

namespace Model.Domain.Interfaces
{
    public interface IServiceUser
    {
        Task<User> Insert(User user);
        Task<List<UserResponse>> FindByFilters(FilterRequestModel requestFilter);
        Task<UserResponse> DisableUser(UserRequestUpdateModel requestDisableUser);
    }
}
