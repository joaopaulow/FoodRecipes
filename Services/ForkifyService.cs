using FoodRecipes.Interfaces;
using FoodRecipes.Models;
using System.Net;
using System.Text.Json;

namespace FoodRecipes.Services
{
    public class ForkifyService : IForkifyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ForkifyService> _logger;

        public ForkifyService(HttpClient httpClient, ILogger<ForkifyService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ForkifyResponse?> BuscarReceitasAsync(string prato)
        {
            try
            {
                var resposta = await _httpClient.GetAsync($"search?q={Uri.EscapeDataString(prato)}");                
                
                if (!resposta.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Erro na API Forkify. Status: {StatusCode} para o prato: {Prato}", 
                        resposta.StatusCode, prato);
                    
                    if (resposta.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    
                    resposta.EnsureSuccessStatusCode();
                }

                var opcoes = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var resultado = await resposta.Content.ReadFromJsonAsync<ForkifyResponse>(opcoes);                
                
                if (resultado == null || resultado.Recipes == null || resultado.Count == 0)
                {
                    _logger.LogInformation("Nenhuma receita encontrada para o prato: {Prato}", prato);
                    return new ForkifyResponse 
                    { 
                        Count = 0, 
                        Recipes = new List<Receita>() 
                    };
                }

                return resultado;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro de conexão ao buscar receitas para o prato: {Prato}", prato);
                throw new Exception("Erro ao conectar com o serviço de receitas. Verifique sua conexão com a internet.", ex);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout ao buscar receitas para o prato: {Prato}", prato);
                throw new Exception("O serviço de receitas demorou muito para responder. Tente novamente.", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Erro ao processar resposta da API para o prato: {Prato}", prato);
                throw new Exception("Erro ao processar a resposta do serviço de receitas.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar receitas para o prato: {Prato}", prato);
                throw;
            }
        }
    }
}