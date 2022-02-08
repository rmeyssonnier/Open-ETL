import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {LoadPostgresConfiguration} from "../../../Models/ActionConfiguration";

@Component({
  selector: 'app-load-postgres-action',
  templateUrl: './load-postgres-action.component.html',
  styleUrls: ['./load-postgres-action.component.scss']
})
export class LoadPostgresActionComponent implements OnInit, OnChanges {

  @Input() configuration!: LoadPostgresConfiguration | any;
  @Output() configurationChange: EventEmitter<LoadPostgresConfiguration> = new EventEmitter<LoadPostgresConfiguration>();

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.configuration) {
      if (changes.configuration.currentValue == undefined && changes.configuration.firstChange) {
        this.configuration = new LoadPostgresConfiguration
      }
    }
  }

  notifyChanges() {
    this.configurationChange.emit(this.configuration);
  }
}
