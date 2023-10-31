import { Component, Input } from '@angular/core';
import { IParticipant } from '../models/participant';

@Component({
  selector: 'app-participant',
  templateUrl: './participant.component.html',
  styleUrls: ['./participant.component.scss']
})
export class ParticipantComponent {
    @Input() public value!: IParticipant;
    constructor() { }
    ngOnInit() {
    }
}
