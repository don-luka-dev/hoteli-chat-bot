import { Component, inject, input } from '@angular/core';
import { SobaService } from '../../services/soba.service';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-delete-soba',
  standalone: true,
  imports: [ButtonModule],
  templateUrl: './delete-soba.component.html',
  styleUrl: './delete-soba.component.css',
})
export class DeleteSobaComponent {
  private readonly sobaService = inject(SobaService);
  private readonly router = inject(Router);

  readonly hotelId = input<string>();
  readonly sobaId = input<string>();

  onDelete() {
    const hotelId = this.hotelId();
    const sobaId = this.sobaId();

    if (!hotelId || !sobaId) return;

    this.sobaService.delete(hotelId, sobaId).subscribe({
      next: () => this.router.navigate([`/hoteli/${this.hotelId}`]),
      error: (err) => console.error('Greška pri brisanju hotela', err),
    });
  }
}
