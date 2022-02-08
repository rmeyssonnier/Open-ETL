import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})
export class AlertComponent implements OnInit {

  public title: string = ""
  public message: string = ""

  constructor() { }

  ngOnInit(): void {
  }

}
