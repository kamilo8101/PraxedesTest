import { HttpErrorResponse, HttpResponse, type HttpInterceptorFn } from '@angular/common/http';
import { CommonService } from '../services/common.service';
import { finalize, tap } from 'rxjs';
import { SweetAlertUtils } from '../shared/sweet-alert';

export const serviceInterceptor: HttpInterceptorFn = (req, next) => {
let loader = CommonService.loaderMap.next(true);
  return next(req).pipe(     
    tap({
      next: (evt) => {
        if(evt instanceof HttpResponse){
              if(evt != null){
                CommonService.loaderMap.next(false);
              }
            }
      },
      error: (error: HttpErrorResponse) => {
        CommonService.loaderMap.next(false);
        debugger;;
        SweetAlertUtils.SwallErro(error.error.Message);
      },
    })
  )
};
