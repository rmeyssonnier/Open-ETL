import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {TransformKeepConfiguration} from "../../../Models/ActionConfiguration";

@Component({
  selector: 'app-transform-keep-columns',
  templateUrl: './transform-keep-columns.component.html',
  styleUrls: ['./transform-keep-columns.component.scss']
})
export class TransformKeepColumnsComponent implements OnInit, OnChanges {

  @Input() configuration!: TransformKeepConfiguration | any;
  @Output() configurationChange: EventEmitter<TransformKeepConfiguration> = new EventEmitter<TransformKeepConfiguration>();

  toKeep: string = '';

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.configuration) {
      if (changes.configuration.currentValue) {
        changes.configuration.currentValue.toKeep.forEach((v : string[]) => {
          this.toKeep += v + ';';
        });
        this.toKeep = this.toKeep.slice(0, -1);
      }
      if (changes.configuration.currentValue == undefined && changes.configuration.firstChange) {
        this.configuration = new TransformKeepConfiguration;
      }
    }
  }

  notifyChanges() {
    this.configuration.toKeep = this.toKeep.split(';');
    this.configurationChange.emit(this.configuration);
  }
}
