// https://angular.io/guide/styleguide#feature-modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { CoreModule } from "../../core/core.module";

import { InvoicesComponent } from './components/invoices/invoices.component';

@NgModule({
  declarations: [
    InvoicesComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CoreModule
],
  providers: []
})
export class InvoicesModule { }
