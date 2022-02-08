import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {LoadMinioConfiguration} from "../../../Models/ActionConfiguration";

@Component({
  selector: 'app-load-minio-action',
  templateUrl: './load-minio-action.component.html',
  styleUrls: ['./load-minio-action.component.scss']
})
export class LoadMinioActionComponent implements OnInit, OnChanges {

  @Input() configuration!: LoadMinioConfiguration | any;
  @Output() configurationChange: EventEmitter<LoadMinioConfiguration> = new EventEmitter<LoadMinioConfiguration>();

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.configuration) {
      if (changes.configuration.currentValue == undefined && changes.configuration.firstChange) {
        this.configuration = new LoadMinioConfiguration
      }
    }
  }

  notifyChanges() {
    this.configurationChange.emit(this.configuration);
  }
}
