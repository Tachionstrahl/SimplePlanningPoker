import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RoomApiService } from '../services/room-api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private router: Router, private api: RoomApiService) {}

  createRoom() {
    var roomId = this.api
      .createRoom()
      .subscribe((roomId) => this.router.navigate(['/room', roomId]));
  }
}
