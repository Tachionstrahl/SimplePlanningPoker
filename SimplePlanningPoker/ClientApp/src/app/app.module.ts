import { BrowserModule } from '@angular/platform-browser';
import { NgModule, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterModule, RouterStateSnapshot } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { RoomComponent } from './room/room.component';
import { ParticipantComponent } from './participant/participant.component';
import { CardComponent } from './card/card.component';
import { ToastService } from './services/toast.service';
import { ToastComponent } from './toast/toast.component';
import { UsernameComponent } from './username/username.component';

const canActivateRoom: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  if (!localStorage.getItem('username'))
    return inject(Router).createUrlTree(['/username'], {
      queryParams: {
        redirectTo: state.url,
      },
    });
  return true;
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RoomComponent,
    ParticipantComponent,
    CardComponent,
    ToastComponent,
    UsernameComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'room/:roomid', component: RoomComponent, canActivate: [canActivateRoom]},
      { path: 'username', component: UsernameComponent}
    ])
  ],
  providers: [ToastService],
  bootstrap: [AppComponent]
})
export class AppModule { }



