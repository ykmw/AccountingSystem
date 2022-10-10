import { Component, OnInit, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { EventService } from 'core/services/event.service';
import { PageServiceEvent } from 'core/services/event.service';
import { LogService, LogLevel } from 'core/services/log.service';
import { AuthorizeService } from 'core/api-authorization/services/authorize.service';
import { MatDrawer } from '@angular/material/sidenav';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit {
  isAuthenticated!: boolean;
  isHandset!: Observable<boolean>;
  page = 'Registration';
  subscription: any;

  @ViewChild('drawer') public drawer: MatDrawer | undefined;

  constructor(
    private service: EventService,
    private breakpointObserver: BreakpointObserver,
    private logger: LogService,
    private authorizeService: AuthorizeService
  ) {}

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe((value) => {
      this.isAuthenticated = value;
    });
    this.service.onChange.subscribe({
      next: (event: PageServiceEvent) => {
        this.page = event.page;
        this.logger.log(NavComponent.name, LogLevel.Information, event.page);
      },
    });
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay()
  );

  drawertoggle() {
    if (this.drawer) {
      this.drawer.toggle();
    }
  }
}
