import { HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";

export class ProblemDetails {
  static defaultError = new ProblemDetails("An error occurrred");
  static clientOrNetworkError = new ProblemDetails("An error occurred.");
  static notAuthenticated = new ProblemDetails("Unable to access resource. Please re-login and try again.", 401);
  static serverError = new ProblemDetails("A server error occurred.", 500);
  static notFound = new ProblemDetails("The resource was not found.", 404);

  private static fromErrorObject(data: any): ProblemDetails {
    let problemDetails = Object.assign(new ProblemDetails(""), data);

    return problemDetails;
  }

  static handleHttpError(err: HttpErrorResponse): Observable<ProblemDetails> {
    if (err.error instanceof Error)
      return throwError(ProblemDetails.clientOrNetworkError);

    if (err.status == 401)
      return throwError(ProblemDetails.notAuthenticated);

    let problemDetails = ProblemDetails.defaultError;

    const isClientError = err.status >= 400 && err.status < 500;
    const isServerError = err.status >= 500 && err.status < 600;

    if (isServerError)
      problemDetails = ProblemDetails.serverError;

    if (isClientError)
      problemDetails = ProblemDetails.fromErrorObject(err.error);

    // override with more friendly error message
    if (err.status == 404)
      problemDetails = ProblemDetails.notFound;

    return throwError(problemDetails);
  }

  title: string = "";
  detail: string = "";
  status: number = 0;

  constructor(message: string, status: number = 0) {
    this.title = message;
    this.status = status;
  }

  get message(): string {
    return (this.detail === "")
      ? this.title : this.detail;
  }
}

