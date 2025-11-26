import { Component, inject, input, signal } from '@angular/core';
import { toObservable, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { combineLatest, filter, switchMap } from 'rxjs';
import { Soba } from '../../models/soba';
import { SlikaSobe } from '../../models/slikaSobe';
import { SobaService } from '../../services/soba.service';
import { DeleteSobaComponent } from '../delete-soba/delete-soba.component';
import { AddRezervacijaComponent } from '../add-rezervacija/add-rezervacija.component';
import { AddSlikaSobeComponent } from '../add-slika-sobe/add-slika-sobe.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-soba-details',
  standalone: true,
  templateUrl: './soba-details.component.html',
  styleUrls: ['./soba-details.component.css'],
  imports: [DeleteSobaComponent, AddRezervacijaComponent, AddSlikaSobeComponent],
})
export class SobaDetailsComponent {
  private readonly sobaService = inject(SobaService);

  readonly hotelId = input<string>();
  readonly id = input<string>();

  soba = signal<Soba | null>(null);
  slike = signal<SlikaSobe[]>([]);
  activeIndex = signal(0);

  // UI state
  imgLoaded = signal(false);
  private swipeStartX: number | null = null;

  constructor(public authService: AuthService) {
    // soba
    toObservable(this.id)
    .pipe(
      takeUntilDestroyed(),
      filter((sobaId): sobaId is string => !!sobaId),
      switchMap((sobaId) => {
        const hid = this.hotelId();
        return hid
          ? this.sobaService.getByHotelIdAndId(hid, sobaId)
          : this.sobaService.getById(sobaId); // <-- global fallback
      })
    )
    .subscribe({ next: d => this.soba.set(d) });
    // slike
    toObservable(this.id)
      .pipe(
        takeUntilDestroyed(),
        filter((sobaId): sobaId is string => !!sobaId),
        switchMap(sobaId => this.sobaService.getAllSlike(sobaId))
      )
      .subscribe({ next: d => { this.slike.set(d ?? []); this.select(0); } });
  }

  /** navigation */
  prev() {
    const n = this.slike().length; if (!n) return;
    this.setActive((this.activeIndex() - 1 + n) % n);
  }
  next() {
    const n = this.slike().length; if (!n) return;
    this.setActive((this.activeIndex() + 1) % n);
  }
  select(i: number) {
    if (i < 0 || i >= this.slike().length) return;
    this.setActive(i);
  }
  private setActive(i: number) {
    this.activeIndex.set(i);
    this.imgLoaded.set(false); // trigger fade-in for new image
  }

  /** image events */
  onImageLoad() { this.imgLoaded.set(true); }
  onImageError(ev: Event) { (ev.target as HTMLImageElement).src = '/assets/no-image.png'; }

  /** keyboard + swipe */
  onKey(ev: KeyboardEvent) {
    if (ev.key === 'ArrowLeft') { ev.preventDefault(); this.prev(); }
    else if (ev.key === 'ArrowRight') { ev.preventDefault(); this.next(); }
  }
  onPointerDown(ev: PointerEvent) { this.swipeStartX = ev.clientX; }
  onPointerUp(ev: PointerEvent) {
    if (this.swipeStartX == null) return;
    const dx = ev.clientX - this.swipeStartX;
    this.swipeStartX = null;
    const THRESH = 40;
    if (dx > THRESH) this.prev();
    else if (dx < -THRESH) this.next();
  }

  /** refresh nakon uploada */
  refreshSlike() {
    const sobaId = this.id(); if (!sobaId) return;
    this.sobaService.getAllSlike(sobaId).subscribe({
      next: d => { this.slike.set(d ?? []); this.select(0); }
    });
  }
}
