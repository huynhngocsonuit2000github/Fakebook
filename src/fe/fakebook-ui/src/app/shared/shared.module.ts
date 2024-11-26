import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PermissionDirective } from '../directive/permission.directive';

@NgModule({
    declarations: [PermissionDirective],  // Declare the directive here
    imports: [CommonModule],
    exports: [PermissionDirective], // Export it for use in other modules
})
export class SharedModule { }
