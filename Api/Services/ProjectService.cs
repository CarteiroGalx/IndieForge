using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Context;
using Microsoft.EntityFrameworkCore;

namespace IndieForge.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CriarProjeto()
        {
            
        }

        public async Task VerProjetos()
        {
            
        }

        public async Task VerProjetoDetalhes()
        {
            
        }

        public async Task EditarProjeto()
        {
            
        }

        public async Task PublicarProjeto()
        {
            
        }

        public async Task CancelarProjeto()
        {
            
        }

        public async Task VerApoiadores()
        {
            
        }
    }
}