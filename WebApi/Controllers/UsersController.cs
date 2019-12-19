using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.ApiRequests;
using WebApi.Configuration;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WarehouseContext _context;
        private readonly JwtOptions jwtOptions;
        private readonly TokenValidationParameters tokenValidationParameters;

        public UsersController(WarehouseContext context, JwtOptions jwtOptions, TokenValidationParameters validationParameters)
        {
            _context = context;
            this.jwtOptions = jwtOptions;
            tokenValidationParameters = validationParameters;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            user.Password = "";
            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Authenticate([FromBody]AuthenticateRequest request)
        {
            request.Password = EncryptionService.Encrypt(request.Password, jwtOptions.Secret);
            User user = _context.Users.FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);
            if (user == null)
            {
                return BadRequest(new AuthResponse
                {
                    Message = "Prisijungti nepavyko, patikrinti įvestį."
                });
            }
            return Ok(GenerateAuthentificationResultForUser(user));
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Refresh([FromBody]RefreshTokenRequest request)
        {
            AuthResponse response = RefreshToken(request.Token, request.RefreshToken);
            if (response.Message != "Authenticated")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.Role = "User";
            user.Password = EncryptionService.Encrypt(user.Password, jwtOptions.Secret);
            if (_context.Users.FirstOrDefault(x => x.Email == user.Email) != null)
            {
                return BadRequest(
                    new AuthResponse
                    {
                        Message = "Toks vartotojas jau užregistruotas"
                    }
                    );
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            user.Token = GenerateAuthentificationResultForUser(user).Token;
            user.Password = "";
            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
        private AuthResponse RefreshToken(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthResponse
                {
                    Message = "Invalid token",
                    Payload = token
                };
            }

            var expirationDate = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            DateTime expirationDateUtc = new DateTime(DateTime.UnixEpoch.Ticks, DateTimeKind.Utc).AddSeconds(expirationDate);

            if (expirationDateUtc > DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    Message = "This token hasn't expired yet.",
                    Payload = expirationDate
                };
            }
            string jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
            if (storedRefreshToken == null)
            {
                return new AuthResponse
                {
                    Message = "This refresh token doesn't exist",
                    Payload = refreshToken
                };
            }
            if (DateTime.UtcNow > storedRefreshToken.ExpirationDate)
            {
                return new AuthResponse
                {
                    Message = "This refresh token has expired",
                    Payload = storedRefreshToken.ExpirationDate
                };
            }
            if (storedRefreshToken.Invalidated)
            {
                return new AuthResponse
                {
                    Message = "This refresh token has been invalidated",
                    Payload = storedRefreshToken
                };
            }
            if (storedRefreshToken.Used)
            {
                return new AuthResponse
                {
                    Message = "This refresh token has been used already",
                    Payload = storedRefreshToken
                };
            }
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthResponse
                {
                    Message = "This refresh token doesn't match this Jwt",
                    Payload = storedRefreshToken.JwtId
                };
            }

            storedRefreshToken.Used = true;
            User user = _context.Users.SingleOrDefault(x => x.ID == int.Parse(validatedToken.Claims.Single(x => x.Type == "id").Value));
            _context.Entry(storedRefreshToken).State = EntityState.Modified;
            _context.SaveChanges();

            return GenerateAuthentificationResultForUser(user);
        }
        private AuthResponse GenerateAuthentificationResultForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("id", user.ID.ToString())
                }),
                Expires = DateTime.UtcNow.Add(jwtOptions.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserID = user.ID,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString()
            };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();
            return new AuthResponse
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                Message = "Authenticated"
            };
        }
        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = this.tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch (SecurityTokenValidationException ex)
            {
                throw ex;
            }
        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
