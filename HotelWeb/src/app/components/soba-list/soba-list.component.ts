import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, startWith } from 'rxjs/operators';
import { Soba } from '../../models/soba';
import { SobaService } from '../../services/soba.service';
import { HotelService } from '../../services/hotel.service';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputNumberModule } from 'primeng/inputnumber';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-soba-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ButtonModule,
    SelectModule,
    InputNumberModule,
    RouterLink,
  ],
  templateUrl: './soba-list.component.html',
  styleUrl: './soba-list.component.css',
})
export class SobaListComponent {
  sobe = signal<Soba[]>([]);
  hoteli = signal<{ id: string; naziv: string }[]>([]);
  showFilters = signal(false);

  // infinite scroll state
  private page = 1;
  private readonly pageSize = 12;
  loading = signal(false);
  hasMore = signal(true);

  // form
  hotelId = new FormControl<string | null>(null);
  brojKreveta = new FormControl<number | null>(null);
  cijenaMin = new FormControl<number | null>(null);
  cijenaMax = new FormControl<number | null>(null);

  constructor(
    private readonly fb: FormBuilder,
    private readonly sobaService: SobaService,
    private readonly hotelService: HotelService
  ) {
    // dropdown hoteli
    this.hotelService.getAll({}).subscribe({
      next: (data) =>
        this.hoteli.set(data.map((h) => ({ id: h.id, naziv: h.naziv }))),
      error: (e) => console.error('Greška pri dohvaćanju hotela', e),
    });

    // inicijalni load
    this.loadSobe();

    // reaktivni filteri
    this.hotelId.valueChanges.pipe(startWith(this.hotelId.value)).subscribe(() => this.applyFilters());
    this.brojKreveta.valueChanges
      .pipe(debounceTime(200), distinctUntilChanged(), startWith(this.brojKreveta.value))
      .subscribe(() => this.applyFilters());
    this.cijenaMin.valueChanges
      .pipe(debounceTime(200), distinctUntilChanged(), startWith(this.cijenaMin.value))
      .subscribe(() => this.applyFilters());
    this.cijenaMax.valueChanges
      .pipe(debounceTime(200), distinctUntilChanged(), startWith(this.cijenaMax.value))
      .subscribe(() => this.applyFilters());
  }

  private loadSobe(reset = true) {
    if (reset) {
      this.page = 1;
      this.sobe.set([]);
      this.hasMore.set(true);
    }

    if (!this.hasMore()) return;

    this.loading.set(true);

    const f = {
      hotelId: this.hotelId.value ?? undefined,
      brojKreveta: this.brojKreveta.value ?? undefined,
      cijenaNocenjaMin: this.cijenaMin.value ?? undefined,
      cijenaNocenjaMax: this.cijenaMax.value ?? undefined,
      page: this.page,
      pageSize: this.pageSize,
    };

    this.sobaService.getAll(f).subscribe({
      next: (data) => {
        if (reset) {
          this.sobe.set(data);
        } else {
          this.sobe.update((prev) => [...prev, ...data]);
        }
        this.hasMore.set(data.length === this.pageSize);
        this.page++;
        this.loading.set(false);
      },
      error: (e) => {
        console.error('Greška pri dohvaćanju soba', e);
        this.loading.set(false);
      },
    });
  }

  private applyFilters() {
    this.loadSobe(true);
  }

  loadNextPage() {
    this.loadSobe(false);
  }

  toggleFilters() {
    this.showFilters.update((v) => !v);
  }

  // scroll listener
  onScroll() {
    const pos =
      (document.documentElement.scrollTop || document.body.scrollTop) +
      window.innerHeight;
    const max = document.documentElement.scrollHeight;

    if (pos >= max - 100 && !this.loading()) {
      this.loadNextPage();
    }
  }
}
