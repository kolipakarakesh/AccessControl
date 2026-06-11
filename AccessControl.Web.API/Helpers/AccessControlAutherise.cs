using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace AccessControl.Web.API.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AccessControlAutherise : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null)
            {
                Microsoft.Extensions.Primitives.StringValues authTokens;

                context.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);

                var _token = authTokens.FirstOrDefault();


                string _jwtToken = _token;

                if (_jwtToken != null)
                {
                    string authToken = _jwtToken;

                    if (authToken != null)
                    {
                        if (IsValidToken(authToken))
                        {
                            var handler = new JwtSecurityTokenHandler();
                            var jwtToken = handler.ReadJwtToken(authToken);

                            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                            context.HttpContext.User = new ClaimsPrincipal(identity);

                            context.HttpContext.Response.Headers.Add("Authorization", authToken);
                            context.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                            context.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");
                            return;
                        }
                        else
                        {
                            context.HttpContext.Response.Headers.Add("Authorization", authToken);
                            context.HttpContext.Response.Headers.Add("AuthStatus", "UnAuthorized");
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                            context.Result = new JsonResult("NotAuthorized")
                            {
                                Value = new
                                {
                                    Status = "Error",
                                    Message = "Invalid Token"
                                },
                            };
                        }
                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                        context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide Authorization";
                        context.Result = new JsonResult("Please Provide Authorization")
                        {
                            Value = new
                            {
                                Status = "Error",
                                Message = "Please Provide authToken"
                            },
                        };
                    }
                }
                else
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide Authorization";
                    context.Result = new JsonResult("Please Provide Authorization")
                    {
                        Value = new
                        {
                            Status = "Error",
                            Message = "Please Provide authToken"
                        },
                    };
                }

            }
        }

        public bool IsValidToken(string authToken)
        {
            return CheckTokenIsValid(authToken);
        }

        public bool CheckTokenIsValid(string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);

            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }

        public long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }
    }
}
