import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth-service/auth.service';
import { TaskService } from '../../services/task-service/task.service';
import { Color, ScaleType } from '@swimlane/ngx-charts';
import { API_CONFIG, CHART_CONFIG } from '../../../config';

type ChartConfigKeys = keyof typeof CHART_CONFIG['chartTypeMap'];
type ReadableNamesKeys = keyof typeof CHART_CONFIG['readableNamesMap'];

@Component({
  selector: 'app-user-analytics',
  templateUrl: './user-analytics.component.html',
  styleUrls: ['./user-analytics.component.css']
})
export class UserAnalyticsComponent implements OnInit {
  data: any;
  singleValueProperties: any[] = [];
  chartData: any[] = [];
  colorScheme: Color = {
    domain: CHART_CONFIG.colorScheme.domain,
    group: ScaleType.Ordinal,
    selectable: CHART_CONFIG.colorScheme.selectable,
    name: CHART_CONFIG.colorScheme.name
  };

  constructor(private http: HttpClient, private authService: AuthService, private taskService: TaskService) { }

  async ngOnInit(): Promise<void> {
    await this.loadData();
  }

  async loadData(): Promise<void> {
    await this.taskService.getUserTaskAnalytics().subscribe((result: any) => {
      this.data = result;
      this.parseData();
    });
  }

  parseData(): void {
    for (const key in this.data) {
      if (this.data.hasOwnProperty(key)) {
        const value = this.data[key];
        if (value !== null) {
          if (key.endsWith('Json')) {
            const chartType = CHART_CONFIG.chartTypeMap[key as ChartConfigKeys] || 'bar-vertical';
            this.chartData.push({ name: CHART_CONFIG.readableNamesMap[key as ReadableNamesKeys] || key, value: this.transformChartData(value), chartType: chartType });
          } else {
            this.singleValueProperties.push({ key: CHART_CONFIG.readableNamesMap[key as ReadableNamesKeys] || key, value: value });
          }
        }
      }
    }
  }

  transformChartData(rawData: string): any[] {
    const entries = rawData.split(',').filter(entry => entry);
    return entries.map(entry => {
      const [name, value] = entry.split(':');
      return { name, value: +value };
    });
  }
}
