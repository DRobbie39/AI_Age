namespace AI_Age_FrontEnd.Models.AIToolViewModel
{
    public class HomeViewModel
    {
        public List<AIToolCategoryViewModel> Categories { get; set; }
        public List<AIToolViewModel> AITools { get; set; }

        public HomeViewModel()
        {
            Categories = new List<AIToolCategoryViewModel>();
            AITools = new List<AIToolViewModel>();
        }
    }
}
