import { Routes } from '@angular/router';
import { LoginComponent } from './views/login/login.component';
import { ProjectComponent } from './views/project/List-page/project.component';
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { CreateUpdateComponent as CreateUpdateProjectComponent } from './views/project/create-update/create-update.component';
// import { ListPageComponent } from './views/users/list-page/list-page.component';
// import { CreateUpdateComponent as CreateUpdateUserComponent } from './views/users/create-update/create-update.component';
import { ListPageComponent as ListPageTaskComponent } from './views/task/list-page/list-page.component';
import { CreateUpdateComponent as CreateUpdateTaskComponent } from './views/task/create-update/create-update.component';
import { authGuard } from './guards/auth.guard';


export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        canMatch: [authGuard],
        path: '',
        component: DefaultLayoutComponent,
        children: [
            // {
            //     path: 'users',
            //     component: ListPageComponent
            // },
            // {
            //     path: 'users/create',
            //     component: CreateUpdateUserComponent
            // },
            // {
            //     path: 'users/edit/:id',
            //     component: CreateUpdateUserComponent
            // },
            {
                path: 'project',
                component: ProjectComponent
            },
            {
                path: 'project/create',
                component: CreateUpdateProjectComponent
            },
            {
                path: 'project/edit/:id',
                component: CreateUpdateProjectComponent
            },
            {
                path: 'task/:projectId',
                component: ListPageTaskComponent
            }
        ]
    },
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    }
];
