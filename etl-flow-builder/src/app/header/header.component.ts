import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {PipelineService} from "../services/pipeline.service";
import {Pipeline} from "../Models/Pipeline";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  pipelines: Pipeline[] = [];
  selected: string = "";

  constructor(private pipelineService: PipelineService) {
    pipelineService.ReadAll().subscribe(r => {
      this.pipelines = r;
      this.selected = this.pipelines[0].pipelineName;
    });
  }

  @Output() runClicked: EventEmitter<void> = new EventEmitter<void>();
  @Output() discardClicked: EventEmitter<void> = new EventEmitter<void>();
  @Output() saveClicked: EventEmitter<void> = new EventEmitter<void>();
  @Output() openClicked: EventEmitter<number> = new EventEmitter<number>();

  ngOnInit(): void {
  }

  pipelineNames(): string[] {
    return this.pipelines.map(p => p.pipelineName);
  }

  open() {
    this.openClicked.emit(this.pipelines.find(p => p.pipelineName === this.selected)!.pipelineId);
  }
}
