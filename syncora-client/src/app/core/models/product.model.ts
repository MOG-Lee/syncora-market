export interface ProductComment {
  userName: string;
  comment: string;
  rating: number;
  createdAt: string;
}

export interface Product {
  id: string; 
  name: string;
  category: string;
  description: string;
  imageFileName: string; 
  price: number;
  attributes: { [key: string]: string }; // Dictionary mapping
  comments: ProductComment[]; // Nested documents
}

export interface Category {
  name: string;
  imageFileName: string;
}
