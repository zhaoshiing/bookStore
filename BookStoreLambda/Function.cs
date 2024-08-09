using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using BookStore.Services;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BookStoreLambda
{
    public class Function
    {
        private readonly IUserService _userService;
        private readonly ILogger<Function> _logger;
        private readonly TokenService _tokenService;

        public Function(IUserService userService, ILogger<Function> logger, TokenService tokenService)
        {
            _userService = userService;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<APIGatewayProxyResponse> Authenticate(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var loginRequest = JsonConvert.DeserializeObject<LoginRequest>(request.Body);
                var user = _userService.Authenticate(loginRequest.Username, loginRequest.Password);

                if (user == null)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 401,
                        Body = "Unauthorized"
                    };
                }
                var token = _tokenService.GenerateToken(user);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(new { Token = token })
                };
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error: {ex.Message}");
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = "Internal Server Error"
                };
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}