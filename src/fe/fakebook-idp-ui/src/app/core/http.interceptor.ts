import { HttpInterceptorFn } from '@angular/common/http';

export const httpRequestInterceptor: HttpInterceptorFn = (req, next) => {

  const modifiedRequest = req.clone({
    setHeaders: {
      Authorization: `Bearer YOUR_TOKEN_HERE`,
    },
  });

  const val = next(modifiedRequest);

  return val;
};