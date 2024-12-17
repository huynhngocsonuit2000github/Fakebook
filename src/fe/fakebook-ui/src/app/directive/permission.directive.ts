import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { PermissionService, UserCredential } from '../services/permission.service';
import { Observable } from 'rxjs';

@Directive({
  selector: '[appHasPermission]',
  standalone: false  // Mark as standalone
})
export class PermissionDirective {
  private permission$: Observable<UserCredential> | null = null;

  @Input() set appHasPermission(permission: string) {
    // Get the observable for the permission
    this.permission$ = this.permissionService.hasPermission(permission);

    // Subscribe to the observable and manage the view
    this.permission$.subscribe((hasPermission) => {
      if (hasPermission.isAuth && hasPermission.validPermission) {
        // If the user has the permission, add the template to the view
        this.viewContainer.createEmbeddedView(this.templateRef);
      } else {
        // If not, clear the view
        this.viewContainer.clear();
      }
    });
  }

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionService: PermissionService
  ) { }
}
