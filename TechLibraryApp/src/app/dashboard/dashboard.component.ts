import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { BooksResponse } from './dashboard.model';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  providers: [DashboardService]
})
export class DashboardComponent implements OnInit {
  public books: BooksResponse[] = [];
  public title: string;
  public description: string;
  public lastPageNumber;
  private totalNumberOfBooks: number;
  private currentPageNumber = 1;

  constructor(
    private dashboardService: DashboardService,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    // Improvement: I am calling getAllBooks to retrieve total number of books.
    // Can avoid this call by including total number of books count in the getAllBooksPerPage
    this.dashboardService.getAllBooks().subscribe((response: BooksResponse[]) => {
      this.totalNumberOfBooks = response.length;
      this.lastPageNumber = Math.floor(this.totalNumberOfBooks / 10) + 1;
    });
    this.dashboardService.getAllBooksPerPage(1).subscribe((response: BooksResponse[]) => {
      this.books = response;
    });
  }

  onViewOrEditClick(bookId: string): void {
    this.router.navigate(['book'], { queryParams: { bookid: bookId } });
  }

  onSearchClick(): void {
    // search based on title or description or both
    if (!this.title && !this.description) {
      this.toastr.error('Please enter title or description or both');
    }
    this.dashboardService.searchBook(this.title, this.description).subscribe((response: BooksResponse[]) => {
      if (response) {
        this.books = response;
      }
      this.toastr.success('Search book successfull');
    });
  }

  onPageClick(page?: number): void {
    this.currentPageNumber = !!page ? page : this.currentPageNumber;
    this.dashboardService.getAllBooksPerPage(this.currentPageNumber).subscribe((response: BooksResponse[]) => {
      if (response) {
        // better to do deep copy using loadash
        this.books = response;
      }
    }, (error: string) => {
      this.toastr.error('Error occured while fetching page data');
    });
  }

  onPreviousPageClick(): void {
    if (this.currentPageNumber === 1) {
      return;
    }
    this.currentPageNumber--;
    this.onPageClick();
  }

  onNextPageClick(): void {
    if (this.currentPageNumber >= this.lastPageNumber) {
      return;
    }
    this.currentPageNumber++;
    this.onPageClick();
  }

  onLastPageClick(): void {
    this.currentPageNumber = this.lastPageNumber;
    this.onPageClick();
  }
}
