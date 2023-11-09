import { TestBed } from '@angular/core/testing';

import { RoomhubService } from './roomhub.service';

describe('RoomhubService', () => {
  let service: RoomhubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoomhubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
