using FoodRecipes.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodRecipes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceitasController : ControllerBase
    {
        private readonly IForkifyService _forkifyService;
        private readonly ILogger<ReceitasController> _logger;

        public ReceitasController(IForkifyService forkifyService, ILogger<ReceitasController> logger)
        {
            _forkifyService = forkifyService;
            _logger = logger;
        }
      
        [HttpGet("buscarReceitas")]
        public async Task<IActionResult> BuscarReceitas([FromQuery] string prato)
        {
            if (string.IsNullOrWhiteSpace(prato))
            {
                return BadRequest(new { mensagem = "O parâmetro 'prato' é obrigatório." });
            }

            try
            {
                var resultado = await _forkifyService.BuscarReceitasAsync(prato);
                
                if (resultado == null || resultado.Count == 0)
                {
                    return NotFound(new { mensagem = "Nenhuma receita encontrada para o prato informado." });
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar receitas");
                return StatusCode(500, new { mensagem = "Erro ao buscar receitas. Tente novamente mais tarde." });
            }
        }
    }
}