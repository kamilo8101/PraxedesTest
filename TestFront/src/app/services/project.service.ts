import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { saveAs } from 'file-saver';
import { environments } from '../enviroments/environments';


@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private _httpClient: HttpClient) { }

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('userData')}` })
  };

  public GetData(): Observable<any> {
    return this._httpClient.get(`${environments._root}Project/List`, this.httpOptions);
  }

  public GetById(id: number): Observable<any>
  {
    return this._httpClient.get(`${environments._root}Project/Get?id=${id}`, this.httpOptions);
  }

  public Create(project: any): Observable<any> {
    return this._httpClient.post(`${environments._root}Project/Create`, JSON.stringify(project), this.httpOptions);
  }

  public Edit(project: any): Observable<any>
  {
    return this._httpClient.post(`${environments._root}Project/Update`, project, this.httpOptions);
  }

  public Delete(Id: any): Observable<any> {
    return this._httpClient.post(`${environments._root}Project/Delete?id=${Id}`, this.httpOptions);
  }
  

  // public downloadExcel() {
  //   this._httpClient.get(`https://localhost:7243/api/Project/ExcelReport`, { ...this.httpOptions, responseType: 'blob' }).subscribe((data: Blob) => {
  //     saveAs(data);
  //   });
  // }

  public downloadExcel() {
    debugger;
    this._httpClient.get(`${environments._root}Project/ExcelReport`, { ...this.httpOptions, responseType: 'blob' }).subscribe((response: any) => {
      // Crear un objeto Blob con el contenido del archivo
      debugger;
      const file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      saveAs(file, 'reporte.xlsx');

    });
  }
}
