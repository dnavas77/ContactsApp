import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { Event } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";

// Modal
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
    selector: 'contact-form',
    templateUrl: './form.component.html'
})
export class ContactFormComponent implements OnInit {
    @Input()
    actionType: string = "";

    @Input()
    contactModel?: Contact;

    modalRef?: BsModalRef;
    invalidImage: boolean = false;
    photo: File | null = null;
    contactForm!: FormGroup;

    constructor(
        private http: HttpClient,
        private router: Router,
        private modalService: BsModalService
    ) { }

    openModal(template: TemplateRef<any>) {
        this.modalRef = this.modalService.show(template);
    }

    ngOnInit() {
        this.contactForm = new FormGroup({
            firstName: new FormControl('', Validators.required),
            lastName: new FormControl('', Validators.required),
            email: new FormControl('', [
                Validators.required,
                Validators.pattern(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)
            ]),
            phone: new FormControl('', Validators.pattern(/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/)),
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
        formData.append('Phone', this.contactForm.controls.phone.value);
        formData.append('Comments', this.contactForm.controls.comments.value);
        if (this.contactForm.controls.birthday.value) {
            let _date = (new Date(this.contactForm.controls.birthday.value)).toLocaleDateString('en-us');
            formData.append('Birthday', _date);
        }
        if (this.photo && this.photo.type) {
            formData.append('ProfilePicture', this.photo);
        }

        const uploadReq = new HttpRequest('POST', 'api/contacts', formData);

        this.http.request(uploadReq).subscribe((event: any) => {
            this.router.navigate(['/contacts']);
        }, error => {
            console.warn(error);
        });
    }

    cancelAdd() {
        this.router.navigate(['/contacts']);
        if (this.modalRef) {
            this.modalRef.hide();
        }
    }
}

interface Contact {
    contactID: string;
    firstName: string;
    lastName: number;
    email: number;
    birthday: Date;
    profilePicture: string;
    comments: string;
}
