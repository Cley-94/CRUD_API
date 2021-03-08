using System;
using Model.Domain;
using Model.Domain.Models;
using Model.Domain.Entities;
using System.Threading.Tasks;
using Model.Domain.Responses;
using Model.Domain.Interfaces;
using System.Collections.Generic;

namespace Model.Service.Services
{
    public class UserService : IServiceUser
    {
        private readonly IRepositoryUser _repositoryUser;

        public UserService(IRepositoryUser repositoryUser)
        {
            _repositoryUser = repositoryUser;
        }

        public async Task<User> Insert(User user)
        {
            return await _repositoryUser.AddUser(user);
        }

        public async Task<List<UserResponse>> FindByFilters(FilterRequestModel requestFilter)
        {
            var usersFiltered = await _repositoryUser.FindByFilters(requestFilter);
            return usersFiltered;
        }

        public async Task<UserResponse> DisableUser(UserRequestUpdateModel requestDisableUser)
        {
            var userDisabled = await _repositoryUser.DisableUser(requestDisableUser);
            return userDisabled;
        }
    }
}
