import {
  AfterContentChecked,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import {
  ControlValueAccessor,
  FormBuilder,
  FormControl,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  Validators,
} from '@angular/forms';
import { MatFormFieldControl } from '@angular/material/form-field';
import { Subscription } from 'rxjs';
import { Address } from '../../models/address';
import * as countryData from '../../countries.json';

declare var AddressFinder: any;

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.scss'],

  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: AddressComponent,
    },
    {
      provide: MatFormFieldControl,
      multi: true,
      useExisting: AddressComponent,
    },
    {
      provide: NG_VALIDATORS,
      multi: true,
      useExisting: AddressComponent,
    },
  ],
})
export class AddressComponent
  implements ControlValueAccessor, Validators, OnDestroy, OnInit, AfterContentChecked
{
  Country: string = 'New Zealand';

  countries: any = (countryData as any).default;

  imagePath: string = './assets/16x16/';
  tail: string = '.png';

  form!: FormGroup;
  subscriptions: Subscription[] = [];

  onTouched: any = () => {};
  onChange: any = () => {};
  changeDetector: any;

  get value(): Address {
    return this.form.value;
  }

  set value(value: Address) {
    this.form.setValue(value);
    this.onChange(value);
    this.onTouched();
  }

  constructor(private fb: FormBuilder, private ref: ChangeDetectorRef) {
    this.form = this.fb.group({
      addressLine1: [null, [Validators.required]],
      addressLine2: [null],
      addressLine3: [null, [Validators.required]],
      Country: [null, [Validators.required]],
      postCode: [null, [Validators.required]],
    });

    this.subscriptions.push(
      this.form.valueChanges.subscribe((value) => {
        this.onChange(value);
        this.onTouched();
      })
    );
  }

  ngOnInit() {
    let script = document.createElement('script');
    script.src = 'https://api.addressfinder.io/assets/v3/widget.js';
    script.async = true;
    script.onload = this.addressFinder;
    document.body.appendChild(script);
  }

  addressFinder() {
    let widget = new AddressFinder.Widget(document.getElementById('address'), 'API_KEY', 'NZ', {});

    widget.on('result:select', function (fullAddress: string, metaData: any) {
      var selected = new AddressFinder.NZSelectedAddress(fullAddress, metaData);

      var addressLine1 = <HTMLInputElement>document.getElementById('addressLine1');
      var addressLine2 = <HTMLInputElement>document.getElementById('addressLine2');
      var addressLine3 = <HTMLInputElement>document.getElementById('addressLine3');
      var postCode = <HTMLInputElement>document.getElementById('postCode');

      addressLine1.value = selected.address_line_1();
      addressLine2.value = selected.suburb();
      addressLine3.value = selected.city();
      postCode.value = selected.postcode();
    });
  }

  ngAfterContentChecked(): void {
    this.ref.detectChanges();
  }

  setDisabledState(disabled: boolean) {
    if (disabled) {
      this.form.disable();
    } else {
      this.form.enable();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach((s) => s.unsubscribe());
  }

  writeValue(value: any) {
    if (value) {
      this.value = value;
    }

    if (value === null) {
      this.form.reset();
    }
  }
  registerOnChange(fn: any) {
    this.onChange = fn;
  }
  registerOnTouched(fn: any) {
    this.onTouched = fn;
  }

  public checkError = (controlName: string, errorName: string) => {
    return this.form.controls[controlName].hasError(errorName);
  };

  get addressLine1Control() {
    return this.form.controls.addressLine1;
  }

  validate(_: FormControl) {
    return this.form.valid ? null : { address: { valid: false } };
  }
}
