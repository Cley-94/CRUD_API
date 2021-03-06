using System;
using Model.Domain;
using System.Linq;
using Model.Domain.Models;
using Model.Domain.Entities;
using System.Threading.Tasks;
using Model.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Model.Infra.Data.Context;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Model.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        /// <summary>
        /// Create an user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /usuario
        ///     {
        ///         "Name": "Teste",
        ///         "BirthDate" : "1998-02-09T00:00:00",
        ///         "Email": "teste@gmail.com",
        ///         "Password": "123456",
        ///         "Sex": "Male"
        ///     }
        /// 
        /// </remarks>
        /// <returns>User id registered</returns>
        [HttpPost]
        [Route("usuario")]
        public async Task<IActionResult> Create([FromServices] DataContext context, [FromBody] User userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Users.Add(userModel);
                    await context.SaveChangesAsync();
                    return Created($"/api/usuario/{userModel.Id}", "user id: " + userModel.Id);
                }
                else
                {
                    return BadRequest(ModelState);
                }
                //return Created($"/api/users/{user?.Id}", user?.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Find an User or Inactivated Users
        /// </summary>
        /// <remarks>
        /// Sample requests (1 = Ativo; 2 = Desativo):
        ///     
        ///     GET /usuarios
        ///     {
        ///     }
        /// 
        ///     GET /usuarios
        ///     {
        ///         "Name": "Cleiton"
        ///     }
        ///     
        ///     GET /usuarios
        ///     {
        ///         "IsActive" : "1"
        ///     }
        ///     
        ///     GET /usuarios
        ///     {
        ///         "Name": "Cleiton",
        ///         "IsActive" : "1"
        ///     }
        /// 
        /// </remarks>
        /// <returns>User(s) registered</returns>
        [HttpGet]
        [Route("usuarios")]
        public async Task<List<UserResponse>> GetUsers([FromServices] DataContext context, [FromQuery] FilterRequestModel filterRequest = null)
        {
            try
            {
                var users = await context.Users.ToListAsync();
                var userResponse = new List<UserResponse>();

                //Com o nome e ativo VAZIO
                if (string.IsNullOrEmpty(filterRequest.Name) && filterRequest.IsActive == 0)
                {
                        userResponse = users.Select(x => new UserResponse { 
                                                          Id = x.Id
                                                          , Name = x.Name
                                                          , Email = x.Email
                                                          , Sex = x.Sex
                                                          , IsActive = x.IsActive
                                                          , Age = x.CalculateAge()
                                                          }).ToList();
                
                    return userResponse;
                }

                //COM o nome E ativo Preenchido com valor ativo "1" ou "2"
                else if (!string.IsNullOrEmpty(filterRequest.Name) && filterRequest.IsActive != 0)
                {
                    userResponse = users.Where(u => u.Name == filterRequest.Name && u.IsActive.Equals(filterRequest.IsActive))
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
                else if(!string.IsNullOrEmpty(filterRequest.Name))
                {
                    userResponse = users.Where(u => u.Name == filterRequest.Name)
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

                //Apenas Ativo preenchido
                else
                {
                    userResponse = users.Where(u => u.IsActive == filterRequest.IsActive)
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
            }

            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }

        /// <summary>
        /// Enable or disable an user
        /// </summary>
        /// <remarks>
        /// Sample request (1 = Ativo; 2 = Desativo):
        /// 
        ///     PATCH /id
        ///     {
        ///         "id": "1",
        ///         "IsActive" : false
        ///     }
        /// 
        /// </remarks>
        /// <returns>User updated</returns>
        [HttpPatch]
        [Route("inativar-usuario")]
        public async Task<IActionResult> UpdatePartialAsync([FromServices] DataContext context, [FromQuery] UserRequestUpdateModel request)
        {
            var users = await context.Users.FindAsync(request.Id); 

            try
            {
                if (users == null)
            {
                return NotFound();
            }
            users.IsActive = request.IsActive;
            context.Entry(users).CurrentValues.SetValues(request.IsActive);
            context.Entry(users).State = EntityState.Modified;

            if (context.Entry(users).Properties.Any(property => property.IsModified))
            {
                await context.SaveChangesAsync();
            }

                var userResponse = new UserResponse() {
                    Id = users.Id
                                                        , Name = users.Name
                                                        , Email = users.Email
                                                        , Sex = users.Sex
                                                        , IsActive = users.IsActive
                                                        , Age = users.CalculateAge()
                                                 };

            return Ok(userResponse);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
