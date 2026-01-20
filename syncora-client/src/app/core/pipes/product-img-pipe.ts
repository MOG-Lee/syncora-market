import { Pipe, PipeTransform } from '@angular/core';
import { environment } from '../../../environments/environment';

@Pipe({
  name: 'productImg',
  standalone: true
})
export class ProductImgPipe implements PipeTransform {

  // default size is m
  transform(filename: string | null | undefined, size: 's' | 'm' | 'l' = 'm'): string {

    if (!filename) {
      return `${environment.imageBaseUrl}/placeholder.png`;
    }

    // Input: "a1b2-c3d4.jpg", size: 's' 
    // Output: "a1b2-c3d4_s.jpg"

    const modifiedName = filename.replace('.', `_${size}.`);
    return `${environment.imageBaseUrl}/${modifiedName}`;
  }
}


// <img [src]="item.image | productImg:'s'"