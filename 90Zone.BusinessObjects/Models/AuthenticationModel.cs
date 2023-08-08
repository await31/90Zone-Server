using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace _90Zone.BusinessObjects.Models {

    public class User : IdentityUser {

        //public string? ImgPath { get; set; }

        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }

    public class Response {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class AuthResult {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
    }
}