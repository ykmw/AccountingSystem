// https://angular.io/guide/styleguide#file-tree
// https://itnext.io/choosing-the-right-file-structure-for-angular-in-2020-and-beyond-a53a71f7eb05

import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ApiAuthorizationModule } from './api-authorization/api-authorization.module';
import { MaterialModule } from "./material.module";

import { NavComponent } from 'shared/components/nav/nav.component';
import { SpinnerComponent } from 'shared/components/spinner/spinner.component';

@NgModule({
  declarations: [
    NavComponent,
    SpinnerComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ApiAuthorizationModule,
    MaterialModule,
    RouterModule
  ],
  exports: [
    MaterialModule,
    NavComponent,
    SpinnerComponent
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule?: CoreModule) {
    if (parentModule) {
      throw new Error(
        'ApiAuthorizationModule is already loaded. Import it in the AppModule only');
    }
  }
 }
