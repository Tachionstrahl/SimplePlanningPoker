import { Component } from '@angular/core';
import { IParticipant } from '../models/participant';
import { ToastService } from '../services/toast.service';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss'],
})
export class RoomComponent {
  selectedCard?: string;
  canReveal: boolean = true;
  public cards: string[] = [
    '0',
    '0.5',
    '1',
    '2',
    '3',
    '5',
    '8',
    '13',
    '20',
    '40',
    '100',
    '?',
    '☕',
  ]; //TODO: Move to service
  public participants: IParticipant[] = [
    { name: 'John', estimated: false },
    { name: 'Jane', estimated: true, estimation: '8' },
    { name: 'Jack', estimated: true },
  ]; //TODO: Move to service
  constructor(private toastService: ToastService) {}

  ngOnInit() {}

  copyLink() {
    let clipboard = navigator.clipboard;
    clipboard.writeText(window.location.href);
    this.toastService.add('Link copied!');
  }

  reveal() {
    throw new Error('Method not implemented.');
  }
  reset() {
    throw new Error('Method not implemented.');
  }

  select(cardValue: string) {
    console.log(cardValue);
    this.selectedCard = cardValue;
    
  }
}
