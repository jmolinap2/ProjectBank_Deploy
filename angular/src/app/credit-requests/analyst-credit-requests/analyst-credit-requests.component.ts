import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CreditRequestServiceProxy,
  CreditRequestDto,
  CreditRequestDtoListResultDto
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-analyst-credit-requests',
  templateUrl: './analyst-credit-requests.component.html'
})
export class AnalystCreditRequestsComponent extends AppComponentBase implements OnInit {
  solicitudes: CreditRequestDto[] = [];
  estado = 'Pendiente';

  constructor(
    injector: Injector,
    private service: CreditRequestServiceProxy,
        private cdr: ChangeDetectorRef,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.obtenerSolicitudes();
  }

  obtenerSolicitudes(): void {
    abp.ui.setBusy(); 
    this.service.getByEstado(this.estado)
      .pipe(finalize(() => abp.ui.clearBusy()))
      .subscribe((result: CreditRequestDtoListResultDto) => {
        this.solicitudes = result.items || [];
        this.cdr.detectChanges();
      });
  }

  aprobar(id: number): void {
    abp.ui.setBusy();
    this.service.approve(id)
      .pipe(finalize(() => abp.ui.clearBusy()))
      .subscribe(() => this.obtenerSolicitudes());
  }

  rechazar(id: number): void {
    abp.ui.setBusy();
    this.service.reject(id)
      .pipe(finalize(() => abp.ui.clearBusy()))
      .subscribe(() => this.obtenerSolicitudes());
  }
}
