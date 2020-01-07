import { Component, ChangeDetectorRef, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-counter-component',
    templateUrl: './counter.component.html'
})
export class CounterComponent {
    productForm: FormGroup;
    _baseUrl: string;
    constructor(private router: Router, private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
        private formBuilder: FormBuilder, private cd: ChangeDetectorRef) {
        this._baseUrl = baseUrl;
        this.createForm();
    }

    addProduct() {
        if (this.productForm.valid) {
            const formData = new FormData();
            formData.append('file', this.productForm.value.file, this.productForm.value.fileName);
            formData.append('name', this.productForm.value.name);
            formData.append('category', this.productForm.value.category);
            formData.append('price', this.productForm.value.price);
            this.http.post(this._baseUrl + 'api/products', formData).subscribe(result => {
                alert('added');
            }, error => console.error(error));
        }
    }

    private createForm() {
        let category = '';
        let name = '';
        let price = 0;

        this.productForm = this.formBuilder.group({
            category: [category, Validators.required],
            name: [name, Validators.required],
            price: [price, Validators.required],
            file: new FormControl(null, [Validators.required]),
            fileName: new FormControl(null),
        });
    }
    onFileChange(files: FileList) {
        this.productForm.patchValue({
            file: files[0],
            fileName: files[0].name 
        });
        
    }
}
