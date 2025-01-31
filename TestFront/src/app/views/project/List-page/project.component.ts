import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProjectService } from '../../../services/project.service';
import { RouterLink } from '@angular/router';
import { SweetAlertUtils } from '../../../shared/sweet-alert';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-project',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss'
})
export class ProjectComponent {

  public projectForm: FormGroup;
  ListProject: any;
CanCreate: any;
  
  constructor(private projectService: ProjectService, private authService: AuthService) {
    this.projectForm = new FormGroup({
      name: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      status: new FormControl('', Validators.required)
    });

    this.authService.currentUser$.subscribe(result => {
      debugger;
      this.CanCreate = (Array.isArray(result.UserRoles) ? result.UserRoles.includes('Líder de Proyecto') || result.UserRoles.includes('Administrador') : result.UserRoles === 'Líder de Proyecto' || result.UserRoles === 'Administrador');
    });

    this.loadGridItems();
  }

  private loadGridItems(): void {
    this.projectService.GetData().subscribe((result) => {
      if (result.statusCode == 200) {
           this.ListProject = result.data;
      }
    });
  }
  
  onDelete(Id: any) {
     SweetAlertUtils.SwallQuestion("¿Estás seguro de eliminar este proyecto?", (result: any) => {
      if (result) {
        this.projectService.Delete(Id).subscribe((result) => {
          if (result.statusCode == 200) {
            SweetAlertUtils.SwallSucces("¡Correcto!", "El proyecto ha sido eliminado correctamente");
            this.loadGridItems();
          }
        }); 
      }
     });
  }

  downloadReport() {
    this.projectService.downloadExcel();
  }
}