export interface BasketItem {
  productId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  pictureUrl: string;
}

export interface CustomerBasket {
  buyerId?: string;  
  items: BasketItem[];
}
