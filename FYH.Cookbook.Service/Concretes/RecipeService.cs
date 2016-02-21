using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.DAO.Abstracts;
using FYH.Cookbook.Model.CustomException;
using FYH.Cookbook.Model.DBEntity;
using FYH.Cookbook.Model.ViewModels;
using FYH.Cookbook.Service.Abstracts;
using NHibernate.Criterion;
using NHibernate.Util;

namespace FYH.Cookbook.Service.Concretes
{
    public class RecipeService : IRecipeService
    {
        private IBaseRepository BaseRepository { get; set; }

        public RecipeInfoViewModel GetRecipeInfo(int recipeId)
        {
            var model = BaseRepository.GetEntityById<Recipe>(recipeId);
            if (model == null || model.IsDeleted)
                throw new RecipeNotFoundException();

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
                throw new RecipeNotFoundException();
            model.Name = viewModel.Name;
            model.Description = viewModel.Description;
            model.Directions = viewModel.Directions;
            // Update recipe entity
            BaseRepository.UpdateEntity(model);

            // Update ingredient mapping
            var oldIngredients = BaseRepository.GetEntityList<RecipeIngredientMapping>(new List<ICriterion>
            {
                Restrictions.Eq(RecipeIngredientMapping.COL_RECIPEID, viewModel.RecipeId)
            });
            var oldIngredientIds = oldIngredients.Select(i => i.IngredientId);
            var newIngredientIds = viewModel.Ingredients.Select(i => i.IngredientId);
            oldIngredientIds.Where(i => !newIngredientIds.Contains(i)).ForEach(i =>
            {
                // delete old ingredient
                BaseRepository.DeleteEntity(oldIngredients.First(j => j.IngredientId == i));
            });
            foreach (var ingredient in viewModel.Ingredients)
            {
                var oldIngredient = oldIngredients.FirstOrDefault(i => i.IngredientId == ingredient.IngredientId);
                // update or add ingredient
                BaseRepository.AddOrUpdateEntity(new RecipeIngredientMapping
                {
                    MappingId = oldIngredient == null ? 0 : oldIngredient.MappingId,
                    IngredientId = ingredient.IngredientId,
                    RecipeId = viewModel.RecipeId.Value,
                    Quantity = ingredient.Quantity,
                    Unit = ingredient.Unit
                });
            }

            // Update tag mapping
            var oldTags = BaseRepository.GetEntityList<RecipeTagMapping>(new List<ICriterion>
            {
                Restrictions.Eq(RecipeTagMapping.COL_RECIPEID, viewModel.RecipeId)
            });
            var oldTagIds = oldTags.Select(i => i.TagId);
            var newTagIds = viewModel.TagIds;
            oldTagIds.Where(i => !newTagIds.Contains(i)).ForEach(i =>
            {
                // delete old tag
                BaseRepository.DeleteEntity(oldTags.First(j => j.TagId == i));
            });
            foreach (var tagId in viewModel.TagIds.Where(i => !oldTagIds.Contains(i)))
            {
                // add tag
                BaseRepository.AddOrUpdateEntity(new RecipeTagMapping
                {
                    TagId = tagId,
                    RecipeId = viewModel.RecipeId.Value
                });
            }

            // Update image mapping
            var oldImages = BaseRepository.GetEntityList<Image>(new List<ICriterion>
            {
                Restrictions.Eq(Image.COL_RECIPEID, viewModel.RecipeId)
            });
            var oldImageIds = oldImages.Select(i => i.ImageId.GetValueOrDefault());
            var newImageIds = viewModel.Images.Select(i => i.ImageId).ToList();
            oldImageIds.Where(i => !newImageIds.Contains(i)).ForEach(i =>
            {
                // delete old image
                BaseRepository.DeleteEntity(oldImages.First(j => j.ImageId == i));
            });
            foreach (var image in viewModel.Images)
            {
                // update image
                BaseRepository.AddOrUpdateEntity(new Image
                {
                    ImageId = image.ImageId,
                    RecipeId = viewModel.RecipeId.Value
                });
            }
        }

        public void DeleteRecipe(int recipeId)
        {
            var model = BaseRepository.GetEntityById<Recipe>(recipeId);
            if (model == null)
                throw new RecipeNotFoundException();
            model.IsDeleted = true;
            BaseRepository.UpdateEntity(model);
        }
    }
}
