import { Component, Input, OnInit } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { Event } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";

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

    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit() {
        this.contactForm = new FormGroup({
            firstName: new FormControl('', Validators.required),
            lastName: new FormControl('', Validators.required),
            email: new FormControl('', [
                Validators.required,
                Validators.pattern("[^ @]*@[^ @]*")
            ]),
            phone: new FormControl('', Validators.pattern("/^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im")),
            birthday: new FormControl(),
            comments: new FormControl('', Validators.maxLength(2000)),
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
        formData.append('FirstName', this.contactForm.controls.firstName.value);
        formData.append('LastName', this.contactForm.controls.lastName.value);
        formData.append('Email', this.contactForm.controls.email.value);

        const uploadReq = new HttpRequest('POST', 'api/contacts', formData);

        this.http.request(uploadReq).subscribe((event: any) => {
            this.router.navigate(['/contacts']);
        });
    }

    cancelAdd() {
        console.warn('cancel add..');
    }
}
