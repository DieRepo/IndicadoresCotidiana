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
        desc2: "Numero de terminados en el mes",
        desc3: "Tiempo promedio en días para dictar una sentencia difinitiva en el mes",
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
        desc1: "Tiempo total en días de expedientes en tramite en el mes",
        desc2: "Total de expdientes en trámite en el mes",
        desc3: "Tiempo promedio en días de expedientes en tramite en el mes",
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
        desc1: "Tiempo en dicatar en días una terminación en horas durante el mes ",
        desc2: "Numero de terminados en el mes",
        desc3: "Tiempo promedio en días para dictar una terminación en el mes",
        sim: " días"
    },
    16: {
        titulo: "",
        desc1: "Numero de correcciones el mes ",
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
    }
    //,
    //18: {
    //    titulo: "",
    //    desc1: "Número de audiencias videograbadas en el mes ",
    //    desc2: "Total de audiencias en el mes",
    //    desc3: "Porcentaje de audiencias videograbadas respecto al total audiencias en el mes",
    //    sim: "%"
    //}

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
                titulo: "Sentencias definitiva",
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
            }
            //},
            //id18: {
            //    id: "18",
            //    titulo: "Audiencias videograbadas",
            //    descripcion: "Audiencias videograbadas por juicios",
            //    padre: "6",
            //    submenu: "n",
            //    hijos: {
            //    }
            //}
        }
    },
    id9: {
        id: "9",
        titulo: "Tiempo promedio de expedientes en trámite",
        descripcion: "Tiempo promedio expedientes en tramite por juzgados",
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

}
