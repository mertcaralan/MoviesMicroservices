using CORE.Authentication.Models;
using CORE.Authentication.Services;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Users.APP.Domain;

namespace Users.APP.Features.Authentication
{
    public class RefreshTokenRequest : JwtRefreshRequest,
        IRequest<JwtResponse>
    {
    }

    public class RefreshTokenHandler : DbService<User>,
        IRequestHandler<RefreshTokenRequest, JwtResponse>
    {
        private readonly IJwtAuthService _jwtAuthService;

        public RefreshTokenHandler(DbContext db,
            IJwtAuthService jwtAuthService) : base(db)
        {
            _jwtAuthService = jwtAuthService;
        }

        protected override IQueryable<User> DbQuery()
        {
            return base.DbQuery()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role);
        }

        public async Task<JwtResponse> Handle(RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            IEnumerable<Claim> claims;
            try
            {
                claims = _jwtAuthService.GetClaims(request.Jwt, request.SecurityKey);
            }
            catch (Exception exception) when (exception is SecurityTokenException
                || exception is ArgumentException)
            {
                return null;
            }
            if (!int.TryParse(claims?.FirstOrDefault(c => c.Type == "Id")?.Value, out var userId)
                || claims.FirstOrDefault(c => c.Type == "iss")?.Value != request.Issuer
                || claims.FirstOrDefault(c => c.Type == "aud")?.Value != request.Audience)
                return null;

            var userEntity = await DbSingleAsync(user =>
                user.Id == userId && user.StatusId == 1 &&
                user.RefreshToken == request.RefreshToken &&
                user.RefreshTokenExpiration >= DateTime.Now,
                cancellationToken);

            if (userEntity is null)
                return null;

            userEntity.RefreshToken = _jwtAuthService.GetRefreshToken();

            await DbUpdateAsync(userEntity, cancellationToken);

            var expiration = DateTime.Now.AddMinutes(5);
            return _jwtAuthService.GetJwtResponse(userEntity.Id,
                userEntity.UserName,
                userEntity.UserRoles.Select(ur => ur.Role.Name),
                expiration, request.SecurityKey, request.Issuer,
                request.Audience, userEntity.RefreshToken);
        }
    }
}
