import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ProblemDetails } from '../../../core/problem-details';
import { ILineitem } from 'features/invoice/models/lineitem';

@Injectable()
export class LineitemService {
  private lineitemUrl: string = '/api/Lineitem/';

  constructor(private http: HttpClient) { }

  public getLineitem()
    : Observable<ILineitem>
  {
    return this.http.get<ILineitem>(this.lineitemUrl);
  }

  public addLineitem( name: string , description: string, invoiceId: number)
    : Observable<Object | ProblemDetails>
  {
    const body = {
      name: name,
      description: description,
      //TODO
      invoiceId: invoiceId
    };

    return this.http
    .post(this.lineitemUrl, body)
    .pipe(catchError(ProblemDetails.handleHttpError));
  }
}

 