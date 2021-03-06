using System;
using System.Linq;
using Model.Domain;
using Model.Domain.Models;
using Model.Domain.Entities;
using Model.Domain.Responses;
using System.Threading.Tasks;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Model.Infra.Data.Repository
{
    public class UserRepository : IRepositoryUser
    {
        protected readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                _dataContext.Users.Add(user);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            var users = await _dataContext.Users.ToListAsync();
            return users;
        }

        public async Task<List<UserResponse>> FindByFilters(FilterRequestModel requestFilter)
        {
            var users = await GetAll();

            var userResponse = new List<UserResponse>();

            //Nome e ativo VAZIO
            if (string.IsNullOrEmpty(requestFilter.Name) && requestFilter.IsActive == 0)
            {
                return users.Select(x => new UserResponse {
                                                            Id = x.Id
                                                            , Name = x.Name
                                                            , Email = x.Email
                                                            , Sex = x.Sex
                                                            ,IsActive = x.IsActive
                                                            , Age = x.CalculateAge()
                                                          }).ToList();
            }

            //Nome E ativo Preenchido com valor ativo "1" ou "2"
            else if (!string.IsNullOrEmpty(requestFilter.Name) && requestFilter.IsActive != 0)
            {
                userResponse = users.Where(u => u.Name == requestFilter.Name && u.IsActive.Equals(requestFilter.IsActive))
                                    .Select(x => new UserResponse { 
                                                        Id = x.Id
                                                        , Name = x.Name
                                                        , Email = x.Email
                                                        , Sex = x.Sex
                                                        , IsActive = x.IsActive
                                                        , Age = x.CalculateAge()
                                                        }).ToList();

                return userResponse;
            }

            //Apenas Nome preenchido
            else if(!string.IsNullOrEmpty(requestFilter.Name))
            {
                userResponse = users.Where(u => u.Name == requestFilter.Name)
                                    .Select(x => new UserResponse { 
                                                        Id = x.Id
                                                        , Name = x.Name
                                                        , Email = x.Email
                                                        , Sex = x.Sex
                                                        , IsActive = x.IsActive  
                                                        , Age = x.CalculateAge()
                                                        }).ToList();

                return userResponse;
            }

            else
            {
                userResponse = users.Where(u => u.IsActive == requestFilter.IsActive)
                                    .Select(x => new UserResponse { 
                                                        Id = x.Id
                                                        , Name = x.Name
                                                        , Email = x.Email
                                                        , Sex = x.Sex
                                                        , IsActive = x.IsActive  
                                                        , Age = x.CalculateAge()
                                                        }).ToList();

                return userResponse;
            }

            //TODO Refactoring
            //userResponse = users.Where(x => x.Name == requestFilter.Name && x.IsActive.Equals(requestFilter.IsActive))
            //                    .Where(x => x.Name == requestFilter.Name)
            //                    .Where(x => x.IsActive == requestFilter.IsActive)
            //                    .Select(x => new UserResponse { 
            //                                              Id = x.Id
            //                                              , Name = x.Name
            //                                              , Email = x.Email
            //                                              , Sex = x.Sex
            //                                              , IsActive = x.IsActive  
            //                                              , Age = x.CalculateAge()
            //                                              }).ToList();

            //return userResponse;
        }

        public async Task<UserResponse> DisableUser(UserRequestUpdateModel requestDisableUser)
        {
            var users = await _dataContext.Users.FindAsync(requestDisableUser.Id); 

            users.IsActive = requestDisableUser.IsActive;
            _dataContext.Entry(users).CurrentValues.SetValues(requestDisableUser.IsActive);
            _dataContext.Entry(users).State = EntityState.Modified;

            if (_dataContext.Entry(users).Properties.Any(property => property.IsModified))
            {
                await _dataContext.SaveChangesAsync();
            }

            var userResponse = new UserResponse(){
                                                   Id = users.Id
                                                   , Name = users.Name
                                                   , Email = users.Email
                                                   , Sex = users.Sex
                                                   , IsActive = users.IsActive
                                                   , Age = users.CalculateAge()
                                                 };

            return userResponse;
        }
    }
}
