import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = '';

  jwttoken = '';
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit() {
    this.loginForm = this.fb.group({
      username: [''],
      password: [''],
    });
    this.jwttoken = '';
  }

  onSubmit(logindata: { username: string; password: string }) {
    this.jwttoken = '';

    this.http
      .post<string>('https://localhost:7160/login', logindata, {
        observe: 'response',
      })
      .subscribe({
        next: (data) => {
          if (data.body['token']) {
            console.log(data);
            this.title = String(data.status);
            this.jwttoken = data.body['token'];
          }
        },
        error: (err) => {
          this.title = err.status;
          console.log('Some error occured', err);
        },
        complete() {
          console.log('Login Authorized');
        },
      });
  }

  adminlogin() {
    // Add the JWT token to the request headers
    const headers = {
      Authorization: `Bearer ${this.jwttoken}`,
    };

    // Send the GET request with the headers
    this.http.get('https://localhost:7160/example', { headers }).subscribe({
      next: (data) => {
        console.log(data);
      },
      error: (err) => {
        console.log('some error occured');
      },
      complete() {
        console.log('Login Successful !');
      },
    });
  }
}
