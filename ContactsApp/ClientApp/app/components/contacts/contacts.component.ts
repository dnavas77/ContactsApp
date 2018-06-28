import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'contacts',
    templateUrl: './contacts.component.html',
    styleUrls: ['./contacts.component.css']
})
export class ContactsComponent implements OnInit {
    public contacts: Contact[] = [];
    public fetchingContacts: boolean = false;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    ngOnInit() {
        this.GetContacts(this.http, this.baseUrl);
    }

    GetContacts(http: HttpClient, baseUrl: string): void {
        this.fetchingContacts = true;
        http.get <Contact[]>(baseUrl + 'api/contacts').subscribe(result => {
            this.contacts = result;
            this.fetchingContacts = false;
        }, error => {
            console.error(error);
            this.fetchingContacts = false;
        });

    }

    delete() {
        console.warn('deleting...');
    }

    edit() {
        console.warn('editing...');
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
