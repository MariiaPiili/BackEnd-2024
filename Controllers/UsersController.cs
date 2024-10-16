﻿using BackEnd_2024_Project.Models;
using BackEnd_2024_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd_2024_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService service)
        {
            _userService = service;
        }

        // GET: api/Users
        /// <summary>
        /// Gets the info of all users in database
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        // GET: api/Users/username
        /// <summary>
        /// Get a user specified by username
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <returns>User info for one user or empty</returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            UserDTO? user = await _userService.GetUserAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/Users/username
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUser(string username, User user)
        {
            if (username != user.UserName)
            {
                return BadRequest();
            }
            if (await _userService.UpdateUserAsync(user))
            {
                return NoContent();
            }
            return NotFound();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            UserDTO? newUser = await _userService.NewUserAsync(user);
            if(newUser == null)
            {
                return Problem("Username not available", statusCode:400);
            }
            return CreatedAtAction("GetUser", new { username = user.UserName }, user);
        }

        // DELETE: api/Users/username
        /// <summary>
        /// Delete user specified by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            bool result = await _userService.DeleteUserAsync(username);
            if (!result)
            {
                return NoContent();
            }
            return NoContent();
        }        
    }
}
