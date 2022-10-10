import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { ProblemDetails } from 'core/problem-details';
import { MessageService } from 'core/services/message.service';
import { EventService } from 'core/services/event.service';

import { CustomerService } from '../../../../core/services/customer.service';
import { ICustomer } from 'features/customer/models/icustomer';

@Component({
  selector: 'app-invoice-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss'],
  providers: [CustomerService],
})
export class CustomerComponent {
  customer: ICustomer | any;
  invoiceId = '2';
  customerForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(50)]],
    contactName: ['', [Validators.required, Validators.maxLength(50)]],
    contactEmail: ['', [Validators.required, Validators.maxLength(50)]],
    isGSTExempt: ['', [Validators.required, Validators.maxLength(50)]],
    addressLine1: ['', [Validators.required, Validators.maxLength(50)]],
    addressLine2: ['', [Validators.maxLength(50)]],
    addressLine3: ['', [Validators.required, Validators.maxLength(50)]],
    country: ['', [Validators.maxLength(50)]],
    postCode: ['', [Validators.required, Validators.maxLength(50)]],
    phoneNumberPrefix: ['', [Validators.required, Validators.maxLength(50)]],
    phoneNumber: ['', [Validators.required, Validators.maxLength(50)]],
    //invoiceId: ['', [Validators.required]]
  });

  readonly page = 'Add Customer';

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private messageService: MessageService,
    private eventService: EventService
  ) {
    this.eventService.pageChange(this.page);
  }

  getCustomer() {
    this.customerService.getCustomer(this.invoiceId).subscribe((data) => {
      this.customer = data;
    });
  }

  onSubmit(event: Event) {
    event.preventDefault();

    const value = this.customerForm.value;

    this.customerService
      .addCustomer(
        value.name,
        false,
        value.contactName,
        value.contactEmail,
        value.addressLine1,
        value.addressLine2,
        value.addressLine3,
        value.country,
        value.postCode,
        value.phoneNumberPrefix,
        value.phoneNumber,
        1
      )
      .subscribe(
        (success: Object) => this.messageService.info('Customer details added.'),
        (error: ProblemDetails) => this.messageService.info(error.message)
      );
  }
}
