import {
  ComponentFactoryResolver,
  Injectable, ViewContainerRef
} from '@angular/core';
import {AlertComponent} from "../alert/alert.component";

@Injectable()
export class AlertService {
  private rootViewContainer!: ViewContainerRef;
  private currentAlert?: any;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) {

  }

  setRootViewContainerRef(viewContainerRef : ViewContainerRef) {
    this.rootViewContainer = viewContainerRef
  }

  showAlert(title: string, message: string) {
    // Create component dynamically inside the ng-template
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(AlertComponent);
    this.currentAlert = this.rootViewContainer.createComponent(componentFactory);
    this.currentAlert.instance.title = title;
    this.currentAlert.instance.message = message;

    setTimeout(() => this.rootViewContainer.remove(0), 5000);
  }

}
