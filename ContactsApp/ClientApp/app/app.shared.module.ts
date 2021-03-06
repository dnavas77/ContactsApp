import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';

import { ContactsComponent } from './components/contacts/contacts.component';
import { NewContactComponent } from './components/contacts/new/new.component';
import { EditContactComponent } from './components/contacts/edit/edit.component';
import { ContactFormComponent } from './components/contacts/form/form.component';

// ngx-bootstrap
import { BsDatepickerModule, DatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';

// ngx-select
import { NgxSelectModule } from 'ngx-select-ex';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        ContactsComponent,
        NewContactComponent,
        EditContactComponent,
        ContactFormComponent,
        HomeComponent
    ],
    imports: [
        PaginationModule.forRoot(),
        NgxSelectModule,
        BsDatepickerModule.forRoot(),
        DatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        ModalModule.forRoot(),
        CommonModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'contacts', component: ContactsComponent },
            { path: 'contacts/new', component: NewContactComponent },
            { path: 'contacts/edit/:id', component: EditContactComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
