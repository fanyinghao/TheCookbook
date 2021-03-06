﻿// ========================================
// This file is generated by T4, DO NOT MODIFY!
// 2016-02-21 18:08:34
// ========================================

using FYH.Cookbook.Model.Extensions;
using NHibernate.Mapping.Attributes;
using System;

namespace FYH.Cookbook.Model.DBEntity
{
    /// <summary>
    /// 
    /// </summary>
    [Class(0, Table = "Image")]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class Image
    {
        public const string TABLE_NAME = "Image";
        public const string COL_IMAGEID = "ImageId";
        public const string COL_URL = "Url";
        public const string COL_DESCRIPTION = "Description";
        public const string COL_RECIPEID = "RecipeId";
        public const string COL_CREATEDDATE = "CreatedDate";

        /// <summary>
        /// 
        /// </summary>
        [Id(0, Name = "ImageId", Column = "ImageId", TypeType = typeof(int))]
        [Key(1)]
        [Generator(2, Class = "native")]
        public virtual int? ImageId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Property(NotNull = true, Length = 2147483647)]
        public virtual string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Property(Length = 2147483647)]
        public virtual string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Property()]
        public virtual int? RecipeId { get; set; }

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
        public virtual DateTime CreatedDate { get; set; }
    }
}
