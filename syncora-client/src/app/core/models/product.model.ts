export interface Product {
  id: string; 
  name: string;
  description: string | null;
  price: number;
  imageUrl: string;
  stockQuantity: number;
  vendorId: string;
  vendorName: string | null;
  category: string;
  averageRating: number;
  isOnFlashSale: boolean | null;
}