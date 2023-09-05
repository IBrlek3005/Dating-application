import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileComponent } from './profile.component';
import { ProfileDetailComponent } from './profile-detail/profile-detail.component';
import { MaterialModule } from 'src/app/shared/material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CameraModalComponent } from './camera-modal/camera-modal.component';
import { WebcamModule } from 'ngx-webcam';


@NgModule({
  declarations: [
    ProfileComponent,
    ProfileDetailComponent,
    CameraModalComponent
  ],
  imports: [
    CommonModule,
    ProfileRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    WebcamModule
  ]
})
export class ProfileModule { }
