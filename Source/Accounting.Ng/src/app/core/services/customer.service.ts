import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ProblemDetails } from '../problem-details';
import { ICustomer } from 'features/customer/models/customer';

@Injectable()
export class CustomerService {
  private customerUrl: string = '/api/Customer/';

  constructor(private http: HttpClient) { }

  public getCustomer(invoiceId: string)
    : Observable<ICustomer>
  {
    const opts = { params: new HttpParams({fromString: "id=" + invoiceId}) };
    return this.http.get<ICustomer>(this.customerUrl + invoiceId, opts);
  }

  public addCustomer( name: string, isGSTExempt: boolean, contactName: string, contactEmail: string, addressLine1: string, addressLine2: string, addressLine3: string, country: string, postCode: string, phoneNumberPrefix: string, phoneNumber: string, invoiceId: number )
    : Observable<Object | ProblemDetails>
  {
    const body = {
      name: name,
      isGSTExempt: isGSTExempt,
      contactName: contactName,
      contactEmail: contactEmail,
      address: {
       addressLine1: addressLine1,
       addressLine2: addressLine2,
       addressLine3: addressLine3,
       country: country,
       postCode: postCode
      },
      phone: {
       phoneNumberPrefix: phoneNumberPrefix,
       phoneNumber: phoneNumber
      },
      invoiceId: invoiceId
    };

    return this.http
    .post(this.customerUrl, body)
    .pipe(catchError(ProblemDetails.handleHttpError));
  }
}

 