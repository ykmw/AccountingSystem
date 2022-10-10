import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from 'core/core.module';
import { MatCardModule } from '@angular/material/card';
import { NewCustomerComponent } from './components/newcustomer/newcustomer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AddressComponent } from './components/address/address.component';
import { TelInputComponent } from '../../shared/components/tel-input/tel-input.component';
import { HttpClientModule } from '@angular/common/http';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';

@NgModule({
  declarations: [NewCustomerComponent, AddressComponent, TelInputComponent],
  imports: [
    CommonModule,
    CoreModule,
    MatCardModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    HttpClientModule,
    FlexLayoutModule,
    FormsModule,
    MatSelectModule,
    MatInputModule,
  ],
  exports: [],
  providers: [],
})
export class CustomerModule {}
