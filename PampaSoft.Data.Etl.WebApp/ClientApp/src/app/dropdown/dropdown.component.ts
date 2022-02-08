import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss']
})
export class DropdownComponent implements OnInit {
  showDropdown: boolean = false;

  @Input() placeholder = "";
  @Input() items: string[] = [];

  @Input() selected?: string;
  @Output() selectedChange: EventEmitter<string> = new EventEmitter<string>();

  constructor() { }

  ngOnInit(): void {
  }

  selectItem(item: string) {
    this.selected = item;
    this.selectedChange.emit(item);
    this.showDropdown = false;
  }
}
