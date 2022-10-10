// https://angular.io/guide/styleguide#app-root-module

import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from '@angular/cdk/layout';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthorizeInterceptor } from './core/api-authorization/interceptor/authorize.interceptor';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';

import { InvoicesModule } from './features/invoices/invoices.module';
import { InvoiceModule } from './features/invoice/invoice.module';
import { OrganisationModule } from './features/organisation/organisation.module';


import { LogService } from "./core/services/log.service";
import { HomeComponent } from './core/components/home/home.component';
import { CustomerModule } from './features/customer/customer.module';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    LayoutModule,

    AppRoutingModule,
    CoreModule,
    
    InvoiceModule,
    InvoicesModule,
    OrganisationModule,
    CustomerModule
  
  ],
  providers: [ 
    LogService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
