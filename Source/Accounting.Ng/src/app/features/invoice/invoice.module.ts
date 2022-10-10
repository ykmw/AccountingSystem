// https://angular.io/guide/styleguide#feature-modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from "../../core/core.module";

import { StepperComponent } from "./components/stepper/stepper.component";
import { CustomerComponent } from './components/customer/customer.component';
import { LineitemComponent } from './components/item/lineitem.component';


@NgModule({
  declarations: [
    CustomerComponent,
    LineitemComponent,
    StepperComponent    
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CoreModule
  ]
})
export class InvoiceModule { }
