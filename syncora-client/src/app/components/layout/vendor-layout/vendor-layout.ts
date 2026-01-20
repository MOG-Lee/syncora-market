import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from "@angular/router";

@Component({
  selector: 'app-vendor-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './vendor-layout.html',
  styleUrl: './vendor-layout.scss',
})
export class VendorLayout {

  showMobileSidebar = signal(false);

  toggleMobileSidebar() {
    this.showMobileSidebar.update(val => !val);
  }

  closeMobileSidebar() {
    this.showMobileSidebar.set(false);
  }

}
