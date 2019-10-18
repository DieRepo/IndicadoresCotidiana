$(function () {

    creaJsonMenuIndicadoresG($("#indicadores-gestion > ul"));
    eventosClickG();
    $('[data-toggle="tooltip"]').tooltip(); 
});

var idActual;

function creaJsonMenuIndicadoresG(origen) {

    var i = 1;
    var menu = "";

    $.each(jsonGestion, function (val, item) { 

        if (($("#Materia").val() == 1 && item.id == 4) != true) {
            if (item.activo) {
                menu += '<li class="gestion click"> <a data-activo="true" data-id= "' + item.id + '">' + i + '.- ' + item.titulo + '</a></li>\n';
            } else {
                menu += '<li class="gestion click"> <a data-activo="false" data-id= "' + 0 + '" data-toggle="tooltip" data-placement="right" title="Indicador proximamente">' + i + '.- ' + item.titulo + ' *</a></li>\n';
            }
        }
        i++;
    });



    $(origen).append(menu);



}

function consultarReporte(id) {
 
    pestActual = "gestion";

    $("#datos > *").show();
    $("#graficas > *").show();

    var cveDistrito = $("#Juzgados").val();
    var cveJuzgado = ($("#Juzgado").val().length > 0) ? $("#Juzgado").val() : 0;
    var materia = $("#Materia").val();
    var anio = $("#Anios").val();
    var mes = $("#Meses").val();

    var dataJson = "{id:" + id + ", anio:'" + anio + "', mes:'" + mes + "', cveDistrito:'" + cveDistrito + "', materia:'" + materia + "', cveJuzgado:" + cveJuzgado + "}";

    var json = [];

    $("#Tex1, #Tex2, #Tex3, #Desc1, #Desc2, #Desc3, .page-header-pj > span").text("");
    $("#morris-bar-chart, #morris-donut-chart, #formula").empty();
    $("#Complemento *:not(#button-pdf-exporta)").remove();
    $("#button-pdf-exporta").hide();

    consulta("IndicadorGestion", dataJson, function (data) {

        
        $.each(data, function (index, item) {

            if (item.Anio.toString() === "2019") {
                if (item.Mes >= "9") {
                    json.push(item);
                }
            } else {
                json.push(item);
            }

        });
       
    });

    var es = "";
    if ($('#Materia option:selected').val() != 5) {
        es = "es";
    }
    $("#titulo-indicador").html("<span style='font-weight: bolder'>" + $('#Materia option:selected').text() + "</span>: " + jsonGestion[id].descripcion);

    var formula =
        "<div style=\"text-align:center; padding:0px !important;\" class=\"col-lg-12\" >" +
        "    <table id=\"formula-tabla\" style=\"text-align:center;\ class=\"table-condensed\">" +
        "        <tr>" +
        "           <td rowspan=\"2\" style=\"font-size: 30px; padding:2px;\">FÓRMULA =</td>" +
        "            <td style=\"font-size: 15px; font-weight:bolder;\">" + jsonGestion[id].valor1 +"</td>" +
        "            <td rowspan=\"2\" style=\"font-size: 20px; padding:2px;\">" + jsonGestion[id].porcien +"</td>" +
        "        </tr>" +
        "        <tr>" +
        "            <td style=\"font-size: 15px; font-weight:bolder; border-top: 1px black solid;\">" + jsonGestion[id].valor2 +"</td>" +
        "        </tr>" +
        "    </table>" +
        "            </div >" +
        "" +
        "<div id=\"fuente\" style=\"padding:0px !important; text-align: right;\" class=\"col-lg-12\"><span><br/>Fuente: " + jsonGestion[id].fuente+"</span></div>";

    $("#formula").html(formula);

    if (String($('#Juzgados option:selected').val()) != '') {
        $("#distrito").html("<br/>"+String($('#Juzgado option:selected').text()));
    } else {
        $("#distrito").html("<br/>"+String($('#Juzgados option:selected').text()).capital());
    }

    if (json.length) {
        //Barra("morris-bar-chart", json, id);
        Linea("morris-bar-chart", json, id);
        Dona(json, id, 0);

        $("#morris-bar-chart circle").each(function (i, e) {
            $(this).attr("data-point", (json.length - (i+1)));
            $(this).attr("style", "cursor: pointer");
        });
    }
  

    jActual = json;
    idActual = id;

   // $("#morris-bar-chart circle[data-point='3']").trigger("mouseover"); 
}

function consultaTabla() {

    pestActual = "gestionTabla";

    $("#datos > *").hide();
    $("#graficas > *").hide();

    var cveDistrito = $("#Juzgados").val();
    var cveJuzgado = ($("#Juzgado").val().length > 0) ? $("#Juzgado").val() : 0;
    var materia = $("#Materia").val();
    var anio = $("#Anios").val();
    var mes = $("#Meses").val();

    var dataJson = "{anio:'" + anio + "', mes:'" + mes + "', cveDistrito:'" + cveDistrito + "', materia:'" + materia + "', cveJuzgado:" + cveJuzgado + "}";

    $("#Tex1, #Tex2, #Tex3, #Desc1, #Desc2, #Desc3, .page-header-pj > span").text("");
    $("#morris-bar-chart, #morris-donut-chart, #formula").empty();
    $("#Complemento *:not(#button-pdf-exporta)").remove();
    $("#button-pdf-exporta").hide();

    $("#div-flotante-tabla *").remove();
    $("#div-flotante-tabla").html("<table id=\"tabla-indicadores\" class=\"display nowrap table table-striped table-bordered\" cellspacing=\"0\" style=\"width: 80 %\"></table>");
    var cabeceraTabla = "<th>Id</th> <th>Proceso</th> <th>Indicador</th> <th>Unidad</th> <th>Mata</th> <th>Periodicidad</th> <th>Bueno</th> <th>Regular</th> <th>Malo</th>";

    var datosTabla = [];
    var json = {};
    

    consulta("IndicadoresGestionGeneral", dataJson, function (data) {

        json = JSON.parse(JSON.stringify(data));

        var u = true;

        $.each(data, function (index, item) {

            json[index].Datos.length = 0;

            var dato = [item.IdGestion, jsonGestion[item.IdGestion].proceso, jsonGestion[item.IdGestion].titulo, jsonGestion[item.IdGestion].unidad, jsonGestion[item.IdGestion].meta, jsonGestion[item.IdGestion].periodicidad
                , jsonGestion[item.IdGestion].bueno, jsonGestion[item.IdGestion].regular, jsonGestion[item.IdGestion].malo];

            $.each(item.Datos, function (i, it) {

                if (it.Anio.toString() === "2019") {
                    if (it.Mes >= "9") {
                        json[index].Datos.push(it);
                    }
                } else {
                    json[index].Datos.push(it);
                } 

            });
            
            $.each(json[index].Datos, function (i, t) {
                dato.push(t.Calculo);
                if (u) {
                    cabeceraTabla += "<th>" + Meses[t.Mes - 1] + " " + t.Anio + "</th>";
                }
                
            });
            u = false;

            datosTabla.push(dato);

        });

        $("#tabla-indicadores").html("<thead><tr>" + cabeceraTabla+"</tr></thead>");

    });

    $("#div-flotante-tabla").dialog({
        open: function () {
            $(this).closest(".ui-dialog")
                .find(".ui-dialog-titlebar-close")
                .removeClass("ui-dialog-titlebar-close")
                .html("<span class='ui-button-icon-primary ui-icon ui-icon-closethick'></span>");
        },
        title: "INDICADORES DE JUZGADOS EN MATERIA " + ($('#Materia option:selected').text()).toUpperCase() + " DE " + $('#Juzgados option:selected').text()
    });

    $("#div-flotante-tabla").dialog("open");

    var b = 0;

    if (!$.fn.DataTable.isDataTable('#tabla-indicadores')) {

        $('#tabla-indicadores').DataTable({
            data: datosTabla,
            //columns: [
            //    { title: "Id" },
            //    { title: "Proceso" },
            //    { title: "Indicador" },
            //    { title: "Unidad" },
            //    { title: "Meta" },
            //    { title: "Periodicidad" },
            //    { title: "Bueno" },
            //    { title: "Regular" },
            //    { title: "Malo" },
            //    { title: mesF[0]},
            //    { title: mesF[1]},
            //    { title: mesF[2]},
            //    { title: mesF[3]}
            //],
            scrollCollapse: true,
            paging: true,
            dom: 'Bfrtip',
            buttons: [
                    /*'copy', 'csv', 'excel',*/ {
                    //extend: 'pdfHtml5',
                    extend: 'pdf',
                    title: 'TABLERO DE CONTROL',

                    download: 'open',
                    messageTop: "INDICADORES DEL JUZGADO DE " + $("#Juzgados option:selected").text().toUpperCase(),
                    orientation: 'landscape',
                    pageSize: 'LEGAL'
                    
                    //,customize: function (doc) {
                    //    doc.content.splice(1, 0, {
                    //        margin: [0, 0, 0, 12],
                    //        alignment: 'right',
                    //        image: 'data:image/jpeg;base64,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'
                    //    });
                    //}

                }

            ],
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
                    "print": "IMPRIMIR"
                },
                "columnDefs": [
                    { "title": "My column title", "targets": 0 }
                ]
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                var row = aData[0]-1;

                $(nRow).find('td:eq(6)').css('color', "#65baaf");
                $(nRow).find('td:eq(7)').css('color', "#ffb259");
                $(nRow).find('td:eq(8)').css('color', "#9d2424");

                $(nRow).find('td:eq(6)').css('font-weight', "bolder");
                $(nRow).find('td:eq(7)').css('font-weight', "bolder");
                $(nRow).find('td:eq(8)').css('font-weight', "bolder");

                if ($(nRow).find('td:eq(9)').length > 0) {
                    $(nRow).find('td:eq(9)').css('color', json[row].Datos[0].Semaforo);
                }
                if ($(nRow).find('td:eq(10)').length > 0) {
                    $(nRow).find('td:eq(10)').css('color', json[row].Datos[1].Semaforo);
                }
                if ($(nRow).find('td:eq(11)').length > 0) {
                    $(nRow).find('td:eq(11)').css('color', json[row].Datos[2].Semaforo);
                }
                if ($(nRow).find('td:eq(12)').length > 0) {
                    $(nRow).find('td:eq(12)').css('color', json[row].Datos[3].Semaforo);
                }
                if ($(nRow).find('td:eq(13)').length > 0) {
                    $(nRow).find('td:eq(13)').css('color', json[row].Datos[4].Semaforo);
                }
                if ($(nRow).find('td:eq(14)').length > 0) {
                    $(nRow).find('td:eq(14)').css('color', json[row].Datos[5].Semaforo);
                }
                if ($(nRow).find('td:eq(15)').length > 0) {
                    $(nRow).find('td:eq(15)').css('color', json[row].Datos[6].Semaforo);
                }
                if ($(nRow).find('td:eq(16)').length > 0) {
                    $(nRow).find('td:eq(16)').css('color', json[row].Datos[7].Semaforo);
                }
                if ($(nRow).find('td:eq(17)').length > 0) {
                    $(nRow).find('td:eq(17)').css('color', json[row].Datos[8].Semaforo);
                }
                if ($(nRow).find('td:eq(18)').length > 0) {
                    $(nRow).find('td:eq(18)').css('color', json[row].Datos[9].Semaforo);
                }
                if ($(nRow).find('td:eq(19)').length > 0) {
                    $(nRow).find('td:eq(19)').css('color', json[row].Datos[10].Semaforo);
                }
                if ($(nRow).find('td:eq(20)').length > 0) {
                    $(nRow).find('td:eq(20)').css('color', json[row].Datos[11].Semaforo);
                }

                b++;

            }
        });
    }

}

function consultarTodo() {

    pestActual = "gestionTabla";

    $("#datos > *").hide();
    $("#graficas > *").hide();

    $("#Tex1, #Tex2, #Tex3, #Desc1, #Desc2, #Desc3, .page-header-pj > span").text("");
    $("#morris-bar-chart, #morris-donut-chart, #formula").empty();
    $("#Complemento *:not(#button-pdf-exporta)").remove();
    $("#button-pdf-exporta").show();

    var cveDistrito = $("#Juzgados").val();
    var cveJuzgado = ($("#Juzgado").val().length > 0) ? $("#Juzgado").val() : 0;
    var materia = $("#Materia").val();
    var anio = $("#Anios").val();
    var mes = $("#Meses").val();

    var dataJson = "{anio:'" + anio + "', mes:'" + mes + "', cveDistrito:'" + cveDistrito + "', materia:'" + materia + "', cveJuzgado:" + cveJuzgado + "}";

    $("#Complemento").append("<div id=\"complemento_temporal\"></div>");
    var json = [];

    consulta("IndicadoresGestionGeneral", dataJson, function (data) {

        json = JSON.parse(JSON.stringify(data));

        $.each(data, function (index, item) {

            json[index].Datos.length = 0;

            $.each(item.Datos, function (i, it) {

                if (it.Anio.toString() === "2019") {
                    if (it.Mes >= "9") {
                        json[index].Datos.push(it);
                    }
                } else {
                    json[index].Datos.push(it);
                }

            });
        });     

        $.each(json, function (index, item) {

            var clasecss = "";

            if (position == 1) {
                clasecss = "divIzquierdo";
                position = 2;
            } else if (position == 2) {
                clasecss = "divDerecha";
                position = 1;
            }


            htmlStringDatos = ' <div>'
                + ' <div style = "width:100%; display:inline-table;">'
                + ' <table class="tablaGraficas">'
                + ' <tr>'
                + ' <td> <h5 class="page-header"> <b>' + jsonGestion[item.IdGestion].titulo + '</b></h5 ></td>'
                + ' </tr>'
                + ' <tr>'
                + ' <td> <div id="grafica-' + item.IdGestion + '" class="ContenGreafica"></div></td>'
                + ' </tr>'
                + ' </table> </div>'
                + ' <div style="width:100%; display:inline-table;"> <table class="tablavariables" >'
                + ' <tr ALIGN=LEFT>'
                + ' <td><label>' + jsonGestion[item.IdGestion].desc1 + ': ' + item.Datos[0].Valor + '</label></td>'
                + ' </tr>'
                + ' <tr ALIGN=LEFT>'
                + ' <td><label>' + jsonGestion[item.IdGestion].desc2 + ': ' + item.Datos[0].Total + '</label></td>'
                + ' </tr>'
                + ' <tr ALIGN=LEFT>'
                + ' <td><label>' + jsonGestion[item.IdGestion].desc2 + ': ' + item.Datos[0].Calculo + '</label></td>'
                + ' </tr>'
                + ' </table>'
                + ' </div>'
                + '</div>';

            $("#complemento_temporal").prepend(htmlStringDatos);

            Linea("grafica-" + item.IdGestion, item.Datos, 1);

        });

        var htmlStringDatos = '<table id="tablaDatosReporte">'
            + '<tr class="Encabezado"> '
            + '<td><h2 class="page-header"><b>' + 'INFORMACION GENERAL</b></h2></td>'
            + '<td style="text-align:center;"> <img src="data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/4QAiRXhpZgAATU0AKgAAAAgAAQESAAMAAAABAAEAAAAAAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCABkAGQDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD9/KKKKACiiigAooqG6uo7GBpZpI4Yk5Z3IVV+pNAHGftH+MPF3w9+DWt+IPA/h+z8XeItEgN9DodxPJbnWI4/mlt4pURyk7IG8smNwXCqQA25flD4Uf8ABfb4UeMLW3Hifw5448GzOitLO1kmp2PP/PN7d2mcAYOTAvXgGvuCwv4dRt/Nt5obiFujxuHU/iOK/M/9sn9hWw8F/HrWpNMsY4dJ1yQ6rZxomEh80kyRgAAALKHwo4VCg9KqNmG2jPsT4P8A/BSv4JfHrxfpvh/wv46t73WtYdo7KyudOvLCa4ZUZyoFxDHztRjg8nGBk4Fe6V+Ntl+zDc+HNZs9S00yWOpabcR3lndRLiS1njcPHIueNyuqsD6gV+tnwb8ff8LR+F+h+IGjjgm1K1R7iJCSsM4+WVAT1CyK65PpQ1YDpqKKKkAooooAKKKKACo551t42d2VEQFmZjgKBzkn0qSvyZ/4Oaf28dW8DeDtH+APhW+n0+48aWB1fxdPA5SR9KMjRQWIYDOy4kimMoBBMcAjYMk7isMTiI0KbqS6HrZHk9fNMbDBYfeXXslq2/T/AIBY/b1/4Ojfh/8AB/X9S8K/BrwvJ8VtQtHktLnxBLqJ03QUYfKWtZI1aa7AYMN6CKMja8c0gINfmf41/wCCtOn/AB08WS6l8Wv2dfht44jck/abbxJ4g0/WIxjjF9PeXWcYHDRFeACpHFfOZ8Lgjla/WL/ggF/wRe8P/FvQbf46fFvQrXWdFknZPB3h+/jElreGJykmo3MR4kQSKyQxyDadrylXDQMvzkMTVx1T2dk12aTS++5+z1slwnCuE+uxnOEtk4zlGUn2Ti1bS/or3PP/AIT/ALCFn4s+AVr+0J8ItW+JXwt8KMU81/EWnS6RrGlAhCtxDfWvlpeWLMwMd3Ds4I3qpDkfUn7N/wC2J4muLbT/AA98WPHfhX4gaPZ5Sx19ZUh1bTgwAKzgYW5i+VPmIEykMxaYkKP1F8Q+HrHxZol7peqWdrqGm6lbyWl3aXMYlhuYZFKPG6nhlZWIIOQQSK/mi/ae+E6/s6/tI+O/Ais0sHhPXLrT7WSVt8klsshNuzkjlzAYi3uT1615ebYevlVSGJwUmot6x6X9Oz/Dp5fdeHeYYXxGwuIybiS3taSUoT5IufJe3xpRlzRdru9pJrmTs7/ttH4Z0HULeOaG6t5YZkWWKRCHSVGAZXVhkMpBBBHBBBFe2fsyNBYeFb7TYJllW1ufOUA/6tZB0/76Vj+Jr8Zf+Ccf7YF9ouj6j4A1K7eaDTYW1TRizFmhgMipcwZ7RpLLC6Dk5uJRkKiKP0u/4JzfFz/hOfG/iWxEu9VsIZyM5+7Iyj/0M19pgMZHFYaNePXp2fVfefzdxlwzW4ezmvlFd8zpy0ltzRaTjLyumnbo9D62ooorqPmQooooAKKKKACv5sf+CvfjqT4tf8FK/i9qcrs0djrQ0WBNxZYUsoIrQquemZIZHI/vSNX9J1fzOf8ABQHw9Lov7ePxohlH7xvG+r3Az/dlvJZV/wDHXFfM8UVHChBef6H739H/AC+nic4xEpK8o09PRyjf8kYFr/wTz+KGu6FoWoWfh/zdG8T21rc2mq211BNHDBdIjxztHvEu1UkDn5cAA896/pS+Cdt4R8P/AA30Xw74IvdJuvDvhXT7bSrGHT7qO4jtLeGMRRR5QkDCRgD/AHa/I7/gm1/wUA8PzfsxaL4K8SWdrLrnw5H/AAjs5dtrvZpk6fIBn7pszDHnu9vLjgYH3r+w/wDtFeCfE3xGvtG0ny7PUddtVeNWkB+0NBvbYvctskkbHojHtXpZXgaNKn7Wi2+ZJ6nxHiHxRmWY4z6hmMIweHlONopq+qV2m3rZaWtvsfVlfzG/t6fHLT/jx+2t8UvF+kzx3Wkaz4juf7PuYjuju7aEi3hmU/3ZI4kcezjODkV/Sh8U9J0TXvhr4jsfE1wtn4bvdLuYNWna/fT1gtGiZZnNwjo8IEZY+arqyY3BlIyP5qf29PAHhi5/aG1T/hnfw5baR8M9OjW1tItb1C+uLrU5VLeZdRNKWeKJsqscchLbY97FWk8uPmz3A18TSjGkk0nfz7afie14P8V5ZkOOrYjHylGU4qMWleKV7u7ve7aVrJ9ThvhZ4/k8I/FvwzcxyFDcT3NvIQesZsrlyP8AvuOM/gK/Yn/ggXq1x498R/E7XJPM+y6fbadYQueVkkka5kkH1VY4T/20FfiX4M8HeL77x9Zy6x4bm0uLS4JjEYblbv7bNLsjRUCZIO0yfLyTkDuBX9In/BJf9kK8/Y0/Y30fQtcg+z+LfEM7+IfEEW8N9mup1RVg4JG6GCOCJtpKl43ZThhW+R4adDCclRWd3oed4tZ9hc3z94vBzU48kFddWr/52PpiiiivWPzIKKKKACiiigAr8C/+C3/wlk+HX/BSLxnc+Ssdr4ws9P8AENqqjjY9utrIf+BT2k7HPdvTFfvpX5w/8HD/AOzLJ4x+Dnhb4qafC0lx4JujpWr7FHNhdsgjlY9T5VysagDtdyMeFr57ifDSq4CTjvH3vu3/AAZ+1/R/zyjl/GFGliHaGITpXe15WcfvnGMfmfi9rum614d1mHxF4adV1i1iNvPaPJ5cWq22dxhZv4XVstG/ZiQcqxx9Am4+L37OXg/wP8UGsdc0PQ9atbLXdC8SW6+bZQSyBXWCWXBjiuY5N8MlvOAxeKQbZIyGbyoRLiv1t/4N7P2l7Xxj8HPE/wAG9UkjkvPCs0mr6VBLgrcabdPm4jCnO4RXTszk8YvYxjgmvA4Rzqaq/Uqmqd7evb7rn7f9JHwtoLL3xVgI2qQcVVS2cXopvzT5U/J30tr4F8TP2+7X/gpV4L0rS/EvxPsfhXrVokKXfhvXN1p4P1idDk3keoRIzQ8oreRfh1jYr5cvys5574b/APBP7xL8XvF1tonh/wAVfCXUry8XzLdrbxxYXazIOrqlu0sxUdciM/Sv1o1j9gD4EeIdRmvNQ+Cnwjvry5O6We48H6dLJKfVmaEkn613nw7+FPhf4Q6J/ZnhPw3oPhjTc7vsmk6fDZQZ9dkSqv6V+hcx/Ep8sfsQ/wDBITwv+zX4js/F3iu/j8ZeMrFxNYgQeXpukyAcPFG2WlmUk4lcgD5SsaMNx+x6KKkAooooAKKKKACiiigArF8f+BdI+J/gjWPDevWUWpaHr1nLp9/aS52XMEqFJEOCCMqSMggjORXyj8bv+CiPjr4O/F34xLbeA4fFHgf4QQXVxqcltDcWs1vDD4bi1hZ5L1i1uS9zLFafZ1j8xEnFwT5cbgzfEL9tn4mfCDXrfwfrdv8ADXU/FuvxeHL7S9S0T7XPplhBqniCx0aRbmBpFkbYb0ywSrIi3QgnGyHyTu5ZYim009le+h71HIsdGdOdOycuWUWpa62aemzSab6q6aON8S/8G8PwR1e3C2GvfErRZFHDQapbTBjjjcJrZyR6gEE+orhvhn/wRb+In7Fnx68O/En4U/ELSfFU+gXJa50PWbN9Lk1KxcbLi1+0RtLGzyRltheNESRYnJ+XI9Zf/goF8RNQ+ONp8N7XR/C82uaTq+uaRrmqaPpN/r9ncmwi0CeOSCCKWKSBSmuLHN5ryCCe2ePdIMMev/Ym/bn8Q/tLfHvx54R1jR9Js7Pw6L2aznt4p7WXZBrup6WqATMReBksFlaeDbHE8hiYbsE+Usry51E4Q5ZJ6NXWq16afgfpD8ROO6WAq08VjHWoThacanLUUoSvHXmTkr2eqkpLTVaH1IrbhS18T+Cf+CnviHxJ+zr4+8ZXWg+G7XUvCfhnw/rtraedII7l9TmuIdrZbdt3wFEK8swZeSMVB8UP+CkXjr4RfDzV/F17pPgXU9L1DUfiBo+g6fDJcW2oWlx4aXW5I5rgM7rcW8yaMVkePyTDJdQKA4bcvrfWqdr/ANdvzPzb/VzHc7g4q9+Xdb8vNb/wHX/gn2/RmvluL9r3x54q0n4l+LtFs/Adn4P+GF1qOkXmnahLcNrF/d2liJzLuRhHbq8rxeXCySNLAyS+ZH5qovMaJ/wVRj1L4+2Hg+XT9BjtbrwvCzTi8b7UniWTShrAsvIP/Ln9iI/f7s+eyxY3EGqliIK1+plHIcZLm5I35Vd2eyVm7/evvPsuiuL/AGdviPdfGT9n/wAC+L761hs73xX4e0/WJ7eEkxwSXFtHMyLu52guQM84FdpW0ZJq6PLq05U5unPdNp+qCiiimZhRRRQB8v8AxM/bJ0j4F/E/4gWNj4O8IzXkWt2VnqctvrcdtqN9LLY6bt1HUIhbExWccNxFbfaXeQhorePaFkBj5fVdU+Bvgf8AY08aajo/wd+Cv9jSeJ7bS/EfhSO30620u6n/ALbTT4Lq9K2pQ/wXSNJCTgLg9Hr7IorF0m9336d/6+Z6sMwpwUeWDTTi3abV+W2nl3Vvh6bHxfe6l8JfGdna6BrPwG+DeoeCPhefEbwQG2tdSXw9Bp8to8v2G1SwaDzbhLm2lMUEo2vmN286NkWxZftgeHPDuo2X2X4VaNo+v2fhm9v57DSJlj8TWF3e3OpyarbWFq1pG8yNe6P5ss5aLzmaOZoyVGfsiil7FrZ/gW8ypyXLODa/xytd3v8AffX/AIe/xX4A/wCFVeE/gd4L1n/hXPwh1a8+FerWvhHw54m1fXINRsLJVtYrsXlvrstmZcb5NjOsSk3KyLnI3VZ+Gvjf4ZeILT4va9o/wB+H1hrl9DothrdmLWwt9W8VDX7a0upbfUwbZdqmS+2OJnkWVkkLbecfZdFL2P8AVkOWbKXNeLu3/PLun8+vzd90fDOp/HHwB4x+LGpeJvEP7P8A8Pj4v0Hw9K+3UYbC78WXcLa5faENPtisDb5Wt7aUCBZWjeW7S38wITM3UaX+0T4Fl0//AIRdfhn8MbfQ7W6sta/s/wC326wa3fPa2GoKdHg+xhdQv45bhQFxC+/7I25TOBF9fUVSotdfwQp5lSla9N6be/LTtb01t6nz/wDsHSeAYPD+v2vgH4f/AAu8E2e60urqX4fPbXGjXzyxEiN54ba3DXcSr88ZjJSOa3bcfN2r9AUUVpGPKrHnYmt7Wq6muvdtv73qwoooqjAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAP//Z" alt="Smiley face" height="150" width="150"></td>'
            + '</tr> '
            + '<tr class="Distrito"> '
            + '<td colspan="2"><h4><b>Distrito:</b> ' + $("#Juzgados option:selected").text() + '</h4></td>'
            + '</tr>'
            + '<tr class="Juzgado"> '
            + '<td colspan="2"><h4><b>Juzgado:</b> ' + $("#Juzgado option:selected").text() + '</h4></td>'
            + '</tr>'
            + '</table>';

        $("#Complemento").prepend(htmlStringDatos);
    });
}

function Barra(elementId,json, id) {

    var datos = [];
    for (var x = 0; x < json.length; x++) {
        datos.push({
            y: json[x].Fecha,
            x: json[x].Calculo
        });
    }

    Morris.Bar({
        element: elementId,
        data: datos,
        xkey: 'y',
        ykeys: ['x'],
        labels: ['Indicador'],
        stacked: true,
        //xLabels: 'month',
        //dateFormat: function (x) {
        //    var t = new Date(x);
        //    return String((t.getMonth() + 1)).getMes() + '/' + t.getFullYear();
        //},
        //xLabelFormat: function (d) {
        //    return String((d.getMonth() + 1)).getMes() + '/' + d.getFullYear();
        //},
        postUnits: jsonGestion[String(id)].unidad,
        barOpacity: .8,
        hideHover: 'auto',
        ymax: 'auto',
        resize: true
    });

}

function Linea(elementId, json, id) {

    var datos = [];
    for (var x = 0; x < json.length; x++) {
        datos.push({
            y: json[x].Fecha,
            x: json[x].Calculo
        });
    }

    Morris.Line({
        element: elementId,
        data: datos,
        xkey: 'y',
        ykeys: ['x'],
        labels: ['Indicador'],
        xLabels: 'month',
        dateFormat: function (x) {
            var t = new Date(x);
            return String((t.getMonth() + 1)).getMes() + '/' + t.getFullYear();
        },
        xLabelFormat: function (d) {
            return String((d.getMonth() + 1)).getMes() + '/' + d.getFullYear();
        },
        postUnits: jsonGestion[String(id)].unidad,
        barOpacity: .8,
        hideHover: 'auto',
        ymax: 'auto',
        resize: true
    });

}

function Dona(json, id, idx) {

    var datos = [];

    datos.push({
        label: jsonGestion[String(id)].descdona +" "+ json[idx].Fecha,
        value: json[idx].Calculo
    });

    var color = [json[idx].Semaforo];

    var donut = Morris.Donut({
        element: 'morris-donut-chart',
        data: datos,
        formatter: function (y, data) {
            return y + jsonGestion[String(id)].sim;
        },
        colors: color,
        resize: true
    });

    $("#Desc1").text(json[idx].Valor);
    $("#Tex1").text(jsonGestion[id].desc1 + " de " + mesAnio(json[idx].Fecha));
    $("#Desc2").text(json[idx].Total);
    $("#Tex2").text(jsonGestion[id].desc2 + " de " + mesAnio(json[idx].Fecha));
    $("#Desc3").text(json[idx].Calculo + jsonGestion[id].sim);
    $("#Tex3").text(jsonGestion[id].desc3 + " de " + mesAnio(json[idx].Fecha));

    donut.select(0);
}

function eventosClickG() {

    $("#indicadores-gestion").on("click", "ul > li > a", function () {

        if ($(this).attr("data-activo").toString().match("true")) {
            mascara2("maskRef", "0.6");
            var id = $(this).attr("data-id");
            consultarReporte(id);
            desvaneceMascara("maskRef");
        }       
    });

    $("#tabla-gestion").on("click", function () {
       /// pestActual = "gen-reporte";

        mascara2("maskRef", "0.6");
        consultaTabla();
        desvaneceMascara("maskRef");

    });

    $("#indicadores-generales").on("click", function () {
        mascara2("maskRef", "0.6");
        consultarTodo();
        desvaneceMascara("maskRef");
    });
}

function getGET() {
    var loc = document.location.href;
    // si existe el interrogante
    if (loc.indexOf('?') > 0) {
        // cogemos la parte de la url que hay despues del interrogante
        var getString = loc.split('?')[1];
        // obtenemos un array con cada clave=valor
        var GET = getString.split('&');
        var get = {};
        // recorremos todo el array de valores
        for (var i = 0, l = GET.length; i < l; i++) {
            var tmp = GET[i].split('=');
            get[tmp[0]] = unescape(decodeURI(tmp[1]));
        }
        return get;
    }
}

function ValidaUsuario() {

    var gets = getGET();
    var mat = gets["mat"];
    var dataJson = "{mat:'" + mat + "'}";

    consulta("Construye", dataJson, function (data) {
        json = data;
        $("#Materia").html(data);
    });

    if (mat == "0") {
        $("#Materia").val('1');
    } else {
        $("#Materia").val(mat);
    }
    
    if (mat.toString() === "1") {
        $("li.gestion > a[data-id=4]").remove();
    }

}