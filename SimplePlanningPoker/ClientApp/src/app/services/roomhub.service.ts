import { EventEmitter, Injectable, effect, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { IRoomState } from '../models/room-state';

@Injectable({
  providedIn: 'root',
})
export class RoomhubService {
  private connection: signalR.HubConnection;

  public roomState = signal<Partial<IRoomState>>({});
  public wasReset = new EventEmitter<void>();

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/roomhub')
      .build();
  }

  async connect() {
    if (this.connection.state == signalR.HubConnectionState.Connected)
      await this.connection.stop();
    await this.connection.start().then(() => {
      console.log('connected');
    });

    this.connection.on('SendRoomState', (roomState: IRoomState) => {
      this.roomState.set(roomState);
    });
    this.connection.on('Reset', () => {
      this.wasReset.emit();
    });
    
  }

  async join(roomId: string, username: string) {
    return this.connection.send('Join', roomId, username);
  }

  async estimate(estimate: string) {
    return this.connection.send("Estimate", estimate);
  }

  async reveal() {
    return this.connection.send("Reveal");
  }

  async reset() {
    return this.connection.send("Reset");
  }
}
