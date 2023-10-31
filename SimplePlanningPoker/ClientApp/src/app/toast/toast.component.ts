import { Component, Input } from '@angular/core';
import { ToastService } from '../services/toast.service';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss']
})
export class ToastComponent {
  @Input() toast: any;

  constructor(private toastService: ToastService) {
    setTimeout(() => {
      this.remove();
    }, 3000);
   }

  // Remove the toast message
  remove() {
    this.toastService.remove(this.toast);
  }
}
