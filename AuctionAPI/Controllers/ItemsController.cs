using Microsoft.AspNetCore.Mvc;
using AuctionAPI.Data;
using AuctionAPI.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/items/listarTodositens
        [HttpGet("listarTodosItens")]
        public async Task<IActionResult> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }

        // GET: api/items/procuraPeloNome?name={name}
        [HttpGet("procuraPeloNome")]
        public async Task<IActionResult> GetItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Nome inválido.");
            }

            var items = await _context.Items
                                      .Where(i => i.Descricao.ToLower().Contains(name.ToLower()))
                                      .ToListAsync();

            if (items == null || !items.Any())
            {
                return NotFound("Nenhum item encontrado.");
            }

            return Ok(items);
        }

        // POST: api/items/adicionarNovoItem
        [HttpPost("adicionarNovoItem")]
        public async Task<IActionResult> PostItem([FromBody] Item item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Descricao))
            {
                return BadRequest("Dados do item inválidos.");
            }

            try
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetItem", new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

        // POST: api/items/procurarEFazerLance
        [HttpPost("procurarEFazerLance")]
        public async Task<IActionResult> PlaceBid([FromBody] PlaceBidRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || request.BidAmount <= 0)
            {
                return BadRequest("Dados do lance inválidos.");
            }

            var item = await _context.Items
                                     .Where(i => i.Descricao.ToLower().Contains(request.Name.ToLower()))
                                     .FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound("Item não encontrado.");
            }

            if (request.BidAmount <= item.LanceAtual)
            {
                return BadRequest("O lance deve ser maior que o lance atual.");
            }

            item.LanceAtual = request.BidAmount;

            try
            {
                _context.Items.Update(item);
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }
    }

    public class PlaceBidRequest
    {
        public string Name { get; set; }
        public decimal BidAmount { get; set; }
    }
}
