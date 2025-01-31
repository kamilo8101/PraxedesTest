import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { __values } from 'tslib';
import { TaskService } from '../../../services/task.service';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { SweetAlertUtils } from '../../../shared/sweet-alert';

@Component({
  selector: 'app-list-page',
  imports: [ReactiveFormsModule, CommonModule, FormsModule, RouterModule],
  templateUrl: './list-page.component.html',
  styleUrl: './list-page.component.scss'
})
export class ListPageComponent implements OnInit {
  taskForm: FormGroup;
  projectId: string = '';
  timelineItems: any[] = [];
  CanCreate: boolean = false;
  users: any[] = [];

  constructor(private activatedRoute: ActivatedRoute, 
    private taskService: TaskService, 
    private authService: AuthService,
    private userService: UserService
  ) {
    this.activatedRoute.params.subscribe(params => {
      this.projectId = params['projectId'];
    });

    this.taskForm = new FormGroup({
      id: new FormControl(null),
      projectId:  new FormControl(this.projectId, Validators.required),
      name:  new FormControl('', Validators.required),
      description:  new FormControl('', Validators.required),
      type:  new FormControl('', Validators.required),
      state:  new FormControl('', Validators.required),
      userid:  new FormControl('', Validators.required),
      startDate:  new FormControl('', Validators.required),
      endDate:  new FormControl(null),     
    });
  }


  ngOnInit(): void {

    this.authService.currentUser$.subscribe(result => {
      debugger;
      this.CanCreate = (Array.isArray(result.UserRoles) ? result.UserRoles.includes('Líder de Proyecto') || result.UserRoles.includes('Administrador') : result.UserRoles === 'Líder de Proyecto' || result.UserRoles === 'Administrador');
    });

    this.taskService.GetData().subscribe(result => {
      if(result.statusCode == 200){
        this.timelineItems = result.data;
      }
    });
  }

  onTypeChange() {
    this.userService.GetUserTask(this.taskForm.get('type')?.value).subscribe(result => {
      if(result.statusCode == 200){
        this.users = result.data;
      }
    });
    }

  onSubmit() {

    if(!this.taskForm.valid){this.taskForm.markAllAsTouched(); return;}

    this.taskService.Create(this.taskForm.value).subscribe(result => {
      SweetAlertUtils.SwallSucces("¡Correcto!", "La tarea ha sido creada correctamente");
    }); 
    }

}
