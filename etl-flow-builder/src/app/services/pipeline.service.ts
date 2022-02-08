import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Pipeline} from "../Models/Pipeline";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class PipelineService {

  constructor(private httpClient: HttpClient) { }

  public Create(pipeline: Pipeline): Observable<Pipeline> {
    return this.httpClient.post<Pipeline>('https://localhost:5001/Pipeline', pipeline);
  }

  public Read(id: number): Observable<Pipeline> {
    return this.httpClient.get<Pipeline>('https://localhost:5001/Pipeline/' + id);
  }

  public ReadAll(): Observable<Pipeline[]> {
    return this.httpClient.get<Pipeline[]>('https://localhost:5001/Pipeline/all');
  }

  public Update(pipeline: Pipeline): Observable<void> {
    return this.httpClient.patch<void>('https://localhost:5001/Pipeline', pipeline);
  }

  public Delete(id: number): Observable<void> {
    return this.httpClient.delete<void>('https://localhost:5001/Pipeline/' + id);
  }
}
