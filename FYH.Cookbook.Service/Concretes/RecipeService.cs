using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.DAO.Abstracts;
using FYH.Cookbook.Model.DBEntity;
using FYH.Cookbook.Model.ViewModels;
using FYH.Cookbook.Service.Abstracts;
using NHibernate.Criterion;

namespace FYH.Cookbook.Service.Concretes
{
    public class RecipeService : IRecipeService
    {
        private IBaseRepository BaseRepository { get; set; }

        public RecipeInfoViewModel GetRecipeInfo(int recipeId)
        {
            var model = BaseRepository.GetEntityById<Recipe>(recipeId);
            var ingredientMappings = BaseRepository.GetEntityList<RecipeIngredientMapping>(new List<ICriterion>
            {
                Restrictions.Eq(RecipeIngredientMapping.COL_RECIPEID, recipeId)
            });
            var tagMappings = BaseRepository.GetEntityList<RecipeTagMapping>(new List<ICriterion>
            {
                Restrictions.Eq(RecipeTagMapping.COL_RECIPEID, recipeId)
            });
            var images = BaseRepository.GetEntityList<Image>(new List<ICriterion>
            {
                Restrictions.Eq(Image.COL_RECIPEID, recipeId)
            });
            return new RecipeInfoViewModel
            {
                RecipeId = model.RecipeId,
                Name = model.Name,
                Description = model.Description,
                Directions = model.Directions,
                Ingredients = ingredientMappings.Select(i => new IngredientInfo { IngredientId = i.IngredientId, Name = i.Ingredient.Name, Unit = i.Unit, Quantity = i.Quantity }).ToList(),
                Tags = tagMappings.Select(i => new TagInfo { TagId = i.TagId, Name = i.Tag.Name, Description = i.Tag.Description }).ToList(),
                Images = images.Select(i => new ImageInfo { Url = i.Url, Description = i.Description }).ToList()
            };
        }

        public void AddRecipe(RecipeInfoViewModel viewModel)
        {
            // Add recipe entity
            var model = new Recipe
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Directions = viewModel.Directions,
                CreatedDate = DateTime.Now
            };
            BaseRepository.AddOrUpdateEntity(model);
            viewModel.RecipeId = model.RecipeId;

            // Add ingredient mappings
            foreach (var ingredient in viewModel.Ingredients)
            {
                BaseRepository.AddEntity(new RecipeIngredientMapping
                {
                    IngredientId = ingredient.IngredientId,
                    RecipeId = viewModel.RecipeId.Value,
                    Unit = ingredient.Unit,
                    Quantity = ingredient.Quantity
                });
            }

            // Add tag mappings
            foreach (var id in viewModel.TagIds)
            {
                var mapping = new RecipeTagMapping
                {
                    TagId = id,
                    RecipeId = viewModel.RecipeId.Value
                };
                BaseRepository.AddEntity(mapping);
                viewModel.Tags.Add(new TagInfo
                {
                    TagId = id,
                    Name = mapping.Tag.Name,
                    Description = mapping.Tag.Name
                });
            }

            // Add images
            foreach (var image in viewModel.Images)
            {
                BaseRepository.AddEntity(new Image
                {
                    Url = image.Url,
                    Description = image.Description,
                    CreatedDate = DateTime.Now
                });
            }
        }

        public void UpdateRecipe(RecipeInfoViewModel viewModel)
        {
            var model = BaseRepository.GetEntityById<Recipe>(viewModel.RecipeId);
            if (model == null)
                return;
            model.Name = viewModel.Name;
            model.Description = viewModel.Description;
            model.Directions = viewModel.Directions;
            // Update recipe entity
            BaseRepository.UpdateEntity(model);
        }

        public void DeleteRecipe(int recipeId)
        {
            var model = BaseRepository.GetEntityById<Recipe>(recipeId);
            model.IsDeleted = true;
            BaseRepository.UpdateEntity(model);
        }
    }
}
