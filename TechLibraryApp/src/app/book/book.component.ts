import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { DashboardService } from '../dashboard/dashboard.service';
import { BooksResponse, EditBookResponse } from '../dashboard/dashboard.model';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css'],
  providers: [DashboardService]
})
export class BookComponent implements OnInit {
  bookId: string;
  bookData: BooksResponse = {
    bookId: undefined, title: undefined, thumbnailUrl: undefined,
    descr: undefined, isbn: undefined, publishedDate: undefined};
  constructor(
    private route: ActivatedRoute,
    private dashboardService: DashboardService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params: Params) => {
      this.bookId = params.bookid;
    });
    this.dashboardService.getBookbyId(this.bookId).subscribe((response: BooksResponse) => {
      this.mapBookResponseData(response);
    }, (error: string) => {
      this.toastr.error('Error while getting book data');
    });
  }

  onEditBookClick(): void {
    this.dashboardService.editBook(this.bookData).subscribe((response: EditBookResponse) => {
      if (!!response && response.status) {
        this.toastr.success('Book updated successfully');
      }
    }, (error: string) => {
      this.toastr.error('Error book update');
    });
  }

  onDashboardClick(): void {
    this.router.navigate(['dashboard']);
  }

  private mapBookResponseData(response: BooksResponse): void {
    this.bookData.title = response.title;
    this.bookData.isbn = response.isbn;
    this.bookData.bookId = response.bookId;
    this.bookData.publishedDate = response.publishedDate;
    this.bookData.descr = response.descr;
    this.bookData.thumbnailUrl = response.thumbnailUrl;
  }

}
