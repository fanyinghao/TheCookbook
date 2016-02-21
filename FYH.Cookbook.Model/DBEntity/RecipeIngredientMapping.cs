﻿// ========================================
// This file is generated by T4, DO NOT MODIFY!
// 2016-02-18 23:56:42
// ========================================

using FYH.Cookbook.Model.Extensions;
using NHibernate.Mapping.Attributes;
using System;

namespace FYH.Cookbook.Model.DBEntity
{
    /// <summary>
    /// 
    /// </summary>
    [Class(0, Table = "RecipeIngredientMapping")]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class RecipeIngredientMapping
    {
        public const string TABLE_NAME = "RecipeIngredientMapping";
        public const string COL_MAPPINGID = "MappingId";
        public const string COL_RECIPEID = "RecipeId";
        public const string COL_INGREDIENTID = "IngredientId";
        public const string COL_UNIT = "Unit";
        public const string COL_QUANTITY = "Quantity";

        /// <summary>
        /// 
        /// </summary>
        [Id(0, Name = "MappingId", Column = "MappingId", TypeType = typeof(int))]
        [Key(1)]
        [Generator(2, Class = "native")]
        public virtual int? MappingId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Property(NotNull = true)]
        public virtual int RecipeId { get; set; }

        private Recipe _recipe = null;
        /// <summary>
        /// Foreign Key RecipeId Refer Recipe Entity Instance;
        /// </summary>
        [Core.Attributes.IgnoreConvertTypeScalar]
        public virtual Recipe Recipe { get { return _recipe ?? (_recipe = NHibernateExtension.GetEntityById<Recipe>(RecipeId)); } }

        /// <summary>
        /// 
        /// </summary>
        [Property(NotNull = true)]
        public virtual int IngredientId { get; set; }

        private Ingredient _ingredient = null;
        /// <summary>
        /// Foreign Key IngredientId Refer Ingredient Entity Instance;
        /// </summary>
        [Core.Attributes.IgnoreConvertTypeScalar]
        public virtual Ingredient Ingredient { get { return _ingredient ?? (_ingredient = NHibernateExtension.GetEntityById<Ingredient>(IngredientId)); } }

        /// <summary>
        /// 
        /// </summary>
        [Property(NotNull = true, Length = 50)]
        public virtual string Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Property(NotNull = true)]
        public virtual double Quantity { get; set; }
    }
}