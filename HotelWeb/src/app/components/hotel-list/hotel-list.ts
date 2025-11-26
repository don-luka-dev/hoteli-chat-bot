import { Component, signal, computed, effect, input } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { Hotel, HotelStatus } from '../../models/hotel';
import { Mjesto } from '../../models/mjesto';
import { HotelService } from '../../services/hotel.service';
import { MjestoService } from '../../services/mjesto.service';
import { AuthService } from '../../services/auth.service';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-hotel-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ButtonModule,
    SelectModule,
    InputTextModule,
    RouterLink,
  ],
  templateUrl: './hotel-list.html',
  styleUrls: ['./hotel-list.css'],
})
export class HotelListComponent {
  hoteli = signal<Hotel[]>([]);
  mjesta = signal<Mjesto[]>([]);
  statusi = signal<HotelStatus[]>([]);
  filterForm: FormGroup;
  nazivControl =  new FormControl<string | null>(null);
  showFilters = signal(false);

  constructor(
    private hotelService: HotelService,
    private mjestoService: MjestoService,
    public authService: AuthService,
    private fb: FormBuilder
  ) {
    this.filterForm = this.fb.group({
      mjestoId: new FormControl<string | null>(null),
      statusHotelaId: new FormControl<number | null>(null),
    });

    this.mjestoService.getAll().subscribe({
      next: (data) => this.mjesta.set(data),
      error: (err) => console.error('Greška pri dohvaćanju mjesta', err),
    });

    this.hotelService.getAllStatusi().subscribe({
      next: (data) => this.statusi.set(data),
      error: (err) => console.error('Greška pri dohvaćanju statusa', err),
    });

    this.loadHotels();

    this.handleFilterChanges();
  }

  private loadHotels(filter?: any) {
    this.hotelService.getAll(filter).subscribe({
      next: (data: Hotel[]) => this.hoteli.set(data),
      error: (err) => console.error('Greška pri dohvaćanju hotela', err),
    });
  }

  private handleFilterChanges() {
  this.filterForm.valueChanges.subscribe((partialFilter) => {
    const filter = {
      ...partialFilter,
      naziv: this.nazivControl.value
    };
    this.loadHotels(filter);
  });

  this.nazivControl.valueChanges
    .pipe(
      debounceTime(300),
      distinctUntilChanged()
    )
    .subscribe((naziv) => {
      const filter = {
        ...this.filterForm.value,
        naziv: naziv
      };
      this.loadHotels(filter);
    });
}

 toggleFilters() {
    this.showFilters.update(value => !value);
  }

}
