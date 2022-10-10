// https://angular.io/guide/styleguide#shared-feature-module

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FetchDataComponent } from './components/fetch-data/fetch-data.component';
import { CounterComponent } from './components/counter/counter.component';

@NgModule({
  declarations: [
    FetchDataComponent,
    CounterComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    FetchDataComponent,
    CounterComponent
  ]
})
export class SharedModule { }
