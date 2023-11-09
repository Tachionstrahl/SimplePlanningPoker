import { TestBed } from '@angular/core/testing';

import { RoomApiService } from './room-api.service';
import { provideHttpClient } from '@angular/common/http';

describe('RoomApiService', () => {
  let service: RoomApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient()] 
    });
    service = TestBed.inject(RoomApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
