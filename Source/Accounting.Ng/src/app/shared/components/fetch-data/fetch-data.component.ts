import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LogService, LogLevel } from '../../../core/services/log.service';
import { IWeatherForecast } from 'app/core/models/weatherforecast';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
})
export class FetchDataComponent {
  public forecasts: IWeatherForecast[] | undefined;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private logger: LogService) {
    http.get<IWeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(
      (result) => {
        this.forecasts = result;
      },
      (error) => this.logger.log(FetchDataComponent.name, LogLevel.Error, error)
    );
  }
}
