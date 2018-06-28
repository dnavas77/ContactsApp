import { Component, Inject, OnInit, TemplateRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';

// Modal
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { Router } from '@angular/router';

@Component({
    selector: 'contacts',
    templateUrl: './contacts.component.html',
    styleUrls: ['./contacts.component.css']
})
export class ContactsComponent implements OnInit {
    public contacts: Contact[] = [];
    public fetchingContacts: boolean = false;

    modalRef?: BsModalRef;
    contactToDelete?: Contact;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string,
        private modalService: BsModalService,
        private router: Router
    ) { }

    ngOnInit() {
        this.GetContacts();
    }

    GetContacts(): void {
        this.fetchingContacts = true;
        this.http.get <Contact[]>(this.baseUrl + 'api/contacts').subscribe(result => {
            this.contacts = result;
            this.fetchingContacts = false;
        }, error => {
            console.error(error);
            this.fetchingContacts = false;
        });
    }

    openModal(template: TemplateRef<any>, contact: Contact) {
        this.modalRef = this.modalService.show(template);
        this.contactToDelete = contact;
    }

    delete() {
        if (this.contactToDelete) {
            let _ = this.modalRef ? this.modalRef.hide() : null;
            this.http.delete('api/contacts/' + this.contactToDelete.contactID).subscribe(result => {
                this.contacts = this.contacts.filter(cont => {
                    let _id = this.contactToDelete ? this.contactToDelete.contactID : '';
                    return cont.contactID !== _id;
                });
            });
        }
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
