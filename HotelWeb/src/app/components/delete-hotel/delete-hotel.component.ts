import { Component, inject, input, Input } from '@angular/core';
import { HotelService } from '../../services/hotel.service';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-delete-hotel',
  standalone: true,
  imports: [ButtonModule],
  templateUrl: './delete-hotel.component.html',
  styleUrls: ['./delete-hotel.component.css'],
})
export class DeleteHotelComponent {
  private readonly hotelService = inject(HotelService);
  private readonly router = inject(Router);

  readonly hotelId = input<string>();

  onDelete() {
    const id = this.hotelId();
    if (!id) return;

    this.hotelService.delete(id).subscribe({
      next: () => this.router.navigate(['/hoteli']),
      error: (err) => console.error('Greška pri brisanju hotela', err),
    });
  }
}
