import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { take, tap } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  public loginForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private _messageService: MatSnackBar,
    private _router: Router,
    private _authService: AuthService
    ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (!this.loginForm.valid) {
      this._messageService.open("You didn't fill required fields", undefined,
      {
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
      return;
    }
    
    this._authService.login(this.loginForm.get('username')?.value, this.loginForm.get('password')?.value)
      .pipe(
        take(1),
        tap(_ => this._router.navigate(['/profile']))
      ).subscribe();
  }
}
