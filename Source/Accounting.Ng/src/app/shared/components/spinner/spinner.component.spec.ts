import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { By } from '@angular/platform-browser';
import { Event, NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { SpinnerComponent } from './spinner.component';

let routerStub: Partial<Router>;
let eventSubject = new Subject<Event>();

routerStub = {
  events: eventSubject.asObservable()
}

const fakeEventId = 1;
const fakeEventUrl = "fakeUrl";

const navStartEvent = new NavigationStart(fakeEventId, fakeEventUrl);
const navEndEvent =new NavigationEnd(fakeEventId, fakeEventUrl, fakeEventUrl);
const navErrorEvent=new NavigationEnd(fakeEventId, fakeEventUrl, "fakeError");
const navCancelEvent = new NavigationCancel(fakeEventId, fakeEventUrl, "fakeReason");


describe('SpinnerComponent', () => {
  let component: SpinnerComponent;
  let fixture: ComponentFixture<SpinnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SpinnerComponent],
      imports: [MatProgressSpinnerModule],
      providers: [{ provide: Router, useValue: routerStub }]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpinnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should start spinning on nav start', () => {
    eventSubject.next(navStartEvent);
    fixture.detectChanges();

    expect(fixture.debugElement.query(By.css('.spinner-container'))).toBeTruthy();
  });

  it('should stop spinning on nav end', () => {
    eventSubject.next(navEndEvent);
    fixture.detectChanges();

    expect(fixture.debugElement.query(By.css('.spinner-container'))).toBeFalsy();
  });

  it('should stop spinning on nav error', () => {
    eventSubject.next(navErrorEvent);
    fixture.detectChanges();

    expect(fixture.debugElement.query(By.css('.spinner-container'))).toBeFalsy();
  });

  it('should stop spinning on nav cancel', () => {
    eventSubject.next(navCancelEvent);
    fixture.detectChanges();

    expect(fixture.debugElement.query(By.css('.spinner-container'))).toBeFalsy();
  });
});
