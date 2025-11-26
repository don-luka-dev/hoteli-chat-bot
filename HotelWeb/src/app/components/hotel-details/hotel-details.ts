import { Component, inject, input, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { filter, switchMap } from 'rxjs';
import { Hotel } from '../../models/hotel';
import { HotelService } from '../../services/hotel.service';
import { DeleteHotelComponent } from '../delete-hotel/delete-hotel.component';
import { HotelSobeListComponent } from '../hotel-sobe-list/hotel-soba-list.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-hotel-details',
  standalone: true,
  imports: [HotelSobeListComponent, DeleteHotelComponent, RouterLink, ButtonModule],
  templateUrl: './hotel-details.html',
  styleUrls: ['./hotel-details.css'],
})
export class HotelDetailsComponent {
  private readonly hotelService = inject(HotelService);

  readonly id = input<string>();
  hotel = signal<Hotel | null>(null);

  constructor(public authService: AuthService) {
    const id = this.id();
    toObservable(this.id)
      .pipe(
        filter((id) => !!id),
        switchMap((id) => this.hotelService.getById(id!))
      )
      .subscribe({
        next: (data) => this.hotel.set(data),
        error: (err) => console.error('Greška pri dohvaćanju hotela', err),
      });
  }
}
