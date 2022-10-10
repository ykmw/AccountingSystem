import { Component, EventEmitter, Output} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { ProblemDetails } from 'core/problem-details';
import { MessageService } from 'core/services/message.service';
import { EventService } from 'core/services/event.service';

import { LineitemService } from '../../../invoices/services/lineitem.service';
import { ILineitem } from 'features/invoice/models/lineitem';

@Component({
  selector: 'app-invoice-lineitem',
  templateUrl: './lineitem.component.html',
  styleUrls: ['./lineitem.component.scss'],
  providers: [LineitemService]
})

export class LineitemComponent {
  lineitem: ILineitem | any;

  lineitemForm = this.fb.group({
    description: ['', [Validators.required, Validators.maxLength(50)]],
    quantity: ['', [Validators.required, Validators.maxLength(50)]],
    price: ['', [Validators.required, Validators.maxLength(50)]],
    total: ['', [Validators.required, Validators.maxLength(50)]]
    /*
      TODO: Link to new Invoice.
      invoiceId: ['', [Validators.required, Validators.maxLength(50)]]
    */
  })

  readonly page = 'Add Items';

  constructor(private fb: FormBuilder,
    private lineitemService: LineitemService,
    private messageService: MessageService,
    private eventService: EventService) {
      this.eventService.pageChange(this.page);
      //this.getLineitem();
  }

  
  getLineitem() {
    this.lineitemService.getLineitem()
      .subscribe(
        (data: ILineitem) => {
          this.lineitem = data;
        }
      )
  }
  
  onSubmit(event: Event) {
    event.preventDefault();

    const value = this.lineitemForm.value;
    /*
    this.lineitemService
      .addCustomer(value.name, false, value.contactName, value.contactEmail, value.addressLine1, value.addressLine2, value.addressLine3, value.country, value.postCode, value.phoneNumberPrefix, value.phoneNumber, 1 )
      .subscribe(
        (success: Object) => this.messageService.info("Customer details added."),
        (error: ProblemDetails) => this.messageService.info(error.message)
      );
    */
  }

}
