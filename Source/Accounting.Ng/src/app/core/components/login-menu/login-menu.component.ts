import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/services/authorize.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { MaterialModule } from "../../material.module";

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.scss']
})

export class LoginMenuComponent implements OnInit {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;

  constructor(private authorizeService: AuthorizeService) {
    this.isAuthenticated = new BehaviorSubject<boolean>(false);
    this.userName = new BehaviorSubject<string>('');
  }

  ngOnInit() {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getUser().pipe(map(u => u?.name || ''));
  }
}
