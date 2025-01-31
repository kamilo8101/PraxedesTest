import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environments } from '../enviroments/environments';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private _httpClient: HttpClient) { }

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('userData')}` })
  };


  public GetData(): Observable<any> {
    return this._httpClient.get(`${environments._root}task/List`, this.httpOptions);
  }

  public GetById(id: number): Observable<any>
  {
    return this._httpClient.get(`${environments._root}task/Get?id=${id}`, this.httpOptions);
  }

  public Create(project: any): Observable<any> {
    return this._httpClient.post(`${environments._root}task/Create`, JSON.stringify(project), this.httpOptions);
  }

  public Edit(project: any): Observable<any>
  {
    return this._httpClient.post(`${environments._root}task/Update`, project, this.httpOptions);
  }

  public Delete(Id: any): Observable<any> {
    return this._httpClient.post(`${environments._root}task/Delete?id=${Id}`, this.httpOptions);
  }
}
