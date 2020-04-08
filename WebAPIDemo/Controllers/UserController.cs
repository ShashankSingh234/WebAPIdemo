using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using WebAPIDemo.Models;
using Microsoft.Extensions.Configuration;
using WebAPIDemo.Context;
using WebAPIDemo.Healpers;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(IConfiguration configuration, DatabaseContext context)
        {
            Configuration = configuration;
            _context = context;
        }

        public IConfiguration Configuration { get; }

        private readonly DatabaseContext _context;

        /// <summary>
        /// Register a new User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody]User user)
        {
            if(!Validation.IsValidEmail(user.Email))
            {
                return BadRequest("Enter valid email.");
            }

            if(string.IsNullOrEmpty(user.Password) || user.Password.Trim().Length < 6)
            {
                return BadRequest("Password not valid. Password should be 6 character in length.");
            }

            if (_context.Users.Where(x => x.Email.Equals(user.Email)).FirstOrDefault() != null)
                return BadRequest("User already registered.");

            string encrptedPassword = PasswordHelper.Encrypt(user.Password);
            user.Password = encrptedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully.");
        }

        /// <summary>
        /// Authenticate already registered user.
        /// </summary>
        /// <param name="authenticateModel"></param>
        /// <returns>Token on successfull authentication.</returns>
        /// <response code="200">User authenticated successfully. Generated token will be returned.</response>
        /// <response code="400">Invalid username or password.</response>  
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody]AuthenticateModel authenticateModel)
        {
            if (!Validation.IsValidEmail(authenticateModel.Email))
            {
                return BadRequest("Enter valid email.");
            }

            if (string.IsNullOrEmpty(authenticateModel.Password) || authenticateModel.Password.Trim().Length < 6)
            {
                return BadRequest("Password not valid. Password should be 6 character in length.");
            }

            User user = _context.Users.Where(x => x.Email.Equals(authenticateModel.Email)).FirstOrDefault();


            if (user != null)
            {
                string decryptedPassword = PasswordHelper.Decrypt(user.Password);
                if (decryptedPassword.Equals(authenticateModel.Password))
                {
                    var token = GenerateToken(user);
                    return Ok(token);
                }
            }

            return BadRequest("Invalid email or password.");
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("firstName", user.FirstName));
            permClaims.Add(new Claim("lastName", user.LastName));
            permClaims.Add(new Claim("email", user.Email));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"], //Issure    
                            Configuration["Jwt:Issuer"],  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }

        //[AllowAnonymous]
        //[HttpPost("CheckTokenValidity")]
        //public String CheckTokenValidity()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var identity = User.Identity as ClaimsIdentity;
        //        if (identity != null)
        //        {
        //            IEnumerable<Claim> claims = identity.Claims;
        //        }
        //        return "Valid";
        //    }
        //    else
        //    {
        //        return "Invalid";
        //    }
        //}

            /// <summary>
            /// List of all registered user.
            /// </summary>
            /// <returns></returns>
        [HttpGet("GetUsersList")]
        public IActionResult GetUsersList()
        {
            return Ok(_context.Users.Select(x => new UserResponseDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList());
        }

    }
}