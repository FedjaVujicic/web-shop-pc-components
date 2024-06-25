import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { CartDto } from '../models/cart-dto';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  url: string = `${environment.apiBaseUrl}/Cart`;

  cartItems: Array<CartDto> = [];
  cartTotal: number;

  constructor(private http: HttpClient) { }

  getImageSrc(imageFile: any): string {
    if (imageFile === null || imageFile === undefined) {
      return "favicon.ico"
    }
    return 'data:' + imageFile.contentType + ';base64,' + imageFile.fileContents;
  }

  getTotal(cartItems: Array<CartDto>): number {
    let total: number = 0;
    cartItems.forEach(cartItem => {
      total += cartItem.product.price * cartItem.quantity;
    });
    return total;
  }

  getCart(): Subscription {
    return this.http.get(this.url, { withCredentials: true }).subscribe({
      next: (res: Array<CartDto>) => {
        this.cartItems = res;
        this.cartTotal = this.getTotal(this.cartItems);
      },
      error: err => {
        console.log(err.message);
      }
    });;
  }
}
