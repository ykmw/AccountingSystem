import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  constructor(private snackBar: MatSnackBar) { }

  public info(message: string) {
    this.showMessage(message);
  }

  private showMessage(message: string)
  {
      this.snackBar.open(message, 'Dismiss', {
        horizontalPosition: 'center',
        verticalPosition: 'top'
      });
  }
}

