import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss'],
})
export class CardComponent {
  @Input() selected: boolean = false;
  @Output() cardClicked = new EventEmitter<void>();
  
  constructor() {}

  ngOnInit() {}
}
