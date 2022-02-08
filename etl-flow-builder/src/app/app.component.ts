import {
  AfterViewInit,
  Component,
  ElementRef,
  ViewChild,
  ViewContainerRef
} from '@angular/core';
import {Engine} from './flowy/engine';
import {Action, actionSet, getPlacedElement, getUniqueId} from "./Models/Action";
import {ActionConfiguration, loadConfiguration} from "./Models/ActionConfiguration";
import {AlertService} from "./services/alert.service";
import {PipelineService} from "./services/pipeline.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterViewInit {
  @ViewChild('canvas', { static: false }) public canvas!: ElementRef;
  @ViewChild('alertZone', {read: ViewContainerRef}) alertZone!: ViewContainerRef;

  spacingx = 40;
  spacingy = 40;
  engine!: Engine;

  showMenu: boolean = false;
  currentBlock?: HTMLElement;

  categories: {selected: boolean, text: string}[] = [
    {selected: true, text: 'Extract'},
    {selected: false, text: 'Transform'},
    {selected: false, text: 'Load'}
  ]

  actionFiltered: Action[] = [];
  private inBlockClick: boolean = false;
  leftPanelHidden: boolean = false;

  actionConfiguration: ActionConfiguration[] = [];
  selectedAction: number = -1;

  constructor(private pipelineService: PipelineService,
              private alertService: AlertService) {
  }

  ngAfterViewInit(): void {
    this.engine = new Engine(
      this.canvas.nativeElement,
      this.spacingx,
      this.spacingy,
      this.onGrab,
      this.onRelease.bind(this),
      this.onSnap.bind(this)
    );

    this.filterActions(this.categories[0].text);
    this.engine.ItemSelected.subscribe(e => {
      this.elementSelected(e);
    });

    this.alertService.setRootViewContainerRef(this.alertZone);
  }

  filterActions(category: string) {
    this.actionFiltered = actionSet.filter(a => a.category === category);
  }

  runFlow() {
    const stages: string[] = [];

    this.actionConfiguration.forEach(a => stages.push(JSON.stringify(a)));

    // this.httpClient.post('https://localhost:5001/EtlFlow', {stages} as EtlFlowCommand)
    //   .subscribe(
    //     r => this.alertService.showAlert("ETL Flow result", r == true ? "Succeed" : "Failed"),
    //     error => this.alertService.showAlert("ETL Flow result", "Failed")
    //   );
  }

  save() {
    const pipelineName = prompt('Pipeline name', '')
    if (pipelineName !== null && pipelineName.length > 0) {
      const dump = this.engine.output();
      dump.actionConfiguration = this.actionConfiguration;
      const exportDump = JSON.stringify(dump);
      this.pipelineService.Create({pipelineName: pipelineName, pipelineProp: exportDump})
        .subscribe(
          r => r.pipelineId ? window.location.reload() : this.alertService.showAlert("Pipeline create result", "Failed"),
          error => this.alertService.showAlert("Pipeline create result", "Failed")
        );
    }
  }

  clear() {
    this.actionConfiguration = [];
    this.engine.deleteBlocks();
  }

  open($event: number) {
    this.pipelineService.Read($event).subscribe(r => {
      this.clear();
      const toLoad = JSON.parse(r.pipelineProp) as any;
      this.engine.import(toLoad);
      if (toLoad.actionConfiguration) {
        this.actionConfiguration = [];
        toLoad.actionConfiguration.forEach((c: any) => {
          c.configuration = loadConfiguration(c)
          this.actionConfiguration.push(c);
        });
      }
    });
  }

  onGrab(block : any){
  }

  onRelease(){
    const blockOnCanvas = this.engine.output()?.blocks.length;

    if (blockOnCanvas !== undefined) {
      if (blockOnCanvas < this.actionConfiguration.length) {
        console.log("Current actions : " + this.actionConfiguration.length + ' engine size : ' + this.engine.output()?.blocks.length)
        this.actionConfiguration = this.actionConfiguration.slice(0, blockOnCanvas);
        console.log(this.actionConfiguration)
      }
    }
  }

  onSnap(drag : any){
    this.actionConfiguration.push(
      {
        type: actionSet.find(action => action.id === drag.querySelector('.blockelemtype').value)!.type,
        actionId: drag.querySelector('.blockid').value
      }
    )

    const grab = drag.querySelector('.grabme');
    grab.parentNode.removeChild(grab);
    const blockin = drag.querySelector('.blockin');
    blockin.parentNode.removeChild(blockin);
    drag.innerHTML += getPlacedElement(drag.querySelector('.blockelemtype').value);

    return true;
  }

  setSelected(id: number) {
    this.categories[0].selected = false;
    this.categories[1].selected = false;
    this.categories[2].selected = false;

    this.categories[id].selected = true;
    this.filterActions(this.categories[id].text);
  }

  private elementSelected(e: { id: number; type: string; blockRef: HTMLElement }) {
    this.inBlockClick = true;
    this.clearSelected();

    if (this.currentBlock !== undefined)
      this.currentBlock.classList

    e.blockRef.classList.add('selectedblock')
    this.currentBlock = e.blockRef;

    this.showMenu = true;
    this.selectedAction = e.id;
    setTimeout(() => this.inBlockClick = false, 500);
  }

  clearSelected() {
    this.canvas.nativeElement.querySelectorAll(`.blockelem`)
      .forEach((n : any) => n.classList.remove('selectedblock'));
    this.selectedAction = -1;
    this.updateBlockDescription();
  }

  canvaClicked($event: MouseEvent) {
    if (!this.inBlockClick) {
      this.showMenu = false;
      this.clearSelected();
    }
  }

  updateBlockDescription() {
    this.actionConfiguration.forEach(c => {
      const block = this.getBlockById(c.actionId);
      if (block && c.configuration?.getDescription())
        this.setBlockDescription(block, c.configuration?.getDescription());
    });
  }

  private getBlockById(id: number): HTMLElement | undefined {
    let result = undefined;

    this.canvas.nativeElement.querySelectorAll(`.blockelem`)
      .forEach((b : any) => {
        if (b.querySelector(`.blockid`)!.value === id) {
          console.log('Found')
          result = b;
          return;
        }
      });

    return result;
  }

  private setBlockDescription(block: HTMLElement, description: string) {
    block.getElementsByClassName("blockyinfo")[0].innerHTML = description;
  }
}
