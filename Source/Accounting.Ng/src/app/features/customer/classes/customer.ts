import { Address } from '../models/address';
import { ICustomer } from '../models/customer';
import { MyTel } from '../models/my-test';

export class Customer implements ICustomer {
  Name!: string;
  IsGSTExempt!: boolean;
  ContactName!: string;
  ContactEmail!: string;
  address!: Address;
  phone!: MyTel;
  InvoiceId!: number;

  constructor(
    name: string,
    isGSTEx: boolean,
    contName: string,
    contEmail: string,
    address: Address,
    phone: MyTel,
    invoice: number
  ) {
    (this.Name = name),
      (this.IsGSTExempt = isGSTEx),
      (this.ContactName = contName),
      (this.ContactEmail = contEmail),
      (this.address = address),
      (this.phone = phone),
      (this.InvoiceId = invoice);
  }
}
