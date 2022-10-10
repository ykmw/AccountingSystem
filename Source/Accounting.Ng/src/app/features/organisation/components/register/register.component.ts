import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { ProblemDetails } from 'core/problem-details';
import { MessageService } from 'core/services/message.service';
import { EventService } from 'core/services/event.service';

import { OrganisationService } from '../../services/organisation.service';
import { IOrganisation } from 'features/organisation/models/organisation';

@Component({
  selector: 'app-org-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  providers: [OrganisationService],
})
export class RegisterComponent {
  organisation: IOrganisation | any;
  hasUnitNumber: boolean = false;
  gstRegistered: boolean = false;
  countries = ['', 'New Zealand', 'USA', 'Germany', 'Italy', 'France'];

  registerForm = this.formBuilder.group({
    organisationName: ['', [Validators.required, Validators.maxLength(50)]],
    shortCode: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(4)]],
    gstNumber: ['', [Validators.minLength(8), Validators.maxLength(9)]],
    addressLine1: ['', [Validators.required, Validators.maxLength(50)]],
    addressLine2: ['', [Validators.maxLength(50)]],
    townCity: ['', [Validators.required, Validators.maxLength(50)]],
    country: [''],
    postCode: ['', [Validators.required, Validators.maxLength(10)]],
    phonePrefix: ['', [Validators.required, Validators.maxLength(4)]],
    phone: ['', [Validators.required, Validators.maxLength(9)]],
  });

  readonly page = 'My Organisation';

  constructor(
    private formBuilder: FormBuilder,
    private organisationService: OrganisationService,
    private messageService: MessageService,
    private eventService: EventService
  ) {
    this.eventService.pageChange(this.page);
    this.getMyOrganisation();
  }

  getMyOrganisation() {
    this.organisationService.getMyOrganisation().subscribe((data: IOrganisation) => {
      this.organisation = data;
    });
  }

  setGST(completed: boolean) {
    this.gstRegistered = completed;
  }

  onSubmit(event: Event) {
    event.preventDefault();

    const value = this.registerForm.value;

    this.organisationService
      .registerOrganisation(
        value.organisationName,
        value.shortCode,
        value.gstNumber,
        value.addressLine1,
        value.addressLien2,
        value.townCity,
        value.country,
        value.postCode,
        value.phonePrefix,
        value.phone
      )
      .subscribe(
        (_success: Object) =>
          this.messageService.info('Your organisation details have been updated.'),
        (error: ProblemDetails) => this.messageService.info(error.message)
      );
  }
}
