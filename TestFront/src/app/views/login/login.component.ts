import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators,  } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',  
  styleUrl: './login.component.scss'
})
export class LoginComponent 
{
  loginForm: FormGroup;
  decodedToken: any;

  constructor(private fb: FormBuilder, private _authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    console.log(this.loginForm.value);

    this._authService.login(this.loginForm.value.username, this.loginForm.value.password).subscribe( result => {
      if (result.statusCode == 200) { 
        localStorage.setItem('userData', result.data);
        this.router.navigate(['/project']);
      }  
    });

  }


}



