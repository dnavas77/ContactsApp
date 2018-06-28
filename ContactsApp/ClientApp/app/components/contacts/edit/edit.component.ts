import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'edit-contact',
    templateUrl: './edit.component.html'
})
export class EditContactComponent implements OnInit {

    constructor(private activeRoute: ActivatedRoute) { }

    submitNewContact() {
        console.warn('submitted..');
    }

    cancelAdd() {
        console.warn('cancel add..');
    }

    ngOnInit() {
        console.warn(this.activeRoute.snapshot.params);
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
