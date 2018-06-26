import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'contact-form',
    templateUrl: './form.component.html'
})
export class ContactFormComponent {

    submitNewContact() {
        console.warn('submitted..');
    }

    cancelAdd() {
        console.warn('cancel add..');
    }
}
