namespace FoodRecipes.Models
{
    public class ForkifyResponse
    {
        public int Count { get; set; }
        public List<Receita> Recipes { get; set; } = new();
    }
}