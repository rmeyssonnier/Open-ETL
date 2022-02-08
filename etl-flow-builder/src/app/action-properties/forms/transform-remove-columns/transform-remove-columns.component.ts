import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {TransformRemoveConfiguration} from "../../../Models/ActionConfiguration";

@Component({
  selector: 'app-transform-remove-columns',
  templateUrl: './transform-remove-columns.component.html',
  styleUrls: ['./transform-remove-columns.component.scss']
})
export class TransformRemoveColumnsComponent implements OnInit, OnChanges {

  @Input() configuration!: TransformRemoveConfiguration | any;
  @Output() configurationChange: EventEmitter<TransformRemoveConfiguration> = new EventEmitter<TransformRemoveConfiguration>();

  toRemove: string = '';

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.configuration) {
      if (changes.configuration.currentValue) {
        changes.configuration.currentValue.toRemove.forEach((v : string[]) => {
          this.toRemove += v + ';';
        });
        this.toRemove = this.toRemove.slice(0, -1);
      }
      if (changes.configuration.currentValue == undefined && changes.configuration.firstChange) {
        this.configuration = new TransformRemoveConfiguration;
      }
    }
  }

  notifyChanges() {
    this.configuration.toRemove = this.toRemove.split(';');
    this.configurationChange.emit(this.configuration);
  }
}
