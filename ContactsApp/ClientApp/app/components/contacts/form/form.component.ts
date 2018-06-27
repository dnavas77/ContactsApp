import { Component, Input, OnInit } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { Event } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'contact-form',
    templateUrl: './form.component.html'
})
export class ContactFormComponent implements OnInit {
    @Input()
    actionType: string = "";

    invalidImage: boolean = false;
    photo: File | null = null;
    contactForm!: FormGroup;

    constructor(private http: HttpClient) { }

    ngOnInit() {
        this.contactForm = new FormGroup({
            firstName: new FormControl('', Validators.required),
            lastName: new FormControl('', Validators.required),
            email: new FormControl('', Validators.required),
            phone: new FormControl(),
            birthday: new FormControl(),
            comments: new FormControl(),
        });
    }

    getFile(event: any) {
        this.photo = event.target.files[0];
        const type = this.photo ? this.photo.type : "";
        if (type.indexOf("png") < 0 && type.indexOf("jpeg") < 0 && type.indexOf("gif") < 0) {
            this.photo = null;
            this.invalidImage = true;
        } else {
            this.invalidImage = false;
        }
    }

    submitNewContact() {
        if (this.contactForm.invalid || this.invalidImage) {
            // Notify: there's errors in the form
            return;
        }
        const formData = new FormData();
        formData.set('FirstName', this.contactForm.controls.firstName.value);
        formData.set('LastName', this.contactForm.controls.lastName.value);
        formData.set('Email', this.contactForm.controls.email.value);

        const uploadReq = new HttpRequest('POST', 'api/contacts', formData);

        this.http.request(uploadReq).subscribe((event: any) => {
            console.warn('contact saved....');
        });
    }

    cancelAdd() {
        console.warn('cancel add..');
    }
}
