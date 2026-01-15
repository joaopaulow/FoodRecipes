namespace FoodRecipes.Models
{
    public class Receita
    {
        public string Publisher { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Source_Url { get; set; } = string.Empty;
        public string Recipe_Id { get; set; } = string.Empty;
        public string Image_Url { get; set; } = string.Empty;
        public double Social_Rank { get; set; }
        public string Publisher_Url { get; set; } = string.Empty;
    }
}