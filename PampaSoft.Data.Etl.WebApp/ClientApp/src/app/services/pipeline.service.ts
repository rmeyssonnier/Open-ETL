import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Pipeline} from "../Models/Pipeline";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class PipelineService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public Create(pipeline: Pipeline): Observable<Pipeline> {
    return this.httpClient.post<Pipeline>(this.baseUrl + 'Pipeline', pipeline);
  }

  public Read(id: number): Observable<Pipeline> {
    return this.httpClient.get<Pipeline>(this.baseUrl + 'Pipeline/' + id);
  }

  public ReadAll(): Observable<Pipeline[]> {
    return this.httpClient.get<Pipeline[]>(this.baseUrl + 'Pipeline/all');
  }

  public Update(pipeline: Pipeline): Observable<void> {
    return this.httpClient.patch<void>(this.baseUrl + 'Pipeline', pipeline);
  }

  public Delete(id: number): Observable<void> {
    return this.httpClient.delete<void>(this.baseUrl + 'Pipeline/' + id);
  }
}
