using System;
using System.Collections.Generic;

namespace StudiBase.Shared.Contracts.Paging
{
 public class PagedResult<T>
 {
 public int Total { get; set; }
 public List<T> Items { get; set; } = new();
 public int Page { get; set; }
 public int PageSize { get; set; }
 public int TotalPages => PageSize <=0 ?0 : (int)Math.Ceiling((double)Total / PageSize);
 }
}
