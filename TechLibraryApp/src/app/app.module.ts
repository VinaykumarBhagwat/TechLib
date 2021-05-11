import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BookComponent } from './book/book.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    BookComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      closeButton: true,
      positionClass: 'toast-top-right',
      progressBar: false,
      tapToDismiss: true,
      preventDuplicates: true,
      timeOut: 15000,
      maxOpened: 5,
      newestOnTop: true,
      autoDismiss: true
    }),
  ],
  providers: [ToastrService],
  bootstrap: [AppComponent]
})
export class AppModule { }
