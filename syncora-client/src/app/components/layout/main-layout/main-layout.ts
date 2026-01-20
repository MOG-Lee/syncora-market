import { Component } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { Navbar } from "../navbar/navbar";
import { Footer } from "../footer/footer";
import { CartDrawer } from "../../features/cart/cart-drawer/cart-drawer";

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, Navbar, Footer, CartDrawer],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout {

}
