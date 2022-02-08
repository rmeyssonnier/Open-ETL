import {Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild} from '@angular/core';
import {Action} from "../Models/Action";
import {ActionConfiguration} from "../Models/ActionConfiguration";

@Component({
  selector: 'app-action-properties',
  templateUrl: './action-properties.component.html',
  styleUrls: ['./action-properties.component.scss']
})
export class ActionPropertiesComponent implements OnInit {

  @Input() currentAction?: ActionConfiguration;
  @Output() currentActionChange: EventEmitter<ActionConfiguration> = new EventEmitter<ActionConfiguration>();

  constructor() { }

  ngOnInit(): void {
  }
}
