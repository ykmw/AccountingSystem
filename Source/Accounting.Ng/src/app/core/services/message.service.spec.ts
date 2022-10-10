import {TestBed, ComponentFixture} from '@angular/core/testing';
import {TestbedHarnessEnvironment} from '@angular/cdk/testing/testbed';
import {MatSnackBarHarness} from '@angular/material/snack-bar/testing';
import {HarnessLoader} from '@angular/cdk/testing';

import {MatSnackBarModule} from '@angular/material/snack-bar';
import {NoopAnimationsModule} from '@angular/platform-browser/animations';

import {MessageService} from './message.service';
import {MessageTestComponent} from './message-test.component';

describe('MessageService', () => {
  let fixture: ComponentFixture<MessageTestComponent>;
  let loader: HarnessLoader;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatSnackBarModule, NoopAnimationsModule],
      providers: [MessageService],
      declarations: [MessageTestComponent],
      teardown: {destroyAfterEach: true}
    }).compileComponents();

    fixture = TestBed.createComponent(MessageTestComponent);
    fixture.detectChanges();
    loader = TestbedHarnessEnvironment.documentRootLoader(fixture);
  });

  it('should display message in snack-bar', async () => {
    fixture.componentInstance.open('Test message.');
    let snackBar = await loader.getHarness(MatSnackBarHarness);
    expect(await snackBar.getMessage()).toBe('Test message.');
  });
});
