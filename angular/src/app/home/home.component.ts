import { Component, Injector, ChangeDetectionStrategy, ChangeDetectorRef, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chart, ChartConfiguration, ChartOptions } from 'chart.js';
import { TreemapController, TreemapElement } from 'chartjs-chart-treemap';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Cliente } from '../models/credit.model';

Chart.register(TreemapController, TreemapElement, ChartDataLabels);

@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('treemapRegionCanvas', { static: false }) treemapCanvas!: ElementRef<HTMLCanvasElement>;

  clientes: Cliente[] = [];
  clientesOriginal: Cliente[] = [];

  filtroSucursal: string = '';
  filtroAnio: number | string = '';

  sucursalesUnicas: string[] = [];
  aniosUnicos: number[] = [];

  totalCreditos = 0;
  totalPagos = 0;
  totalClientes = 0;
  totalAprobados = 0;
  totalRechazados = 0;

  tabSeleccionado: string = 'creditos';
  private treemapInstance: any = null;

  public creditChartOptions: ChartOptions<'bar'> = { responsive: true, plugins: { legend: { display: true, position: 'top' }, title: { display: true, text: 'Créditos otorgados por mes' } } };
  public creditChartData: ChartConfiguration<'bar'>['data'] = { labels: [], datasets: [] };

  public creditTypeChartOptions: ChartOptions<'doughnut'> = { responsive: true, plugins: { legend: { position: 'right' }, title: { display: true, text: 'Tipos de Crédito' } } };
  public creditTypeChartData: ChartConfiguration<'doughnut'>['data'] = { labels: [], datasets: [] };

  public yearlyIncomeChartOptions: ChartOptions<'line'> = { responsive: true, plugins: { legend: { display: true, position: 'bottom' }, title: { display: true, text: 'Ingresos por Año (en miles USD)' } } };
  public yearlyIncomeChartData: ChartConfiguration<'line'>['data'] = { labels: [], datasets: [] };

  public branchPerformanceChartOptions: ChartOptions<'radar'> = { responsive: true, plugins: { legend: { position: 'top' }, title: { display: true, text: 'Desempeño por Sucursal' } } };
  public branchPerformanceChartData: ChartConfiguration<'radar'>['data'] = { labels: [], datasets: [] };

  public regionTreemapChartData: any = { datasets: [] };
  public regionTreemapChartOptions: any = {};

  constructor(injector: Injector, private http: HttpClient, private cd: ChangeDetectorRef) {
    super(injector);
  }

  ngOnInit(): void {
    this.http.get<any>('assets/dashboard_creditos_simulado_completo.json').subscribe(data => {
      this.clientes = data.clientes;
      this.clientesOriginal = [...this.clientes];
      this.sucursalesUnicas = [...new Set(this.clientes.flatMap(c => c.creditos.map(cr => cr.sucursal)).filter(Boolean))];
      this.aniosUnicos = [...new Set(this.clientes.flatMap(c => c.creditos.map(cr => new Date(cr.fechaInicio).getFullYear())))] as number[];
      this.actualizarGraficos();
    });
  }

  ngAfterViewInit(): void {
    if (this.regionTreemapChartData?.datasets?.length) {
      this.cd.detectChanges();
      this.renderTreemapChart();
    }
  }

  aplicarFiltros(): void {
    const filtrados = this.clientesOriginal.filter(cliente =>
      cliente.creditos.some(c =>
        (!this.filtroSucursal || c.sucursal === this.filtroSucursal) &&
        (!this.filtroAnio || new Date(c.fechaInicio).getFullYear() === +this.filtroAnio)
      )
    );
    this.clientes = filtrados;
    this.actualizarGraficos();
  }

  actualizarGraficos(): void {
    const meses = ['Ene','Feb','Mar','Abr','May','Jun','Jul','Ago','Sep','Oct','Nov','Dic'];
    const creditosPorMes = new Array(12).fill(0);
    const tiposCredito: Record<string, number> = {};
    const ingresosPorAnio: Record<number, number> = {};
    const totalPorRegion: Record<string, number> = {};
    const radarLabels = ['Aprobación','Colocación','Recuperación','Mora baja','Atención'];
    const sucursales = this.filtroSucursal ? [this.filtroSucursal] : [...new Set(this.clientes.flatMap(c => c.creditos.map(cr => cr.sucursal)))].filter(Boolean);

    let totalAprobados = 0;
    let totalRechazados = 0;
    let totalCreditos = 0;
    let totalPagos = 0;

    this.clientes.forEach(cliente => {
      cliente.creditos.forEach(credito => {
        const fecha = new Date(credito.fechaInicio);
        creditosPorMes[fecha.getMonth()]++;
        tiposCredito[credito.tipo] = (tiposCredito[credito.tipo] || 0) + 1;
        ingresosPorAnio[fecha.getFullYear()] = (ingresosPorAnio[fecha.getFullYear()] || 0) + credito.monto;
        totalPorRegion[credito.region] = (totalPorRegion[credito.region] || 0) + credito.monto;

        totalCreditos++;
        totalPagos += credito.pagos?.reduce((s, p) => s + p.monto, 0) || 0;
        if (credito.estado === 'Aprobado') totalAprobados++;
        else if (credito.estado === 'Rechazado') totalRechazados++;
      });
    });

    this.creditChartData = { labels: meses, datasets: [{ data: creditosPorMes, label: 'Créditos otorgados', backgroundColor: '#42A5F5' }] };
    this.creditTypeChartData = { labels: Object.keys(tiposCredito), datasets: [{ data: Object.values(tiposCredito), backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#81C784', '#BA68C8', '#90A4AE'] }] };
    this.yearlyIncomeChartData = { labels: Object.keys(ingresosPorAnio), datasets: [{ data: Object.values(ingresosPorAnio), label: 'Ingresos', borderColor: '#AB47BC', fill: false, tension: 0.3 }] };

    const colorPalette = ['#3F51B5','#FF9800','#4CAF50','#E91E63','#9C27B0','#00BCD4','#CDDC39','#F44336','#009688','#FFC107'];
    this.branchPerformanceChartData = {
      labels: radarLabels,
      datasets: sucursales.map((sucursal, i) => {
        const cr = this.clientes.flatMap(c => c.creditos).filter(c => c.sucursal === sucursal);
        const aprobados = cr.filter(c => c.estado === 'Aprobado').length;
        const sinMora = cr.filter(c => c.estado !== 'En mora').length;
        const pagosTotales = cr.flatMap(c => c.pagos).length;
        const pagosHechos = cr.flatMap(c => c.pagos.filter(p => p.monto > 0)).length;
        const total = cr.length;

        const color = colorPalette[i % colorPalette.length];
        return {
          label: `Sucursal ${sucursal}`,
          data: [
            total > 0 ? Math.round(aprobados / total * 100) : 0,
            total,
            pagosTotales > 0 ? Math.round(pagosHechos / pagosTotales * 100) : 0,
            total > 0 ? Math.round(sinMora / total * 100) : 0,
            Math.floor(Math.random() * 30) + 70
          ],
          backgroundColor: this.hexToRgba(color, 0.3),
          borderColor: color,
          pointBackgroundColor: color,
          pointBorderColor: '#fff',
          pointHoverBackgroundColor: '#fff',
          pointHoverBorderColor: color
        };
      })
    };

    this.regionTreemapChartData = {
      datasets: [
        {
          label: 'Región',
          tree: Object.entries(totalPorRegion).map(([region, value]) => ({ region, value })),
          key: 'value',
          groups: ['region'],
          backgroundColor: (ctx: any) => {
            const region = ctx?.raw?._data?.region ?? 'Desconocida';
            const regionColors: { [key: string]: string } = {
              'Sierra': '#F06292',
              'Costa': '#42A5F5',
              'Amazonía': '#FFCE56',
              'Insular': '#81C784'
            };
            return regionColors[region] || '#BDBDBD';
          },
          borderWidth: 1
        }
      ]
    };

    this.regionTreemapChartOptions = {
      plugins: {
        title: { display: true, text: 'Distribución de Créditos por Región (Monto Total)' },
        legend: { display: false },
        datalabels: {
          formatter: (val: any, ctx: any) => {
            const region = ctx?.chart?.data?.datasets?.[0]?.tree?.[ctx.dataIndex]?.region ?? '';
            const value = ctx?.chart?.data?.datasets?.[0]?.tree?.[ctx.dataIndex]?.value ?? '';
            return `${region}\n$${value.toLocaleString()}`;
          },
          color: '#fff',
          font: { weight: 'bold', size: 14 },
          align: 'center',
          anchor: 'center',
          clamp: true,
          display: (ctx: any) => ctx.dataset.data[ctx.dataIndex].w > 60
        }
      }
    };

    this.totalCreditos = totalCreditos;
    this.totalPagos = totalPagos;
    this.totalClientes = this.clientes.length;
    this.totalAprobados = totalAprobados;
    this.totalRechazados = totalRechazados;

    this.cd.detectChanges();
    this.renderTreemapChart();
  }

  private renderTreemapChart(): void {
    if (this.treemapInstance) this.treemapInstance.destroy();
    if (this.treemapCanvas?.nativeElement) {
      this.treemapInstance = new Chart(this.treemapCanvas.nativeElement, {
        type: 'treemap' as any,
        data: this.regionTreemapChartData,
        options: this.regionTreemapChartOptions
      });
    }
  }

  mostrarTab(tab: string): void {
    this.tabSeleccionado = tab;
    if (tab === 'ingresos') {
      setTimeout(() => this.renderTreemapChart(), 50);
    }
  }

  hexToRgba(hex: string, alpha: number): string {
    const bigint = parseInt(hex.slice(1), 16);
    const r = (bigint >> 16) & 255;
    const g = (bigint >> 8) & 255;
    const b = bigint & 255;
    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
  }
}
