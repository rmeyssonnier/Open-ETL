import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {DataFormat, ExtractHttpConfiguration} from "../../../Models/ActionConfiguration";

@Component({
  selector: 'app-extract-http-action',
  templateUrl: './extract-http-action.component.html',
  styleUrls: ['./extract-http-action.component.scss']
})
export class ExtractHttpActionComponent implements OnInit, OnChanges {

  @Input() configuration!: ExtractHttpConfiguration | any;
  @Output() configurationChange: EventEmitter<ExtractHttpConfiguration> = new EventEmitter<ExtractHttpConfiguration>();

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.configuration) {
      if (changes.configuration.currentValue == undefined && changes.configuration.firstChange) {
        this.configuration = new ExtractHttpConfiguration();
      }
    }
  }

  notifyChanges() {
    if (this.configuration.format === DataFormat.CSV && this.configuration.csvFormat === undefined) {
      this.configuration.csvFormat = {colDelimiter: ';', lineDelimiter: '\\n', haveHeader: 'True'};
    }
    if (this.configuration.format !== DataFormat.CSV && this.configuration.csvFormat !== undefined) {
      this.configuration.csvFormat = undefined;
    }
    this.configurationChange.emit(this.configuration);
  }
}
