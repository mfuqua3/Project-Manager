export interface PagedList<T> {
    page: number;
    pageSize: number;
    pageCount: number;
    totalCount: number;
    itemCount: number;
    items: Array<T>;
}