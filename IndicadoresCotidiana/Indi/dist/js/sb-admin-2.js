/*!
 * Start Bootstrap - SB Admin 2 v3.3.7+1 (http://startbootstrap.com/template-overviews/sb-admin-2)
 * Copyright 2013-2016 Start Bootstrap
 * Licensed under MIT (https://github.com/BlackrockDigital/startbootstrap/blob/gh-pages/LICENSE)
 */
var Meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
var IndicadorActual = 0;
var Indicadores = new Array();
var position = 1;
$(function () {
    try { 
        $('#side-menu').metisMenu();

        var fecha = new Date();
        var anio = fecha.getFullYear();
        var month = fecha.getMonth();

        var primerDia = new Date(anio, month, 1);
        var ultimoDia = new Date(anio, month-1, 0);

        var actionData = "{}";
        
        var datosServicio = new servicioAjax("POST", "../../Servicio.aspx/ObtenDistritos", actionData, ObtenJuzgados);

        for (var x = anio; x>=2019; x--) {
               $('#Anios').append('<option value="' +x+'" selected="selected">'+x+'</option>');
           }

        $('#Anios').val(anio);

        // MESES 

        for (var x = 1; x <= month; x++) {
            if (anio === 2019) {
                if (x >= 9) {
                    $('#Meses').append('<option value="' + x + '">' + Meses[x - 1] + '</option>');
                }
            } else {
                $('#Meses').append('<option value="' + x + '">' + Meses[x - 1] + '</option>');
            }
        }
        
        $('#Meses').val(month);

        //

    } catch (ex) { console.log(ex); }
});

function ObtenJuzgados(Response) {
    //console.log(Response);


    for (var x = 0; x < Response.d.length; x++) {
        $('#Juzgados').append('<option value="' + Response.d[x].idDistrito + '" selected="selected">' + Response.d[x].Descripcion + '</option>');
    }

    $('#Juzgados').val(Response.d[15].idDistrito);

    ValidaUsuario();

    cargaJuzgados(); 

   // var actionData = "{}";
   // var datosServicio = new servicioAjax("POST", "../../Servicio.aspx/ObtenIndicadores", actionData, ValidaIndicadores);
}

function GeneraInfoGeneral(Response) {
    try {

        $("#Desc1").text(Response.d[0].TotalCarpetas);
        $("#Tex1").text("Total de Carpetas");

        $("#Desc2").text("18");
        $("#Tex2").text("Distritos");

        $("#Desc3").text("");
        $("#Tex3").text("Informacion General");

        $("#morris-bar-chart").empty();
        $("#morris-donut-chart").empty();

        var datos = new Array();
        var Etiquetas = new Array();
        var registro = 0;
        for (var x = 0; x < 18; x++) {
            //console.log(registro);
            var idJuzgado = 0;
            idJuzgado = Response.d[registro].IdJuzgado;
          
            datos.push({
                y: Response.d[registro].DesJuzgado /*+ "/" + Response.d[registro].Mes*/,
                a: parseFloat((Response.d[registro].IdJuzgado == idJuzgado) ? (Response.d[registro].Causas) : 0 ),
                b: parseFloat((Response.d[registro + 1].IdJuzgado == idJuzgado) ? (Response.d[registro + 1].Causas) : 0 ),
                c: parseFloat((Response.d[registro + 2].IdJuzgado == idJuzgado) ? (Response.d[registro + 2].Causas) : 0 ),
                d: parseFloat((Response.d[registro + 3].IdJuzgado == idJuzgado) ? (Response.d[registro + 3].Causas) : 0)
            });
           

            for (var y = 0; y < 4; y++){
                if (Response.d[registro + y].IdJuzgado == idJuzgado) {
                } else { registro--;}
            }
            
            registro += 4;
        }
        Etiquetas.push(ValidaMes(parseInt(Response.d[0].Mes)), ValidaMes(parseInt(Response.d[1].Mes)), ValidaMes(parseInt(Response.d[2].Mes)), ValidaMes(parseInt(Response.d[3].Mes)));
        Morris.Bar({
            element: 'morris-bar-chart',
            data: datos,
            xkey: 'y',
            ykeys: ['a', 'b', 'c', 'd'],
            labels: Etiquetas,//[/*'Series A', 'Series B', 'Series C', 'Series D'*/],
            hideHover: 'auto',
            xLabelAngle: 50,
            resize: true
        });

        Morris.Donut({
            element: 'morris-donut-chart',
            data: [{
                label: "Sin Estatus",
                value: 0
            }],
            colors: [ValidaEstatusIndicador(0, 0)],
            resize: true

        });

       

    } catch (ex) { console.log(ex); generaReporteGraficasPdf(); }
}

function generaReporteGeneralPdf(Response) {
    try {

        $("#Desc1").text(Response.d[0].TotalCarpetas);
        $("#Tex1").text("total de Causas");

        $("#Desc2").text("18");
        $("#Tex2").text("distritos");

        $("#Desc3").text("");
        $("#Tex3").text("Informacion General");

        $("#morris-bar-chart").empty();
        $("#morris-donut-chart").empty();

        $("#morris-bar-chart").empty();
        $("#morris-donut-chart").empty();

        $("#Complemento").empty();
        var htmlStringDatos = '<table id="tablaDatosReporte">'
            + '<tr class="Encabezado"> '
            + '<td><h2 class="page-header"><b>' + 'INDICADORES' + ' ' + $("#Juzgados option:selected").text() + '</b></h2></td><td><img src="data:image/jpeg;base64,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" alt="Smiley face" height="100" width="120" style="float:left"></td>'
            + '</tr> '
            + '<tr class="Encabezado"> '
            + '<td colspan="2"><h4 class="page-header  ">' + '<b>Total de Causas en el periodo:</b> ' + ' ' + Response.d[0].TotalCarpetas + '</h4></td>'
            + '</tr> '
            + '<tr class="Encabezado"> '
            + '<td colspan="2"><h4 class="page-header  ">' + '<b>Distritos:</b> 18' + '</h4></td>'
            + '</tr>'
          + '</table > ';


        $("#Complemento").append(htmlStringDatos);

       generaReporteGraficasPdf();

    } catch (ex) { console.log(ex); }
}

function ValidaIndicadores(Response) {
    try {
       // console.log(Response)
        Indicadores.push(Response.d);
        for (var x = 0; x < Response.d.length; x++) {
            $("#Indicadores").append("<li><a onClick='validaInformacion(" + Response.d[x].idIndicador + "," + '"' + Response.d[x].Descripcion + '"' + ")'>" + Response.d[x].idIndicador+".- "+ Response.d[x].Descripcion + "</a></li>");

        }
        
    } catch (ex) { console.log(ex)};
    };

function validaInformacion(indicador,Descrpcion) {
    try {
        //alert(indicador);
        $("#Complemento").empty();
        IndicadorActual = indicador;
        $("#TitleIndicador").text(Descrpcion +" "+ $("#Juzgados option:selected").text());

        var idJuzgado = $("#Juzgados").val();
        var anio = $("#Anios").val();
        var mes = $("#Meses").val();

        //console.log(idJuzgado);
        //console.log(anio);
        //console.log(mes);

        $("#morris-bar-chart").empty();
        $("#morris-donut-chart").empty();

        if (indicador == 1) {
            $("#Desc1").text("0");
            $("#Tex1").text("Total de encuetas en el mes");
          
            $("#Desc2").text("0");
            $("#Tex2").text("Numero de quejas en el mes");

            $("#Desc3").text("5% o menos");
            $("#Tex3").text("Meta");
        } else if (indicador == 2) {
            $("#Desc1").text("0");
            $("#Tex1").text("Horas de solicitud");

            $("#Desc2").text("0");
            $("#Tex2").text("Horas del despacho de la solicitud");

            $("#Desc3").text("10 min o menos");
            $("#Tex3").text("Meta");
        }else if (indicador == 3) {
            $("#Desc1").text("0");
            $("#Tex1").text("Total de causas en el mes ");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de causas sin salida");

            $("#Desc3").text("30% o menos");
            $("#Tex3").text("Meta");
        }else if (indicador == 4) {
            $("#Desc1").text("0");
            $("#Tex1").text("Total de causas en el mes ");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de causas con salida");

            $("#Desc3").text("100% o mas");
            $("#Tex3").text("Meta");
        }else if (indicador == 5) {
            $("#Desc1").text("0");
            $("#Tex1").text("Total de Meses ");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de causas en tramite");

            $("#Desc3").text("6 meses o menos");
            $("#Tex3").text("Meta");
        }else if (indicador == 6) {
            $("#Desc1").text("0");
            $("#Tex1").text("Total de Meses");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de horas al mes");

            $("#Desc3").text("Tiempo marcado por CNPP");
            $("#Tex3").text("Meta");
        } else if (indicador == 7) {
            $("#Desc1").text("0");
            $("#Tex1").text("Total dentro del termino ");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de audiencias programadas");

            $("#Desc3").text("100%");
            $("#Tex3").text("Meta");
        } else if (indicador == 8) {
            $("#Desc1").text("0");
            $("#Tex1").text("Total de dentro del termino ");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de audiencias programadas");

            $("#Desc3").text("100%");
            $("#Tex3").text("Meta");
        } else if (indicador == 9 || indicador == 10) {
            $("#Desc1").text("0");
            $("#Tex1").text("Total de dentro del termino ");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de audiencias programadas");

            $("#Desc3").text("100%");
            $("#Tex3").text("Meta");
        } else if (indicador == 11) {
            $("#Desc1").text("0");
            $("#Tex1").text("Audiencias desistidas por juzgado");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de causas turnadas por juzgado");

            $("#Desc3").text("5% o menos");
            $("#Tex3").text("Meta");
        }else if (indicador == 12) {
            $("#Desc1").text("0");
            $("#Tex1").text("Audiencias diferidas por juzgado");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de audiencias programadas por juzgado");

            $("#Desc3").text("10% o menos");
            $("#Tex3").text("Meta");
        } else if (indicador == 13) {
            $("#Desc1").text("0");
            $("#Tex1").text("Audiencias diferidas por juzgado");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de audiencias programadas por juzgado");

            $("#Desc3").text("10% o menos");
            $("#Tex3").text("Meta");
        } else if (indicador == 14 || indicador == 15) {
            $("#Desc1").text("0");
            $("#Tex1").text("Audiencias diferidas por juzgado");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de audiencias programadas por juzgado");

            $("#Desc3").text("10% o menos");
            $("#Tex3").text("Meta");
        } else if (indicador == 16) {
            $("#Desc1").text("0");
            $("#Tex1").text("Audiencias programadas en el mes");

            $("#Desc2").text("0");
            $("#Tex2").text("Costo operativo por juzgado");

            $("#Desc3").text("");
            $("#Tex3").text("Meta");
        }else if (indicador == 17) {
            $("#Desc1").text("0");
            $("#Tex1").text("Audiencias iniciadas igual o antes de la hora programada");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de Audiencias programadas");

            $("#Desc3").text("80% o mas");
            $("#Tex3").text("Meta");
        } else if (indicador == 18) {
            $("#Desc1").text("0");
            $("#Tex1").text("Suma (inicio real - inicio programado)");

            $("#Desc2").text("0");
            $("#Tex2").text("Suma (fin real - fin programado)");

            $("#Desc3").text("60 min o menos");
            $("#Tex3").text("Meta");
        } else if (indicador == 19) {
            $("#Desc1").text("0");
            $("#Tex1").text("Horas de duracion de las audiencias por juzgado");

            $("#Desc2").text("0");
            $("#Tex2").text("horas laborales del periodo");

            $("#Desc3").text("mas del 40%,menos del 70%");
            $("#Tex3").text("Meta");
        } else if (indicador == 20) {
            $("#Desc1").text("0");
            $("#Tex1").text("Horas de duracion de las audiencias por sala");

           $("#Desc2").text("0");
            $("#Tex2").text("horas laborales del periodo");

            $("#Desc3").text("mas del 50%,menos del 70%");
            $("#Tex3").text("Meta");
        } else if (indicador == 21) {
            $("#Desc1").text("0");
            $("#Tex1").text("total de audiencias celebradas");

            $("#Desc2").text("0");
            $("#Tex2").text("Total de audiencias respaldadas");

            $("#Desc3").text("100%");
            $("#Tex3").text("Meta");
        }

        var primerDia = new Date(anio, mes , 1);
        var ultimoDia = new Date(anio, mes, 0);

       

        var actionData = "{'idJuzgado': '" + idJuzgado + "','idIndicador': '" + IndicadorActual + "','fechaInicio': '" + ObtenFechaInicio(anio, mes) + "','fechaFin': '" + anio + "-" + mes + "-" + ultimoDia.getDate()+" 23:59:59'}";

        var datosServicio = new servicioAjax("POST", "../../Servicio.aspx/ObtenCalculoIndicadores", actionData, GeneraDatosIndicador);

    } catch (ex) { console.log(ex); }
    }

function ObtenFechaInicio(anio,mes) {
    try {
        var fechaInicioReporte = "" 
        var mesInicio = mes - 4;

        if (mesInicio == 0) {
            fechaInicioReporte = anio + "-01-01 00:00:00";
        } else if (mesInicio < 0) {

            mesInicio = mesInicio + 1;

            fechaInicioReporte = (anio - 1) + "-" + (12 - (mesInicio * -1)) + "-01 00:00:00";

        } else {
            mesInicio = mesInicio + 1;
            fechaInicioReporte = anio + "-" + mesInicio + "-01 00:00:00";
        }
        //console.log(fechaInicioReporte);
        return fechaInicioReporte;
    } catch (ex) { console.log(ex); }
}

function GeneraDatosIndicador(Response) {
    try {
        //console.log(Response);
        datos = new Array();
        $("#MenuIndi").click();
        $("#Desc1").text(Response.d[0].Total);

        $("#Desc2").text(Response.d[0].Porcentaje);
        

        Morris.Donut({
            element: 'morris-donut-chart',
            data: [{
                label: "Estatus " + Response.d[0].mes + "/" + Response.d[0].Anio + ((Response.d[0].idIndicador == 1 || Response.d[0].idIndicador == 3 || Response.d[0].idIndicador == 4 || Response.d[0].idIndicador == 7 || Response.d[0].idIndicador == 8 || Response.d[0].idIndicador == 9 || Response.d[0].idIndicador == 10 || Response.d[0].idIndicador == 11 || Response.d[0].idIndicador == 12 || Response.d[0].idIndicador == 13 || Response.d[0].idIndicador == 14 || Response.d[0].idIndicador == 15 || Response.d[0].idIndicador == 17 || Response.d[0].idIndicador == 19 || Response.d[0].idIndicador == 20 || Response.d[0].idIndicador == 21) ? " en %" : ""),//label: "Estatus " + Response.d[0].mes + "/" + Response.d[0].Anio ,
                value: Response.d[0].Calculo
            }],
            colors: [ValidaEstatusIndicador(IndicadorActual, Response.d[0].Calculo)],
            resize: true
        });


        if (Response.d[0].idIndicador == 5) {
            for (var x = 0; x < 4; x++) {
                var totalMeses = 0;
                for (var y = x; y < Response.d.length; y++) {
                    totalMeses += parseInt(Response.d[y].Porcentaje)
                }

                datos.push({
                    y: Response.d[x].Anio + "/" + Response.d[x].mes,
                    a: parseFloat(Response.d[x].Total / totalMeses).toFixed(2)
                });
                if (x == 0) {

                    $("#morris-donut-chart").empty();

                    $("#Desc1").text(Response.d[x].Total);
                    $("#Desc2").text(totalMeses);

                    Morris.Donut({
                        element: 'morris-donut-chart',
                        data: [{
                            label: "Estatus " + Response.d[0].mes + "/" + Response.d[0].Anio + ((Response.d[0].idIndicador == 1 || Response.d[0].idIndicador == 3 || Response.d[0].idIndicador == 4 || Response.d[0].idIndicador == 7 || Response.d[0].idIndicador == 8 || Response.d[0].idIndicador == 9 || Response.d[0].idIndicador == 10 || Response.d[0].idIndicador == 11 || Response.d[0].idIndicador == 12 || Response.d[0].idIndicador == 13 || Response.d[0].idIndicador == 14 || Response.d[0].idIndicador == 15 || Response.d[0].idIndicador == 17 || Response.d[0].idIndicador == 19 || Response.d[0].idIndicador == 20 || Response.d[0].idIndicador == 21) ? " en %" : ""),//label: "Estatus " + Response.d[0].mes + "/" + Response.d[0].Anio ,
                            value: parseFloat(Response.d[x].Total / totalMeses).toFixed(2)
                        }],
                        colors: [ValidaEstatusIndicador(IndicadorActual, parseFloat(Response.d[x].Total / totalMeses).toFixed(2))],
                        resize: true
                    });


                }
                //console.log(totalMeses);
                //console.log(Response.d[x]);
            }
            

        } else {

            for (var x = 0; x < Response.d.length; x++) {
                // console.log(Response.d[x]);
                datos.push({
                    y: Response.d[x].Anio + "/" + Response.d[x].mes,
                    a: parseFloat(Response.d[x].Calculo)
                });
            }
        }

        Morris.Bar({
            element: 'morris-bar-chart',
            data: datos,
            xkey: 'y',
            ykeys: ['a'],
            labels: ['Indicador'],
            barColors: function (row, series, type) {
                return ValidaEstatusIndicador(IndicadorActual, row.y);
            },
            barOpacity: .8,
            hideHover: 'auto',
            resize: true
        });
        
    } catch (ex) { console.log(ex);}
}

function ValidaEstatusIndicador(indicador, calculo) {
    try {
        var status = "";

        if (indicador == 1) {
            if (calculo > 5) {
                status = "#E60F08"
            } else if (calculo <= 5) {
                status = "#02B005";
            }

        } else if (indicador == 2) {
            if (calculo > 10) {
                status = "#E60F08"
            } else if (calculo <= 10) {
                status = "#02B005";
            }

        } else if (indicador == 3) {
            if (calculo > 30) {
                status = "#E60F08"
            } else if (calculo <= 30) {
                status = "#02B005";
            }

        } /*else if (indicador == 3) {
            if (calculo >= 100) {
                status = "#02B005";
            } else if (calculo < 100) {
                status = "#E60F08"
            }


        } */else if (indicador == 4) {
            if (calculo >= 100) {
                status = "#02B005";
            } else if (calculo < 100) {
                status = "#E60F08"
            }

        } else if (indicador == 5) {
            if (calculo <= 6) {
                status = "#02B005";
            } else if (calculo > 6) {
                status = "#E60F08"
            }

        } else if (indicador == 6) {
            if (calculo <= 24) {
                status = "#02B005";
            } else if (calculo > 24) {
                status = "#E60F08"
            }

        } else if (indicador == 7 || indicador == 8 || indicador == 9 || indicador == 10) {
            if (calculo >= 100) {
                status = "#02B005";
            } else if (calculo < 100) {
                status = "#E60F08"
            }

        } else if (indicador == 11) {
            if (calculo <= 5) {
                status = "#02B005";
            } else if (calculo > 5) {
                status = "#E60F08"
            }

        } else if (indicador == 12 || indicador == 13 || indicador == 14 || indicador == 15) {
            if (calculo <= 10) {
                status = "#02B005";
            } else if (calculo > 10) {
                status = "#E60F08"
            }

        } else if (indicador == 16) {
            status = "#02B005";
            /* if (calculo <= 24) {
                 status = "#02B005";
             } else if (calculo > 24) {
                 status = "#E60F08"
             }*/

        } else if (indicador == 17) {
            if (calculo >= 80) {
                status = "#02B005";
            } else if (calculo < 80) {
                status = "#E60F08"
            }

        } else if (indicador == 18) {
            if (calculo <= 60) {
                 status = "#02B005";
             } else if (calculo > 60) {
                 status = "#E60F08"
             }

        } else if (indicador == 19) {
            if (calculo >= 40 && calculo <= 70) {
                 status = "#02B005";
             } else  {
                 status = "#E60F08"
             }

        } else if (indicador == 20) {
            if (calculo >= 50 && calculo <= 70) {
                status = "#02B005";
            } else {
                status = "#E60F08"
            }

        } else if (indicador == 21) {
            if (calculo == 100) {
                status = "#02B005";
            } else {
                status = "#E60F08"
            }

        }


        return status;
    } catch (ex) { }
}

function ValidaMes(mes) {
    try {
        if (mes == 1) {
            return  "Enero";
        } else if (mes == 2) {
            return "Febrero";
        } else if (mes == 3) {
            return "Marzo";
        } else if (mes == 4) {
            return "Abril";
        } else if (mes == 5) {
            return "Mayo";
        } else if (mes == 6) {
            return "Junio";
        } else if (mes == 7) {
            return "Julio";
        } else if (mes == 8) {
            return "Agosto";
        } else if (mes == 9) {
            return "Septiembre";
        } else if (mes == 10) {
            return "Octubre";
        } else if (mes == 11) {
            return "Noviembre";
        } else if (mes == 12) {
            return "Diciembre";
        }

    } catch (ex) { }
}

function generaReportePdf() {
    try {
        //console.log(Indicadores[0]);
       
        var idJuzgado = $("#Juzgados").val();
        var anio = $("#Anios").val();
        var mes = $("#Meses").val();

        var primerDia = new Date(anio, mes, 1);
        var ultimoDia = new Date(anio, mes, 0);
        
        for (var x = 0; x < Indicadores[0].length; x++) {

            var actionData = "{'idJuzgado': '" + idJuzgado + "','idIndicador': '" + Indicadores[0][x].idIndicador + "','fechaInicio': '" + ObtenFechaInicio(anio, mes) + "','fechaFin': '" + anio + "-" + mes + "-" + ultimoDia.getDate()+" 23:59:59'}";

            var datosServicio = new servicioAjax("POST", "../../Servicio.aspx/ObtenCalculoIndicadores", actionData, CargadDatosTabla);          
            }
  
        } catch (ex) { console.log(ex); }
}

function CargadDatosTabla(Response) {
    try {
        console.log(Response.d);

        var table = $('#example').DataTable();
        if (parseInt(Response.d[0].idIndicador) == 3) {

            $(table.column(5).header()).text(ValidaMes(parseInt(Response.d[0].mes)) + " " + parseInt(Response.d[0].Anio));

            (Response.d.length < 2) ? "" : $(table.column(6).header()).text(ValidaMes(parseInt(Response.d[1].mes)) + " " + parseInt(Response.d[1].Anio));
            (Response.d.length < 3) ? "" : $(table.column(7).header()).text(ValidaMes(parseInt(Response.d[2].mes)) + " " + parseInt(Response.d[2].Anio));
            (Response.d.length < 4) ? "" : $(table.column(8).header()).text(ValidaMes(parseInt(Response.d[3].mes)) + " " + parseInt(Response.d[3].Anio));
        }
        if (Response.d[0].idJuzgado == "0") {
            for (var x = 0; x < Indicadores[0].length; x++) {

                if (parseInt(Response.d[0].idIndicador) == parseInt(Indicadores[0][x].idIndicador )) {
                    var rowNode = table
                        .row.add([Response.d[0].idIndicador, Indicadores[0][x].Descripcion, 0, 0, obtenLabels(parseInt(Response.d[0].idIndicador)), 0, 0, 0,0])
                        .draw()
                        .node();
                }
            }
        } else {
            for (var x = 0; x < Indicadores[0].length; x++) {
               
                if (parseInt(Response.d[0].idIndicador) == parseInt(Indicadores[0][x].idIndicador)) {
                    
                    var rowNode = table
                        .row.add([
                            Response.d[0].idIndicador,
                            Indicadores[0][x].Descripcion,
                            Response.d[0].Total,
                            Response.d[0].Porcentaje,
                            obtenLabels(parseInt(Response.d[0].idIndicador)),
                            parseFloat(Response.d[0].Calculo).toFixed(2),
                            (Response.d.length < 2) ? 0 : parseFloat(Response.d[1].Calculo).toFixed(2),
                            (Response.d.length < 3) ? 0 : parseFloat(Response.d[2].Calculo).toFixed(2),
                            (Response.d.length < 4) ? 0 : parseFloat(Response.d[3].Calculo).toFixed(2)])
                        .draw()
                        .node();
                }
            }
        }
        
       
    } catch (ex) { console.log(ex); }

}

function obtenLabels(indicador) {
    try {
        if (indicador == 1) {
           return "5% o menos";
        } else if (indicador == 2) {
           return"10 min o menos";
        } else if (indicador == 3) {
           return "30% o menos";
        } else if (indicador == 4) {
           return "100% o mas";
        } else if (indicador == 5) {
           return "6 meses o menos";
        } else if (indicador == 6) {
            return "Tiempo marcado por CNPP";
        } else if (indicador == 7) {
            return "100%";
        } else if (indicador == 8) {
            return "100%";
        } else if (indicador == 9) {
            return "100%";
        } else if (indicador == 10) {
            return "5% o menos";
        } else if (indicador == 11) {
            return "10% o menos";
        } else if (indicador == 12) {
            return "10% o menos";
        } else if (indicador == 13) {
            return "10% o menos";
        } else if (indicador == 14) {
            return " ";
        } else if (indicador == 15) {
            return "80% o mas";
        } else if (indicador == 16) {
            return "60 min o menos";
        } else if (indicador == 17) {
            return "mas del 40%,menos del 70%";
        } else if (indicador == 18) {
            return "mas del 50%,menos del 70%";
        } else if (indicador == 19) {
            return "100%";
        }
    } catch (ex) { console.log(ex); }
}

function generaReporteGraficasPdf() {
    try {
        //console.log(Indicadores[0]);

        var idJuzgado = $("#Juzgados").val();
        var anio = $("#Anios").val();
        var mes = $("#Meses").val();
        var primerDia = new Date(anio, mes, 1);
        var ultimoDia = new Date(anio, mes, 0);

        for (var x = 0; x < Indicadores[0].length; x++) {

            var actionData = "{'idJuzgado': '" + idJuzgado + "','idIndicador': '" + Indicadores[0][x].idIndicador + "','fechaInicio': '" + ObtenFechaInicio(anio, mes) + "','fechaFin': '" + anio + "-" + mes + "-" + ultimoDia.getDate()+" 23:59:59'}";

            var datosServicio = new servicioAjax("POST", "../../Servicio.aspx/ObtenCalculoIndicadores", actionData, generaReporteHtml);

        }

    } catch (ex) { console.log(ex); }
}

function generaReporteHtml(Response) {
    try {
        //console.log(Response);
        $("#TitleIndicador").text("INFORMACION GENERAL" + " " + $("#Juzgados option:selected").text());
        var htmlStringDatos = "";
        var nombreIndicador = "";
        for (var x = 0; x < Indicadores[0].length; x++) {

            if (parseInt(Indicadores[0][x].idIndicador) == Response.d[0].idIndicador) {
                nombreIndicador = Indicadores[0][x].Descripcion;
            }
        }

       /* htmlStringDatos = '<div class="col-lg-12"> ' 
            + ' <h1 id= "TitleIndicador"' + Response.d[0].idIndicador + ' class="page-header">' + Response.d[0].idIndicador +".- " + nombreIndicador +'</h1>'
                       + ' </div >'
                            + ' <div class="col-lg-3 col-md-6">'
                            + ' <div class="panel panel-primary">'
                            + ' <div class="panel-heading">'
                            + ' <div class="row">'
                            + ' <div class="col-xs-3">'
                            + ' <i class="fa fa-tasks fa-5x"></i>'
                            + ' </div>'
                            + ' <div class="col-xs-9 text-right">'
                            + ' <div id="DescInd' + Response.d[0].idIndicador+'_1" class=""></div>'
                            + ' <div id="TexInd' + Response.d[0].idIndicador+'_1"></div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' <div class="col-lg-3 col-md-6">'
                            + ' <div class="panel panel-green">'
                            + ' <div class="panel-heading">'
                            + ' <div class="row">'
                            + ' <div class="col-xs-3">'
                            + ' <i class="fa fa-tasks fa-5x"></i>'
                            + ' </div>'
                            + ' <div class="col-xs-9 text-right">'
                            + ' <div id="DescInd' + Response.d[0].idIndicador+'_2" class=""></div>'
                            + ' <div id="TexInd' + Response.d[0].idIndicador+'_2"></div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' <div class="col-lg-3 col-md-6">'
                            + ' <div class="panel panel-yellow">'
                            + ' <div class="panel-heading">'
                            + ' <div class="row">'
                            + ' <div class="col-xs-3">'
                            + ' <i class="fa fa-tasks fa-5x"></i>'
                            + ' </div>'
                            + ' <div class="col-xs-9 text-right">'
                            + ' <div id="DescInd' + Response.d[0].idIndicador+'_3" class=""></div>'
                            + ' <div id="TexInd' + Response.d[0].idIndicador+'_3"></div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' <div class="col-lg-8">'
                            + ' <div class="panel panel-default">'
                            + ' <div class="panel-heading">'
                            + ' <i class="fa fa-bar-chart-o fa-fw"></i> Logros de los ultimos 4 mese'
                            + ' <div class="pull-right">'
                            + ' </div>'
                            + ' </div>'
                            + ' <div class="panel-body">'
                            + ' <div class="row">'
                            + ' <div class="panel-body">'
                            + ' <div id="morris-bar-chart-Ind' + Response.d[0].idIndicador+'"></div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'
                            + ' <div class="col-lg-4">'
                            + ' <div class="panel panel-default">'
                            + ' <div class="panel-heading">'
                            + ' <i class="fa fa-bar-chart-o fa-fw"></i> ESTATUS MENSUAL'
                            + ' </div>'
                            + ' <div class="panel-body">'
                            + ' <div id="morris-donut-chart-Ind' + Response.d[0].idIndicador+'"></div>'
                            + ' </div>'
                            + ' </div>'
                            + ' </div>'*/;
                            

                            

                            //htmlStringDatos = '<tr></tr>'
                            //        + ' <tr> '
                            //    + ' <td class="variables" >'
                            //    + ' <table class="tablavariables" >'
                            //    + ' <tr ALIGN=LEFT>'
                            //                        + ' <td><label id="TexInd' + Response.d[0].idIndicador + '_1" ></label> </label> <label id="DescInd' + Response.d[0].idIndicador + '_1"></td>'
                            //                    + ' </tr>'
                            //    + ' <tr ALIGN=LEFT>'
                            //                        + ' <td><label id="TexInd' + Response.d[0].idIndicador + '_2"></label> </label> <label id="DescInd' + Response.d[0].idIndicador +'_2"></td>'
                            //                    + ' </tr>'
                            //    + ' <tr ALIGN=LEFT>'
                            //                        + ' <td><label id="TexInd' + Response.d[0].idIndicador + '_3"></label> <label id="DescInd' + Response.d[0].idIndicador +'_3"></label></td>'
                            //                    + ' </tr>'
                            //    + ' <tr ALIGN=LEFT>'
                            //                        + ' <td><label>Logro: </label> <label id="DescLogro' + Response.d[0].idIndicador + '"></label></td>'
                            //                    + ' </tr>'
                            //                + ' </table>'
                            //            + ' </td>'
                            //            + ' <td>'
                            //    + ' <table class="tablaGraficas">'
                            //                        + ' <tr>'
                            //                            + ' <td> <h3 id= "TitleIndicador"' + Response.d[0].idIndicador + ' class="page-header"> ' + Response.d[0].idIndicador + ".- " + nombreIndicador + '</h3 ></td>'
                            //                        + ' </tr>'
                            //                        + ' <tr>'
                            //                        + ' <td> <div id="line-example' + Response.d[0].idIndicador+'" class="ContenGreafica"></div></td>'
                            //                        + ' </tr>'
                            //                + ' </table>'
                            //            + ' </td>'

                            //    + ' </tr>';

                            //$("#tablaDatosReporte").append(htmlStringDatos);


        var clasecss = "";
        if (position == 1) { clasecss = "divIzquierdo"; position = 2; } else if (position == 2) { clasecss = "divDerecha"; position = 1;}

        htmlStringDatos = ' <div class="' + clasecss+'"> <div style="width:75%; display:inline-table;"><table class="tablaGraficas">'
                        + ' <tr>'
                        + ' <td> <h5 id="TitleIndicador"' + Response.d[0].idIndicador + ' class="page-header"> <b>' + Response.d[0].idIndicador + ".- " + nombreIndicador + '</b></h5 ></td>'
                        + ' </tr>'
                        + ' <tr>'
                        + ' <td> <div id="line-example' + Response.d[0].idIndicador + '" class="ContenGreafica"></div></td>'
                        + ' </tr>'
                        + ' </table> </div>'
                        + ' <div style="width:75%; display:inline-table;"> <table class="tablavariables" >'
                        + ' <tr ALIGN=LEFT>'
                        + ' <td><label id="TexInd' + Response.d[0].idIndicador + '_1" ></label> </label> <label id="DescInd' + Response.d[0].idIndicador + '_1"></td>'
                        + ' </tr>'
                        + ' <tr ALIGN=LEFT>'
                        + ' <td><label id="TexInd' + Response.d[0].idIndicador + '_2"></label> </label> <label id="DescInd' + Response.d[0].idIndicador + '_2"></td>'
                        + ' </tr>'
                        + ' <tr ALIGN=LEFT>'
                        + ' <td><label id="TexInd' + Response.d[0].idIndicador + '_3"></label> <label id="DescInd' + Response.d[0].idIndicador + '_3"></label></td>'
                        + ' </tr>'
                        + ' <tr ALIGN=LEFT>'
                        + ' <td><label>Logro: </label> <label id="DescLogro' + Response.d[0].idIndicador +'"></label></td>'
                        + ' </tr>'
                        + ' </table>'
                        + ' </div> </div>';
        
        $("#Complemento").append(htmlStringDatos);
        
        //Morris.Line({
        //    element: 'line-example' + Response.d[0].idIndicador ,
        //    data: [
        //        { year: '2008', value: 20 },
        //        { year: '2009', value: 10 },
        //        { year: '2010', value: 5 },
        //        { year: '2011', value: 5 },
        //        { year: '2012', value: 20 }
        //    ],
        //    // The name of the data record attribute that contains x-values.
        //    xkey: 'year',
        //    // A list of names of data record attributes that contain y-values.
        //    ykeys: ['value'],
        //    // Labels for the ykeys -- will be displayed when you hover over the
        //    // chart.
        //    labels: ['Value']
        //});

        datos = new Array();
        $("#MenuIndi").click();
        $("#DescInd" + Response.d[0].idIndicador+"_1").text(Response.d[0].Total);
        $("#DescInd" + Response.d[0].idIndicador + "_2").text(Response.d[0].Porcentaje);
              
           //Morris.Donut({
           //     element: 'morris-donut-chart-Ind' + Response.d[0].idIndicador,
           //     data: [{
           //         label: "Estatus " + Response.d[0].mes + "/" + Response.d[0].Anio + ((Response.d[0].idIndicador == 1 || Response.d[0].idIndicador == 3 || Response.d[0].idIndicador == 4 || Response.d[0].idIndicador == 7 || Response.d[0].idIndicador == 8 || Response.d[0].idIndicador == 9 || Response.d[0].idIndicador == 10 || Response.d[0].idIndicador == 11 || Response.d[0].idIndicador == 12 || Response.d[0].idIndicador == 13 || Response.d[0].idIndicador == 14 || Response.d[0].idIndicador == 15 || Response.d[0].idIndicador == 17 || Response.d[0].idIndicador == 19 || Response.d[0].idIndicador == 20 || Response.d[0].idIndicador == 21) ? " en %":""),
           //         value: Response.d[0].Calculo
           //     }],
           //     colors: [ValidaEstatusIndicador(Response.d[0].idIndicador, Response.d[0].Calculo)],
           //     resize: true
           // });

        $("#DescLogro" + Response.d[0].idIndicador).append('<div style="background-color:' + ValidaEstatusIndicador(Response.d[0].idIndicador, Response.d[0].Calculo) + ';">' + Response.d[0].Calculo + ' ' + agregaDescripcion(Response.d[0].idIndicador) + '</div>' ) ;/* falta aqui Agregar la descripcion*/

            if (Response.d[0].idIndicador == 5) {
                for (var x = 0; x < 4; x++) {
                    var totalMeses = 0;
                    for (var y = x; y < Response.d.length; y++) {
                        totalMeses += parseInt(Response.d[y].Porcentaje)
                    }

                    datos.push({
                        y: Response.d[x].Anio + "-" + Response.d[x].mes,
                        a: parseFloat(Response.d[x].Total / totalMeses).toFixed(2)
                    });
                    if (x == 0) {

                        console.log(parseFloat(Response.d[x].Total / totalMeses).toFixed(2));

                       // $("#DescLogro" + Response.d[0].idIndicador).text(ValidaEstatusIndicador(Response.d[0].idIndicador,totalMeses));
                        $("#DescLogro" + Response.d[0].idIndicador).html('<div style="background-color:' + ValidaEstatusIndicador(Response.d[0].idIndicador, parseFloat(Response.d[x].Total / totalMeses).toFixed(2)) + ';">' + parseFloat(Response.d[x].Total / totalMeses).toFixed(2) + ' ' + agregaDescripcion(Response.d[0].idIndicador) + '</div>');
                      //  $("#" + 'morris-donut-chart-Ind' + Response.d[0].idIndicador).empty();

                        $("#DescInd" + Response.d[0].idIndicador + "_1").text(Response.d[x].Total);
                        $("#DescInd" + Response.d[0].idIndicador + "_2").text(totalMeses);

                        //Morris.Donut({
                        //    element: 'morris-donut-chart-Ind' + Response.d[0].idIndicador,
                        //    data: [{
                        //        label: "Estatus " + Response.d[0].mes + "/" + Response.d[0].Anio + ((Response.d[0].idIndicador == 1 || Response.d[0].idIndicador == 3 || Response.d[0].idIndicador == 4 || Response.d[0].idIndicador == 7 || Response.d[0].idIndicador == 8 || Response.d[0].idIndicador == 9 || Response.d[0].idIndicador == 10 || Response.d[0].idIndicador == 11 || Response.d[0].idIndicador == 12 || Response.d[0].idIndicador == 13 || Response.d[0].idIndicador == 14 || Response.d[0].idIndicador == 15 || Response.d[0].idIndicador == 17 || Response.d[0].idIndicador == 19 || Response.d[0].idIndicador == 20 || Response.d[0].idIndicador == 21) ? " en %" : ""),//label: "Estatus " + Response.d[0].mes + "/" + Response.d[0].Anio ,
                        //        value: parseFloat(Response.d[x].Total / totalMeses).toFixed(2)
                        //    }],
                        //    colors: [ValidaEstatusIndicador(Response.d[0].idIndicador, parseFloat(Response.d[x].Total / totalMeses).toFixed(2))],
                        //    resize: true
                        //});

                    }
                    //console.log(totalMeses);
                    //console.log(Response.d[x]);
                }
                console.log(datos);


            } else {

                for (var x = 0; x < Response.d.length; x++) {
                    // console.log(Response.d[x]);
                    datos.push({
                        y: Response.d[x].Anio + "-" + Response.d[x].mes,
                        a: parseFloat(Response.d[x].Calculo)
                    });
                }
                //console.log(datos);

        }
          //console.log(datos);
            Morris.Line({
                element: 'line-example' + Response.d[0].idIndicador,
                data: datos,
                xkey: 'y',
                ykeys: ['a'],
                labels: ['Indicador'],
                xLabels: 'month',
                xLabelFormat: function (d) { return d.getFullYear() + '/' + (d.getMonth() + 1)},
                //barColors: function (row, series, type) {
                //    return ValidaEstatusIndicador(Response.d[0].idIndicador, row.y);
                //},
                barOpacity: .8,
                /*hideHover: 'false',*/
                ymax: 'auto',
                resize: 'true',
                });
            validaInformacionGeneral(Response.d[0].idIndicador);

    } catch (ex) { console.log(ex); }
}



