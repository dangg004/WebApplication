using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs.Account;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("WebApplication/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName.ToLower());
            if (user == null) {
                return Unauthorized("Username not found and/or password incorrect");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded) {
                return Unauthorized("Username not found and/or password incorrect");
            }
            return Ok(
                new NewUserDTO {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email
                };
                var createdUser = await _userManager.CreateAsync(appUser, registerDTO.Password);
                if (createdUser.Succeeded) {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded) {
                        return Ok(
                            new NewUserDTO {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else {
                    return StatusCode(500, createdUser.Errors);
                }
            } catch (Exception e) {
                return StatusCode(500, e);
            }
        }
    }
}