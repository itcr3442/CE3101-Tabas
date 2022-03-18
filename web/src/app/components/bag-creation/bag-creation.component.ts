import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Bags } from 'src/app/interfaces/Bags.model';
import { lastValueFrom } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { RepositoryService } from 'src/app/services/repository.service';
import { BagsListComponent } from '../bags-list/bags-list.component';
import jsPDF from 'jspdf';

@Component({
  selector: 'app-bag-creation',
  templateUrl: './bag-creation.component.html',
  styleUrls: ['./bag-creation.component.css']
})
export class BagCreationComponent implements OnInit {

  public bags_list!: Bags[];

  registerForm = new FormGroup({
    id: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    nvuelo: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    peso: new FormControl('', [Validators.required, Validators.pattern('[0-9.]*')]),
    costo: new FormControl('', [Validators.required, Validators.pattern('[0-9.]*')]),
    color: new FormControl('', [Validators.required, Validators.pattern('#[0-9a-fA-F]{6}')]),
  })
  message: string = ""

  constructor(
    private router: Router,
    private repo: RepositoryService,
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.getAllBags();
  }

  get id() {
    return this.registerForm.controls['id'].value
  }
  get nvuelo() {
    return this.registerForm.controls['nvuelo'].value
  }
  get peso() {
    return this.registerForm.controls['peso'].value
  }
  get costo() {
    return this.registerForm.controls['costo'].value
  }

  set color(new_color: string) {
    this.registerForm.controls['color'].setValue(new_color)
  }

  get color() {
    return this.registerForm.controls['color'].value
  }

  public getAllBags = () => {
    let registerUrl = "maletas"
    this.repo.getData(registerUrl)
      .subscribe(res => {
        console.log("Result:" + JSON.stringify(res));
        this.bags_list = res as Bags[];
      }
      )
  }

  onSubmit() {
    if (this.registerForm.valid) {

      if (!this.authService.isLoggedIn()) {
        this.router.navigate(['/login/redirect']);
        return
      }

      let token = this.authService.getCredentials()

      let registerUrl = "maletas?cedula=" + token.id + "&password_hash=" + token.password

      let int_color = parseInt(this.color.substring(1), 16)

      let new_bag = {
        "cedula_usuario": this.id,
        "nvuelo": this.nvuelo,
        "color": int_color,
        "peso": this.peso,
        "costo_envio": this.costo
      }

      console.log("New bag: " + JSON.stringify(new_bag))

      console.log("POST url: " + registerUrl)

      this.repo.create(
        registerUrl, new_bag).subscribe(res => {
          console.log("post result: " + JSON.stringify(res))

          if ((<any>res).success === 1) {
            console.log("Bag register successful");
            this.message = ""
            this.generateXML_PDF(this.id, this.costo, this.peso)
          }
          else if ((<any>res).success === -1) {
            this.message = "La maleta ya existe o su usuario no es válido.";
          } else if ((<any>res).success === -2) {
            this.message = "La cédula ingresada no está registrada para ningún usuario en el sistema.";
          }
          else {
            this.message = "Código de error desconocido: " + (<any>res).success;
          }
        }
        )


    }
    else {
      this.message = "Por favor verifique que ingresó todos los campos en su formato correcto";
    }
  }

  getRndDigits(n: number) {
    return this.getRndInteger(10 ** (n - 1), 10 ** n)
  }

  getRndInteger(min: number, max: number) {
    return Math.floor(Math.random() * (max - min)) + min;
  }

  async generateXML_PDF(cedula_usuario: string, costo: number, peso: string) {
    const d = new Date();
    const day = d.getDay().toString()
    const month = d.getMonth().toString()
    const year = d.getFullYear().toString()
    const hour = d.getHours().toString()
    const mins = d.getMinutes().toString()
    const secs = d.getSeconds().toString()
    const num_consecutiva = this.getRndDigits(20)
    const iva13: string = (costo * 0.13) + ""

    let usuario: any = await lastValueFrom(this.repo.getData("usuarios/" + cedula_usuario))
    console.log("Usuario: " + JSON.stringify(usuario))
    let nombre_completo = usuario.nombre + " " + usuario.primer_apellido + " " + usuario.segundo_apellido


    const clave = "506" + day.padStart(2, "0") + month.padStart(2, "0") + year.substring(2) + this.id.padStart(12, "0") + num_consecutiva + "1" + this.getRndDigits(8)
    {
      /// XML
      let xmlDoc = document.implementation.createDocument(null, "FacturaElectronica");

      let root = xmlDoc.documentElement;
      root.setAttribute("xmlns:ds", "http://www.w3.org/2000/09/xmldsig#");
      root.setAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
      root.setAttribute("xmlns", "https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.3/facturaElectronica");
      root.setAttribute("xsi:schemaLocation", "https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.3/facturaElectronica https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.3/facturaElectronica.xsd");

      let clave_node = xmlDoc.createElement("Clave")
      clave_node.appendChild(xmlDoc.createTextNode(clave))
      root.appendChild(clave_node)

      let codigo_node = xmlDoc.createElement("CodigoActividad")
      codigo_node.appendChild(xmlDoc.createTextNode("621001")) //SERVICIO DE TRANSPORTE DE CARGA POR VIA AEREA
      root.appendChild(codigo_node)

      let consecutivo_node = xmlDoc.createElement("NumeroConsecutivo")
      let numeracion = Math.floor(this.getRndDigits(10))
      consecutivo_node.appendChild(xmlDoc.createTextNode("0010000101" + numeracion)) //001 = ofi central, 00001 = punto de venta, 01 = FE, 
      root.appendChild(consecutivo_node)

      let fecha_node = xmlDoc.createElement("FechaEmision")
      let fecha_completa = year + "-" + month.padStart(2, "0") + "-" + day.padStart(2, "0") + "T" + hour.padStart(2, "0") + ":" + mins.padStart(2, "0") + ":" + secs.padStart(2, "0") + "-06:00"
      fecha_node.appendChild(xmlDoc.createTextNode(fecha_completa))
      root.appendChild(fecha_node)

      let condicion_venta_node = xmlDoc.createElement("CondicionVenta")
      condicion_venta_node.appendChild(xmlDoc.createTextNode("01")) //01 = al contado
      root.appendChild(condicion_venta_node)

      let medio_pago_node = xmlDoc.createElement("MedioPago")
      medio_pago_node.appendChild(xmlDoc.createTextNode("0" + Math.floor(Math.random() * 2) + 1)) //01/02 = efectivo/tarjeta
      root.appendChild(medio_pago_node)

      let emisor = xmlDoc.createElement("Emisor")

      let emisor_name_node = xmlDoc.createElement("Nombre")
      emisor_name_node.appendChild(xmlDoc.createTextNode("TECAIRLINES SOCIEDAD ANÓNIMA"))
      emisor.appendChild(emisor_name_node)

      let emisor_id_node = xmlDoc.createElement("Identificación")
      let emisor_id_type_node = xmlDoc.createElement("Tipo")
      let emisor_id_num_node = xmlDoc.createElement("Numero")
      emisor_id_type_node.appendChild(xmlDoc.createTextNode("02")) // 02 = cédula jurídica
      emisor_id_num_node.appendChild(xmlDoc.createTextNode("3101573381"))
      emisor_id_node.appendChild(emisor_id_type_node)
      emisor_id_node.appendChild(emisor_id_num_node)
      emisor.appendChild(emisor_id_node)

      let emisor_commercial_node = xmlDoc.createElement("NombreComercial")
      emisor_commercial_node.appendChild(xmlDoc.createTextNode("TEC AIRLINES"))
      emisor.appendChild(emisor_commercial_node)

      let emisor_email_node = xmlDoc.createElement("CorreoElectronico")
      emisor_email_node.appendChild(xmlDoc.createTextNode("contabilidad@tecairlines.com"))
      emisor.appendChild(emisor_email_node)

      let emisor_tel_node = xmlDoc.createElement("Telefono")
      let emisor_tel_country_node = xmlDoc.createElement("CodigoPais")
      let emisor_tel_num_node = xmlDoc.createElement("NumTelefono")
      emisor_tel_country_node.appendChild(xmlDoc.createTextNode("506"))
      emisor_tel_num_node.appendChild(xmlDoc.createTextNode("22694200"))
      emisor_tel_node.appendChild(emisor_tel_country_node)
      emisor_tel_node.appendChild(emisor_tel_num_node)
      emisor.appendChild(emisor_tel_node)

      let emisor_loc_node = xmlDoc.createElement("Ubicacion")

      let emisor_loc_provincia_node = xmlDoc.createElement("Provincia")
      let emisor_loc_canton_node = xmlDoc.createElement("Canton")
      let emisor_loc_distrito_node = xmlDoc.createElement("Distrito")
      let emisor_loc_barrio_node = xmlDoc.createElement("Barrio")
      let emisor_loc_senas_node = xmlDoc.createElement("OtrasSenas")
      emisor_loc_provincia_node.appendChild(xmlDoc.createTextNode("2")) // Alajuela
      emisor_loc_canton_node.appendChild(xmlDoc.createTextNode("01")) // Alajuela
      emisor_loc_distrito_node.appendChild(xmlDoc.createTextNode("05")) // Guácima
      emisor_loc_barrio_node.appendChild(xmlDoc.createTextNode("04")) // Coco
      emisor_loc_senas_node.appendChild(xmlDoc.createTextNode("Aeropuerto Juan Santamaría"))
      emisor_loc_node.appendChild(emisor_loc_provincia_node)
      emisor_loc_node.appendChild(emisor_loc_canton_node)
      emisor_loc_node.appendChild(emisor_loc_distrito_node)
      emisor_loc_node.appendChild(emisor_loc_barrio_node)
      emisor_loc_node.appendChild(emisor_loc_senas_node)

      emisor.appendChild(emisor_loc_node)

      root.appendChild(emisor)

      let receptor = xmlDoc.createElement("Receptor")

      let receptor_name_node = xmlDoc.createElement("Nombre")
      receptor_name_node.appendChild(xmlDoc.createTextNode(nombre_completo.toUpperCase()))
      receptor.appendChild(receptor_name_node)

      let receptor_id_node = xmlDoc.createElement("Identificación")
      let receptor_id_type_node = xmlDoc.createElement("Tipo")
      let receptor_id_num_node = xmlDoc.createElement("Numero")
      receptor_id_type_node.appendChild(xmlDoc.createTextNode("01")) // 01 = cédula física
      receptor_id_num_node.appendChild(xmlDoc.createTextNode(usuario.cedula))
      receptor_id_node.appendChild(receptor_id_type_node)
      receptor_id_node.appendChild(receptor_id_num_node)
      receptor.appendChild(receptor_id_node)

      let receptor_email_node = xmlDoc.createElement("CorreoElectronico")
      receptor_email_node.appendChild(xmlDoc.createTextNode("usuario@email.com"))
      receptor.appendChild(receptor_email_node)

      root.appendChild(receptor)

      let detalle = xmlDoc.createElement("DetalleServicio")

      let linea_detalle = xmlDoc.createElement("LineaDetalle")

      let cod_comercial_node = xmlDoc.createElement("CodigoComercial")
      let cod_comercia_type_node = xmlDoc.createElement("Tipo")
      let cod_comercia_num_node = xmlDoc.createElement("Codigo")
      cod_comercia_type_node.appendChild(xmlDoc.createTextNode("01"))
      cod_comercia_num_node.appendChild(xmlDoc.createTextNode("069"))
      cod_comercial_node.appendChild(cod_comercia_type_node)
      cod_comercial_node.appendChild(cod_comercia_num_node)
      linea_detalle.appendChild(cod_comercial_node)

      let num_linea_node = xmlDoc.createElement("NumeroLinea")
      num_linea_node.appendChild(xmlDoc.createTextNode("1"))
      linea_detalle.appendChild(num_linea_node)

      let cabys_node = xmlDoc.createElement("Codigo")
      cabys_node.appendChild(xmlDoc.createTextNode("6531900000000"))
      linea_detalle.appendChild(cabys_node)

      let cantidad_node = xmlDoc.createElement("Cantidad")
      cantidad_node.appendChild(xmlDoc.createTextNode("1.00"))
      linea_detalle.appendChild(cantidad_node)

      let unidad_node = xmlDoc.createElement("UnidadMedida")
      unidad_node.appendChild(xmlDoc.createTextNode("Unid"))
      linea_detalle.appendChild(unidad_node)

      let detalle_node = xmlDoc.createElement("Detalle")
      detalle_node.appendChild(xmlDoc.createTextNode("Maleta de " + peso + "kg"))
      linea_detalle.appendChild(detalle_node)

      let unitario_node = xmlDoc.createElement("PrecioUnitario")
      unitario_node.appendChild(xmlDoc.createTextNode(costo + ""))
      linea_detalle.appendChild(unitario_node)

      let total_node = xmlDoc.createElement("MontoTotal")
      total_node.appendChild(xmlDoc.createTextNode(costo + ""))
      linea_detalle.appendChild(total_node)

      let subtotal_node = xmlDoc.createElement("SubTotal")
      subtotal_node.appendChild(xmlDoc.createTextNode(costo + ""))
      linea_detalle.appendChild(subtotal_node)

      let impuesto_node = xmlDoc.createElement("Impuesto")
      let impuesto_cod_node = xmlDoc.createElement("Codigo")
      let impuesto_cod_tar_node = xmlDoc.createElement("CodigoTarifa")
      let impuesto_tarifa_node = xmlDoc.createElement("Tarifa")
      let impuesto_monto_node = xmlDoc.createElement("Monto")
      impuesto_cod_node.appendChild(xmlDoc.createTextNode("01"))  // IVA
      impuesto_cod_tar_node.appendChild(xmlDoc.createTextNode("08")) // 13%
      impuesto_tarifa_node.appendChild(xmlDoc.createTextNode("13")) // 13% 
      impuesto_monto_node.appendChild(xmlDoc.createTextNode(iva13))
      impuesto_node.appendChild(impuesto_cod_node)
      impuesto_node.appendChild(impuesto_cod_tar_node)
      impuesto_node.appendChild(impuesto_tarifa_node)
      impuesto_node.appendChild(impuesto_monto_node)

      linea_detalle.appendChild(impuesto_node)

      let imp_neto_node = xmlDoc.createElement("ImpuestoNeto")
      imp_neto_node.appendChild(xmlDoc.createTextNode(iva13))
      linea_detalle.appendChild(imp_neto_node)

      let total_line_node = xmlDoc.createElement("MontoTotalLinea")
      total_line_node.appendChild(xmlDoc.createTextNode((costo * 1.13) + ""))
      linea_detalle.appendChild(total_line_node)

      detalle.appendChild(linea_detalle)
      root.appendChild(detalle)

      let resumen = xmlDoc.createElement("ResumenFactura")

      let currency_node = xmlDoc.createElement("CodigoTipoMoneda")
      let currency_code_node = xmlDoc.createElement("CodigoMoneda")
      let currency_exchange_node = xmlDoc.createElement("TipoCambio")
      currency_code_node.appendChild(xmlDoc.createTextNode("CRC"))
      currency_exchange_node.appendChild(xmlDoc.createTextNode("1"))
      currency_node.appendChild(currency_code_node)
      currency_node.appendChild(currency_exchange_node)
      resumen.appendChild(currency_node)


      let TotalServGravados_node = xmlDoc.createElement("TotalServGravados")
      TotalServGravados_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalServGravados_node)
      let TotalServExentos_node = xmlDoc.createElement("TotalServExentos")
      TotalServExentos_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalServExentos_node)
      let TotalServExonerado_node = xmlDoc.createElement("TotalServExonerado")
      TotalServExonerado_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalServExonerado_node)
      let TotalMercanciasGravadas_node = xmlDoc.createElement("TotalMercanciasGravadas")
      TotalMercanciasGravadas_node.appendChild(xmlDoc.createTextNode(costo + ""))
      resumen.appendChild(TotalMercanciasGravadas_node)
      let TotalMercanciasExentas_node = xmlDoc.createElement("TotalMercanciasExentas")
      TotalMercanciasExentas_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalMercanciasExentas_node)
      let TotalMercExonerada_node = xmlDoc.createElement("TotalMercExonerada")
      TotalMercExonerada_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalMercExonerada_node)
      let TotalGravado_node = xmlDoc.createElement("TotalGravado")
      TotalGravado_node.appendChild(xmlDoc.createTextNode(costo + ""))
      resumen.appendChild(TotalGravado_node)
      let TotalExento_node = xmlDoc.createElement("TotalExento")
      TotalExento_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalExento_node)
      let TotalExonerado_node = xmlDoc.createElement("TotalExonerado")
      TotalExonerado_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalExonerado_node)
      let TotalVenta_node = xmlDoc.createElement("TotalVenta")
      TotalVenta_node.appendChild(xmlDoc.createTextNode(costo + ""))
      resumen.appendChild(TotalVenta_node)
      let TotalDescuentos_node = xmlDoc.createElement("TotalDescuentos")
      TotalDescuentos_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalDescuentos_node)
      let TotalVentaNeta_node = xmlDoc.createElement("TotalVentaNeta")
      TotalVentaNeta_node.appendChild(xmlDoc.createTextNode(costo + ""))
      resumen.appendChild(TotalVentaNeta_node)
      let TotalImpuesto_node = xmlDoc.createElement("TotalImpuesto")
      TotalImpuesto_node.appendChild(xmlDoc.createTextNode(iva13))
      resumen.appendChild(TotalImpuesto_node)
      let TotalIVADevuelto_node = xmlDoc.createElement("TotalIVADevuelto")
      TotalIVADevuelto_node.appendChild(xmlDoc.createTextNode("0"))
      resumen.appendChild(TotalIVADevuelto_node)
      let TotalComprobante_node = xmlDoc.createElement("TotalComprobante")
      TotalComprobante_node.appendChild(xmlDoc.createTextNode((costo * 1.13) + ""))
      resumen.appendChild(TotalComprobante_node)

      root.appendChild(resumen)

      let signature_node = xmlDoc.createElement("ds:Signature")
      signature_node.setAttribute("Id", "Signature-4ed368f7-41d7-4c21-a47c-1eaeeacee7aa")


      let xmltext = new XMLSerializer().serializeToString(xmlDoc)
      xmltext = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + xmltext
      console.log("XML:" + xmltext)

      let bb = new Blob([xmltext], { type: 'text/plain' })

      var pom = document.createElement('a');
      pom.setAttribute('href', window.URL.createObjectURL(bb));
      pom.setAttribute('download', "FE-" + clave + ".xml");

      pom.dataset['downloadurl'] = ['text/plain', pom.download, pom.href].join(':');
      pom.draggable = true;
      pom.classList.add('dragout');

      pom.click();
      nombre_completo
    }

    //PDF
    {
      const pdf = new jsPDF();
      pdf.setFontSize(11)
      pdf.text("Factura electrónica: " + num_consecutiva, 5, 5);
      pdf.text("Clave Numérica: " + clave, 5, 10);
      pdf.text("Fecha de emisión: " + day + "/" + month + "/" + year, 5, 15);

      pdf.setFontSize(8)
      pdf.text("Teléfono: " + "+(506) 2269-4200", 5, 20);
      pdf.text("Correo: " + "contabilidad@tecairlines.com", 5, 23.5);
      pdf.text("Dirección: " + "Alajuela, la Guácima, Aeropuerto Internacional Juan Santamaría", 5, 27);

      pdf.setFontSize(20)
      pdf.text("TECAIRLINES SOCIEDAD ANÓNIMA", 42, 40);

      pdf.setFontSize(9)
      pdf.text("Cédula jurídica: 3101573381", 80, 50);

      pdf.setFontSize(10)
      pdf.text("Receptor: " + nombre_completo, 5, 60)
      pdf.text("Cédula: " + usuario.cedula, 5, 65)
      pdf.text("Teléfono: (506) " + usuario.telefono, 5, 70)

      pdf.setFontSize(18)
      pdf.text("Lineas de Detalle", 75, 95)

      pdf.setFontSize(9)
      pdf.text("Código", 10, 105)
      pdf.text("Cantidad", 23, 105)
      pdf.text("Unidad medida", 37, 105)
      pdf.text("Descripción", 62, 105)
      pdf.text("Precio unitario", 88, 105)
      pdf.text("Descuento", 113, 105)
      pdf.text("Subtotal", 135, 105)
      pdf.text("Monto impuestos", 155, 105)
      pdf.text("Total Factura", 185, 105)

      pdf.setFontSize(7)
      pdf.text("069", 11, 112)
      pdf.text("1.00", 24, 112)
      pdf.text("Unid", 38, 112)
      pdf.text("Maleta de " + peso + "kg", 63, 112)
      pdf.text(costo + "", 89, 112)
      pdf.text("0.00", 113, 112)
      pdf.text(costo + "", 136, 112)
      pdf.text(iva13, 156, 112)
      pdf.text((costo * 1.13) + "", 186, 112)



      pdf.save("FE-" + clave + ".pdf");
    }

  }

}
