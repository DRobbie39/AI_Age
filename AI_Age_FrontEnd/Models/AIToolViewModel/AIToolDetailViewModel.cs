namespace AI_Age_FrontEnd.Models.AIToolViewModel
{
    public class AIToolDetailViewModel
    {
        public AIToolViewModel Tool { get; set; }
        public List<ArticleViewModel> Articles { get; set; }
        public List<VideoArticleViewModel> Videos { get; set; }

        public AIToolDetailViewModel()
        {
            Tool = new AIToolViewModel();
            Articles = new List<ArticleViewModel>();
            Videos = new List<VideoArticleViewModel>();
        }
    }
}
