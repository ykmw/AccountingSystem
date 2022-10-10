import { Component } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { MessageService } from './message.service';

@Component({
  selector: 'acc-message-test',
  template: `
    <p>
      message-test works!
    </p>
  `,
  styles: [
  ]
})
export class MessageTestComponent {

  constructor(readonly snackBar: MatSnackBar,
    readonly messageService: MessageService) {
   }

   open(message: string, action = '', config?: MatSnackBarConfig) {
    return this.snackBar.open(message, action, config);
  }

}
