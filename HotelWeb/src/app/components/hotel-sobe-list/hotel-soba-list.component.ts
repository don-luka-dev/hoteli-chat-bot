import { Component, input, signal } from '@angular/core';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { SobaService } from '../../services/soba.service';
import { Soba } from '../../models/soba';
import { RouterLink } from '@angular/router';
import { switchMap, filter } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-soba-list',
  standalone: true,
  imports: [RouterLink, ButtonModule],
  templateUrl: './hotel-soba-list.component.html',
  styleUrl: './hotel-soba-list.component.css',
})
export class HotelSobeListComponent {
  sobe = signal<Soba[]>([]);
  readonly hotelId = input<string>();

  constructor(
    private sobaService: SobaService,
    public authService: AuthService) {
    toObservable(this.hotelId)
      .pipe(
        takeUntilDestroyed(),
        filter((id): id is string => !!id),
        switchMap((id) => this.sobaService.getSobeByHotelId(id))
      )
      .subscribe({
        next: (data) => this.sobe.set(data),
        error: (err) => console.error('Greška pri dohvaćanju soba', err),
      });
  }
}
