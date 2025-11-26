import { CommonModule } from '@angular/common';
import { Component, signal, ViewChild } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { NavbarComponent } from "./components/navigacija/navigacija.component"
import { SidePanelComponent } from './components/side-panel/side-panel.component';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, CommonModule, NavbarComponent, SidePanelComponent, ButtonModule, ], 
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent {
  showNavbar = signal(true);
    @ViewChild('panel') panel?: SidePanelComponent;

  constructor(private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.showNavbar.set(!['/login', '/register'].includes(event.urlAfterRedirects));
      }
    });
  }


}
