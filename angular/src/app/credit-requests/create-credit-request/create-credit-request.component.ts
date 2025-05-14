// ✅ Componente: CreateCreditRequestComponent (creación + edición)

import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { CreditRequestServiceProxy, CreditRequestDto } from '@shared/service-proxies/service-proxies';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-create-credit-request',
  templateUrl: './create-credit-request.component.html',
  styleUrls: ['./create-credit-request.component.css']
})
export class CreateCreditRequestComponent extends AppComponentBase implements OnInit {
  form: FormGroup;
  saving = false;
  editId?: number;

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _creditRequestService: CreditRequestServiceProxy,
    private _router: Router,
    private route: ActivatedRoute
  ) {
    super(injector);
    this.form = this.fb.group({
      montoSolicitado: [null, Validators.required],
      plazoMeses: [null, Validators.required],
      ingresoMensual: [null, Validators.required],
      antiguedadLaboral: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    const id = +this.route.snapshot.params['id'];
    if (id) {
      this.editId = id;
      this.loadRequestForEdit(id);
    }
  }

  loadRequestForEdit(id: number): void {
    this._creditRequestService.get(id).subscribe(result => {
      this.form.patchValue(result);
    });
  }
  cancel(): void {
  this._router.navigate(['/app/credit-requests/mine']);
}


  submit(): void {
    if (this.form.invalid) {
      this.notify.warn('Formulario inválido');
      return;
    }

    const input = new CreditRequestDto();
    input.init(this.form.value);
    if (this.editId) input.id = this.editId;

    this.saving = true;
    const request$ = this.editId
      ? this._creditRequestService.update(input)
      : this._creditRequestService.create(input);

    request$
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.editId ? 'Solicitud actualizada exitosamente' : 'Solicitud enviada exitosamente');
        this._router.navigate(['/app/credit-requests/mine']);
      });
  }
}

