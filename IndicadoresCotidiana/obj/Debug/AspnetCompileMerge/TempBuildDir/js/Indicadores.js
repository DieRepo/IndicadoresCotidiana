var strJuzgados = '';
var distritos = [];/*[["1", "CHALCO"],
    ["2", "CUAUTITLAN"],
    ["3", "ECATEPEC"],
    ["4", "EL ORO"],
    ["5", "IXTLAHUACA"],
    ["6", "JILOTEPEC"],
    ["7", "LERMA"],
    ["8", "NEZAHUALCOYOTL"],
    ["9", "OTUMBA"],
    ["10", "SULTEPEC"],
    ["11", "TEMASCALTEPEC"],
    ["12", "TENANGO DEL VALLE"],
    ["13", "TENANCINGO"],
    ["14", "TEXCOCO"],
    ["15", "TLALNEPANTLA"],
    ["16", "TOLUCA"],
    ["17", "VALLE DE BRAVO"],
    ["18", "ZUMPANGO"]];*/
var indicadorIdTab = "";
var indicadorIdTabGrafica = "";
var anio = 0;
var semanaDatos = 0;
$(function () {
    
    var actionData = "{}";
    var datosServicio = new servicioAjax("POST", "Servicio.aspx/ObtenSemanas", actionData, ObtenSemanas);
    var fecha = new Date();
    anio = fecha.getFullYear();
    semanaDatos = (semanaISO(fecha)-1)

    var alto = ($(window).height());

    //generaTambs();
    
});

function ObtenSemanas(Response) {
   // //console.log(Response);

    for (var x = 0 ; x < Response.d.length ; x++) {
        $('#Semanas').append('<option value="' + Response.d[x].Semana + '/' + Response.d[x].Anio+ '" selected="selected">Año: ' + Response.d[x].Anio + ' Semana: ' + Response.d[x].Semana + ' del ' + Response.d[x].F1 + ' al ' + Response.d[x].F2 + '</option>');
    }

    $('#Semanas').val(Response.d[0].Semana + "/" + Response.d[0].Anio);


    var actionData = "{}";
    var datosServicio = new servicioAjax("POST", "Servicio.aspx/ObtenIndicadores", actionData, ValidaIndicadores);

}

function ValidaIndicadores(Response) {
    try {
        ////console.log(Response)
        
        ////console.log(distritos)
        strJuzgados += '<div id="tabs" class="tabsStyles">'
                       + '<div class="scroller"> '
                       + ' <ul>';
        for (var x = 0; x < Response.d.length; x++) {
            strJuzgados += '<li><a href="#tabs-' + (x + 1) + '">' + Response.d[x].Descripcion + '</a></li>';
        }

        strJuzgados += ' </ul> </div>';
        for (var x = 0; x < Response.d.length; x++) {
            strJuzgados += ' <div id="tabs-' + (x + 1) + '">'
            //+ '<p>Trabajando indicador ' + (x + 1) + '</p>'
            generaSubTabs((x + 1));

            strJuzgados += '</div>';
        }

        strJuzgados += ' </div>';


        $("#contenedor").append(strJuzgados);

        $("#tabs").tabs();
        $("#tabs1-1").tabs();
        indicadorIdTab = "1_1"
        creaTablaDatos('#1_1');
       

        $("#tabs").tabs({
            activate: function (event, ui) {

                var activeIndex = $("#accordion").accordion("option", "active");
                ////console.log(ui.newPanel[0].id);
                var idObten = ui.newPanel[0].id;
                var id = idObten.split("-");

                ////console.log(id);
                $("#tabs" + id[1] + "-" + id[1]).tabs();

                $("#tabs" + id[1] + "-" + id[1]).tabs({
                    activate: function (event, ui) {
                        // $("#loader").fadeIn();

                        var activeIndex = $("#accordion").accordion("option", "active");
                        // //console.log(ui.newPanel[0].id);
                        var idObten = ui.newPanel[0].id;
                        var id = idObten.replace("tabs", "");
                        id = id.replace("-", "_");


                        indicadorIdTabGrafica = id;

                        if (!$.fn.DataTable.isDataTable("#" + id)) {
                            $("#loader").fadeIn();

                            //console.log(id);
                            var idDatos = id.split("_");
                            //console.log(idDatos[0]);
                            var actionData = "{'idIndicador': '" + idDatos[0] + "','Anio': '" + anio + "','Semana': '" + semanaDatos + "'}";
                            ////console.log(actionData);
                            var datosServicio = new servicioAjax("POST", "Servicio.aspx/Obtencalculos", actionData, GeneraTablaDatos);
                            indicadorIdTab = id + "_1";

                            setTimeout(function () { 
                            google.charts.load("current", { packages: ['corechart'] });
                            google.charts.setOnLoadCallback(drawChart2);
                            }, 1000);

                        } else {
                            $("#loader").fadeOut("slow");
                            indicadorIdTab = id + "_1";
                        }

                    }
                });

                if (!$.fn.DataTable.isDataTable("#" + id[1] + "_1")) {
                    $("#loader").fadeIn();
                    ////console.log(id[1])
                    creaTablaDatos("#" + id[1] + "_1");
                    var actionData = "{'idIndicador': '" + id[1] + "','Anio': '" + anio + "','Semana': '" + semanaDatos + "'}";
                    ////console.log(actionData);
                    var datosServicio = new servicioAjax("POST", "Servicio.aspx/Obtencalculos", actionData, GeneraTablaDatos);
                    indicadorIdTab = id[1] + "_1";
                    setTimeout(function () { 
                    google.charts.load("current", { packages: ['corechart'] });
                    google.charts.setOnLoadCallback(drawChart);
                    }, 1000);
                } else {
                    $("#loader").fadeOut("slow");
                    indicadorIdTab = id[1] + "_1";
                }

            }
        });

        $("#tabs1-1").tabs({
            activate: function (event, ui) {

                var activeIndex = $("#accordion").accordion("option", "active");
               // console.log(ui.newPanel[0].id);
                var idObten = ui.newPanel[0].id;
                var id = idObten.replace("tabs", "");
                id = id.replace("-", "_");
               
               // console.log(id);
                indicadorIdTabGrafica = id;

                if (!$.fn.DataTable.isDataTable("#" + id)) {
                    $("#loader").fadeIn();
                 
                    //console.log(id);
                    var idDatos = id.split("_");
                    //console.log(idDatos[0]);
                    var actionData = "{'idIndicador': '" + id[1] + "','Anio': '" + anio + "','Semana': '" + semanaDatos + "'}";
                    ////console.log(actionData);
                    var datosServicio = new servicioAjax("POST", "Servicio.aspx/Obtencalculos", actionData, GeneraTablaDatos);
                    indicadorIdTab = id;


                        google.charts.load("current", { packages: ['corechart'] });
                        google.charts.setOnLoadCallback(drawChart2);

                } else {
                    $("#loader").fadeOut("slow");
                    indicadorIdTab = id ;
                }


            }
        });

        /*Genera la primera tabla del indicador*/
        
        var actionData = "{'idIndicador': '1','Anio': '" + anio + "','Semana': '" + semanaDatos + "'}";

        var datosServicio = new servicioAjax("POST", "Servicio.aspx/Obtencalculos", actionData, GeneraTablaDatos);

        google.charts.load("current", { packages: ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);
      

    } catch (ex) { }
}

function drawChart() {
    ////console.log(distritos[0]);
    for (var x = 0; x < distritos[0].length; x++) {

        var actionData = "{'idIndicador': '" + distritos[0][x].idIndicador + "','idJuzgado':'" + distritos[0][x].idJuzgado + "','Anio':'" + anio + "','Semana':'" + semanaDatos + "'}";
        var datosServicio = new servicioAjax("POST", "Servicio.aspx/ObtenDatosGrafica", actionData, generaDatosGrafica);

     }

     $("#loader").fadeOut("slow");
  
}

function generaDatosGrafica(Response) {
    //console.log(Response);
    //console.log(indicadorIdTab);
    var DatosGrafica = [];
        DatosGrafica.push(["Periodo", "Indicador", { role: "style" }]);
    for (var x = 0; x < Response.d.length; x++) {
        DatosGrafica.push(["Año: " + Response.d[x].Anio + " Semana:" + Response.d[x].Semana, parseInt(Response.d[x].Calculo), 'stroke-color: #76A7FA; stroke-opacity: 0.6; stroke-width: ; fill-color: #76A7FA; fill-opacity: 0.7']);
    }

    var data = google.visualization.arrayToDataTable(
              
              DatosGrafica
              
    );

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
                     {
                         calc: "stringify",
                         sourceColumn: 1,
                         type: "string",
                         role: "annotation"
                     },
                     2]);

    var options = {
        //title: "Density of Precious Metals, in g/cm^3",
        width: 400,
        height: 150,
        bar: { groupWidth: "50%" },
        legend: { position: "none" },
    };
    var chart = new google.visualization.ColumnChart(document.getElementById("columnchart" + indicadorIdTab + "_" + Response.d[0].DescripcionJuzgado));
    chart.draw(view, options);
}

 function drawChart2() {
    // //console.log(distritos);
     
     var juzgadosArray = new Array();
     juzgadosArray.push(["Distrito", "Indicador", { role: "style" }])
     for (var x = 0; x < distritos[0].length; x++) {
         juzgadosArray.push(['' + distritos[0][x].DescripcionJuzgado + ' Del ' + distritos[0][x].Fecha1R + ' al ' + distritos[0][x].Fecha2R, parseInt(distritos[0][x].Calculo), 'stroke-color: #76A7FA; stroke-opacity: 0.6; stroke-width: ; fill-color: #76A7FA; fill-opacity: 0.5'])
     }
     var data = google.visualization.arrayToDataTable(juzgadosArray);

         var view = new google.visualization.DataView(data);
         view.setColumns([0, 1,
                          {
                              calc: "stringify",
                              sourceColumn: 1,
                              type: "string",
                              role: "annotation"
                          },
                          2]);
         var alto = ($(window).height());
         var options = {
             //title: "Density of Precious Metals, in g/cm^3",
             width: "100%",
             height: ((alto - 200)),
             bar: { groupWidth: "50%" },
             legend: { position: "none" },
         };
         var chart = new google.visualization.ColumnChart(document.getElementById("columnchart" + indicadorIdTabGrafica + "_GraficaGeneral"));
         chart.draw(view, options);
     
     //var chalco = new google.visualization.ColumnChart(document.getElementById("columnchart_Chalco"));
     //chalco.draw(view, options);

     //var Texcoco = new google.visualization.ColumnChart(document.getElementById("columnchart_Texcoco"));
     //Texcoco.draw(view, options);
     //var Neza = new google.visualization.ColumnChart(document.getElementById("columnchart_Neza"));
     //Neza.draw(view, options);
         $("#loader").fadeOut("slow");

 }

function generaTambs() {
    try {
       

        strJuzgados += '<div id="tabs" class="tabsStyles">'
                        + '<div class="scroller"> '
                        + ' <ul>';
        for (var x = 0; x < 23; x++) {
            strJuzgados += '<li><a href="#tabs-' + (x + 1) + '">Indicador ' + (x + 1) + '</a></li>';
        }

        strJuzgados += ' </ul> </div>';
        for (var x = 0; x < 23; x++) {
            strJuzgados += ' <div id="tabs-' + (x + 1) + '">'
                                //+ '<p>Trabajando indicador ' + (x + 1) + '</p>'
            generaSubTabs((x + 1));

            strJuzgados += '</div>';
        }

        strJuzgados += ' </div>';


        $("#contenedor").append(strJuzgados);
       creaTablaDatos('#1_1'); 
    } catch (ex) { console.log(ex); }
}

function generaSubTabs(idtab) {
    try {


        strJuzgados += '<div id="tabs' + idtab + '-' + idtab + '" >'
                       
                        + ' <ul>';
        for (var x = 0; x < 2; x++) {
            if (x == 0) { strJuzgados += '<li><a href="#tabs' + idtab + '-' + (x + 1) + '">Juzgados</a></li>' }
            else { strJuzgados += '<li><a href="#tabs' + idtab + '-' + (x + 1) + '">Grafica General</a></li>' };
        }

        strJuzgados += ' </ul>';
        for (var x = 0; x < 2; x++) {
            strJuzgados += ' <div id="tabs' + idtab + '-' + (x + 1) + '">'
            if (x == 0) { generaTabla(idtab + "_" + (x + 1)); }
            else {
                strJuzgados += '<div id="columnchart' + idtab + '_' + (x + 1) + '_GraficaGeneral" style="width: 100%; height: 100%;"></div>'
            }
            strJuzgados += '</div>';
        }

        strJuzgados += ' </div>';

      
       // $("#contenedor").append(strJuzgados);
    } catch (ex) { console.log(ex); }
}

function generaTabla(idtabla) {



    var stringTbl = ' <table id="' + idtabla + '" class="display" cellspacing="0" width="100%">'
                            + '<thead>'
                               + ' <tr>'
                                    + '<th>Juzgado</th>'
                                    + '<th class="tdStyle">Cifra</th>'
                                    + '<th class="tdStyle">Estatus</th>'
                                    + '<th>Cifras por semanas</th>'
                                + '</tr>'
                            + '</thead>';

    stringTbl += ' <tbody>'

    
    //for (var x = 0; x < distritos.length; x++) {
    //    stringTbl +=' <tr>'
    //                    + ' <td>' + distritos[x][1]+ '</td>'
    //                    +'<td class="tdStyle">5</td>'
    //                    +'<td class="tdStyle">0</td>'
    //                    + '<td align="center" style="width:400px; height: 150px;"><div id="columnchart' + idtabla + '_' + distritos[x][1] + '" style="width: 400px; height: 150px;"></div></td>'
    //                  + '</tr>';
    //}
                                
    stringTbl +=' </tbody>  </table>';

    strJuzgados += stringTbl;

   
}

function GeneraTablaDatos(response) {
    try {
        ////console.log(response);
        distritos = new Array();
        distritos.push(response.d);

        $("#loader").fadeOut("slow");
       
       for (var x = 0; x < response.d.length; x++) {
            var table = $('#'+indicadorIdTab).DataTable();
            var rowNode = table
                .row.add([response.d[x].DescripcionJuzgado,
                            response.d[x].Calculo,
                            ValidaEstatusIndicador(response.d[x].idIndicador, response.d[x].Calculo),
                            '<div id="columnchart' + indicadorIdTab + '_' + response.d[x].DescripcionJuzgado + '" style="width: 120px; height: 150px;"></div>'
                        ])
                .draw()
                .node();
        }

        
    } catch (ex) { }
}

function ValidaEstatusIndicador(indicador,calculo) {
    try {
        var status = "";

        if (indicador == 1) {
            if (calculo > 5) {
                status = "<div class='Rojo'></div>";
            } else if (calculo <= 5) {
                status = "<div class='Verde'></div>";
            }

        } else if (indicador == 2) {
            if (calculo > 10) {
                status = "<div class='Rojo'></div>";
            } else if (calculo <= 10) {
                status = "<div class='Verde'></div>";
            }

        } else if (indicador == 3) {
            if (calculo > 30) {
                status = "<div class='Rojo'></div>";
            } else if (calculo <= 30) {
                status = "<div class='Verde'></div>";
            }

        } else if (indicador == 3) {
            if (calculo >= 100) {
                status = "<div class='Verde'></div>";
            } else if (calculo < 100) {
                status = "<div class='Rojo'></div>";
            }

        
    } else if (indicador == 4) {
        if (calculo >= 100) {
            status = "<div class='Verde'></div>";
        } else if (calculo < 100) {
            status = "<div class='Rojo'></div>";
        }

        } else if (indicador == 5) {
            if (calculo <=6 ) {
                status = "<div class='Verde'></div>";
            } else if (calculo > 6) {
                status = "<div class='Rojo'></div>";
            }

        } else if (indicador == 6) {
            if (calculo <= 24) {
                status = "<div class='Verde'></div>";
            } else if (calculo >24) {
                status = "<div class='Rojo'></div>";
            }

        } else if (indicador == 7 || indicador == 8 || indicador == 9) {
            if (calculo >= 100) {
                status = "<div class='Verde'></div>";
            } else if (calculo < 100) {
                status = "<div class='Rojo'></div>";
            }

        } else if (indicador == 10) {
            if (calculo <= 5) {
                status = "<div class='Verde'></div>";
            } else if (calculo > 5) {
                status = "<div class='Rojo'></div>";
            }

        } else if (indicador == 11 || indicador == 12 || indicador == 13) {
            if (calculo <= 10) {
                status = "<div class='Verde'></div>";
            } else if (calculo > 10) {
                status = "<div class='Rojo'></div>";
            }

        } else if (indicador == 14) {
            status = "<div class='Verde'></div>";
           /* if (calculo <= 24) {
                status = "<div class='Verde'></div>";
            } else if (calculo > 24) {
                status = "<div class='Rojo'></div>";
            }*/

        } else if (indicador == 15) {
            if (calculo >= 80) {
                status = "<div class='Verde'></div>";
            } else if (calculo < 80) {
                status = "<div class='Rojo'></div>";
            }

        } else if (indicador == 16) {
           /* if (calculo <= 5) {
                status = "<div class='Verde'></div>";
            } else if (calculo > 5) {
                status = "<div class='Rojo'></div>";
            }*/

        }


        return status;
    } catch (ex) { }
}

function creaTablaDatos(idtable) {
    try {
        var alto = ($(window).height());

        if (!$.fn.DataTable.isDataTable(idtable)) { 

            $(idtable).DataTable({
                scrollY: ((alto - 342)),
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningun dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    },
                    "buttons": {
                        "copy": "Copiar",
                        "print": "Imprimir"
                    }
                }
            });
        }
        
    } catch (ex) { }
}

function CargaNuevosDatos() {
    try {
        console.log(indicadorIdTab);
        //alert($('#Semanas').val());
        var semanaAnio = $('#Semanas').val();
        var nuevoDatos = [];
        var indicador = [];

        nuevoDatos = semanaAnio.split("/")
        anio = nuevoDatos[1];
        semanaDatos = nuevoDatos[0];
        indicador = indicadorIdTab.split("_")

        var table = $('#'+indicadorIdTab).DataTable();

        var rows = table
            .rows()
            .remove()
            .draw();
        
        var actionData = "{'idIndicador': '" + indicador[0] + "','Anio': '" + nuevoDatos[1] + "','Semana': '" + semanaDatos + "'}";

        var datosServicio = new servicioAjax("POST", "Servicio.aspx/Obtencalculos", actionData, GeneraTablaDatos);

        setTimeout(function () {
            google.charts.load("current", { packages: ['corechart'] });
            google.charts.setOnLoadCallback(drawChart);
        }, 1000);


    } catch (ex) { console.log(ex); }
}

function semanaISO(d) {


        // Create a copy of this date object  
        var target  = new Date(d.valueOf());  
  
        // ISO week date weeks start on monday  
        // so correct the day number  
        var dayNr   = (d.getDay() + 6) % 7;  

        // Set the target to the thursday of this week so the  
        // target date is in the right year  
        target.setDate(target.getDate() - dayNr + 3);  

        // ISO 8601 states that week 1 is the week  
        // with january 4th in it  
        var jan4    = new Date(target.getFullYear(), 0, 4);  

        // Number of days between target date and january 4th  
        var dayDiff = (target - jan4) / 86400000;    

        // Calculate week number: Week 1 (january 4th) plus the    
        // number of weeks between target date and january 4th    
        var weekNr =  Math.ceil(dayDiff / 7);    

        return weekNr;    

    }