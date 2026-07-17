using IndieForge.Models;

namespace IndieForge.DTOs
{
    public class LoginResponseDto
    {
        public string Message { get; set; }

        public LoginResponseDto(string message)
        {
            Message = message;
        }
    }
}
