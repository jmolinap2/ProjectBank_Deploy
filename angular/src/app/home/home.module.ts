import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';

import { NgChartsModule } from 'ng2-charts';

@NgModule({
    declarations: [HomeComponent],
    imports: [SharedModule,NgChartsModule, HomeRoutingModule, CommonModule,ChartModule],
})
export class HomeModule {}
