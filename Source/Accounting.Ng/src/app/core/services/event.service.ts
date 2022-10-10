import { Injectable, EventEmitter } from '@angular/core';

@Injectable({
    providedIn: 'root'
  })
export class EventService{
public onChange: EventEmitter<PageServiceEvent> = new EventEmitter<PageServiceEvent>();

public pageChange(page: string) {
    this.onChange.emit({page: page});
}
}

export class PageServiceEvent {
    page=" ";
    }

