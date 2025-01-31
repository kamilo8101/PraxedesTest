import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../models/User.model';
import { environments } from '../enviroments/environments';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _currentUser = new BehaviorSubject<any | null>(this.decodeToken());

  currentUser$ = this._currentUser.asObservable();

  constructor(private _httpClient: HttpClient) { }


  public login(username: string, password: string):Observable<any> {
    return this._httpClient.post(`${environments._root}Account/login`, { Username: username, Password: password });
  }

  private decodeToken() {
    const userData = localStorage.getItem('userData');

    return userData ? jwtDecode(userData) : null;
  }

}
