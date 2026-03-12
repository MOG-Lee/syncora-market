export interface OrderAddress {
    street: string;
    city: string;
    zipCode: string;
}

export interface OrderItem {
    productId: string;
    productName: string;
    unitPrice: number;
    pictureUrl: string;
    units: number;
}

export interface OrderRequest {
    shippingAddress: OrderAddress;
    orderItems: OrderItem[];
}

export enum OrderStatus {
    Submitted = 'Submitted',
    AwaitingValidation = 'AwaitingValidation',
    StockConfirmed = 'StockConfirmed',
    Paid = 'Paid',
    Shipped = 'Shipped',
    Cancelled = 'Cancelled'
}

export interface OrderDetail {
    id: string;
    status: OrderStatus | string | number;
    total: number;
    date: string;
}