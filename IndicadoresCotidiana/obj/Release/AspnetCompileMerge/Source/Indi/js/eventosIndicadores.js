var jsonMenu;
var jData;
var ancho = ($(window).width());
var alto = ($(window).height());
var Meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

$(function () {
    consulta("VerificaLogin", "{}", function (data) {
        if (data == "si") {           
            creaJsonMenuIndicadoresT($("#Indicadores-materia > ul"));
            eventosMouseover();
            eventosClick();
            eventosMouseleave();
            eventosChange();
            eventosResize();
        } else {
            window.location.href = "../../Login.aspx";
        }
    });

    $('#tabla-pjem').dataTable({

    });
});

function creaJsonMenuIndicadoresT(padre) {

    var actual = $(padre);
    var menuIndicadoresM = "";
    var i = 1;

    $.each(jsonMenu, function (val, item) {

        var mInd = "";
        var arrow = "";
        var c = "n";

        if (item.submenu == 'y') {
            mInd = 'class="m-ind"';
            arrow = '<span class="arrow-ind fa  fa-angle-left" style="float: right">';
        } else {
            c = 'y"';
        }

        menuIndicadoresM += '<li ' + mInd + '><a data-i="' + val + '" data-p="' + item.padre + '" data-a="' + c + '">' + i + '.- ' + item.titulo + arrow + '</a></li>\n';
        i++;
    })

    $(actual).append(menuIndicadoresM);

}

function obtenJson(elemento) {
    var arrayRec = [];
    var jsonMenuTemp = jsonMenu;
    recorrido(elemento, arrayRec);

    for (var i = (arrayRec.length-1); i >= 0; i--) {
        jsonMenuTemp = jsonMenuTemp[arrayRec[i]];
    }

    return jsonMenuTemp;
}

function recorrido(elemento, arrayRec) {

    arrayRec.push($(elemento).children("a").attr("data-i"));
    var padre = $(elemento).children("a").attr("data-p");

    if (padre.length != "") {
        arrayRec.push("hijos");
        recorrido(($("body").find("li > a[data-i=id" + padre + "]")).closest("li"), arrayRec);
    } 
    
}

function obtenYmuestraAllDatos() {

    try {
        
        $("#Tex1, #Tex2, #Tex3, #Desc1, #Desc2, #Desc3, .page-header-pj > span").text("");
        $("#morris-bar-chart, #morris-donut-chart").empty();
        $("#Complemento *:not(#button-pdf-exporta)").remove();
        $("#button-pdf-exporta").show();

        jActual = null;
        secActual = null;
        jData = null;

        var idDisJuz = $("#Juzgados").val();
        var cveJuzgado = $("#Juzgado").val();
        var nomJuz = $("#Materia").val();
        var anio = $("#Anios").val();
        var mes = $("#Meses").val();
        var fechaD = new Date(anio, mes, 0);
        var fechaF = fechaD.toISOString().substring(0, 10);
        var dataJ = "{fechaF:'" + fechaF + "', idDisJuz:'" + idDisJuz + "', materia:'" + nomJuz + "', cveJuzgado:'" + cveJuzgado +"'}";
        var rangoFechas = rangoFechasConsulta(mes, anio);
        var position = 1;


        $("#Complemento").append("<div id=\"complemento_temporal\"></div>");

        consulta("DatosIndicadores", dataJ, function (data) {

            $.each(data, function (index, item) {

                var j = evaluaDatos(item.datoIndicador, anio, mes);

                var clasecss = "";

                if (position == 1) {
                    clasecss = "divIzquierdo";
                    position = 2;
                } else if (position == 2) {
                    clasecss = "divDerecha";
                    position = 1;
                }

                var msj2 = item.id != "1" ? jsonMsj[item.id].desc2 + ': ' + j[0].total : "";
                //var msj3 = item.id != "1" ? jsonMsj[item.id].desc3 + ': ' + j[0].total : "";
                var msj3 = item.id != "1" ? jsonMsj[item.id].desc3 + ' ' + $('#Juzgados option:selected').text() + ': ' + promIndice(j, 0, item.id) + ' ' + jsonMsj[item.id].sim: " ";

                //htmlStringDatos = ' <div class="col-sm-6">' 
                htmlStringDatos = ' <div>' 
                    + ' <div style = "width:100%; display:inline-table;">'
                    + ' <table class="tablaGraficas">'
                    + ' <tr>'
                    //+ ' <td> <h5 class="page-header"> <b>' + item.id + ".- " + item.nomIndicador + '</b></h5 ></td>'
                    + ' <td> <h5 class="page-header"> <b>' + item.nomIndicador + '</b></h5 ></td>'
                    + ' </tr>'
                    + ' <tr>'
                    + ' <td> <div id="grafica-' + item.id + '" class="ContenGreafica"></div></td>'
                    + ' </tr>'
                    + ' </table> </div>'
                    + ' <div style="width:100%; display:inline-table;"> <table class="tablavariables" >'
                    + ' <tr ALIGN=LEFT>'
                    + ' <td><label>' + jsonMsj[item.id].desc1 + ': ' + j[0].valor +'</label></td>'
                    + ' </tr>'
                    + ' <tr ALIGN=LEFT>'
                    + ' <td><label>' + msj2 +'</label></td>'
                    + ' </tr>'
                    + ' <tr ALIGN=LEFT>'
                    + ' <td><label>' + msj3 +'</label></td>'
                    + ' </tr>'
                    + ' </table>'
                    + ' </div>'
                    + '</div>';

                $("#complemento_temporal").prepend(htmlStringDatos);

                gBarra("grafica-" + item.id, j, item.id); 

            });

            var htmlStringDatos = '<table id="tablaDatosReporte">'
                + '<tr class="Encabezado"> '
                + '<td><h2 class="page-header"><b>' + 'INDICADORES' + ' ' + $("#Juzgados option:selected").text() + '</b></h2></td>'
                + '<td> <img src="data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/4QAiRXhpZgAATU0AKgAAAAgAAQESAAMAAAABAAEAAAAAAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCABkAGQDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD9/KKKKACiiigAooqG6uo7GBpZpI4Yk5Z3IVV+pNAHGftH+MPF3w9+DWt+IPA/h+z8XeItEgN9DodxPJbnWI4/mlt4pURyk7IG8smNwXCqQA25flD4Uf8ABfb4UeMLW3Hifw5448GzOitLO1kmp2PP/PN7d2mcAYOTAvXgGvuCwv4dRt/Nt5obiFujxuHU/iOK/M/9sn9hWw8F/HrWpNMsY4dJ1yQ6rZxomEh80kyRgAAALKHwo4VCg9KqNmG2jPsT4P8A/BSv4JfHrxfpvh/wv46t73WtYdo7KyudOvLCa4ZUZyoFxDHztRjg8nGBk4Fe6V+Ntl+zDc+HNZs9S00yWOpabcR3lndRLiS1njcPHIueNyuqsD6gV+tnwb8ff8LR+F+h+IGjjgm1K1R7iJCSsM4+WVAT1CyK65PpQ1YDpqKKKkAooooAKKKKACo551t42d2VEQFmZjgKBzkn0qSvyZ/4Oaf28dW8DeDtH+APhW+n0+48aWB1fxdPA5SR9KMjRQWIYDOy4kimMoBBMcAjYMk7isMTiI0KbqS6HrZHk9fNMbDBYfeXXslq2/T/AIBY/b1/4Ojfh/8AB/X9S8K/BrwvJ8VtQtHktLnxBLqJ03QUYfKWtZI1aa7AYMN6CKMja8c0gINfmf41/wCCtOn/AB08WS6l8Wv2dfht44jck/abbxJ4g0/WIxjjF9PeXWcYHDRFeACpHFfOZ8Lgjla/WL/ggF/wRe8P/FvQbf46fFvQrXWdFknZPB3h+/jElreGJykmo3MR4kQSKyQxyDadrylXDQMvzkMTVx1T2dk12aTS++5+z1slwnCuE+uxnOEtk4zlGUn2Ti1bS/or3PP/AIT/ALCFn4s+AVr+0J8ItW+JXwt8KMU81/EWnS6RrGlAhCtxDfWvlpeWLMwMd3Ds4I3qpDkfUn7N/wC2J4muLbT/AA98WPHfhX4gaPZ5Sx19ZUh1bTgwAKzgYW5i+VPmIEykMxaYkKP1F8Q+HrHxZol7peqWdrqGm6lbyWl3aXMYlhuYZFKPG6nhlZWIIOQQSK/mi/ae+E6/s6/tI+O/Ais0sHhPXLrT7WSVt8klsshNuzkjlzAYi3uT1615ebYevlVSGJwUmot6x6X9Oz/Dp5fdeHeYYXxGwuIybiS3taSUoT5IufJe3xpRlzRdru9pJrmTs7/ttH4Z0HULeOaG6t5YZkWWKRCHSVGAZXVhkMpBBBHBBBFe2fsyNBYeFb7TYJllW1ufOUA/6tZB0/76Vj+Jr8Zf+Ccf7YF9ouj6j4A1K7eaDTYW1TRizFmhgMipcwZ7RpLLC6Dk5uJRkKiKP0u/4JzfFz/hOfG/iWxEu9VsIZyM5+7Iyj/0M19pgMZHFYaNePXp2fVfefzdxlwzW4ezmvlFd8zpy0ltzRaTjLyumnbo9D62ooorqPmQooooAKKKKACv5sf+CvfjqT4tf8FK/i9qcrs0djrQ0WBNxZYUsoIrQquemZIZHI/vSNX9J1fzOf8ABQHw9Lov7ePxohlH7xvG+r3Az/dlvJZV/wDHXFfM8UVHChBef6H739H/AC+nic4xEpK8o09PRyjf8kYFr/wTz+KGu6FoWoWfh/zdG8T21rc2mq211BNHDBdIjxztHvEu1UkDn5cAA896/pS+Cdt4R8P/AA30Xw74IvdJuvDvhXT7bSrGHT7qO4jtLeGMRRR5QkDCRgD/AHa/I7/gm1/wUA8PzfsxaL4K8SWdrLrnw5H/AAjs5dtrvZpk6fIBn7pszDHnu9vLjgYH3r+w/wDtFeCfE3xGvtG0ny7PUddtVeNWkB+0NBvbYvctskkbHojHtXpZXgaNKn7Wi2+ZJ6nxHiHxRmWY4z6hmMIweHlONopq+qV2m3rZaWtvsfVlfzG/t6fHLT/jx+2t8UvF+kzx3Wkaz4juf7PuYjuju7aEi3hmU/3ZI4kcezjODkV/Sh8U9J0TXvhr4jsfE1wtn4bvdLuYNWna/fT1gtGiZZnNwjo8IEZY+arqyY3BlIyP5qf29PAHhi5/aG1T/hnfw5baR8M9OjW1tItb1C+uLrU5VLeZdRNKWeKJsqscchLbY97FWk8uPmz3A18TSjGkk0nfz7afie14P8V5ZkOOrYjHylGU4qMWleKV7u7ve7aVrJ9ThvhZ4/k8I/FvwzcxyFDcT3NvIQesZsrlyP8AvuOM/gK/Yn/ggXq1x498R/E7XJPM+y6fbadYQueVkkka5kkH1VY4T/20FfiX4M8HeL77x9Zy6x4bm0uLS4JjEYblbv7bNLsjRUCZIO0yfLyTkDuBX9In/BJf9kK8/Y0/Y30fQtcg+z+LfEM7+IfEEW8N9mup1RVg4JG6GCOCJtpKl43ZThhW+R4adDCclRWd3oed4tZ9hc3z94vBzU48kFddWr/52PpiiiivWPzIKKKKACiiigAr8C/+C3/wlk+HX/BSLxnc+Ssdr4ws9P8AENqqjjY9utrIf+BT2k7HPdvTFfvpX5w/8HD/AOzLJ4x+Dnhb4qafC0lx4JujpWr7FHNhdsgjlY9T5VysagDtdyMeFr57ifDSq4CTjvH3vu3/AAZ+1/R/zyjl/GFGliHaGITpXe15WcfvnGMfmfi9rum614d1mHxF4adV1i1iNvPaPJ5cWq22dxhZv4XVstG/ZiQcqxx9Am4+L37OXg/wP8UGsdc0PQ9atbLXdC8SW6+bZQSyBXWCWXBjiuY5N8MlvOAxeKQbZIyGbyoRLiv1t/4N7P2l7Xxj8HPE/wAG9UkjkvPCs0mr6VBLgrcabdPm4jCnO4RXTszk8YvYxjgmvA4Rzqaq/Uqmqd7evb7rn7f9JHwtoLL3xVgI2qQcVVS2cXopvzT5U/J30tr4F8TP2+7X/gpV4L0rS/EvxPsfhXrVokKXfhvXN1p4P1idDk3keoRIzQ8oreRfh1jYr5cvys5574b/APBP7xL8XvF1tonh/wAVfCXUry8XzLdrbxxYXazIOrqlu0sxUdciM/Sv1o1j9gD4EeIdRmvNQ+Cnwjvry5O6We48H6dLJKfVmaEkn613nw7+FPhf4Q6J/ZnhPw3oPhjTc7vsmk6fDZQZ9dkSqv6V+hcx/Ep8sfsQ/wDBITwv+zX4js/F3iu/j8ZeMrFxNYgQeXpukyAcPFG2WlmUk4lcgD5SsaMNx+x6KKkAooooAKKKKACiiigArF8f+BdI+J/gjWPDevWUWpaHr1nLp9/aS52XMEqFJEOCCMqSMggjORXyj8bv+CiPjr4O/F34xLbeA4fFHgf4QQXVxqcltDcWs1vDD4bi1hZ5L1i1uS9zLFafZ1j8xEnFwT5cbgzfEL9tn4mfCDXrfwfrdv8ADXU/FuvxeHL7S9S0T7XPplhBqniCx0aRbmBpFkbYb0ywSrIi3QgnGyHyTu5ZYim009le+h71HIsdGdOdOycuWUWpa62aemzSab6q6aON8S/8G8PwR1e3C2GvfErRZFHDQapbTBjjjcJrZyR6gEE+orhvhn/wRb+In7Fnx68O/En4U/ELSfFU+gXJa50PWbN9Lk1KxcbLi1+0RtLGzyRltheNESRYnJ+XI9Zf/goF8RNQ+ONp8N7XR/C82uaTq+uaRrmqaPpN/r9ncmwi0CeOSCCKWKSBSmuLHN5ryCCe2ePdIMMev/Ym/bn8Q/tLfHvx54R1jR9Js7Pw6L2aznt4p7WXZBrup6WqATMReBksFlaeDbHE8hiYbsE+Usry51E4Q5ZJ6NXWq16afgfpD8ROO6WAq08VjHWoThacanLUUoSvHXmTkr2eqkpLTVaH1IrbhS18T+Cf+CnviHxJ+zr4+8ZXWg+G7XUvCfhnw/rtraedII7l9TmuIdrZbdt3wFEK8swZeSMVB8UP+CkXjr4RfDzV/F17pPgXU9L1DUfiBo+g6fDJcW2oWlx4aXW5I5rgM7rcW8yaMVkePyTDJdQKA4bcvrfWqdr/ANdvzPzb/VzHc7g4q9+Xdb8vNb/wHX/gn2/RmvluL9r3x54q0n4l+LtFs/Adn4P+GF1qOkXmnahLcNrF/d2liJzLuRhHbq8rxeXCySNLAyS+ZH5qovMaJ/wVRj1L4+2Hg+XT9BjtbrwvCzTi8b7UniWTShrAsvIP/Ln9iI/f7s+eyxY3EGqliIK1+plHIcZLm5I35Vd2eyVm7/evvPsuiuL/AGdviPdfGT9n/wAC+L761hs73xX4e0/WJ7eEkxwSXFtHMyLu52guQM84FdpW0ZJq6PLq05U5unPdNp+qCiiimZhRRRQB8v8AxM/bJ0j4F/E/4gWNj4O8IzXkWt2VnqctvrcdtqN9LLY6bt1HUIhbExWccNxFbfaXeQhorePaFkBj5fVdU+Bvgf8AY08aajo/wd+Cv9jSeJ7bS/EfhSO30620u6n/ALbTT4Lq9K2pQ/wXSNJCTgLg9Hr7IorF0m9336d/6+Z6sMwpwUeWDTTi3abV+W2nl3Vvh6bHxfe6l8JfGdna6BrPwG+DeoeCPhefEbwQG2tdSXw9Bp8to8v2G1SwaDzbhLm2lMUEo2vmN286NkWxZftgeHPDuo2X2X4VaNo+v2fhm9v57DSJlj8TWF3e3OpyarbWFq1pG8yNe6P5ss5aLzmaOZoyVGfsiil7FrZ/gW8ypyXLODa/xytd3v8AffX/AIe/xX4A/wCFVeE/gd4L1n/hXPwh1a8+FerWvhHw54m1fXINRsLJVtYrsXlvrstmZcb5NjOsSk3KyLnI3VZ+Gvjf4ZeILT4va9o/wB+H1hrl9DothrdmLWwt9W8VDX7a0upbfUwbZdqmS+2OJnkWVkkLbecfZdFL2P8AVkOWbKXNeLu3/PLun8+vzd90fDOp/HHwB4x+LGpeJvEP7P8A8Pj4v0Hw9K+3UYbC78WXcLa5faENPtisDb5Wt7aUCBZWjeW7S38wITM3UaX+0T4Fl0//AIRdfhn8MbfQ7W6sta/s/wC326wa3fPa2GoKdHg+xhdQv45bhQFxC+/7I25TOBF9fUVSotdfwQp5lSla9N6be/LTtb01t6nz/wDsHSeAYPD+v2vgH4f/AAu8E2e60urqX4fPbXGjXzyxEiN54ba3DXcSr88ZjJSOa3bcfN2r9AUUVpGPKrHnYmt7Wq6muvdtv73qwoooqjAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAP//Z" alt="Smiley face" height="150" width="150" style="float:left"></td>'
                + '</tr> '
                + '<tr class="Encabezado"> '
                + '<td colspan="2"><h4 class="page-header  ">' + '<b>Distritos:</b> 18' + '</h4></td>'
                + '</tr>'
                + '</table>';

            $("#Complemento").prepend(htmlStringDatos);

        })

    } catch (e) {
        console.error("Error en consulta todos los datos: "+e.toString());
    } finally {
        desvaneceMascara("maskRef");
    }

}

function tablaAllDatos() {

    //jActual = null;
    //secActual = null;
    //jData = null;

    var idDisJuz = $("#Juzgados").val();
    var nomJuz = $("#Materia").val();
    var cveJuzgado = $("#Juzgado").val();
    var anio = $("#Anios").val();
    var mes = $("#Meses").val();
    var fechaD = new Date(anio, mes, 0);
    var fechaF = fechaD.toISOString().substring(0, 10);
    var dataJ = "{fechaF:'" + fechaF + "', idDisJuz:'" + idDisJuz + "', materia:'" + nomJuz + "', cveJuzgado:'" + cveJuzgado +"'}";

    var rangoFechas = rangoFechasConsulta(mes, anio);

    var datosTabla = [];

    consulta("DatosIndicadores", dataJ, function (data) {

        $.each(data, function (index, item) {

            var j = evaluaDatos(item.datoIndicador, anio, mes);
            datosTabla.push([
                item.id,
                item.nomIndicador,
                j[0].valor,
                j[0].total,
                promIndice(j, 0, item.id) + jsonMsj[item.id].sim,
                promIndice(j, 1, item.id) + jsonMsj[item.id].sim,
                promIndice(j, 2, item.id) + jsonMsj[item.id].sim,
                promIndice(j, 3, item.id) + jsonMsj[item.id].sim
            ]);

        });

    })

    $("#div-flotante-tabla").dialog({
        title: "INDICADORES DE JUZGADOS EN MATERIA " + ($('#Materia option:selected').text()).toUpperCase() + " DE " + $('#Juzgados option:selected').text(),
    });
    $("#div-flotante-tabla").dialog("open");

    if (!$.fn.DataTable.isDataTable('#tabla-indicadores')) {
        $('#tabla-indicadores').DataTable({
            data: datosTabla,
            columns: [
                { title: "Id" },
                { title: "Indicador" },
                { title: "" },
                { title: "Total" },
                { title: mesAnio(rangoFechas[0]) },
                { title: mesAnio(rangoFechas[1]) },
                { title: mesAnio(rangoFechas[2]) },
                { title: mesAnio(rangoFechas[3]) }
            ],
            scrollCollapse: true,
            paging: true,
            dom: 'Bfrtip',
            buttons: [
                    /*'copy', 'csv', 'excel',*/ {
                    extend: 'pdfHtml5',
                    download: 'open',
                    messageTop: "INDICADORES DEL JUZGADO DE " + $("#Juzgados option:selected").text(),
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    customize: function (doc) {
                        doc.content.splice(1, 0, {
                            margin: [0, 0, 0, 12],
                            alignment: 'right',
                            image: 'data:image/jpeg;base64,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'
                        });
                    }

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
            }
        });
    }


    desvaneceMascara("maskRef");
}

function obtenYmuestraDatos(jsonActual) {
    try {
        $(".sub-m-ind").remove();

        jActual = null;
        secActual = null;
        jData = null;

        var cveDistrito = $("#Juzgados").val();
        //var cveJuzgado = $("#Juzgado").val();
        var cveJuzgado = ($("#Juzgado").val().length > 0) ? $("#Juzgado").val() : 0;
        var cveMateria = $("#Materia").val();
        var anio = $("#Anios").val();
        var mes = $("#Meses").val();
        var fechaD = new Date(anio, mes, 0);
        var fechaF = fechaD.toISOString().substring(0, 10);

         //var dataJ = "{ind:'" + jsonActual.id + "', fechaF:'" + fechaF + "', idDisJuz:'" + idDisJuz + "', materia:'" + nomJuz + "', cveJuzgado:'" + cveJuzgado + "'}";

        var dataJson = "{id:" + jsonActual.id + ", anio:'" + anio + "', mes:'" + mes + "', cveDistrito:'" + cveDistrito + "', materia:'" + cveMateria + "', cveJuzgado:" + cveJuzgado + "}";

        var result = [];

        $("#Tex1, #Tex2, #Tex3, #Desc1, #Desc2, #Desc3, .page-header-pj > span").text("");
        $("#morris-bar-chart, #morris-donut-chart").empty();
        $("#Complemento *:not(#button-pdf-exporta)").remove();
        $("#button-pdf-exporta").hide();

        consulta("IndicadorCotidiana", dataJson, function (data) {
            result = evaluaDatos(data, anio, mes);
            jData = result;
        });

        if (result != null && result.length) {

            $("#titulo-indicador").text(jsonActual.descripcion + " " + $('#Materia option:selected').text() + "es");

            if (String($('#Juzgados option:selected').val()) != '') {
                $("#distrito").text(String($('#Juzgado option:selected').text()));
            } else {
                $("#distrito").text(String($('#Juzgados option:selected').text()).capital());
            }
            
            gBarra("morris-bar-chart", result, jsonActual.id);

            $("#morris-bar-chart circle").each(function (i, e) {
                $(this).attr("data-point", (3 - i));
                $(this).attr("style", "cursor: pointer");
            });

            if (!(/\b(1)\b/).test(String(jsonActual.id).trim())) {
                gDona(result, jsonActual.id, 0);
            } else {
                mensajesAct(result, jsonActual.id, 0);
            }

        } else {
            console.log("Algo ocurrio cuando se compoaraba el result");
        }

        secActual = "indicadores";
        jActual = jsonActual;

        
    } catch (e) {
        console.error("Error en consulta: " + e.toString());
    } finally {
        desvaneceMascara("maskRef");
    }
}

function obtenYmuestraDatosTabla(jsonActual) {

    try {
        $(".sub-m-ind").remove();

        jActual = null;
        secActual = null;
        jData = null;

        var idDisJuz = $("#Juzgados").val();
        var cveJuzgado = $("#Juzgado").val();
        var materia = $("#Materia").val();
        var anio = $("#Anios").val();
        var mes = $("#Meses").val();
        var fechaD = new Date(anio, mes, 0);
        var fechaF = fechaD.toISOString().substring(0, 10);


        switch (materia) {
            case "civil":
                materia = "c";
                break;
            case "mercantil":
                materia = "m";
                break;
            case "familiar":
                materia = "f";
                break;
        } 

        var dataJ = "{ind:'" + jsonActual.id + "', fecha:'" + fechaF + "', distrito:'" + idDisJuz + "', materia:'" + materia + "', cveJuzgado:'" + cveJuzgado +"'}";

        var result = [];

        $("#Tex1, #Tex2, #Tex3, #Desc1, #Desc2, #Desc3, .page-header-pj > span").text("");
        $("#morris-bar-chart, #morris-donut-chart").empty();
        $("#Complemento *:not(#button-pdf-exporta)").remove();
        $("#button-pdf-exporta").hide();

        consulta("DatosIndicadorTabla", dataJ, function (data) {
            result = evaluaDatosTabla(data, anio, mes);
            jData = result;
        })

        if (result != null && result.length) {

            $("#titulo-indicador").text(jsonActual.descripcion + " " + $('#Materia option:selected').text() + "es");

            if (String($('#Juzgados option:selected').val()) != '') {
                $("#distrito").text(String($('#Juzgado option:selected').text()));
            } else {
                $("#distrito").text(String($('#Juzgados option:selected').text()).capital());
            }

            gBarra("morris-bar-chart", result, jsonActual.id);

            $("#morris-bar-chart circle").each(function (i, e) {
                $(this).attr("data-point", (3 - i));
                $(this).attr("style", "cursor: pointer");
            })

            gTabla(result, jsonActual.id,0);

            if (!(/\b(1)\b/).test(String(jsonActual.id).trim())) {
                gDona(result, jsonActual.id, 0);
            } else {
                $("#Desc1").text(result[0].valor);
                $("#Tex1").text(jsonMsj[String(jsonActual.id)].desc1);
            }

        } else {
            console.log("Algo ocurrio cuando se compoaraba el result");
        }
        
        secActual = "indicadoresTabla";
        jActual = jsonActual;


    } catch (e) {
        console.error("Error en consulta: " + e.toString());
    } finally {
        desvaneceMascara("maskRef");
    }
}

function obtenYmuestraDatosTerminos(jsonActual) {

    try {
        $(".sub-m-ind").remove();

        jActual = null;
        secActual = null;
        jData = null;

        var idDisJuz = $("#Juzgados").val();
        var cveJuzgado = $("#Juzgado").val();
        var materia = $("#Materia").val();
        var anio = $("#Anios").val();
        var mes = $("#Meses").val();
        var fechaD = new Date(anio, mes, 0);
        var fechaF = fechaD.toISOString().substring(0, 10);

        var dataJ = "{ind:'" + jsonActual.id + "', fecha:'" + fechaF + "', distrito:'" + idDisJuz + "', materia:'" + materia + "', cveJuzgado:'" + cveJuzgado + "'}";

        var result = [];

        $("#Tex1, #Tex2, #Tex3, #Desc1, #Desc2, #Desc3, .page-header-pj > span").text("");
        $("#morris-bar-chart, #morris-donut-chart").empty();
        $("#Complemento *:not(#button-pdf-exporta)").remove();
        $("#button-pdf-exporta").hide();

        consulta("DatosIndicadorTermino", dataJ, function (data) {
            result = evaluaDatosTermino(data, anio, mes);
            jData = result;
        });

        if (result != null && result.length) {

            $("#titulo-indicador").text(jsonActual.descripcion + " " + $('#Materia option:selected').text() + "es");

            if (String($('#Juzgados option:selected').val()) != '') {
                $("#distrito").text(String($('#Juzgado option:selected').text()));
            } else {
                $("#distrito").text(String($('#Juzgados option:selected').text()).capital());
            }

            gBarra("morris-bar-chart", result, jsonActual.id);

            $("#morris-bar-chart circle").each(function (i, e) {
                $(this).attr("data-point", (3 - i));
                $(this).attr("style", "cursor: pointer");
            });

            gTablaResoluciones(result, jsonActual.id, 0);

            if (!(/\b(1)\b/).test(String(jsonActual.id).trim())) {
                gDona(result, jsonActual.id, 0);
            } else {
                $("#Desc1").text(result[0].valor);
                $("#Tex1").text(jsonMsj[String(jsonActual.id)].desc1);
            }

        } else {
            console.log("Algo ocurrio cuando se compoaraba el result");
        }

        secActual = "indicadoresTermino";
        jActual = jsonActual;


    } catch (e) {
        console.error("Error en consulta: " + e.toString());
    } finally {
        desvaneceMascara("maskRef");
    }
}

function obtenYmuestraDatosTramite(jsonActual) {

    try {
        $(".sub-m-ind").remove();

        jActual = null;
        secActual = null;
        jData = null;

        var idDisJuz = $("#Juzgados").val();
        var cveJuzgado = $("#Juzgado").val();
        var materia = $("#Materia").val();
        var anio = $("#Anios").val();
        var mes = $("#Meses").val();
        var fechaD = new Date(anio, mes, 0);
        var fechaF = fechaD.toISOString().substring(0, 10);

        var dataJ = "{ind:'" + jsonActual.id + "', fecha:'" + fechaF + "', distrito:'" + idDisJuz + "', materia:'" + materia + "', cveJuzgado:'" + cveJuzgado + "'}";

        var result = [];

        $("#Tex1, #Tex2, #Tex3, #Desc1, #Desc2, #Desc3, .page-header-pj > span").text("");
        $("#morris-bar-chart, #morris-donut-chart").empty();
        $("#Complemento *:not(#button-pdf-exporta)").remove();
        $("#button-pdf-exporta").hide();

        consulta("DatosIndicadorTramite", dataJ, function (data) {
            result = evaluaDatosTramite(data, anio, mes);
            jData = result;
        });

        if (result != null && result.length) {

            $("#titulo-indicador").text(jsonActual.descripcion + " " + $('#Materia option:selected').text() + "es");

            if (String($('#Juzgados option:selected').val()) != '') {
                $("#distrito").text(String($('#Juzgado option:selected').text()));
            } else {
                $("#distrito").text(String($('#Juzgados option:selected').text()).capital());
            }

            gBarra("morris-bar-chart", result, jsonActual.id);

            $("#morris-bar-chart circle").each(function (i, e) {
                $(this).attr("data-point", (3 - i));
                $(this).attr("style", "cursor: pointer");
            });

            gTablaTramites(result, jsonActual.id, 0);

            if (!(/\b(1)\b/).test(String(jsonActual.id).trim())) {
                gDona(result, jsonActual.id, 0);
            } else {
                mensajesAct(result, jsonActual.id, 0);
            }

        } else {
            console.log("Algo ocurrio cuando se compoaraba el result");
        }

        secActual = "indicadoresTramite";
        jActual = jsonActual;


    } catch (e) {
        console.error("Error en consulta: " + e.toString());
    } finally {
        desvaneceMascara("maskRef");
    }
}

function mesAnio(fec) {
    var fech = fec.split("-");
    return Meses[parseInt(fech[1])-1] + " " + fech[0];
}

function promIndice(json, columna, id) {

    var m = (/\b(9|14|15)\b/).test(String(id).trim()) ? 1 : 100; 

    if ((/\b(17)\b/).test(String(id).trim())) {
        if (json[columna].total != '' && json[columna].total != '0') {
            var t = ((parseInt(json[columna].total) * m) / parseInt(json[columna].valor)).toFixed(3);
            if (t == 'undefined') {
                t = 0;
            }
            return t;
        } else {
            if (json[columna].total != '') {
                return "0";
            } else {
                return "0";
            }
        }
    } else {
        if (json[columna].total != '' && json[columna].total != '0') {
            var t = ((parseInt(json[columna].valor) * m) / parseInt(json[columna].total)).toFixed(2);
            if (t == 'undefined') {
                t = 0;
            }
            return t;
        } else {
            if (json[columna].total != '') {
                return "0";
            } else {
                return "0";
            }
        }
    }

}

function cambioMesAnio(callback) {

    var fecha = new Date();
    var anioActual = fecha.getFullYear();
    var mesActual = fecha.getMonth() ;
    var anio = $("#Anios").val();
    var mes = $("#Meses").val();
    var month = anioActual == anio ? mesActual : 12;

    $("#Meses").empty();

    for (var x = 1; x <= month; x++) {
        if (anio === 2019) {
            if (x >= 8) {
                $('#Meses').append('<option value="' + x + '">' + Meses[x-1] + '</option>');
            }
        } else {
            $('#Meses').append('<option value="' + x + '">' + Meses[x-1] + '</option>');
        }

        //$('#Meses').append('<option value="' + x + '">' + Meses[x - 1] + '</option>');
    }

    if (anioActual == anio && mesActual < mes) {
        $("#Meses").val(mesActual);
    } else {
        $("#Meses").val(mes);
    }

    callback();

}

function gTabla(jr, id, idx) {

    $("#Complemento").empty();

    var tabla = "<div class=\"container contenedor-tabla-pj\" style=\"width:auto\">"
        + "<table class=\"table-datos-pj table table-bordered table-condensed\">"
        + "<tr>"
        + "<th>Juicio</th><th>Total de juicios</th><th>Tiempo en horas</th><th>Tiempo en días</th><th>Tiempo en meses</th>"
        + "</tr>";

    $.each(jr[idx].jucios, function (index, item) {
        tabla += "<tr><td>" + item.juicio + "</td><td>" + item.totaljuicios + "</td><td>" + item.valor + "</td><td>" + parseInt(item.valor) / 24 + "</td><td>" + ((parseInt(item.valor) / 24) / 30.5).toFixed(1) + "</td></tr>"
    });

    tabla += "</table></div>";

    $("#Complemento").append(tabla);
}

function gTablaResoluciones(jr, id, idx) {

    $("#Complemento").empty();

    var tabla = "<div class=\"container contenedor-tabla-pj\" style=\"width:auto\">"
        + "<table class=\"table-datos-pj table table-bordered table-condensed\">"
        + "<tr>"
        + "<th>Resolución</th><th>Total terminados</th>"
        + "</tr>";

    $.each(jr[idx].resoluciones, function (index, item) {
        tabla += "<tr><td>" + item.resolucion + "</td><td>" + item.total + "</td></tr>"
    });

    tabla += "</table></div>";

    $("#Complemento").append(tabla);
}

function gTablaTramites(jr, id, idx) {

    $("#Complemento").empty();
    
    var tabla = "<div class=\"container contenedor-tabla-pj order-column\" style=\"width:auto\">"
        + "<table id=\"table-datos-pj\" class=\"table-datos-pj table table-bordered table-condensed\">"
        + "<tr>"
        + "<th>Expediente</th><th>Fecha radicación</th><th>Fecha consulta</th><th>Juicio</th></th><th>Tiempo Transcurrido (Días)</th><th>Juzgado</th>"
        + "</tr>";


    var t = $('#tabla-pjem').DataTable();
    $.each(jr[idx].resoluciones, function (index, item) {
        tabla += "<tr><td>" + item.expediente + "</td><td>" + item.fecharad + "</td><td>" + item.fechacon + "</td><td>" + item.juicio + "</td><td>" + item.dias + "</td><td>" + item.juzgado + "</td></tr>";

        //t.row.add([
        //    item.expediente,
        //    item.fecharad,
        //    item.fechacon,
        //    item.juicio,
        //    item.dias,
        //    item.juzgado])
        //    .draw()
        //    .node();
    });

    tabla += "</table></div>";

    $("#Complemento").append(tabla);

    //$("#tabla-div").show();
}

function gBarra(idBarra, jr, id) {

    var datos = [];
    for (var x = 0; x < jr.length; x++) {
        datos.push({
            y: jr[x].fecha,
           // x: parseFloat(jr[x].valor)
            x: id != "1" ? promIndice(jr, x, id) : parseFloat(jr[x].valor)
        });
    }

    Morris.Line({
        element: idBarra,
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
        postUnits: jsonMsj[String(id)].sim,
        barOpacity: .8,
        hideHover: 'auto',
        ymax: 'auto',
        resize: true
    });

}

function gDona(jr, id, idx) {

    var datos = [];
    var pc = "";

    if (!(/\b(9|14|15|17)\b/).test(String(id))) {

        var prom = promIndice(jr, idx, id);

        datos.push({
            label: "Estatus " + String(jr[idx].fecha),
            value: prom
        });

        datos.push({
            label: "Total " + String(jr[idx].fecha),
            value: (100 - prom).toFixed(2)
        });

    } else if ((/\b(9|14|15|17)\b/).test(String(id))) {

        var prom = promIndice(jr, idx, id);

        datos.push({
            label: "Estatus " + String(jr[idx].fecha),
            value: prom
        });
    }
    var colors_array = ["#65baaf", "#9d2424"];

    var donut = Morris.Donut({
        element: 'morris-donut-chart',
        data: datos,
        formatter: function (y, data) {
            return y + jsonMsj[String(id)].sim;
        },
        colors: colors_array,
        resize: true
    });

    $("#Desc1").text(jr[idx].valor);
    $("#Tex1").text(jsonMsj[id].desc1 + " de " + mesAnio(jr[idx].fecha));
    $("#Desc2").text(jr[idx].total);
    $("#Tex2").text(jsonMsj[id].desc2 + " de " + mesAnio(jr[idx].fecha));
    $("#Desc3").text(prom + jsonMsj[id].sim);
    $("#Tex3").text(jsonMsj[id].desc3 + " de " + mesAnio(jr[idx].fecha));

    //mensajesAct(jr, id, idx);

    donut.select(0); 
}

function mensajesAct(jr, id, idx) {
    $("#Desc1").text(jr[idx].valor);
    $("#Tex1").text(jsonMsj[id].desc1 + " de " + mesAnio(jr[idx].fecha));
}

function mascara(idMask, opac, callback) {
    //var m = document.createElement('div');
    //m.setAttribute("id", idMask);
    //m.style.position = "fixed";
    //m.style.width = "100%";
    //m.style.height = "100%";
    //m.style.background = "rgba(0,0,0," + opac + ")";
    //m.style.zIndex = "1500";
    //m.style.left = "0px";
    //m.style.top = "0px";
    //m.style.display = "flex";
    //m.style.alignItems = "center";
    //m.style.webkitAlignItems = "center";
    //m.style.justifyContent = "center";
    //m.style.webkitJustifyContent = "center";
    //m.innerHTML = "<img src='../../img/carga.gif' class='carga-img' style='height:50%'/>"
    //document.body.appendChild(m);

    //$("#maskRef").css("display", "flex");
    document.getElementById("maskRef").style.display = "flex";

    callback();
}

function mascara2(idMask, opac, callback) {
    document.getElementById("maskRef").style.display = "flex";
}

function desvaneceMascara(id) {
    //$("#" + id).fadeOut(200, function () {
    //    $("#" + id).remove();
    //});

   // $("#maskRef").css("display", "none");
    document.getElementById("maskRef").style.display = "none";
}

String.prototype.capital = function () {
    return this.toLowerCase().replace(/(^|\s)([a-z])/g, function (m, p1, p2) {
        return p1 + p2.toUpperCase();
    });
};

String.prototype.getMes = function () {
    return this.length == 1 ? "0" + this : this;    
};

var jActual = null;
var secActual = null;
var pestActual = null;

function eventosClick() {

    $("#Indicadores-materia").on("click", "ul > li", function () {
        if ($(this).children("a").attr("data-a") == "y") {
            var b = this;
            pestActual = "indi";
            $("#datos > *").show();
            $("#graficas > *").show();

            if ((/id(15)/).test($(b).children("a").attr("data-i"))) {
                mascara("maskRef", "0.6", function () {
                    obtenYmuestraDatosTabla(obtenJson(b));
                });

            }else if((/id(17)/).test($(b).children("a").attr("data-i"))) {
                mascara("maskRef", "0.6", function () {
                    //obtenYmuestraDatosTerminos(obtenJson(b));
                    obtenYmuestraDatos(obtenJson(b));
                });              
            
            } else if ((/id(9)/).test($(b).children("a").attr("data-i"))) {
                mascara("maskRef", "0.6", function () {
                    obtenYmuestraDatosTramite(obtenJson(b));
                });

            }
            else {
                mascara("maskRef", "0.6", function () {
                    obtenYmuestraDatos(obtenJson(b));
                });
            }

        } else {
            if ($(this).find(".arrow-ind").hasClass("fa-angle-left")) {
                muestraSubMenu(this);
            } else {
                $(this).find(".sub-m-ind").slideUp(function () {
                    $(".sub-m-ind").remove();
                });
                $(this).find(".arrow-ind").removeClass("fa-angle-down activo");
                $(this).find(".arrow-ind").addClass("fa-angle-left");
            }

        }   
    });

    $("#logo-pj").click(function () {
        window.location = "https://infoestadisticapj.pjedomex.gob.mx/IndicadoresGestion/";
    });

    $("#gen-reporte").on("click", function () {
        pestActual = "gen-reporte";
        mascara("maskRef", "0.6", function () {
            tablaAllDatos();
        })
    });

    $("#inf-general").on("click", function () {
        pestActual = "inf-general";
        mascara("maskRef", "0.6", function () {
            obtenYmuestraAllDatos();
        })
    });

    $("#button-pdf-exporta").on("click", function () {
        try {

            $("#button-pdf-exporta").hide(function () {

                var j = $("#Juzgados option:selected").text();

               html2canvas(document.querySelector("#Complemento")).then(canvas => {
                    var imgData = canvas.toDataURL('image/png');
                   var doc = new jsPDF('p', 'mm', 'letter');
                    doc.setFontSize(20);
                   doc.text(15, 25, "INDICADORES GENERALES");
                   doc.addImage(imgData, 'PNG', 5, 35, 205, 110); // 1: canvas, 2: formato, 3: posicion x, 4: posicion: y, 5: ancho, 6: largo
                   doc.save("Indicadores.pdf");
                    $("#button-pdf-exporta").show();
                });

            });           

        } catch (ex) {
            console.log(ex);
        }

    });

    $("#div-flotante-tabla").dialog({
        autoOpen: false,
        width: ancho - 100,
        height: alto - 80,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        },
        modal: true,
    });
    
    $("#cierra-sesion").on("click", function () {

        ejecuta("ExitUser", "{}", function () {
            window.location.href = "http://gestionjudicial.pjedomex.gob.mx/ideas/civil/proceso/tramite-judicial";
        });

    });

}

function muestraSubMenu(ele) {


    var actual = $(".sub-m-ind");
    $(".activo").removeClass("fa-angle-down ");
    $(".activo").addClass("fa-angle-left");
    $(".activo").removeClass("activo");
    $(actual).slideUp(function () {
        $(actual).remove();
    });
    

    jsonActual = obtenJson(ele);
    var subMenuActivo = "";
    var i = 1;
    var c = 'n';

    $.each(jsonActual.hijos, function (val, item) {

        if (item.consulta != "") {
            c = 'y';
        }

        subMenuActivo += '<li><a data-i="' + val + '" data-p="' + item.padre + '" data-a="' + c + '">' + i + '.- ' + item.titulo + '</a></li>\n';
        i++;
    })

    var ul = document.createElement("ul");
    ul.innerHTML = subMenuActivo;
    ul.style.display = "none";
    ul.className = "nav nav-third-level sub-m-ind";

    $(ele).find(".arrow-ind").removeClass("fa-angle-left");
    $(ele).find(".arrow-ind").addClass("fa-angle-down activo");
    $(ele).append(ul);
    $(ul).slideDown();
}

function eventosMouseover() {
    $("#morris-bar-chart").on("mouseover", "circle", function () {

        if (pestActual == "indi") {
            $("#morris-donut-chart").empty();
            if (jActual.id != "1") {
                gDona(jData, jActual.id, $(this).attr("data-point"));
                if (jActual.id == "15") {
                    gTabla(jData, jActual.id, $(this).attr("data-point"));
                } else if (jActual.id == "17") {
                    gTablaResoluciones(jData, jActual.id, $(this).attr("data-point"));
                } else if (jActual.id == "9") {
                    gTablaTramites(jData, jActual.id, $(this).attr("data-point"));
                }
            } else {
                mensajesAct(jData, jActual.id, $(this).attr("data-point"));
            }
        }
        else if (pestActual == "gestion") {
            $("#morris-donut-chart").empty();
            Dona(jActual, idActual, $(this).attr("data-point"));
        }
    });
}

function eventosMouseleave() {

 /*   $("body").on("mouseleave", ".sub-m-ind", function () {
        $(".sub-m-ind").remove();
    })
*/

}

function eventosChange() {

    $(".selects-change > select").on("change", function () {

        cambioMesAnio(function () {
            if (pestActual == "indi") {
                if (secActual != null) {
                    if (secActual == "indicadores") {
                        mascara("maskRef", "0.6", function () {
                            obtenYmuestraDatos(jActual);
                        });
                    } else if (secActual == "indicadoresTabla") {
                        mascara("maskRef", "0.6", function () {
                            obtenYmuestraDatosTabla(jActual);
                        });
                    } else if (secActual == "indicadoresTermino") {
                        mascara("maskRef", "0.6", function () {
                            obtenYmuestraDatosTerminos(jActual);
                        });
                    } else if (secActual == "indicadoresTramite") {
                        mascara("maskRef", "0.6", function () {
                            obtenYmuestraDatosTramite(jActual);
                        });
                    }
                }
            } else if (pestActual == "gestion") {
                mascara2("maskRef", "0.6");
                consultarReporte(idActual);
                desvaneceMascara("maskRef");
            }
            else if (pestActual == "inf-general") {
                mascara("maskRef", "0.6", callback, function () {
                    obtenYmuestraAllDatos();
                });
            } else if (pestActual == "gen-reporte") {

            }
        });
    });

    $(".selects-change > #Juzgados, .selects-change > #Materia").on("change", function () {
        cargaJuzgados();
    });

}

function eventosResize() {

    var resizeListener;
    $(window).resize(function () {

        //every time the window resize is called cancel the setTimeout() function
        clearTimeout(resizeListener);

        //set out function to run after a specified amount of time 
        resizeListener = setTimeout(function () {

            if (String($("#morris-bar-chart").html()).trim().length != 0) {
                $("#morris-bar-chart circle").each(function (i, e) {
                    $(this).attr("data-point", (3 - i));
                    $(this).attr("style", "cursor: pointer");
                })
            }

        }, 250);

    });

}

function rangoFechasConsulta(mes, anio) {

    var aniosCons = [];

    for (var i = 1; i < 4; i++) {
        aniosCons.push((new Date(anio, (mes - i), 0)).toISOString().substring(0, 7));
    }

    return aniosCons;
}

function evaluaDatos(jsonR,anio, mes) {

    if (jsonR != null && jsonR.length) {
    
        var jsonS = [];
        var rangoFechas = rangoFechasConsulta(mes, anio);

        $.each(rangoFechas, function (indexA, itemA) {

            var j = null;

            $.each(jsonR, function (indexB, itemB) {

                if (itemA == (itemB.Anio + "-" + String(itemB.Mes).getMes())) {
                    j = { fecha: itemB.Anio + "-" + String(itemB.Mes).getMes(), valor: itemB.Valor, total: itemB.Total };

                    return false;
                }

            });

            if (j != null) {
                jsonS.push(j);
            } else {
                jsonS.push({ fecha: itemA, valor: 0, total: 0 });
            }

        });

        return jsonS;

    } else if (jsonR != null && !jsonR.length) {
        var jsonS = [];

        for (var i = 0; i < 4; i++) {
            var fec = (new Date(anio, (mes - i), 0)).toISOString().substring(0, 7);
            jsonS.push({ fecha: fec, valor: 0, total: 0 });
        }

        return jsonS;

    } else {
        return null;
    }
}

function evaluaDatosTabla(jsonR, anio, mes) {

    if (jsonR != null && jsonR.length) {

        var jsonS = [];
        var rangoFechas = rangoFechasConsulta(mes, anio);

        $.each(rangoFechas, function (indexA, itemA) {

            var j = null;

            $.each(jsonR, function (indexB, itemB) {

                if (itemA == (itemB.anio + "-" + String(itemB.mes).getMes())) {
                    j = { fecha: itemB.anio + "-" + String(itemB.mes).getMes(), valor: itemB.valor, total: itemB.total, jucios: itemB.juicios};

                    return false;
                }

            });

            if (j != null) {
                jsonS.push(j);
            } else {
                jsonS.push({ fecha: itemA, valor: 0, total: 0, juicios: {}});
            }

        });

        return jsonS;

    } else if (jsonR != null && !jsonR.length) {
        var jsonS = [];

        for (var i = 0; i < 4; i++) {
            var fec = (new Date(anio, (mes - i), 0)).toISOString().substring(0, 7);
            jsonS.push({ fecha: fec, valor: 0, total: 0, juicios: {}});
        }

        return jsonS;

    } else {
        return null;
    }
}

function evaluaDatosTermino(jsonR, anio, mes) {

    if (jsonR != null && jsonR.length) {

        var jsonS = [];
        var rangoFechas = rangoFechasConsulta(mes, anio);

        $.each(rangoFechas, function (indexA, itemA) {

            var j = null;

            $.each(jsonR, function (indexB, itemB) {

                if (itemA == (itemB.anio + "-" + String(itemB.mes).getMes())) {
                    j = { fecha: itemB.anio + "-" + String(itemB.mes).getMes(), valor: itemB.valor, total: itemB.total, resoluciones: itemB.resoluciones };

                    return false;
                }

            });

            if (j != null) {
                jsonS.push(j);
            } else {
                jsonS.push({ fecha: itemA, valor: 0, total: 0, resoluciones: {} });
            }

        });

        return jsonS;

    } else if (jsonR != null && !jsonR.length) {
        var jsonS = [];

        for (var i = 0; i < 4; i++) {
            var fec = (new Date(anio, (mes - i), 0)).toISOString().substring(0, 7);
            jsonS.push({ fecha: fec, valor: 0, total: 0, resoluciones: {} });
        }

        return jsonS;

    } else {
        return null;
    }
}

function evaluaDatosTramite(jsonR, anio, mes) {

    if (jsonR != null && jsonR.length) {

        var jsonS = [];
        var rangoFechas = rangoFechasConsulta(mes, anio);

        $.each(rangoFechas, function (indexA, itemA) {

            var j = null;

            $.each(jsonR, function (indexB, itemB) {

                if (itemA == (itemB.anio + "-" + String(itemB.mes).getMes())) {
                    j = { fecha: itemB.anio + "-" + String(itemB.mes).getMes(), valor: itemB.valor, total: itemB.total, resoluciones: itemB.tramites };

                    return false;
                }

            });

            if (j != null) {
                jsonS.push(j);
            } else {
                jsonS.push({ fecha: itemA, valor: 0, total: 0, tramites: {} });
            }

        });

        return jsonS;

    } else if (jsonR != null && !jsonR.length) {
        var jsonS = [];

        for (var i = 0; i < 4; i++) {
            var fec = (new Date(anio, (mes - i), 0)).toISOString().substring(0, 7);
            jsonS.push({ fecha: fec, valor: 0, total: 0, tramites: {} });
        }

        return jsonS;

    } else {
        return null;
    }
}

function cargaJuzgados() {
  
    var cveDistrito = $("#Juzgados").val();
    var materia = $("#Materia").val();

    var dataJ = "{cveDistrito:'" + cveDistrito + "', materia:'" + materia + "'}";

    var result = [];

    consulta("ObtenJuzgados", dataJ, function (data) {
        result = data;
    });

    if (result != null && result.length) {    
        $("#Juzgado").empty();
        $('#Juzgado').append('<option value="" selected="selected">Total</option>');

        $.each(result, function (index, item) {
            $('#Juzgado').append('<option value="' + item.CveJuzgado + '">' + item.NomJuzgado + '</option>');
        });
    } else {
        $("#Juzgado").empty();
        $('#Juzgado').append('<option value="" selected="selected">Total</option>');
    }

}