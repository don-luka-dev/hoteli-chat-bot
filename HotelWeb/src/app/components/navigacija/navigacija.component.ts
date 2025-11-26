import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { MenuModule } from 'primeng/menu';
import { MenuItem } from 'primeng/api';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, ButtonModule, MenuModule],
  templateUrl: './navigacija.component.html',
  styleUrls: ['./navigacija.component.css'],
})
export class NavbarComponent {
  items: MenuItem[] = [
    { label: 'Login', icon: 'pi pi-sign-in', routerLink: '/login' },
    { label: 'Register', icon: 'pi pi-user-plus', routerLink: '/register' },
    { separator: true },
    { label: 'Logout', icon: 'pi pi-sign-out', command: () => this.logout() }
  ];

  actions: MenuItem[] = [
    { label: 'Moji Hoteli', icon: 'pi pi-building', routerLink: '/hoteli/moji' },
    { label: 'Moje rezervacije', icon: 'pi pi-calendar', routerLink: '/rezervacije/moje' },
  ];

  administration: MenuItem[] = [
    { label: 'Upravljanje korisnicima', icon: 'pi pi-users', routerLink: '/korisnici' },
    { label: 'Zahtjevi', icon: 'pi pi-check', routerLink: '/hoteli/nepotvrdeni' },
  ];

  constructor(
    private router: Router, 
    public authService: AuthService
  ) {}

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
