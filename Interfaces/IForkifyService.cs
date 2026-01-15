using FoodRecipes.Models;

namespace FoodRecipes.Interfaces
{
    public interface IForkifyService
    {
        Task<ForkifyResponse?> BuscarReceitasAsync(string prato);
    }
}