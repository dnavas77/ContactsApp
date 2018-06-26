import { Component, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'contact-form',
    templateUrl: './form.component.html'
})
export class ContactFormComponent {
    @Input()
    actionType: string = "";

    submitNewContact() {
        console.warn('submitted..');
    }

    cancelAdd() {
        console.warn('cancel add..');
    }
}
