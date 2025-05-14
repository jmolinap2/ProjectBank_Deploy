export interface Pago {
  fecha: string;
  monto: number;
}

export interface Credito {
  id: number;
  tipo: string;
  estado: string;
  fechaInicio: string;
  monto: number;
  interes: number;
  cuotas: number;
  pagos: Pago[];
  sucursal: string;
  centroCosto: string;
  region: string;
}

export interface Cliente {
  id: number;
  nombre: string;
  documento: string;
  creditos: Credito[];
}
