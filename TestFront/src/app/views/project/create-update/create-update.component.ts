import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProjectService } from '../../../services/project.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { switchMap } from 'rxjs';
import { SweetAlertUtils } from '../../../shared/sweet-alert';
import { ProjectModel } from '../../../models/Project.model';

@Component({
  selector: 'app-create-update',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './create-update.component.html',
  styleUrls: ['./create-update.component.scss']
})
export class CreateUpdateComponent implements OnInit {


  public projectForm: FormGroup;

  constructor(private projectService: ProjectService, private router: Router, private activatedRoute: ActivatedRoute) {
    this.projectForm = new FormGroup({
      id: new FormControl(null),
      name: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      state: new FormControl('', Validators.required),
      userid:  new FormControl(null),
      startDate:  new FormControl('', Validators.required),
      endDate:  new FormControl('', Validators.required), 
    });
  }

  get currentProject(): ProjectModel {
    const project = this.projectForm.value as ProjectModel;
    return project;
  }

  ngOnInit(): void {
    if ( !this.router.url.includes('edit') ) return;

    this.activatedRoute.params
      .pipe(
        switchMap( ({ id }) => this.projectService.GetById( id ) ),
      ).subscribe( result => {

        if ( !result ) {
          return this.router.navigateByUrl('/');
        }

        this.projectForm.reset( result.data );
        return;
      });

  }

  onSubmit() {
    if (this.projectForm.invalid ) return;

    if ( this.currentProject.id) {
      this.projectService.Edit( this.currentProject )
        .subscribe( result => {
          if (result.statusCode == 200) { 
            SweetAlertUtils.SwallSucces("¡Correcto!", "El proyecto ha sido actualizado correctamente");
            this.router.navigate(['/project']);
          }
        });

      return;
    }
    this.projectService.Create( this.currentProject )
      .subscribe( result => {
        if (result.statusCode == 200) {
          SweetAlertUtils.SwallSucces("¡Correcto!", "El proyecto ha sido creado correctamente");
          this.router.navigate(['/project']);
        }
      });
  }
}
