import { inject } from '@angular/core';
import { CanMatchFn, GuardResult, MaybeAsync, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map } from 'rxjs';

export const authGuard: CanMatchFn = (route, segments):MaybeAsync<GuardResult> => {

  const router = inject(Router).createUrlTree(['/login']);

  return inject(AuthService).currentUser$.pipe(
    map((user) => {
      if (user) {
        return true;
      };
      return router;
    })
  );
};
