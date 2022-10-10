import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ProblemDetails } from '../../../core/problem-details';
import { IOrganisation } from '../models/organisation';

@Injectable()
export class OrganisationService {
  private registerUrl: string = '/api/Organisation/';

  constructor(private http: HttpClient) { }

  public getMyOrganisation()
    : Observable<IOrganisation>
  {
    return this.http.get<IOrganisation>(this.registerUrl);
  }

  public registerOrganisation(organisationName: string, shortCode: string, gstNumber: string, addressLine1: string, addressLine2: string, townCity: string, country: string, postCode: string, phonePrefix: string, phone: string)
    : Observable<Object | ProblemDetails>
  {
    const body = {
      name: organisationName,
      shortCode: shortCode,
      gstNumber: gstNumber,
      addressStreet1: addressLine1,
      addressStreet2: addressLine2,
      addressCityTown: townCity,
      country: country,
      postCode: postCode,
      phonePrefix: phonePrefix,
      phone: phone
    };

    return this.http
      .put(this.registerUrl, body)
      .pipe(catchError(ProblemDetails.handleHttpError));
  }
}

 