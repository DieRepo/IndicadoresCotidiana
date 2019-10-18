var jsonMsj = {
    1: {
        titulo: "",
        desc1: "Iniciados del mes",
        desc2: "",
        desc3: "",
        sim: ""
    },
    3: {
        titulo: "",
        desc1: "Total de sentencias definitivas del mes",
        desc2: "Total de concluidos en el mes",
        desc3: "Porcentaje de sentencias definitivas respecto al total de concluidos del mes",
        sim: "%"
    },
    4: {
        titulo: "",
        desc1: "Total de sentencias interlocutorias del mes",
        desc2: "Total de concluidos en el mes",
        desc3: "Porcentaje de sentencias interlocutorias respecto al total de concluidos del mes",
        sim: "%"
    },
    14: {
        titulo: "",
        desc1: "Tiempo en días en dictar una terminación en horas durante el mes ",
        desc2: "Número de terminados en el mes",
        desc3: "Tiempo promedio en días para dictar una sentencia definitiva en el mes",
        sim: " días"
    },
    7: {
        titulo: "",
        desc1: "Total de audiencias celebradas en el mes",
        desc2: "Total de audiencias en el mes",
        desc3: "Porcentaje de audiencias celebradas respecto al total audiencias en el mes",
        sim: "%"
    },
    8: {
        titulo: "",
        desc1: "Total de audiencias no celebradas en el mes",
        desc2: "Total de audiencias en el mes",
        desc3: "Porcentaje de audiencias no celebradas respecto al total audiencias en el mes",
        sim: "%"
    },
    9: {
        titulo: "",
        desc1: "Tiempo total en días de expedientes en trámite en el mes",
        desc2: "Total de expedientes en trámite en el mes",
        desc3: "Tiempo promedio en días de expedientes en trámite en el mes",
        sim: " días"
    },
    11: {
        titulo: "",
        desc1: "Total de exhortos diligenciados en el mes",
        desc2: "Total de exhortos en el mes",
        desc3: "Porcentaje de exhortos diligenciados respecto al total exhortos en el mes",
        sim: "%"
    },
    12: {
        titulo: "",
        desc1: "Total de exhortos no diligenciados en el mes",
        desc2: "Total de exhortos en el mes",
        desc3: "Porcentaje de exhortos no diligenciados respecto al total exhortos en el mes",
        sim: "%"
    },
    13: {
        titulo: "",
        desc1: "Total de exhortos parcialmente diligenciados en el mes",
        desc2: "Total de exhortos en el mes",
        desc3: "Porcentaje de exhortos parcialmente diligenciados respecto al total exhortos en el mes",
        sim: "%"
    },
    15: {
        titulo: "",
        desc1: "Tiempo en dictar en días una terminación durante el mes ",
        desc2: "Número de terminados en el mes",
        desc3: "Tiempo promedio en días para dictar una terminación en el mes",
        sim: " días"
    },
    16: {
        titulo: "",
        desc1: "Número de correcciones el mes ",
        desc2: "",
        desc3: "",
        sim: ""
    },
    17: {
        titulo: "",
        desc1: "Número de iniciados en el mes ",
        desc2: "Número de terminados en el mes ",
        desc3: "Tasa de resolución en el mes ",
        sim: "%"
    },
    18: {
        titulo: "",
        desc1: "Número de audiencias videograbadas en el mes ",
        desc2: "Total de audiencias en el mes",
        desc3: "Porcentaje de audiencias videograbadas respecto al total audiencias en el mes",
        sim: "%"
    }

}

var jsonMenu = {
    id1: {
        id: "1",
        titulo: "Iniciados",
        descripcion: "Iniciados por juzgados",
        padre: "",
        submenu: "n",
        hijos: {
        }
    },
    id2: {
        id: "2",
        titulo: "Sentencias",
        descripcion: "Sentencias",
        padre: "",
        submenu: "y",
        hijos: {
            id3: {
                id: "3",
                titulo: "Sentencias definitivas",
                descripcion: "Sentencias definitivas por juzgados",
                padre: "2",
                submenu: "n",
                hijos: {
                }
            },
            id4: {
                id: "4",
                titulo: "Sentencias interlocutorias",
                descripcion: "Sentencias interlocutorias por juzgados",
                padre: "2",
                submenu: "n",
                hijos: {
                }
            }
        }
    },
    id5: {
        id: "5",
        titulo: "Tiempo promedio para dictar una terminación",
        descripcion: "Tiempo promedio para dictar una terminación",
        padre: "",
        submenu: "y",
        hijos: {
            id14: {
                id: "14",
                titulo: "Tiempo para dictar una sentencia definitiva por tipo de juzgado",
                descripcion: "Tiempo para dictar una sentencia definitiva por juzgados",
                padre: "5",
                submenu: "n",
                hijos: {
                }
            },
            id15: {
                id: "15",
                titulo: "Tiempo para dictar una sentencia definitiva por tipo de juicio",
                descripcion: "Tiempo para dictar una sentencia definitiva por juicios",
                padre: "5",
                submenu: "n",
                hijos: {
                }
            }
        }
    },
    id6: {
        id: "6",
        titulo: "Audiencias",
        descripcion: "Audiencias",
        padre: "",
        submenu: "y",
        hijos: {
            id7: {
                id: "7",
                titulo: "Audiencias celebradas",
                descripcion: "Audiencias celebradas por juicios",
                padre: "6",
                submenu: "n",
                hijos: {
                }
            },
            id8: {
                id: "8",
                titulo: "Audiencias no celebradas",
                descripcion: "Audiencias no celebradas por juicios",
                padre: "6",
                submenu: "n",
                hijos: {
                }
            },
            id18: {
                id: "18",
                titulo: "Audiencias videograbadas",
                descripcion: "Audiencias videograbadas por juicios",
                padre: "6",
                submenu: "n",
                hijos: {
                }
            }
        }
    },
    id9: {
        id: "9",
        titulo: "Tiempo promedio de expedientes en trámite",
        descripcion: "Tiempo promedio expedientes en trámite por juzgados",
        padre: "",
        submenu: "n",
        hijos: {
        }
    },
    id10: {
        id: "10",
        titulo: "Exhortos",
        descripcion: "Exhortos",
        padre: "",
        submenu: "y",
        hijos: {
            id11: {
                id: "11",
                titulo: "Exhortos diligenciados",
                descripcion: "Exhortos diligenciados por juzgados",
                padre: "10",
                submenu: "n",
                hijos: {
                }
            },
            id12: {
                id: "12",
                titulo: "Exhortos no diligenciados",
                descripcion: "Exhortos no diligenciados por juzgados",
                padre: "10",
                submenu: "n",
                hijos: {
                }
            },
            id13: {
                id: "13",
                titulo: "Exhortos parcialmente diligenciados",
                descripcion: "Exhortos parcialmente diligenciados por juzgados",
                padre: "10",
                submenu: "n",
                hijos: {
                }
            }
        }
    }
    /*,
    id16: {
        id: "16",
        titulo: "Número de correcciones",
        descripcion: "Tiempo promedio expedientes en tramite por juzgados",
        padre: "",
        submenu: "n",
        hijos: {
        }
    }*/
    ,
    id17: {
        id: "17",
        titulo: "Tasa de resolución",
        descripcion: "Tasa de resolución por juzgados",
        padre: "",
        submenu: "n",
        hijos: {
        }
    }

};

var jsonGestion = {

    1: {
        id: "1",
        titulo: "Quejas del servicio de atención al público",
        descripcion: "Proporción de quejas relativas al proceso de atención al público en relación con el número de demandas y promociones (ingresos) recibidas durante el mes. ",
        descdona: "TIEMPO",
        desc1: "Número de quejas",
        desc2: "Total de ingresos",
        desc3: "Tiempo promedio en horas",
        sim: "",
        padre: "",
        submenu: "n",
        proceso: "Atención al Público",
        unidad: "%",
        meta: "5% o menos",
        periodicidad: "Mensual",
        bueno: "0% – 3.9%",
        regular: "4% – 4.9%",
        malo: "5% – 100%",
        valor1: "Número de quejas",
        valor2: "Total de ingresos",
        porcien: "x 100",
        fuente: "Operam y Sistema Expediente Electrónico",
        activo: true

    },
    2: {
        id: "2",
        titulo: "Tiempo para el acuerdo de promociones",
        descripcion: "Promedio de horas requeridas para emitir un acuerdo derivado de la recepción de una promoción. ",
        descdona: "TIEMPO",
        desc1: "Tiempo en horas que pasa una promoción en ser acuerdo",
        desc2: "Número de acuerdos",
        desc3: "Tiempo promedio en horas",
        sim: " horas",
        padre: "",
        submenu: "n",
        proceso: "Trámite Judicial",
        unidad: "horas",
        meta: "24 horas laborales o menos",
        periodicidad: "Mensual",
        bueno: "0h – 24h",
        regular: "/",
        malo: "24hr o más",
        valor1: "∑(Fecha de emisión del acuerdo - Fecha de recepción de la promoción)",
        valor2: "Total de acuerdos en el registro",
        porcien: "",
        fuente: "Sistema Expediente Electrónico",
        activo: true
    },

    3: {
        id: "3",
        titulo: "Tiempo para la práctica de notificaciones",
        descripcion: "Promedio de horas requeridas para notificar un acuerdo. ",
        descdona: "TIEMPO",
        desc1: "Tiempo en horas para notificar un acuerdo",
        desc2: "Total de notificaciones",
        desc3: "Promedio de horas requeridas para notificar un acuerdo",
        sim: " horas",
        padre: "",
        submenu: "n",
        proceso: "Trámite Judicial",
        unidad: "horas",
        meta: "24 horas laborales o menos",
        periodicidad: "Mensual",
        bueno: "0h – 20h",
        regular: "20.1h – 23.9h",
        malo: "24hr o más",
        valor1: "∑(Fecha de emisión del acuerdo - Fecha de la notificación)",
        valor2: "Total de notificaciones",
        porcien: "",
        fuente: "Sistema Electrónico",
        activo: true
    },

    4: {
        id: "4",
        titulo: "Porcentaje de audiencias celebradas",
        descripcion: "Promedio de audiencias efectivamente celebradas en relación con las audiencias señaladas buscando contrarestar el diferimiento innecesario. ",
        descdona: "PORCENTAJE",
        desc1: "Total de audiencias celebradas",
        desc2: "Total de audiencias programadas",
        desc3: "Promedio de audiencias celebradas",
        sim: "",
        padre: "",
        submenu: "n",
        proceso: "Proceso Audiencia de Oralidad",
        unidad: "%",
        meta: "95% o más",
        periodicidad: "Mensual",
        bueno: "95% o más",
        regular: "-",
        malo: "Menor a 95%",
        valor1: "Total de audiencias celebradas",
        valor2: "Total de audiencias programadas",
        porcien: "x 100",
        fuente: "Sistema Expediente Electrónico",
        activo: true
    },

    5: {
        id: "5",
        titulo: "Audiencias desahogadas dentro del plazo",
        descripcion: "Proporción de audiencias desahogadas dentro del plazo legal establecido en el CPC para cada tipo de audiencia. ",
        descdona: "PORCENTAJE",
        desc1: "Total de audiencias agendadas dentro del plazo",
        desc2: "Total de audiencias programadas",
        desc3: "Promedio de audiencias desahogadas dentro del plazo",
        sim: "",
        padre: "",
        submenu: "n",
        proceso: "Proceso de Trámite Judicial",
        unidad: "m",
        meta: "95% o más",
        periodicidad: "Mensual",
        bueno: "100% - 97.1%",
        regular: "97% – 90.1%",
        malo: "90% o menos",
        valor1: "Total de audiencias agendadas dentro del plazo",
        valor2: "Total de audiencias programadas",
        porcien: "x 100",
        fuente: "Sistema Expediente Electrónico",
        activo: false
    },

    6: {
        id: "6",
        titulo: "Tiempo de atención en la recepción de documentos",
        descripcion: "Promedio de minutos invertidos por el usuario para presentar documentos en el juzgado y obtener su acuse de recepción.",
        descdona: "Tiempo",
        desc1: "Total de audiencias agendadas dentro del plazo",
        desc2: "Total de audiencias programadas",
        desc3: "Promedio de audiencias desahogadas dentro del plazo",
        sim: "",
        padre: "",
        submenu: "n",
        proceso: "Proceso de Atención al Público",
        unidad: "m",
        meta: "20 minutos o menos",
        periodicidad: "Mensual",
        bueno: "0m – 17.5m",
        regular: "17.5m – 19.9m",
        malo: "20m o más",
        valor1: "∑(Hora de recepción de los documentos - Hora de entrega del acuse)",
        valor2: "",
        porcien: "",
        fuente: "",
        activo: false
    },

    7: {
        id: "7",
        titulo: "Tiempo de atención en el préstamo de expedientes",
        descripcion: "Promedio de minutos invertidos por el usuario para presentar documentos en el juzgado y obtener su acuse de recepción.",
        descdona: "Tiempo",
        desc1: "Total de audiencias agendadas dentro del plazo",
        desc2: "Total de audiencias programadas",
        desc3: "Promedio de audiencias desahogadas dentro del plazo",
        sim: "",
        padre: "",
        submenu: "n",
        proceso: "Proceso de Atención al Público",
        unidad: "m",
        meta: "20 minutos o menos",
        periodicidad: "Mensual",
        bueno: "0m – 17.5m",
        regular: "17.5m – 19.9m",
        malo: "20m o más",
        valor1: "∑(Hora de recepción de los documentos - Hora de entrega del acuse)",
        valor2: "",
        porcien: "",
        fuente: "",
        activo: false
    },

    8: {
        id: "8",
        titulo: "Satisfacción del usuario externo sobre el trámite",
        descripcion: "Promedio de minutos invertidos por el usuario para presentar documentos en el juzgado y obtener su acuse de recepción.",
        descdona: "Tiempo",
        desc1: "Total de audiencias agendadas dentro del plazo",
        desc2: "Total de audiencias programadas",
        desc3: "Promedio de audiencias desahogadas dentro del plazo",
        sim: "",
        padre: "",
        submenu: "n",
        proceso: "Proceso de Atención al Público",
        unidad: "m",
        meta: "20 minutos o menos",
        periodicidad: "Mensual",
        bueno: "0m – 17.5m",
        regular: "17.5m – 19.9m",
        malo: "20m o más",
        valor1: "∑(Hora de recepción de los documentos - Hora de entrega del acuse)",
        valor2: "",
        porcien: "",
        fuente: "",
        activo: false
    },

    9: {
        id: "9",
        titulo: "Audiencias iniciadas en tiempo",
        descripcion: "Promedio de minutos invertidos por el usuario para presentar documentos en el juzgado y obtener su acuse de recepción.",
        descdona: "Tiempo",
        desc1: "Total de audiencias agendadas dentro del plazo",
        desc2: "Total de audiencias programadas",
        desc3: "Promedio de audiencias desahogadas dentro del plazo",
        sim: "",
        padre: "",
        submenu: "n",
        proceso: "Proceso de Atención al Público",
        unidad: "m",
        meta: "20 minutos o menos",
        periodicidad: "Mensual",
        bueno: "0m – 17.5m",
        regular: "17.5m – 19.9m",
        malo: "20m o más",
        valor1: "∑(Hora de recepción de los documentos - Hora de entrega del acuse)",
        valor2: "",
        porcien: "",
        fuente: "",
        activo: false
    },
   
};
