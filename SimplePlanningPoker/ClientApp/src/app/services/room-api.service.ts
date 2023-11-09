import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomApiService {
  constructor(private http: HttpClient) {}

  createRoom(): Observable<string> {
    return this.http.get<string>('/api/room');
  }
}
