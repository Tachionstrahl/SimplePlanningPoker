import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParticipantComponent } from './participant.component';

describe('ParticipantComponent', () => {
  let component: ParticipantComponent;
  let fixture: ComponentFixture<ParticipantComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ParticipantComponent]
    });
    fixture = TestBed.createComponent(ParticipantComponent);
    component = fixture.componentInstance;
    component.value = {
      name: 'test',
      estimated: true,
      estimate: "0"
    }
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
