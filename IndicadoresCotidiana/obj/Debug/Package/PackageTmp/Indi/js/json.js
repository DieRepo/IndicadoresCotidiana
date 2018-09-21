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
        desc2: "Total de sentencias del mes",
        desc3: "Porcentaje de sentencias definitivas respecto al total de sentencias del mes",
        sim: "%"
    },
    4: {
        titulo: "",
        desc1: "Total de sentencias interlocutorias del mes",
        desc2: "Total de sentencias del mes",
        desc3: "Porcentaje de sentencias interlocutorias respecto al total de sentencias del mes",
        sim: "%"
    },
    14: {
        titulo: "",
        desc1: "Tiempo en dicatar una terminación en horas durante el mes ",
        desc2: "Numero de terminados en el mes",
        desc3: "Tiempo promedio en horas para dictar una terminación en el mes",
        sim: " hr."
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
        desc1: "Total de audiencias no celebradas en el mes",
        desc2: "Total de audiencias en el mes",
        desc3: "Porcentaje de audiencias no celebradas respecto al total audiencias en el mes",
        sim: " hr"
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
        desc1: "Total de exhortos parcialmente diligenciados en el mes",
        desc2: "Total de exhortos en el mes",
        desc3: "Porcentaje de exhortos parcialmente diligenciados respecto al total exhortos en el mes",
        sim: "hr"
    }

}

var jsonMenu = {
    id1: {
        id: "1",
        descripcion: "Iniciados",
        padre: "",
        submenu: "n",
        hijos: {
        }
    },
    id2: {
        id: "2",
        descripcion: "Sentencias",
        padre: "",
        submenu: "y",
        hijos: {
            id3: {
                id: "3",
                descripcion: "Sentencias definitiva",
                padre: "2",
                submenu: "n",
                hijos: {
                }
            },
            id4: {
                id: "4",
                descripcion: "Sentencias interlocutorias",
                padre: "2",
                submenu: "n",
                hijos: {
                }
            }
        }
    },
    id5: {
        id: "5",
        descripcion: "Tiempo promedio para dictar una terminación",
        padre: "",
        submenu: "y",
        hijos: {
            id14: {
                id: "14",
                descripcion: "Tiempo promedio para dictar una terminación por juzgado",
                padre: "5",
                submenu: "n",
                hijos: {
                }
            },
            id15: {
                id: "15",
                descripcion: "Tiempo promedio para dictar una terminación por juicio",
                padre: "5",
                submenu: "n",
                hijos: {
                }
            }
        }
    },
    id6: {
        id: "6",
        descripcion: "Audiencias",
        padre: "",
        submenu: "y",
        hijos: {
            id7: {
                id: "7",
                descripcion: "Audiencias celebradas",
                padre: "6",
                submenu: "n",
                hijos: {
                }
            },
            id8: {
                id: "8",
                descripcion: "Audiencias no celebradas",
                padre: "6",
                submenu: "n",
                hijos: {
                }
            }
        }
    },
    id9: {
        id: "9",
        descripcion: "Tiempo promedio de rezago",
        padre: "",
        submenu: "n",
        hijos: {
        }
    },
    id10: {
        id: "10",
        descripcion: "Exhortos",
        padre: "",
        submenu: "y",
        hijos: {
            id11: {
                id: "11",
                descripcion: "Exhortos diligenciados",
                padre: "10",
                submenu: "n",
                hijos: {
                }
            },
            id12: {
                id: "12",
                descripcion: "Exhortos no diligenciados",
                padre: "10",
                submenu: "n",
                hijos: {
                }
            },
            id13: {
                id: "13",
                descripcion: "Exhortos parcialmente diligenciados",
                padre: "10",
                submenu: "n",
                hijos: {
                }
            }
        }
    },

}
