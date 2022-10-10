// https://angular.io/guide/styleguide#feature-modules

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CoreModule } from "../../core/core.module";

import { RegisterComponent } from './components/register/register.component';

@NgModule({ 
  declarations: [
    RegisterComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    CoreModule
  ]
})
export class OrganisationModule { }
