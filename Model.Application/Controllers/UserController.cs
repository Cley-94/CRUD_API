using System;
using Model.Domain;
using Model.Domain.Models;
using Model.Domain.Entities;
using System.Threading.Tasks;
using Model.Domain.Responses;
using Model.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Infra.Data.Context;
using System.Collections.Generic;

namespace Model.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceUser _serviceUser;

        public UserController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }

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
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _serviceUser.Insert(user);
                    return Created($"/api/usuario/{user.Id}", "user id: " + user.Id);
                }
                else
                {
                    return BadRequest(ModelState);
                } 
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
        public async Task<List<UserResponse>> GetUsers([FromQuery] FilterRequestModel filterRequest = null)
        {
            try
            {
                var usersFiltered = await _serviceUser.FindByFilters(filterRequest);
                return usersFiltered;
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
        public async Task<IActionResult> DisableUser([FromServices] DataContext context, [FromQuery] UserRequestUpdateModel request)
        {
            try
            {
                var userDisabled = await _serviceUser.DisableUser(request);
                return Ok(userDisabled);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
