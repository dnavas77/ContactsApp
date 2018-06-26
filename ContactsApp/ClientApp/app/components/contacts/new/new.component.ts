import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'new-contact',
    templateUrl: './new.component.html'
})
export class NewContactComponent {

    submitNewContact() {
        console.warn('submitted..');
    }

    cancelAdd() {
        console.warn('cancel add..');
    }
}
