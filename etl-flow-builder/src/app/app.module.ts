import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { InputComponent } from './input/input.component';
import {FormsModule} from "@angular/forms";
import { ActionPropertiesComponent } from './action-properties/action-properties.component';
import { CheckboxComponent } from './checkbox/checkbox.component';
import { DropdownComponent } from './dropdown/dropdown.component';
import { TransformRenameColumnsComponent } from './action-properties/forms/transform-rename-columns/transform-rename-columns.component';
import {ExtractHttpActionComponent} from "./action-properties/forms/extract-http-action/extract-http-action.component";
import {ExtractMinioActionComponent} from "./action-properties/forms/extract-minio-action/extract-minio-action.component";
import { LoadPostgresActionComponent } from './action-properties/forms/load-postgres-action/load-postgres-action.component';
import {HttpClientModule} from "@angular/common/http";
import { AlertComponent } from './alert/alert.component';
import {AlertService} from "./services/alert.service";
import {TransformKeepColumnsComponent} from "./action-properties/forms/transform-keep-columns/transform-keep-columns.component";
import {TransformRemoveColumnsComponent} from "./action-properties/forms/transform-remove-columns/transform-remove-columns.component";
import {LoadMinioActionComponent} from "./action-properties/forms/load-minio-action/load-minio-action.component";

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    InputComponent,
    ActionPropertiesComponent,
    CheckboxComponent,
    DropdownComponent,
    ExtractHttpActionComponent,
    ExtractMinioActionComponent,
    TransformRenameColumnsComponent,
    LoadPostgresActionComponent,
    AlertComponent,
    TransformKeepColumnsComponent,
    TransformRemoveColumnsComponent,
    LoadMinioActionComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [HttpClientModule, AlertService],
  bootstrap: [AppComponent],
  entryComponents: [AlertComponent]
})
export class AppModule { }
