﻿// ========================================
// This file is generated by T4, DO NOT MODIFY!
// 2016-02-17 13:33:08
// ========================================

using FYH.Cookbook.Model.Extensions;
using NHibernate.Mapping.Attributes;
using System;

namespace FYH.Cookbook.Model.DBEntity
{
    /// <summary>
    /// 
    /// </summary>
    [Class(0, Table = "ReceiptTagMapping")]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class ReceiptTagMapping
    {
        public const string TABLE_NAME = "ReceiptTagMapping";
        public const string COL_MAPPINGID = "MappingId";
        public const string COL_RECEIPTID = "ReceiptId";
        public const string COL_TAGID = "TagId";

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
        public virtual int ReceiptId { get; set; }

        private Receipt _receipt = null;
        /// <summary>
        /// Foreign Key ReceiptId Refer Receipt Entity Instance;
        /// </summary>
        [Core.Attributes.IgnoreConvertTypeScalar]
        public virtual Receipt Receipt { get { return _receipt ?? (_receipt = NHibernateExtension.GetEntityById<Receipt>(ReceiptId)); } }

        /// <summary>
        /// 
        /// </summary>
        [Property(NotNull = true)]
        public virtual int TagId { get; set; }

        private Tag _tag = null;
        /// <summary>
        /// Foreign Key TagId Refer Tag Entity Instance;
        /// </summary>
        [Core.Attributes.IgnoreConvertTypeScalar]
        public virtual Tag Tag { get { return _tag ?? (_tag = NHibernateExtension.GetEntityById<Tag>(TagId)); } }
    }
}
