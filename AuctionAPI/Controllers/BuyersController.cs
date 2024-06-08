using Microsoft.AspNetCore.Mvc;
using AuctionAPI.Data;
using Microsoft.EntityFrameworkCore;
using AuctionAPI.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BuyersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/buyers/listarTodosCompradores
        [HttpGet("listarTodosCompradores")]
        public async Task<IActionResult> GetAllBuyers()
        {
            var buyers = await _context.Buyers.ToListAsync();
            return Ok(buyers);
        }

        // GET: api/buyers/procurePeloNome/{name}
        [HttpGet("procurePeloNome")]
        public async Task<IActionResult> GetBuyerByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Nome inválido.");
            }

            var buyer = await _context.Buyers
                                      .Where(b => b.Name.ToLower() == name.ToLower())
                                      .FirstOrDefaultAsync();

            if (buyer == null)
            {
                return Ok("Comprador não encontrado.");
            }

            return Ok(buyer);
        }

        // POST: api/buyers/adicionarCompradores
        [HttpPost("adicionarCompradores")]
        public async Task<IActionResult> PostBuyers([FromBody] List<Buyer> buyers)
        {
            if (buyers == null || !buyers.Any())
            {
                return BadRequest("Lista de compradores inválida.");
            }

            if (buyers.Any(b => string.IsNullOrWhiteSpace(b.Name)))
            {
                return BadRequest("Todos os compradores devem ter um nome válido.");
            }

            try
            {
                _context.Buyers.AddRange(buyers);
                await _context.SaveChangesAsync();
                return Ok(buyers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }
    }
}
