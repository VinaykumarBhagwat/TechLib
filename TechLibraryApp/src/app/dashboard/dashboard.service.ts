import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BooksResponse, EditBookResponse } from './dashboard.model';

@Injectable()
export class DashboardService {

    private REST_API_SERVER = 'https://localhost:5001';

    constructor(private httpClient: HttpClient) {
    }

    public getAllBooks(): Observable<BooksResponse[]> {
        const resourceUrl = '/Books';
        const url: string = this.REST_API_SERVER + resourceUrl;
        return this.httpClient.get<BooksResponse[]>(url);
    }
    public getBookbyId(bookId: string): Observable<BooksResponse> {
        const resourceUrl = '/Books/' + bookId;
        const url: string = this.REST_API_SERVER + resourceUrl;
        return this.httpClient.get<BooksResponse>(url);
    }
    public getAllBooksPerPage(pageNumber: number): Observable<BooksResponse[]> {
        const resourceUrl = '/Books/all?PageNumber=' + pageNumber.toString();
        const url: string = this.REST_API_SERVER + resourceUrl;
        return this.httpClient.get<BooksResponse[]>(url);
    }
    public editBook(book: BooksResponse): Observable<EditBookResponse> {
        const resourceUrl = '/Books/edit';
        const url: string = this.REST_API_SERVER + resourceUrl;
        const requestBody = {
            BookId: book.bookId,
            Title: book.title,
            PublishedDate: book.publishedDate,
            ThumbnailUrl: book.thumbnailUrl,
            ShortDescr: book.descr
        };
        return this.httpClient.put<EditBookResponse>(url, requestBody);
    }
    public searchBook(title: string, description: string): Observable<BooksResponse[]> {
        const resourceUrl = '/Books/search';
        const url: string = this.REST_API_SERVER + resourceUrl;
        const requestBody = {
            Title: title,
            Description: description
        };
        return this.httpClient.post<BooksResponse[]>(url, requestBody);
    }
}
