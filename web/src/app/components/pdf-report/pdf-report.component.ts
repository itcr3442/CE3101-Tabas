import { Component, OnInit } from '@angular/core';
import { jsPDF } from "jspdf";
import { RepositoryService } from 'src/app/services/repository.service';
import { ConciliacionMaletas } from 'src/app/interfaces/ConciliacionMaletas.model';
import { MaletasXCliente } from 'src/app/interfaces/MaletasXCliente.model';
import { Flights } from 'src/app/interfaces/Fligths.model';
import { Users } from 'src/app/interfaces/Users.model';

@Component({
  selector: 'app-pdf-report',
  templateUrl: './pdf-report.component.html',
  styleUrls: ['./pdf-report.component.css']
})
export class PdfReportComponent implements OnInit {
  public conciliacionMaletas: ConciliacionMaletas[] = [];
  public maletasXCliente: MaletasXCliente[] = [];

  constructor(
    private repo: RepositoryService
  ) { }

  ngOnInit(): void {
    this.repo.getData('usuarios').subscribe(res => {
      for(const usuario of res as Users[]) {
        const url = `reportes/maletas_x_cliente/${usuario.cedula}`;
        this.repo.getData(url).subscribe(res => {
          const cliente = res as MaletasXCliente;
          if(cliente.maletas.length != 0) {
            this.maletasXCliente.push(cliente);
          }
        });
      }
    });

    this.repo.getData('vuelos').subscribe(res => {
      for(const vuelo of res as Flights[]) {
        const url = `reportes/conciliacion_maletas/${vuelo.numero}`;
        this.repo.getData(url).subscribe(res => {
          this.conciliacionMaletas.push(res as ConciliacionMaletas);
        });
      }
    });
  }

  download(): void {
    const doc = new jsPDF();
    doc.html(document.getElementById('report')!, {
      callback: function(doc) {
        window.open(window.URL.createObjectURL(doc.output('blob')));
      },
      html2canvas: {
        scale: doc.internal.pageSize.getWidth() / window.innerWidth,
        useCORS: true,
        windowWidth: 1000,
      },
    });
  }
}
