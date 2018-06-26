import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'fetchdata',
    templateUrl: './contacts.component.html'
})
export class ContactsComponent {
    public contacts: Contact[] = [];
    public fetchingContacts = false;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.GetContacts(http, baseUrl);
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
}

interface Contact {
    ContactID: string;
    FirstName: string;
    LastName: number;
    Email: number;
    Birthday: Date;
    ProfilePicture: string;
}
