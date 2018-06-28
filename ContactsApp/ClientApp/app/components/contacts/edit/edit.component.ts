import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'edit-contact',
    templateUrl: './edit.component.html'
})
export class EditContactComponent implements OnInit {
    public contactModel?: Contact;

    constructor(
        private activeRoute: ActivatedRoute,
        private http: HttpClient
    ) { }

    submitNewContact() {
        console.warn('submitted..');
    }

    cancelAdd() {
        console.warn('cancel add..');
    }

    ngOnInit() {
        let _id = this.activeRoute.snapshot.params.id;
        this.getContactModel(_id);
    }

    getContactModel(id: string) {
        this.http.get<Contact>('api/contacts/' + id).subscribe(result => {
            this.contactModel = result;
        });
    }
}

interface Contact {
    contactID: string;
    firstName: string;
    lastName: number;
    email: number;
    phone: string,
    birthday: Date;
    profilePicture: string;
    comments: string;
}
