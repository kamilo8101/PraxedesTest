import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environments } from '../enviroments/environments';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _httpClient: HttpClient) { }

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('userData')}` })
  };

  public GetData(): Observable<any> {
    return this._httpClient.post(`${environments._root}user/List`, this.httpOptions);

  }


  public GetUserTask(taskId:number): Observable<any> {
    return this._httpClient.get(`${environments._root}User/GetUserTask/${taskId}`, this.httpOptions);
  }
}


