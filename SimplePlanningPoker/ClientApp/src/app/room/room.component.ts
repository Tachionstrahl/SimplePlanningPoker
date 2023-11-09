import { Component, effect } from '@angular/core';
import { ToastService } from '../services/toast.service';
import {
  ActivatedRoute,
} from '@angular/router';
import { RoomhubService } from '../services/roomhub.service';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss'],
})
export class RoomComponent {
  private roomId?: string;

  selectedCard?: string|null;
  
  constructor(
    private toastService: ToastService,
    private route: ActivatedRoute,
    public hub: RoomhubService
  ) {
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.roomId = params['roomid'];
      if (!this.roomId) throw new Error('No roomId provided');
      const username = localStorage.getItem('username');
      if (!username) throw new Error('No username stored');
      this.hub.connect().then(() => this.hub.join(this.roomId!, username));
    });
    this.hub.wasReset.subscribe(() => this.selectedCard = null);
    
  }

  copyLink() {
    let clipboard = navigator.clipboard;
    clipboard.writeText(window.location.href);
    this.toastService.add('Link copied!');
  }

  async reveal() {
    await this.hub.reveal();
  }
  async reset() {
    await this.hub.reset();
  }

  async select(cardValue: string) {
    await this.hub.estimate(cardValue);
    this.selectedCard = cardValue;
  }
}


