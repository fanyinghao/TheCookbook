namespace FYH.Cookbook.Model.CustomException
{
    public class RecipeNotFoundException: CbBaseException
    {
        public override string Message
        {
            get { return "Recipe is not exist."; }
        }
    }
}
