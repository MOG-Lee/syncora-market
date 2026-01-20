import { Routes } from '@angular/router';
import { MainLayout } from './components/layout/main-layout/main-layout';
import { VendorLayout } from './components/layout/vendor-layout/vendor-layout';
import { Dashboard } from './components/features/vendor/dashboard/dashboard';
import { Home } from './components/features/shop/home/home';


export const routes: Routes = [

    // 1. AUTH ROUTES (No Layout / No Navbar)
    {
        path: 'login', loadComponent: () => import('./components/features/auth/login/login').then(m => m.Login)
    },
    {
        path: 'register', loadComponent: () => import('./components/features/auth/register/register').then(m => m.Register)
    },

    // 2. CUSTOMER ROUTES (Uses MainLayout)
    // Any path inside here gets the Navbar + Footer automatically.
    {
        path: '', component: MainLayout,
        children: [
            { path: '', component: Home },         // http://site.com/

            //site.com/product/1
            { path: 'product/:id', loadComponent: () => import('./components/features/shop/product-details/product-details').then(m => m.ProductDetails) },
            //site.com/search?q=Laptop
            { path: 'search', loadComponent: () => import('./components/features/shop/product-list/product-list').then(m => m.ProductList) },

            { path: 'checkout', loadComponent: () => import('./components/features/cart/checkout/checkout').then(m => m.Checkout) },
            { path: 'order-confirmation/:id', loadComponent: () => import('./components/features/cart/order-success/order-success').then(m => m.OrderSuccess) },
        ]
    },

    // 3. VENDOR ROUTES (Uses VendorLayout)
    // Any path inside here gets the Sidebar + Topbar automatically.
    {
        path: 'vendor',
        component: VendorLayout,
        // (Optional) add canActivate: [AuthGuard] here later to protect it
        children: [
            { path: 'dashboard', component: Dashboard }, // http://site.com/vendor/dashboard

            // { path: 'products', loadComponent: () => import('./components/features/vendor/inventory/inventory.component').then(m => m.InventoryComponent) }
        ]
    },

    // 4. Wildcard (404)
    { path: '**', redirectTo: '' }
];
