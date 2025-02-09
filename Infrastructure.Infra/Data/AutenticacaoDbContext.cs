using Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAutenticador
{
    public class AutenticacaoDbContext : IdentityDbContext<Usuario>
    {
        public AutenticacaoDbContext(DbContextOptions<AutenticacaoDbContext> options)
            : base(options) { }
    }
}
