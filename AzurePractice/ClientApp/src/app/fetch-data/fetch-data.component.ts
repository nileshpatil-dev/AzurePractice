import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-fetch-data',
    templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
    public products: Product[];
    _http: HttpClient;
    _baseUrl: string;
    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._http = http;
        this._baseUrl = baseUrl;
        this.fetchProducts();
    }

    fetchProducts = () => {
        this._http.get<Product[]>(this._baseUrl + 'api/products').subscribe(result => {
            this.products = result;
        }, error => console.error(error));
    }

    handleDelete = (product) => {
        if (confirm("Are you sure want to delete?")) {
            this._http.post(this._baseUrl + 'api/products/delete-product', product).subscribe(result => {
                this.fetchProducts();
            }, error => console.error(error));
        }
    }

    handleOrder = (product) => {

        if (confirm("Are you sure want to Order?")) {
            
            const order = {
                productId: product.rowKey,
                price: product.price,
                customerId: 10001,
                qty: 1,
            }
            this._http.post(this._baseUrl + 'api/orders', order).subscribe(result => {
                alert("Congratulations!! Order Placed")
            }, error => console.error(error));
        }
    }
}

interface Product {
    rowKey: string;
    partitionKey: string;
    productName: string;
    price: string;
    imageSrc: string;
}
