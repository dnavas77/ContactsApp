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
import { ContactFormComponent } from './components/contacts/form/form.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        ContactsComponent,
        NewContactComponent,
        ContactFormComponent,
        HomeComponent
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'contacts', component: ContactsComponent },
            { path: 'contacts/new', component: NewContactComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
