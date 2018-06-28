import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'edit-contact',
    templateUrl: './edit.component.html'
})
export class EditContactComponent implements OnInit {

    submitNewContact() {
        console.warn('submitted..');
    }

    cancelAdd() {
        console.warn('cancel add..');
    }

    ngOnInit() {
        console.log('hey');
    }
}
