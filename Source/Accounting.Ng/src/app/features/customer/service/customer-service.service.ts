import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Customer } from '../classes/customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerServiceService {
  REST_API: string = 'https://localhost:5001';

  httpHeaders = new HttpHeaders().set('Content-Type', 'application/json');

  constructor(private httpClient: HttpClient) { }

  public addCustomer(data: Customer){ 
    //return JSON.stringify(data)
    let API_URL = `${this.REST_API}/api/Customer`;
    return this.httpClient.post(API_URL, data)
        .pipe(
        catchError(this.handleError)
      )   
  }


  handleError(error: HttpErrorResponse){
    let errorMessage = '';
    if(error.error instanceof ErrorEvent){
      errorMessage = error.error.message;
    }else{
      errorMessage = `Error Code: ${error.status}`
    }

    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
