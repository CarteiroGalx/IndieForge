using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Context;

namespace IndieForge.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task Registrar()
        {
            
        }

        public async Task Login()
        {
            
        }

        public async Task StatusCheck()
        {
            
        }
    }
}