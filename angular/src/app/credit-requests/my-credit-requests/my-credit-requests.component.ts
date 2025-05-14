import { Component, Injector, OnInit } from '@angular/core';
import { CreditRequestDto, CreditRequestServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/app-component-base';
import { finalize } from 'rxjs/operators';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-my-credit-requests',
  templateUrl: './my-credit-requests.component.html',
  styleUrls: ['./my-credit-requests.component.css']
})
export class MyCreditRequestsComponent extends AppComponentBase implements OnInit {
  creditRequests: CreditRequestDto[] = [];
  isAnalyst = false;

  constructor(
    injector: Injector,
    private _creditRequestService: CreditRequestServiceProxy,
    private cdr: ChangeDetectorRef,
    private _router: Router
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.isAnalyst = this.permission.isGranted('Pages.Analyst');
    this.getCreditRequests();
  }

  exportToExcel(): void {
  const data = this.creditRequests.map(x => ({
    ID: x.id,
    Monto: x.montoSolicitado,
    PlazoMeses: x.plazoMeses,
    IngresoMensual: x.ingresoMensual,
    AntiguedadLaboral: x.antiguedadLaboral,
    Estado: x.estado
  }));

  const worksheet = XLSX.utils.json_to_sheet(data);
  const workbook = { Sheets: { 'Solicitudes': worksheet }, SheetNames: ['Solicitudes'] };
  const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
  const blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
  FileSaver.saveAs(blob, 'SolicitudesCredito.xlsx');
}
exportToPdf(): void {
  const doc = new jsPDF();
  autoTable(doc, {
    head: [['ID', 'Monto', 'Plazo', 'Ingreso', 'Antigüedad', 'Estado']],
    body: this.creditRequests.map(x => [
      x.id,
      x.montoSolicitado,
      x.plazoMeses,
      x.ingresoMensual,
      x.antiguedadLaboral,
      x.estado
    ])
  });
  doc.save('SolicitudesCredito.pdf');
}

  editRequest(id: number): void {
    this._router.navigate(['/app/credit-requests/create', id]);
  }
  deleteRequest(id: number): void {
  this.message.confirm(
    this.l('¿Está seguro de eliminar esta solicitud?'),
    undefined,
    isConfirmed => {
      if (isConfirmed) {
        this._creditRequestService
          .delete(id)
          .pipe(finalize(() => abp.ui.clearBusy()))
          .subscribe(() => {
            this.notify.success('Solicitud eliminada');
            this.getCreditRequests();
          });
      }
    }
  );
}


  getCreditRequests(): void {
    abp.ui.setBusy();
    this._creditRequestService
      .getAll(undefined, 0, 100)
      .pipe(finalize(() => abp.ui.clearBusy()))
      .subscribe(result => {
        this.creditRequests = result.items || [];
        this.cdr.detectChanges();
      });
  }
}
