Public Class ConsultasGestion


    Public consulta As Hashtable = New Hashtable()

    Public Sub New()

        consulta.Add(1, {"Quejas",
                         "SELECT year(s.fechaRegistro) anio, month(s.fechaRegistro) mes,
                            COUNT(ifnull(case when s.cveMotivo = 32 then s.idSolicitud end,0)) valor1,
                            ifnull(COUNT(s.idSolicitud),0) valor2,
                            round(ifnull(case when s.cveMotivo = 32 then COUNT(s.idSolicitud) end,0)/(ifnull(COUNT(s.idSolicitud),0)))*100 calculo,
                            j.idJuzgado, LAST_DAY(CONCAT(YEAR(s.fechaRegistro),'-',LPAD(MONTH(s.fechaRegistro),2,0),'-01')) corte
                            FROM htsj_operam.tblsolicitudes s
                            INNER JOIN htsj_gestion.tbljuzgados j ON s.cveAdscripcion = j.IdJuzgado
                            INNER JOIN htsj_gestion.tbldistritos d ON j.cveDistrito = d.cveDistrito
                            WHERE
                            s.fechaRegistro between @fechaIni AND @fechaFin
                            and j.cvedistrito = @cveDistrito
                            and j.idjuzgado like @cveJuzgado
                            and @materia
                            GROUP BY year(s.fechaRegistro), month(s.fechaRegistro)
                            order by anio, mes;"})

        consulta.Add(2, {"Tiempo para el acuerdo de promociones",
                         "select year(acuerdo.fechaRegistro) anio, month(acuerdo.fechaRegistro) mes,
                        sum(TIMESTAMPDIFF(hour,promocion.fechaRegistro,acuerdo.fecharegistro)) valor1,
                        count(distinct acuerdo.idactuacion) valor2
                        from htsj_electronico.tblantecedesactuaciones aa
                        inner join htsj_electronico.tblactuaciones promocion on aa.idactuacionPadre = promocion.idactuacion
                        inner join htsj_electronico.tblactuaciones acuerdo on aa.idactuacionHija = acuerdo.idactuacion
                        inner join htsj_electronico.tblcarpetasjudiciales cj on acuerdo.idcarpetajudicial = cj.idcarpetajudicial
                        inner join htsj_electronico.tbltiposactuacionesmaterias m on acuerdo.cvemateria = m.cvemateria
                        inner join htsj_electronico.tbltiposactuacionesmaterias mat on promocion.cvemateria = mat.cvemateria
                        inner join htsj_gestion.tbljuzgados j on j.idjuzgado = cj.cveadscripcion
                        where m.cveTipoActuacion in(2) and mat.cveTipoActuacion in(1)
                        and aa.activo = 'S' and acuerdo.activo = 'S' and promocion.activo = 'S' and cj.activo = 'S' and m.activo = 'S'
                        and cj.cvetiponumero in (1) 
                        and cj.nvainstancia = 1
                        and acuerdo.fechaRegistro between @fechaIni AND @fechaFin 
                        and j.cvedistrito = @cveDistrito
                        and j.idjuzgado like @cveJuzgado
                        and @materia
                        group by anio, mes
                        order by anio, mes;"})

        consulta.Add(3, {"Certidumbre de audiencias",
                        "select year(acuerdo.fechaRegistro) anio, month(acuerdo.fechaRegistro) mes,
                        sum(TIMESTAMPDIFF(hour,acuerdo.fechaRegistro,notificacion.fechaRegistro)) valor1,
                        count(notificacion.idactuacion) valor2 
                        from htsj_electronico.tblantecedesactuaciones aa
                        inner join htsj_electronico.tblactuaciones acuerdo on aa.idactuacionPadre = acuerdo.idactuacion
                        inner join htsj_electronico.tblactuaciones notificacion on aa.idactuacionHija = notificacion.idactuacion
                        inner join htsj_electronico.tblcarpetasjudiciales cj on acuerdo.idcarpetajudicial = cj.idcarpetajudicial
                        inner join htsj_electronico.tbltiposactuacionesmaterias m on acuerdo.cvemateria = m.cvemateria
                        inner join htsj_electronico.tbltiposactuacionesmaterias mat on notificacion.cvemateria = mat.cvemateria
                        inner join htsj_gestion.tbljuzgados j on j.idjuzgado = cj.cveadscripcion
                        where 
                        m.cveTipoActuacion in(2) 
                        and mat.cveTipoActuacion in(20,21)
                        and aa.activo = 'S' and acuerdo.activo = 'S' and notificacion.activo = 'S' and cj.activo = 'S' and m.activo = 'S' and mat.activo = 'S' and j.activo = 'S'
                        and cj.cvetiponumero in (1) 
                        and acuerdo.fechaRegistro between @fechaIni AND @fechaFin 
                        and j.cvedistrito = @cveDistrito
                        and j.idjuzgado like @cveJuzgado
                        and @materia
                        group by anio, mes
                        order by anio, mes;"})

        consulta.Add(4, {"Certidumbre de audiencias",
                        "select year(a.fechaInicial) anio, month(a.fechaInicial) mes,
                        count(distinct case when e.cveestatus in(8,14) then a.idaudiencia end ) valor1, 
                        count(distinct case when e.cveestatus in(8,9,10,48,13,14,15) then a.idaudiencia end ) valor2 
                        from htsj_electronico.tblaudiencias a 
                        inner join htsj_electronico.tblestatusmaterias em on a.idestatusmateria = em.idestatusmateria
                        inner join htsj_electronico.tblestatus e on em.cveestatus = e.cveestatus
                        inner join htsj_electronico.tbltiposestatus te on e.cvetipoestatus = te.cvetipoestatus 
                        inner join htsj_gestion.tbljuzgados j on j.idjuzgado = a.cveadscripcion
                        where te.cvetipoestatus in(5,7) 
                        and a.fechaInicial between @fechaIni AND @fechaFin 
                        and j.cvedistrito = @cveDistrito
                        and j.idjuzgado like @cveJuzgado
                        and @materia
                        and a.activo = 'S'  and em.activo = 'S' and e.activo = 'S' and te.activo = 'S'
                        group by anio, mes
                        order by anio, mes;"})

    End Sub


End Class
