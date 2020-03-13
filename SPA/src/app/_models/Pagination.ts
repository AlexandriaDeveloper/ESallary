/*CurrentPage":1,"ItemsPerPage":50,"TotalItems":6826,"TotalPages":137*/

export interface Pagination {
  CurrentPage: number;
  ItemsPerPage: number;
  TotalItems: number;
  totalPages: number;
  maxSize: number;
}
export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
