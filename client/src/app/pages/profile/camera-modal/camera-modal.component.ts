import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { Slika } from 'src/app/shared/models/slika.model';

@Component({
  selector: 'app-camera-modal',
  templateUrl: './camera-modal.component.html',
  styleUrls: ['./camera-modal.component.scss']
})
export class CameraModalComponent {
  private trigger: Subject<any> = new Subject();
  public formControl: FormControl = new FormControl();
  constructor() {}

  ngOnDestroy(): void {
    this.trigger.next(null);
    this.trigger.complete();
  }

  ngOnInit(): void {

  }

  getSnapshot(): void {
    this.trigger.next(void 0);
  }

  captureImg(webcamImage: any): void {
    const value: Slika = {
      Base64: webcamImage.imageAsBase64,
      Mime: webcamImage._mimeType
    }
    this.formControl.setValue(value);
    console.log(this.formControl.value.Mime + this.formControl.value.Base64)
  }

  get invokeObservable(): Observable<any> {
    return this.trigger.asObservable();
  }
}
