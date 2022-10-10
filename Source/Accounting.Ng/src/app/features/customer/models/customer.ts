import { MyTel } from './my-test';
import { Address } from './address';

export interface ICustomer {
  Name: string;
  IsGSTExempt: boolean;
  ContactName: string;
  ContactEmail: string;
  address: Address;
  phone: MyTel;
  InvoiceId: number;
}
