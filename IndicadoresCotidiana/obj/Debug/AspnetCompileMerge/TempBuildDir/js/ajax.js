﻿/// <reference path="ajax.js" />

function servicioAjax(metodo, urlServicio, datos, FunctionRespuesta) {
    try {
        
        $.ajax(
            {
                url: urlServicio,
                data: datos,
                dataType: "json",
                type: metodo,
                contentType: "application/json; charset=utf-8",
                success: FunctionRespuesta,
                failure: function (response) {
                    console.log(response.d);
                },
                error: function (response) {
                    console.log(response);
                }
            });
    } catch (ex) {
        console.error(ex.toString())
    }
}


function consulta(metodo, datos, callback) {
    try { 

        var urlF = "/Servicio.aspx/" + metodo;       
        $.ajax(
            {
                url: urlF,
                async: false,
                data: datos,
                dataType: "json",
                type: 'post',
                contentType: "application/json; charset=utf-8",
                success: function (response) {                 
                    return callback(response.d);
                },
                failure: function (response, fail) {
                    console.error("Fail: " + fail);
                    return null;
                },
                error: function (response, error) {
                    console.error("Error: " + error);
                    return null;
                }
            });
        

    } catch (e) {
        console.error(e.toString());
    }

}
