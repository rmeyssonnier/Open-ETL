import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {TransformRenameConfiguration} from "../../../Models/ActionConfiguration";

@Component({
  selector: 'app-transform-rename-columns',
  templateUrl: './transform-rename-columns.component.html',
  styleUrls: ['./transform-rename-columns.component.scss']
})
export class TransformRenameColumnsComponent implements OnInit, OnChanges {

  @Input() configuration!: TransformRenameConfiguration | any;
  @Output() configurationChange: EventEmitter<TransformRenameConfiguration> = new EventEmitter<TransformRenameConfiguration>();

  oldNames: string = '';
  newNames: string = '';

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.configuration) {
      if (changes.configuration.currentValue) {
        changes.configuration.currentValue.newNames.forEach((v : {oldName: string; newName: string}) => {
          this.newNames += v.newName + ';';
          this.oldNames += v.oldName + ';';
        });
        this.newNames = this.newNames.slice(0, -1);
        this.oldNames = this.oldNames.slice(0, -1);
      }
      if (changes.configuration.currentValue == undefined && changes.configuration.firstChange) {
        this.configuration = new TransformRenameConfiguration
      }
    }
  }

  notifyChanges() {

    const newNamesArr = this.newNames.split(';');
    const oldNamesArr = this.oldNames.split(';');

    if (newNamesArr.length == oldNamesArr.length) {
      this.configurationChange.emit(this.configuration);
      this.configuration.newNames = [];
      for (let i = 0; i < newNamesArr.length; i++) {
        this.configuration.newNames.push({oldName: oldNamesArr[i], newName: newNamesArr[i]});
      }
      this.configurationChange.emit(this.configuration);
    }
  }
}
