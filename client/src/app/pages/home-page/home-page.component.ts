import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { catchError, of, take, tap } from 'rxjs';
import { environment } from 'src/environments/environmet';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit{
  dateOfBirth?: Date;
  public form!: FormGroup;
  constructor(
    private _formBuilder: FormBuilder,
    private _client: HttpClient,
    private _messageService: MatSnackBar,
    private _router: Router
  ){}
  
  ngOnInit(): void {
    this.form = this._formBuilder.group({
      firstName: [null, {updateOn: 'blur', validators: [Validators.required] }],
      userName: [null, {updateOn: 'blur', validators: [Validators.required] }],
      lastName: [null, {updateOn: 'blur', validators: [Validators.required] }],
      email: [null, {updateOn: 'blur', validators: [Validators.required, Validators.email] }],
      dateOfBirth: [null, {updateOn: 'blur', validators: [Validators.required] }],
      sex: [null, {updateOn: 'blur', validators: [Validators.required] }],
      password: [null, {updateOn: 'blur', validators: [Validators.required] }]
    });
  }

  signIn(): void {
  if(this.form.invalid)
    {
      this._messageService.open("You didn't fill required fields", undefined,
      {
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
      return;
    }
    
    this._client.post(`${environment.apiUrl}/User/registrate`, this.form.value)
      .pipe(
        take(1),
        tap(_ => {
          this._messageService.open('User is created',undefined,
          {
            horizontalPosition: 'right',
            verticalPosition: 'top'
          });

          this._router.navigate(['/login'])
        }),
        catchError(error => {
          console.log(error);
          this._messageService.open(error.error.message, undefined,
          {
            horizontalPosition: 'right',
            verticalPosition: 'top',
            panelClass: [ 'mat-toolbar', 'mat-warn' ]
          });
          return of(error);
        })
      ).subscribe();
  }
}
