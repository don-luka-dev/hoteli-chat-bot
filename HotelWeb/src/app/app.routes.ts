import { Routes } from '@angular/router';
import { HotelListComponent } from './components/hotel-list/hotel-list';
import { HotelDetailsComponent } from './components/hotel-details/hotel-details';
import { SobaDetailsComponent } from './components/soba-details/soba-details.component';
import { HotelSobeListComponent } from './components/hotel-sobe-list/hotel-soba-list.component';
import { AddHotelComponent } from './components/add-hotel/add-hotel.component';
import { AddSobaComponent } from './components/add-soba/add-soba.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './guards/auth.guard';
import { RegisterComponent } from './components/register/register.component';
import { SobaListComponent } from './components/soba-list/soba-list.component';
import { UserHotelListComponent } from './components/user-hotel-list/user-hotel-list.component';
import { UserRezervacijeListComponent } from './components/user-rezervacije-list/user-rezervacije-list.component';
import { KorisiniciListComponent } from './components/korisinici-list/korisinici-list.component';
import { adminGuard } from './guards/admin.guard';
import { NepotvrdeniHoteliComponent } from './components/nepotvrdeni-hoteli/nepotvrdeni-hoteli.component';

export const routes: Routes = [
   // Auth
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // Hoteli
  { path: 'hoteli', component: HotelListComponent },
  { path: 'hoteli/moji', component: UserHotelListComponent, canActivate: [authGuard] },
  { path: 'hoteli/nepotvrdeni', component: NepotvrdeniHoteliComponent, canActivate: [authGuard, adminGuard] },
  { path: 'hoteli/novi', component: AddHotelComponent },
  { path: 'hoteli/update/:id', component: AddHotelComponent },
  { path: 'hoteli/:id', component: HotelDetailsComponent },

  // Sobe (globalno)
  { path: 'sobe', component: SobaListComponent },
  { path: 'sobe/:id', component: SobaDetailsComponent }, 

  //Rezervacije (user)
  { path: 'rezervacije/moje', component: UserRezervacijeListComponent, canActivate: [authGuard] },

  { path: 'korisnici', component: KorisiniciListComponent, canActivate: [authGuard, adminGuard] },

  // Sobe (po hotelu)
  { path: 'hoteli/:hotelId/sobe', component: HotelSobeListComponent },          // lista soba za hotel
  { path: 'hoteli/:hotelId/sobe/novi', component: AddSobaComponent, canActivate: [authGuard, adminGuard] }, // kreiraj sobu za hotel
  { path: 'hoteli/:hotelId/sobe/update/:id', component: AddSobaComponent, canActivate: [authGuard, adminGuard] }, // uredi sobu
  { path: 'hoteli/:hotelId/sobe/:id', component: SobaDetailsComponent },         // detalji sobe

  // Default
  { path: '', redirectTo: '/hoteli', pathMatch: 'full' },
];
