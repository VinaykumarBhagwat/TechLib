export interface BooksResponse {
    bookId: string;
    title: string;
    isbn: string;
    publishedDate: string;
    thumbnailUrl: string;
    descr: string;
}
export interface EditBookResponse {
    status: boolean;
    error: string;
}
