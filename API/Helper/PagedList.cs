using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helper {
    public class PagedList<T> : List<T> {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public PagedList (List<T> items, int count, int pageNumber, int PageSize) {
            this.TotalCount = count;
            this.PageSize = PageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int) System.Math.Ceiling (count / (double) PageSize);
            this.AddRange (items);

        }
        public static async Task<PagedList<T>> CreateAsync (IQueryable<T> source, int pageNumber, int pageSize) {
            if (source == null) {
                return null;
            }
            var count = await source.CountAsync ();
            var items = await source.Skip ((pageNumber - 1) * pageSize).Take (pageSize).ToListAsync ();
            return new PagedList<T> (items, count, pageNumber, pageSize);
        }

    }
    public class PaginationHeader {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public PaginationHeader (int currentPage, int itemsPerPage, int totalItems, int totalPages) {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
        }
    }
}