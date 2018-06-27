import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Event } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
    selector: 'contact-form',
    templateUrl: './form.component.html'
})
export class ContactFormComponent implements OnInit {
    @Input()
    actionType: string = "";

    contactForm!: FormGroup;
    photo: File | null = null;

    ngOnInit() {
        this.contactForm = new FormGroup({
            firstName: new FormControl(),
            lastName: new FormControl(),
            email: new FormControl(),
            phone: new FormControl(),
            birthday: new FormControl(),
            comments: new FormControl(),
        });
    }

    getFile(event: any) {
        this.photo = event.target.files[0];
        const type = this.photo ? this.photo.type : "";
console.warn('222222')
        if (type.indexOf("png") < 0 || type.indexOf("jpeg") < 0 || type.indexOf("gif") < 0) {
console.warn('333333')
            this.photo = null;
            event.target.files = null;
        }

    }

    submitNewContact() {
        console.warn('submitted..');
    }

    cancelAdd() {
        console.warn('cancel add..');
    }
}
