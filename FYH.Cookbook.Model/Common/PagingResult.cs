using System;
using System.Collections.Generic;

namespace FYH.Cookbook.Model.Common
{
    public class PagingResult<TResult>
    {
        /// <summary>
        /// Data total count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page data
        /// </summary>
        public IList<TResult> Data { get; set; }

        /// <summary>
        /// Query count
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Page total count
        /// </summary>
        public int TotalPage { get { return Count.HasValue && Count.Value > 0 ? (int)Math.Ceiling((double)TotalCount / Count.Value) : 1; } }
    }
}
