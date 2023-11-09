import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-username',
  templateUrl: './username.component.html',
  styleUrls: ['./username.component.scss']
})
export class UsernameComponent {
  username?: string;
  redirectTo: string|null = null;

  constructor(private route: ActivatedRoute, private router: Router) {
    this.username = localStorage.getItem('username') ?? undefined;
  }

  ngOnInit() {
    this.redirectTo = this.route.snapshot.queryParamMap.get('redirectTo');
  }

  submit() {
    localStorage.setItem('username', this.username!);
    this.router.navigate([this.redirectTo]);
  }


}
