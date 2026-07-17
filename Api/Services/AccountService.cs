using IndieForge.Context;

namespace IndieForge.Services
{
    public class AccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CriarConta()
        {
            
        }

        public async Task VerConta()
        {
            
        }
        
        public async Task VerContribuicoes()
        {
            
        }
    }
}