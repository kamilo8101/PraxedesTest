import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoaderComponent } from './components/loader/loader.component';
import { CommonService } from './services/common.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LoaderComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  standalone: true  
})
export class AppComponent {


  loader :boolean = false;

  constructor() {
    CommonService.loaderMap.subscribe(loader => this.loader = loader);
  }

  title = 'TestFront';
}
