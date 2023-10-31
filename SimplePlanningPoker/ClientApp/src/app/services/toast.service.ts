import { Injectable, Signal, signal } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private _toasts: any[] = [];
  toasts: any = signal([]);

  // Add a toast message
  add(message: string, options: any = {}) {
    this._toasts.push({ message, ...options });
    this.toasts.set(this._toasts);
    console.log(this.toasts());
    
  }

  // Remove a toast message
  remove(toast: any) {
    this._toasts = this._toasts.filter((t) => t !== toast);
    this.toasts.set(this._toasts);    
  }
}
