import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {DataFormat, ExtractHttpConfiguration, ExtractMinioConfiguration} from "../../../Models/ActionConfiguration";

@Component({
  selector: 'app-extract-minio-action',
  templateUrl: './extract-minio-action.component.html',
  styleUrls: ['./extract-minio-action.component.scss']
})
export class ExtractMinioActionComponent implements OnInit, OnChanges {

  @Input() configuration!: ExtractMinioConfiguration | any;
  @Output() configurationChange: EventEmitter<ExtractMinioConfiguration> = new EventEmitter<ExtractMinioConfiguration>();

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.configuration) {
      if (changes.configuration.currentValue == undefined && changes.configuration.firstChange) {
        this.configuration = new ExtractMinioConfiguration
      }
    }
  }

  notifyChanges() {
    this.configurationChange.emit(this.configuration);
  }
}
