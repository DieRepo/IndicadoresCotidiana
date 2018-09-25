Imports System.Web.UI
Imports System.Web.Services
Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports IronPdf

Public Class Servicio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <WebMethod>
    Public Shared Function ObtenDistritos() As List(Of Distritos)
        Dim datosLlena As New List(Of Distritos)()
        Try

            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(2)

            Dim sql As String = " select * from tbldistritos where activo = 'S';"
            Dim cmd As New MySqlCommand(sql, Coneccion)
            Dim reader As MySqlDataReader = cmd.ExecuteReader

            While reader.Read()

                datosLlena.Add(New Distritos() With {
                    .idDistrito = reader("cveDistrito").ToString,
                    .Descripcion = reader("desDistrito").ToString
                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception
            Debug.WriteLine("Error en funcion ObtenDistritos: " + ex.ToString)
        End Try
        Return datosLlena
    End Function

    <WebMethod> _
    Public Shared Function ObtenSemanas() As List(Of Semanas)
        Dim datosLlena As New List(Of Semanas)()
        Try
            ' Console.WriteLine()

            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(2)

            Dim sql As String = "SELECT semana, anio,DATE_FORMAT(fecha_Inicio,'%d/%m/%Y') AS fecha_Inicio, DATE_FORMAT(fecha_Fin,'%d/%m/%Y') AS fecha_Fin FROM calculo_indicadores group by  semana, anio order by anio desc,semana desc;"
            Dim cmd As New MySqlCommand(sql, Coneccion)
            Dim reader As MySqlDataReader = cmd.ExecuteReader

            While reader.Read()
                'Console.WriteLine(reader(0).ToString)
                datosLlena.Add(New Semanas() With { _
                    .Anio = reader("anio").ToString, _
                    .Semana = reader("semana").ToString, _
                    .F1 = (reader("fecha_Inicio").ToString), _
                    .F2 = reader("fecha_Fin").ToString
                                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception

        End Try
        Return datosLlena
    End Function

    <WebMethod> _
    Public Shared Function ObtenIndicadores() As List(Of Indicadores)
        Dim datosLlena As New List(Of Indicadores)()
        Try

            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(2)

            Dim sql As String = "SELECT * FROM indicadores_modelo_de_gestion where Activo = 1;"
            Dim cmd As New MySqlCommand(sql, Coneccion)
            Dim reader As MySqlDataReader = cmd.ExecuteReader

            While reader.Read()
                'Console.WriteLine(reader(0).ToString)
                datosLlena.Add(New Indicadores() With { _
                    .idIndicador = reader("idIndicador").ToString, _
                    .Descripcion = reader("Descripcion").ToString
                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception

        End Try
        Return datosLlena
    End Function

    <WebMethod> _
    Public Shared Function Obtencalculos(idIndicador As Integer, Anio As Integer, Semana As Integer) As List(Of CalculoSemanas)
        Dim datosLlena As New List(Of CalculoSemanas)()
        Try
            ' Console.WriteLine()


            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(1)



            Dim sql As String = "SELECT *,DATE_FORMAT(fecha_inicio_reporte,'%d/%m/%Y') AS fecha_inicio_reporte1, DATE_FORMAT(fecha_fin_reporte,'%d/%m/%Y') AS fecha_fin_reporte1  FROM calculo_indicadores where semana = @Semana and idIndicador = @idIndicador and activo=1 and Anio = @Anio;"
            Dim cmd As New MySqlCommand(sql, Coneccion)
            cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
            cmd.Parameters.AddWithValue("@Anio", Anio)
            cmd.Parameters.AddWithValue("@Semana", Semana)
            Dim reader As MySqlDataReader = cmd.ExecuteReader



            While reader.Read()
                'Console.WriteLine(reader(0).ToString)
                datosLlena.Add(New CalculoSemanas() With { _
                    .idIndicador = reader("idIndicador").ToString, _
                    .idJuzgado = reader("idJuzgado").ToString, _
                    .DescripcionJuzgado = reader("Nombre_Juzgado").ToString, _
                    .Semana = reader("semana").ToString, _
                     .Fecha1 = reader("fecha_Inicio").ToString, _
                    .Fecha2 = reader("fecha_Fin").ToString, _
                    .Fecha1R = reader("fecha_inicio_reporte1").ToString, _
                    .Fecha2R = reader("fecha_fin_reporte1").ToString, _
                    .Calculo = reader("calculo").ToString
                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception

        End Try
        Return datosLlena
    End Function

    <WebMethod> _
    Public Shared Function ObtenDatosGrafica(idIndicador As Integer, idJuzgado As Integer, Anio As Integer, Semana As Integer) As List(Of CalculoSemanasIndicador)
        Dim datosLlena As New List(Of CalculoSemanasIndicador)()
        Try
            ' Console.WriteLine()


            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(1)



            Dim sql As String = "SELECT " & _
                                " * " & _
                                " FROM " & _
                                " calculo_indicadores " & _
                                " WHERE " & _
                                " idJuzgado = @idJuzgado" & _
                                " AND idIndicador = @idIndicador" & _
                                " AND anio <= @Anio " & _
                                "  AND semana <= @Semana" & _
                                " order by semana limit 4;"
            Dim cmd As New MySqlCommand(sql, Coneccion)
            cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
            cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
            cmd.Parameters.AddWithValue("@Anio", Anio)
            cmd.Parameters.AddWithValue("@Semana", Semana)
            Dim reader As MySqlDataReader = cmd.ExecuteReader

            While reader.Read()
                'Console.WriteLine(reader(0).ToString)
                datosLlena.Add(New CalculoSemanasIndicador() With { _
                    .idIndicador = reader("idIndicador").ToString, _
                    .idJuzgado = reader("idJuzgado").ToString, _
                    .DescripcionJuzgado = reader("Nombre_Juzgado").ToString, _
                    .Semana = reader("semana").ToString, _
                    .Anio = reader("anio").ToString, _
                    .Fecha1 = reader("fecha_Inicio").ToString, _
                    .Fecha2 = reader("fecha_Fin").ToString, _
                    .Fecha1R = reader("fecha_inicio_reporte").ToString, _
                    .Fecha2R = reader("fecha_fin_reporte").ToString, _
                    .Calculo = reader("calculo").ToString
                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception

        End Try
        Return datosLlena
    End Function


    <WebMethod>
    Public Shared Function ObtenCalculoIndicadores(idJuzgado As Integer, idIndicador As Integer, fechaInicio As String, fechaFin As String) As List(Of CalculoIndicador)
        Dim datosLlena As New List(Of CalculoIndicador)()
        Try
            ' Console.WriteLine()


            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(2)
            Dim reader As MySqlDataReader = Nothing
            If idIndicador = 1 Then
                Coneccion.Close()
                Coneccion = Conexion.conexion_global(1)

                Dim sql As String = "SELECT 
                                        idJuzgado as juz_clave,
                                        nombre_Juzgado as juz_des,
                                         total as totales,
                                        CalMal as tramite,
                                        calculo as calculo,
                                         @idIndicador AS Indicador,
                                        anio,
                                        Mes
                                    FROM
                                        indicadores_pjem.calculo_indicadores
                                    WHERE
                                        idIndicador = @idIndicador AND idJuzgado = @idJuzgado
                                        and anio <= year(@fechaFin)
                                    ORDER BY  anio desc,mes desc
                                    LIMIT 4;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 2 Then
            ElseIf idIndicador = 3 Then
                Dim sql As String = "SELECT 
                                        juz_clave,
                                        juz_des,
                                        tramite,
                                        totales,
                                        (tramite / totales) * 100 as calculo,
                                        @idIndicador as Indicador,
                                        anio,
                                        mes
                                    FROM
                                        (SELECT 
                                            j.cveJuzgado AS juz_clave,
                                                j.desJuzgado AS juz_des,
                                                year(cj.fechaRadicacion ) Anio ,month(cj.fechaRadicacion ) Mes,
                                                COUNT(DISTINCT (CONCAT(cj.numero, '/', cj.anio, j.desJuzgado))) AS tramite,
                                                (SELECT 
                                                        COUNT(DISTINCT (CONCAT(cj2.numero, '/', cj2.anio, j2.desJuzgado)))
                                                    FROM
                                                        tblcarpetasjudiciales cj2
                                                    INNER JOIN tbljuzgados j2 ON j2.cvejuzgado = cj2.cveJuzgado
                                                    WHERE
                                                        cj2.fechaRadicacion BETWEEN CONCAT(YEAR(cj.fechaRadicacion), '-', MONTH(cj.fechaRadicacion), '-01 00:00:00') AND concat(year(cj.fechaRadicacion ),'-',month(cj.fechaRadicacion ),'-31 23:59:59')
                                                            AND cj2.cveTipoCarpeta = 2
                                                            AND cj2.activo = 'S'
                                                            AND j2.activo = 'S'
                                                            AND j2.cvejuzgado = j.cveJuzgado
                                                            AND j2.cveDistrito = @idJuzgado
                                                    GROUP BY j2.cveJuzgado) totales
                                        FROM
                                            tblcarpetasjudiciales cj
                                        INNER JOIN tbljuzgados j ON j.cvejuzgado = cj.cveJuzgado
                                        WHERE
                                            cj.fechaRadicacion BETWEEN @fechaInicio AND @fechaFin
                                                AND cj.cveTipoCarpeta = 2
                                                AND cj.activo = 'S'
                                                AND j.activo = 'S'
                                                AND cj.cveEstatusCarpeta != 2
                                                AND j.cveDistrito = @idJuzgado
                                        GROUP BY year(cj.fechaRadicacion ),month(cj.fechaRadicacion ) order by anio desc,mes desc) AS ctram;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 4 Then
                Dim sql As String = "select 762 as juz_clave, 'JUZGADO DE CONTROL DE TOLUCA' as juz_des ,0 as tramite,152 as totales,((0/152)*100) as calculo,4 as Indicador,2018 as anio,3 as Mes
                                        Union
                                        SELECT 
                                        juz_clave,
                                        juz_des,
                                        tramite,
                                        totales,
                                        (tramite / totales) * 100 AS calculo,
                                        @idIndicador AS Indicador,
                                        anio,
                                        Mes
                                    FROM
                                        (SELECT 
                                            j.cveJuzgado AS juz_clave,
                                                j.desJuzgado AS juz_des,
                                                YEAR(cj.fechaRadicacion) Anio,
                                                MONTH(cj.fechaRadicacion) Mes,
                                                COUNT(DISTINCT (CONCAT(cj.numero, '/', cj.anio, j.desJuzgado))) AS tramite,
                                                (SELECT 
                                                        COUNT(DISTINCT (CONCAT(cj2.numero, '/', cj2.anio, j2.desJuzgado)))
                                                    FROM
                                                        tblcarpetasjudiciales cj2
                                                    INNER JOIN tbljuzgados j2 ON j2.cvejuzgado = cj2.cveJuzgado
                                                    WHERE
                                                        cj2.fechaRadicacion BETWEEN CONCAT(YEAR(cj.fechaRadicacion), '-', MONTH(cj.fechaRadicacion), '-01 00:00:00') AND CONCAT(YEAR(cj.fechaRadicacion), '-', MONTH(cj.fechaRadicacion), '-31 23:59:59')
                                                            AND cj2.cveTipoCarpeta = 2
                                                            AND j2.cvejuzgado = j.cveJuzgado
                                                            AND cj2.activo = 'S'
                                                            AND j2.activo = 'S'
                                                            AND j.cveDistrito = @idJuzgado
                                                    GROUP BY j2.cveJuzgado) AS totales
                                        FROM
                                            tblcarpetasjudiciales cj
                                        INNER JOIN tbljuzgados j ON j.cvejuzgado = cj.cveJuzgado
                                        WHERE
                                            cj.fechaRadicacion BETWEEN @fechaInicio AND @fechaFin
                                                AND cj.cveTipoCarpeta = 2
                                                AND cj.cveEstatusCarpeta = 2
                                                AND j.cveDistrito = @idJuzgado
                                        GROUP BY YEAR(cj.fechaRadicacion) , MONTH(cj.fechaRadicacion)
                                        ORDER BY anio DESC , mes DESC) AS Concluidos"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 5 Then
                Dim sql As String = "SELECT juz_clave,
                                            juz_des,
                                            (totales/30) as totales,
                                            tramite,
                                            (totales / tramite)  AS DIAS,
                                            (totales / tramite)/12 AS calculo,
                                            (totales / tramite) / 365 AS ANIO, 
                                             @idIndicador AS Indicador,
	                                         anio,
	                                         Mes
                                        FROM
                                            (SELECT
                                                    j.cveJuzgado AS juz_clave,
                                                    j.desJuzgado AS juz_des,
                                                    YEAR(cj.fechaRadicacion) Anio,
			                                        MONTH(cj.fechaRadicacion) Mes,
                                                    COUNT(DISTINCT (CONCAT(CJ.numero, '/', CJ.anio, j.cveJuzgado))) AS tramite,
                                                    (SELECT 
                                                            SUM(TIMESTAMPDIFF(DAY, (CJ2.fechaRadicacion), @fechaFin)) AS SUMATORIA
                                                        FROM
                                                            tblcarpetasjudiciales cj2
                                                        INNER JOIN tbljuzgados j2 ON CJ2.cveJuzgado = j2.cveJuzgado
                                                        WHERE
                                                            CJ2.cveTipoCarpeta = 2
						                                        AND cj2.fechaRadicacion <=  cj.fechaRadicacion
                                                                AND CJ2.ACTIVO = 'S'
                                                                AND cj2.cveEstatusCarpeta <> 2
                                                                AND j2.cvejuzgado = j.cveJuzgado
                                                                AND j2.cveDistrito = @idJuzgado
                                                        GROUP BY j2.cveJuzgado) AS totales
                                            FROM tblcarpetasjudiciales cj
                                            INNER JOIN tbljuzgados j ON j.cvejuzgado = cj.cveJuzgado
                                            WHERE
                                                cj.cveTipoCarpeta = 2
                                                    AND cj.cveEstatusCarpeta <> 2
                                                    AND cj.fechaRadicacion <= @fechaFin
                                                   AND j.cveDistrito = @idJuzgado
                                              GROUP BY YEAR(cj.fechaRadicacion) , MONTH(cj.fechaRadicacion) ORDER BY anio DESC , mes DESC) AS Antiguedad;"

                Dim cmd As New MySqlCommand(sql, Coneccion)
                cmd.CommandTimeout = 600
                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 6 Then
                Dim sql As String = "SELECT 
                                        jdo.cveJuzgado AS juz_clave,
                                        jdo.desJuzgado AS juz_des,
                                        (SUM(stm.fechaRegistro - src.fechaRegistro)) / (10000 * 24) AS tramite,
                                        COUNT(DISTINCT (src.idReferencia)) AS totales,
                                        (SUM(stm.fechaRegistro - src.fechaRegistro)) / (10000 * 24 * COUNT(DISTINCT (src.idReferencia))) AS calculo,
                                        @idIndicador AS Indicador,
                                        YEAR(src.fechaRegistro) anio,
                                        MONTH(src.fechaRegistro) mes
                                    FROM
                                        tblantecedesactuaciones AS ant
                                            INNER JOIN
                                        tblactuaciones AS src ON ant.idActuacionPadre = src.idActuacion
                                            AND src.cveTipoActuacion IN (1 , 13, 17, 27, 39)
                                            AND ant.activo = 'S'
                                            INNER JOIN
                                        tblactuaciones AS stm ON ant.idActuacionHija = stm.idActuacion
                                            AND stm.cveTipoActuacion
                                            INNER JOIN
                                        tbljuzgados AS jdo ON src.cveJuzgado = jdo.cveJuzgado
                                            AND jdo.cveTipoJuzgado IN (1)
                                            INNER JOIN
                                        tblcarpetasjudiciales AS cpt ON src.idReferencia = cpt.idCarpetaJudicial
                                    WHERE
                                        src.fechaRegistro BETWEEN @fechaInicio AND @fechaFin
                                            AND ((cpt.carpetaInv LIKE '%/%'
                                            AND LENGTH(cpt.carpetaInv) >= 28)
                                            OR (cpt.nuc LIKE '%/%'
                                            AND LENGTH(cpt.nuc) >= 28))
                                            AND jdo.cveDistrito = @idJuzgado
                                    GROUP BY YEAR(src.fechaRegistro) , MONTH(src.fechaRegistro)
                                    ORDER BY anio DESC , mes DESC;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 7 Then
                Dim sql As String = "SELECT 
                                            juz_clave,
                                            juz_des,
                                            case when isnull(intermedias) then 0 else  intermedias end AS totales,
                                            audiencias AS tramite,
                                            ((case when isnull(intermedias) then 0 else  intermedias end) / audiencias) * 100 AS calculo,
                                            @idIndicador AS Indicador,
                                            anio,
                                            Mes
                                        FROM
                                            (SELECT 
                                                juz.cveJuzgado AS juz_clave,
                                                    juz.desJuzgado AS juz_des,
                                                    YEAR(aud.fechaInicial) Anio,
                                                    MONTH(aud.fechaInicial) Mes,
                                                    COUNT(*) AS audiencias,
                                                    (SELECT 
                                                            COUNT(*)
                                                        FROM
                                                            htsj_sigejupe.tblcarpetasjudiciales cj2
                                                        INNER JOIN htsj_sigejupe.tblaudiencias aud2 ON cj2.idCarpetaJudicial = aud2.idReferencia
                                                        INNER JOIN htsj_sigejupe.tbljuzgados juz2 ON juz2.cveJuzgado = cj2.cveJuzgado
                                                        WHERE
                                                            aud2.fechaFinal BETWEEN  CONCAT(YEAR(aud.fechaFinal), '-', MONTH(aud.fechaFinal), '-01 00:00:00') AND CONCAT(YEAR(aud.fechaFinal), '-', MONTH(aud.fechaFinal), '-',DAY(LAST_DAY(aud.fechaFinal)),' 23:59:59')
                                                                AND cj2.cveTipoCarpeta = 2
                                                                AND aud2.cveCatAudiencia = 151
                                                                AND cj2.activo = 'S'
                                                                AND aud2.activo = 'S'
                                                                AND juz2.cveJuzgado = juz.cveJuzgado
                                                                AND DATEDIFF(cj2.fechatermino, aud2.fechainicial) BETWEEN 30 AND 40
                                                                AND juz.cveDistrito = @idJuzgado
                                                        GROUP BY juz2.cvejuzgado) AS intermedias
                                            FROM
                                                htsj_sigejupe.tblcarpetasjudiciales cj
                                            INNER JOIN tbljuzgados juz ON juz.cvejuzgado = cj.cveJuzgado
                                            INNER JOIN htsj_sigejupe.tblaudiencias aud ON cj.idCarpetaJudicial = aud.idReferencia
                                            WHERE
                                                aud.fechaInicial BETWEEN @fechaInicio AND @fechaFin
                                                    AND aud.cveCatAudiencia = 151
                                                    AND cj.cveTipoCarpeta = 2
                                                    AND cj.activo = 'S'
                                                    AND aud.activo = 'S'
                                                    AND juz.cveDistrito = @idJuzgado
                                            GROUP BY YEAR(aud.fechaInicial) , MONTH(aud.fechaInicial)
                                            ORDER BY anio DESC , mes DESC) AS taudiencias;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 8 Then
                Dim sql As String = "SELECT 
                                            juz_clave,
                                            juz_des,
                                            case when isnull(intermedias) then 0 else intermedias end AS totales,
                                            audiencias AS tramite,
                                            case when isnull( (intermedias / audiencias) * 100) then 0 else  ((intermedias / audiencias) * 100) end AS calculo,
                                            @idIndicador AS Indicador,
                                            anio,
                                            Mes
                                        FROM
                                            (SELECT 
                                                juz.cveJuzgado AS juz_clave,
                                                    juz.desJuzgado AS juz_des,
                                                    YEAR(aud.fechaInicial) Anio,
                                                    MONTH(aud.fechaInicial) Mes,
                                                    COUNT(*) AS audiencias,
                                                    (SELECT 
                                                            COUNT(*)
                                                        FROM
                                                            htsj_sigejupe.tblcarpetasjudiciales cj2
                                                        INNER JOIN htsj_sigejupe.tblaudiencias aud2 ON cj2.idCarpetaJudicial = aud2.idReferencia
                                                        INNER JOIN htsj_sigejupe.tbljuzgados juz2 ON juz2.cveJuzgado = cj2.cveJuzgado
                                                        WHERE
                                                           aud2.fechaFinal BETWEEN  CONCAT(YEAR(aud.fechaFinal), '-', MONTH(aud.fechaFinal), '-01 00:00:00') AND CONCAT(YEAR(aud.fechaFinal), '-', MONTH(aud.fechaFinal), '-31 23:59:59')
                                                                AND cj2.cveTipoCarpeta = 2
                                                                AND aud2.cveCatAudiencia in(135)
                                                                AND cj2.activo = 'S'
                                                                AND aud2.activo = 'S'
                                                                AND juz2.cveJuzgado = juz.cveJuzgado
                                                                AND DATEDIFF(cj2.fechatermino, aud2.fechainicial) <=15
                                                                AND juz.cveDistrito = @idJuzgado
                                                        GROUP BY juz2.cvejuzgado) AS intermedias
                                            FROM
                                                htsj_sigejupe.tblcarpetasjudiciales cj
                                            INNER JOIN tbljuzgados juz ON juz.cvejuzgado = cj.cveJuzgado
                                            INNER JOIN htsj_sigejupe.tblaudiencias aud ON cj.idCarpetaJudicial = aud.idReferencia
                                            WHERE
                                                aud.fechaInicial BETWEEN @fechaInicio AND @fechaFin
                                                    AND aud.cveCatAudiencia in(135)
                                                    AND cj.cveTipoCarpeta = 2
                                                    AND cj.activo = 'S'
                                                   AND aud.activo = 'S'
                                                    AND juz.cveDistrito = @idJuzgado
                                            GROUP BY YEAR(aud.fechaInicial) , MONTH(aud.fechaInicial)
                                            ORDER BY anio DESC , mes DESC) AS taudiencias;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 9 Then
                Dim sql As String = "SELECT 
                                            juz_clave,
                                            juz_des,
                                            case when isnull(intermedias) then 0 else intermedias end AS totales,
                                            audiencias AS tramite,
                                            case when isnull( (intermedias / audiencias) * 100) then 0 else  ((intermedias / audiencias) * 100) end AS calculo,
                                            @idIndicador AS Indicador,
                                            anio,
                                            Mes
                                        FROM
                                            (SELECT 
                                                juz.cveJuzgado AS juz_clave,
                                                    juz.desJuzgado AS juz_des,
                                                    YEAR(aud.fechaInicial) Anio,
                                                    MONTH(aud.fechaInicial) Mes,
                                                    COUNT(*) AS audiencias,
                                                    (SELECT 
                                                            COUNT(*)
                                                        FROM
                                                            htsj_sigejupe.tblcarpetasjudiciales cj2
                                                        INNER JOIN htsj_sigejupe.tblaudiencias aud2 ON cj2.idCarpetaJudicial = aud2.idReferencia
                                                        INNER JOIN htsj_sigejupe.tbljuzgados juz2 ON juz2.cveJuzgado = cj2.cveJuzgado
                                                        WHERE
                                                            aud2.fechaFinal BETWEEN  CONCAT(YEAR(aud.fechaFinal), '-', MONTH(aud.fechaFinal), '-01 00:00:00') AND CONCAT(YEAR(aud.fechaFinal), '-', MONTH(aud.fechaFinal), '-31 23:59:59')
                                                                AND cj2.cveTipoCarpeta = 2
                                                                AND aud2.cveCatAudiencia in (142,150,76)
                                                                AND cj2.activo = 'S'
                                                                AND aud2.activo = 'S'
                                                                AND juz2.cveJuzgado = juz.cveJuzgado
                                                                AND DATEDIFF(cj2.fechatermino, aud2.fechainicial) <=3
                                                                AND juz.cveDistrito = @idJuzgado
                                                        GROUP BY juz2.cvejuzgado) AS intermedias
                                            FROM
                                                htsj_sigejupe.tblcarpetasjudiciales cj
                                            INNER JOIN tbljuzgados juz ON juz.cvejuzgado = cj.cveJuzgado
                                            INNER JOIN htsj_sigejupe.tblaudiencias aud ON cj.idCarpetaJudicial = aud.idReferencia
                                            WHERE
                                                 aud.fechaInicial BETWEEN @fechaInicio AND @fechaFin
                                                    AND aud.cveCatAudiencia in (142,150,76)
                                                    AND cj.cveTipoCarpeta = 2
                                                    AND cj.activo = 'S'
                                                   AND aud.activo = 'S'
                                                    AND juz.cveDistrito = @idJuzgado
                                            GROUP BY YEAR(aud.fechaInicial) , MONTH(aud.fechaInicial)
                                            ORDER BY anio DESC , mes DESC) AS taudiencias;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 10 Then
            ElseIf idIndicador = 11 Then
                Dim sql As String = "SELECT 
                                        juz_clave,
                                        juz_des,
                                        adst  AS totales,
                                        resul2 AS tramite,
                                        (adst / resul2) * 100 AS calculo,
                                         @idIndicador AS Indicador,
                                        anio,
                                        Mes
                                    FROM
                                        (SELECT 
                                              j.cveJuzgado AS juz_clave,
                                                j.desJuzgado AS juz_des,
                                                YEAR(act.fechaRegistro ) Anio,
                                                MONTH(act.fechaRegistro ) Mes,
                                                COUNT(*) AS adst,
                                                (SELECT 
                                                        COUNT(*) AS exp
                                                    FROM
                                                        tblactuaciones act2
                                                    INNER JOIN tblcarpetasjudiciales cj2 ON act2.idReferencia = cj2.idCarpetaJudicial
                                                    INNER JOIN tbljuzgados j2 ON j2.cveJuzgado = cj2.cveJuzgado
                                                    WHERE
                                                        cj2.cveTipoCarpeta IN (2)
                                                            AND act2.activo = 'S'
                                                            AND cj2.activo = 'S'
                                                            AND cj2.fechaRadicacion BETWEEN act.fechaRegistro  AND CONCAT(YEAR( act.fechaRegistro), '-', MONTH( act.fechaRegistro), '-31 23:59:59')
                                                            AND j2.cveJuzgado = j.cveJuzgado
                                                            AND j2.cveDistrito = @idJuzgado
                                                    GROUP BY j2.cveJuzgado) AS resul2
                                        FROM
                                            tblactuaciones act
                                        INNER JOIN tblcarpetasjudiciales cj ON act.idReferencia = cj.idCarpetaJudicial
                                        INNER JOIN tbljuzgados j ON j.cveJuzgado = cj.cveJuzgado
                                        WHERE
                                            act.Sintesis LIKE '%Desistimiento%'
                                                AND act.activo = 'S'
                                                AND cj.activo = 'S'
                                                AND cj.cveTipoCarpeta IN (2)
                                                AND act.fechaRegistro BETWEEN @fechaInicio AND @fechaFin
                                                AND j.cveDistrito = @idJuzgado
                                        GROUP BY YEAR(act.fechaRegistro) , MONTH(act.fechaRegistro)
                                        ORDER BY anio DESC , mes DESC) AS resul1;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 12 Then
                Dim sql As String = "SELECT 
                                        juz_clave,
                                        juz_des,
                                        addj AS totales,
                                        result2 AS tramite,
                                        (addj / result2) * 100 AS calculo,
                                        @idIndicador AS Indicador,
                                        anio,
                                        Mes
                                    FROM
                                        (SELECT 
                                            j.cveJuzgado AS juz_clave,
                                                j.desJuzgado AS juz_des,
                                                YEAR(a.fechaFinal) Anio,
                                                MONTH(a.fechaFinal) Mes,
                                                COUNT(*) AS addj,
                                                (SELECT 
                                                        COUNT(*) AS aup
                                                    FROM
                                                        tblaudiencias a2
                                                    INNER JOIN tblcarpetasjudiciales cj2 ON a2.idReferencia = cj2.idCarpetaJudicial
                                                    INNER JOIN tbljuzgados j2 ON j2.cveJuzgado = cj2.cveJuzgado
                                                    INNER JOIN tblcataudiencias ca2 ON ca2.cveCatAudiencia = a2.cveCatAudiencia
                                                    WHERE
                                                        ca2.cveCatAudiencia IN (151)
                                                            AND cj2.cveTipoCarpeta = 2
                                                            AND a2.cveEstatusAudiencia IN (1 , 2)
                                                            AND cj2.activo = 'S'
                                                            AND a2.activo = 'S'
                                                            AND j.cveDistrito = @idJuzgado
                                                            AND a2.fechaFinal BETWEEN  CONCAT(YEAR(a.fechaFinal), '-', MONTH(a.fechaFinal), '-01 00:00:00') AND CONCAT(YEAR(a.fechaFinal), '-', MONTH(a.fechaFinal), '-',DAY(LAST_DAY(a.fechaFinal)),' 23:59:59')
                                                            AND j2.cveJuzgado = j.cveJuzgado
                                                    GROUP BY j2.cvejuzgado) AS result2
                                        FROM
                                            tblaudiencias a
                                        INNER JOIN tblcarpetasjudiciales cj ON a.idReferencia = cj.idCarpetaJudicial
                                        INNER JOIN tbljuzgados j ON j.cveJuzgado = cj.cveJuzgado
                                        INNER JOIN tblcataudiencias ca ON ca.cveCatAudiencia = a.cveCatAudiencia
                                        WHERE
                                            ca.cveCatAudiencia IN (188)
                                                AND a.cveEstatusAudiencia IN (1 , 2)
                                                AND cj.activo = 'S'
                                                AND a.activo = 'S'
                                                AND cj.cveTipoCarpeta = 2
                                                AND j.cveDistrito = @idJuzgado
                                                AND a.fechaInicial BETWEEN @fechaInicio AND @fechaFin
                                        GROUP BY YEAR(a.fechaInicial) , MONTH(a.fechaInicial)
                                        ORDER BY anio DESC , mes DESC) AS result1;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 13 Then
                Dim sql As String = "SELECT 
                                        juz_clave,
                                        juz_des,
                                        addj AS totales,
                                        result2 AS tramite,
                                        (addj / result2) * 100 AS calculo,
                                        @idIndicador AS Indicador,
                                        anio,
                                        Mes
                                    FROM
                                        (SELECT 
                                            j.cveJuzgado AS juz_clave,
                                                j.desJuzgado AS juz_des,
                                                YEAR(a.fechaFinal) Anio,
                                                MONTH(a.fechaFinal) Mes,
                                                COUNT(*) AS addj,
                                                (SELECT 
                                                        COUNT(*) AS aup
                                                    FROM
                                                        tblaudiencias a2
                                                    INNER JOIN tblcarpetasjudiciales cj2 ON a2.idReferencia = cj2.idCarpetaJudicial
                                                    INNER JOIN tbljuzgados j2 ON j2.cveJuzgado = cj2.cveJuzgado
                                                    INNER JOIN tblcataudiencias ca2 ON ca2.cveCatAudiencia = a2.cveCatAudiencia
                                                    WHERE
                                                        ca2.cveCatAudiencia IN (142)
                                                            AND cj2.cveTipoCarpeta = 2
                                                            AND a2.cveEstatusAudiencia IN (1 , 2)
                                                            AND cj2.activo = 'S'
                                                            AND a2.activo = 'S'
                                                            AND j.cveDistrito = @idJuzgado
                                                            AND a2.fechaFinal BETWEEN  CONCAT(YEAR(a.fechaFinal), '-', MONTH(a.fechaFinal), '-01 00:00:00') AND CONCAT(YEAR(a.fechaFinal), '-', MONTH(a.fechaFinal), '-',DAY(LAST_DAY(a.fechaFinal)),' 23:59:59')
                                                            AND j2.cveJuzgado = j.cveJuzgado
                                                    GROUP BY j2.cvejuzgado) AS result2
                                        FROM
                                            tblaudiencias a
                                        INNER JOIN tblcarpetasjudiciales cj ON a.idReferencia = cj.idCarpetaJudicial
                                        INNER JOIN tbljuzgados j ON j.cveJuzgado = cj.cveJuzgado
                                        INNER JOIN tblcataudiencias ca ON ca.cveCatAudiencia = a.cveCatAudiencia
                                        WHERE
                                            ca.cveCatAudiencia IN (142)
                                                AND a.cveEstatusAudiencia IN (1 , 2)
                                                AND cj.activo = 'S'
                                                AND a.activo = 'S'
                                                AND cj.cveTipoCarpeta = 2
                                                AND j.cveDistrito = @idJuzgado
                                                AND a.fechaInicial BETWEEN @fechaInicio AND @fechaFin
                                        GROUP BY YEAR(a.fechaInicial) , MONTH(a.fechaInicial)
                                        ORDER BY anio DESC , mes DESC) AS result1;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 14 Then
                Dim sql As String = "SELECT 
                                        juz_clave,
                                        juz_des,
                                        addj AS totales,
                                        result2 AS tramite,
                                        (addj / result2) * 100 AS calculo,
                                        @idIndicador AS Indicador,
                                        anio,
                                        Mes
                                    FROM
                                        (SELECT 
                                            j.cveJuzgado AS juz_clave,
                                                j.desJuzgado AS juz_des,
                                                YEAR(a.fechaFinal) Anio,
                                                MONTH(a.fechaFinal) Mes,
                                                COUNT(*) AS addj,
                                                (SELECT 
                                                        COUNT(*) AS aup
                                                    FROM
                                                        tblaudiencias a2
                                                    INNER JOIN tblcarpetasjudiciales cj2 ON a2.idReferencia = cj2.idCarpetaJudicial
                                                    INNER JOIN tbljuzgados j2 ON j2.cveJuzgado = cj2.cveJuzgado
                                                    INNER JOIN tblcataudiencias ca2 ON ca2.cveCatAudiencia = a2.cveCatAudiencia
                                                    WHERE
                                                        ca2.cveCatAudiencia IN (135)
                                                            AND cj2.cveTipoCarpeta = 2
                                                            AND a2.cveEstatusAudiencia IN (1 , 2)
                                                            AND cj2.activo = 'S'
                                                            AND a2.activo = 'S'
                                                            AND j.cveDistrito = @idJuzgado
                                                            AND a2.fechaFinal BETWEEN  CONCAT(YEAR(a.fechaFinal), '-', MONTH(a.fechaFinal), '-01 00:00:00') AND CONCAT(YEAR(a.fechaFinal), '-', MONTH(a.fechaFinal), '-',DAY(LAST_DAY(a.fechaFinal)),' 23:59:59')
                                                            AND j2.cveJuzgado = j.cveJuzgado
                                                    GROUP BY j2.cvejuzgado) AS result2
                                        FROM
                                            tblaudiencias a
                                        INNER JOIN tblcarpetasjudiciales cj ON a.idReferencia = cj.idCarpetaJudicial
                                        INNER JOIN tbljuzgados j ON j.cveJuzgado = cj.cveJuzgado
                                        INNER JOIN tblcataudiencias ca ON ca.cveCatAudiencia = a.cveCatAudiencia
                                        WHERE
                                            ca.cveCatAudiencia IN (142)
                                                AND a.cveEstatusAudiencia IN (1 , 2)
                                                AND cj.activo = 'S'
                                                AND a.activo = 'S'
                                                AND cj.cveTipoCarpeta = 2
                                                AND j.cveDistrito = @idJuzgado
                                                AND a.fechaInicial BETWEEN @fechaInicio AND @fechaFin
                                        GROUP BY YEAR(a.fechaInicial) , MONTH(a.fechaInicial)
                                        ORDER BY anio DESC , mes DESC) AS result1;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 15 Then
            ElseIf idIndicador = 16 Then
                Dim sql As String = "SELECT 
                                        j.cveJuzgado as juz_clave,
                                        j.desJuzgado as juz_des,
                                         0 AS totales,
                                        COUNT(aud.idAudiencia) AS tramite,
                                        (1440672.70 / COUNT(aud.idAudiencia)) AS calculo,
                                        @idIndicador AS Indicador,
                                         YEAR(aud.fechaInicial) Anio,
                                                MONTH(aud.fechaInicial) Mes
                                    FROM
                                        htsj_sigejupe.tblcarpetasjudiciales cj
                                            INNER JOIN
                                        htsj_sigejupe.tblaudiencias aud ON cj.idCarpetaJudicial = aud.idreferencia
                                            INNER JOIN
                                        htsj_sigejupe.tbljuzgados j ON j.cveJuzgado = cj.cveJuzgado
                                    WHERE
                                        aud.fechaInicial BETWEEN @fechaInicio AND @fechaFin
                                            AND cj.cveTipoCarpeta = 2
                                            AND cj.activo = 'S'
                                            AND aud.activo = 'S'
                                            AND j.cveDistrito = @idJuzgado
                                    GROUP BY  YEAR(aud.fechaInicial) , MONTH(aud.fechaInicial)
                                        ORDER BY anio DESC , mes DESC;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 17 Then
                Dim sql As String = "SELECT 
                                       juz_clave,
                                       juz_des,
                                       puntuales as tramite,
                                       Total as totales,
                                       (100 * (puntuales / Total)) AS calculo,
                                       @idIndicador AS Indicador,
                                       anio,
                                       mes
                                    FROM
                                        (SELECT 
                                            COUNT(*) AS puntuales, 
                                            ju.cveJuzgado AS juz_clave,
		                                    ju.desJuzgado AS juz_des,
                                            YEAR(aU.fechaInicial) anio,
		                                    MONTH(aU.fechaInicial) mes,
                                            (SELECT 
					                                    COUNT(aud.idAudiencia) AS total
				                                    FROM
					                                    tblaudiencias aud
				                                    INNER JOIN tbljuzgados juzgado ON juzgado.cveJuzgado = aud.cveJuzgado
				                                    WHERE
					                                    aud.fechaInicialProgramada IS NOT NULL
						                                    AND aud.fechaInicial BETWEEN  CONCAT(YEAR(aU.fechaInicial), '-', MONTH(aU.fechaInicial), '-01 00:00:00') AND CONCAT(YEAR(aU.fechaInicial), '-', MONTH(aU.fechaInicial), '-31 23:59:59')
						                                    AND juzgado.cveDistrito = @idJuzgado
						                                    AND aud.cveTipoCarpeta = 2
						                                    and ju.desJuzgado = juzgado.desJuzgado
                                                            GROUP BY juzgado.cvejuzgado) Total
    
                                        FROM
                                            tblaudiencias aU
                                        INNER JOIN tbljuzgados ju ON ju.cveJuzgado = aU.cveJuzgado
                                        WHERE
                                            aU.fechaInicial <= aU.fechaInicialProgramada
                                                AND aU.fechaInicialProgramada IS NOT NULL
                                                AND aU.fechaInicial BETWEEN @fechaInicio AND @fechaFin
                                                AND ju.cveDistrito = @idJuzgado
                                                AND aU.cveTipoCarpeta = 2
                                                AND aU.activo = 'S'
                                        GROUP BY YEAR(aU.fechaInicial) DESC , MONTH(aU.fechaInicial) DESC) a ;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader

            ElseIf idIndicador = 18 Then
                Dim sql As String = "SELECT 
                                    cveJuzgado as juz_clave,
                                        desJuzgado AS juz_des,
                                        difinicio as totales,
                                        diffinal as tramite ,
                                        (difinicio + diffinal) AS calculo,
                                        @idIndicador AS Indicador,
                                        anio,
                                        mes
    
                                    FROM
                                        (SELECT 
                                            ROUND(SUM(TIME_TO_SEC(ABS(TIMEDIFF(aud.fechaInicialProgramada, aud.fechaInicial))) / 60)) AS difinicio,
                                                juzgado.desJuzgado,
                                                juzgado.cveJuzgado,
                                                YEAR(aud.fechaInicial) anio,
			                                    MONTH(aud.fechaInicial) mes,
                                                 (SELECT 
				                                    ROUND(SUM(TIME_TO_SEC(ABS(TIMEDIFF(aud2.fechaFinal, aud2.fechaFinalprogramada))) / 60)) AS diffinal
				                                    FROM
					                                    tblaudiencias aud2
				                                    INNER JOIN tbljuzgados juzgado2 ON juzgado2.cveJuzgado = aud2.cveJuzgado
				                                    WHERE
					                                    aud2.fechaInicialProgramada IS NOT NULL
						                                    AND aud2.fechaInicial BETWEEN  CONCAT(YEAR(aud.fechaInicial), '-', MONTH(aud.fechaInicial), '-01 00:00:00') AND CONCAT(YEAR(aud.fechaInicial), '-', MONTH(aud.fechaInicial), '-31 23:59:59')
						                                    AND juzgado2.cveDistrito = @idJuzgado
						                                    and juzgado2.cveJuzgado = juzgado.cveJuzgado       
						                                    AND aud2.cveTipoCarpeta = 2
				                                    GROUP BY juzgado.desJuzgado) as diffinal
    
                                        FROM
                                            tblaudiencias aud
                                        INNER JOIN tbljuzgados juzgado ON juzgado.cveJuzgado = aud.cveJuzgado
                                        WHERE
                                            aud.fechaInicialProgramada IS NOT NULL
                                                AND aud.fechaInicial BETWEEN  @fechaInicio AND @fechaFin
                                                AND juzgado.cveDistrito = @idJuzgado
                                                AND aud.cveTipoCarpeta = 2
                                                AND aud.activo = 'S'
                                        GROUP BY YEAR(aud.fechaInicial) DESC , MONTH(aud.fechaInicial) DESC) a;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 19 Then
                Dim sql As String = "select juz_clave,
                                    juz_des,
                                    totales,
                                    case when isnull(tramite) then 0 else tramite  end as tramite,
                                    (((case when isnull(tramite) then 0 else tramite  end)/totales)*100) as calculo,
                                    Indicador,
                                    anio,
                                    mes
                                        from (SELECT 
                                        jdo.cveJuzgado as juz_clave,
                                        jdo.desJuzgado AS juz_des,
                                        (SUM(TIMESTAMPDIFF(HOUR,aud.fechaInicial, aud.fechaFinal))) AS totales,
                                        ((((DATEDIFF(CONCAT(YEAR(aud.fechaRegistro),'-', MONTH(aud.fechaRegistro), '-',DAY(LAST_DAY(aud.fechaRegistro)),' 23:59:59') ,CONCAT(YEAR(aud.fechaRegistro), '-', MONTH(aud.fechaRegistro), '-01 00:00:00'))+1.5) * 8))* (SELECT count(*) FROM tbljuzgadoresjuzgados jj inner join tbljuzgados juz on jj.cveJuzgado = juz.cveJuzgado inner join tbljuzgadores juzga on jj.idJuzgador = juzga.idJuzgador where juz.cveDistrito = @idJuzgado and juz.cveTipojuzgado = 1 and jj.activo = 'S')) AS tramite,
	                                    /*(((((DATEDIFF(CONCAT(YEAR(aud.fechaRegistro),'-', MONTH(aud.fechaRegistro), '-30 23:59:59') ,CONCAT(YEAR(aud.fechaRegistro), '-', MONTH(aud.fechaRegistro), '-01 00:00:00'))+.5) * 8))* (select count(*) from tbljuzgadoresjuzgados where cveJuzgado = (select cveJuzgado from tbljuzgados where cveDistrito = @idJuzgado and cveTipoJuzgado = 1) and Activo = 'S'))/(SUM(TIMESTAMPDIFF(HOUR,aud.fechaInicial, aud.fechaFinal))))* 100 AS calculo,*/
                                        @idIndicador AS Indicador,
                                        YEAR(aud.fechaRegistro) Anio,
                                        MONTH(aud.fechaRegistro) Mes
                                    FROM
                                        tblaudiencias AS aud
                                            INNER JOIN
                                        tblactuaciones AS act ON aud.idReferencia = act.idReferencia
                                            INNER JOIN
                                        tbljuzgadores AS jdr ON act.idJuzgadorAcuerdo = jdr.idJuzgador
                                            INNER JOIN
                                        tbljuzgados AS jdo ON aud.cveJuzgadoDesahoga = jdo.cveJuzgado
                                            INNER JOIN
                                        tblregiones AS rgn ON jdo.cveRegion = rgn.cveRegion
                                            INNER JOIN
                                        tbldistritos AS dst ON jdo.cveDistrito = dst.cveDistrito
                                    WHERE
                                        aud.fechaRegistro BETWEEN @fechaInicio AND @fechaFin
                                            AND jdo.cveDistrito = @idJuzgado
                                            AND jdo.cveTipojuzgado = 1
                                            AND aud.activo = 'S'
                                    GROUP BY YEAR(aud.fechaRegistro) , MONTH(aud.fechaRegistro)
                                    ORDER BY anio DESC , mes DESC) as a;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 20 Then
                Dim sql As String = "select juz_clave,
                                    juz_des,
                                    totales,
                                    case when isnull(tramite) then 0 else tramite  end as tramite,
                                    (((case when isnull(tramite) then 0 else tramite  end)/totales)*100) as calculo,
                                    Indicador,
                                    anio,
                                    mes
                                        from (SELECT 
                                        jdo.cveJuzgado as juz_clave,
                                        jdo.desJuzgado AS juz_des,
                                        (SUM(TIMESTAMPDIFF(HOUR,aud.fechaInicial, aud.fechaFinal))) AS totales,
                                        ((((DATEDIFF(CONCAT(YEAR(aud.fechaRegistro),'-', MONTH(aud.fechaRegistro), '-',DAY(LAST_DAY(aud.fechaRegistro)),' 23:59:59') ,CONCAT(YEAR(aud.fechaRegistro), '-', MONTH(aud.fechaRegistro), '-01 00:00:00'))+1.5) * 8))* (select count(*) from tblsalas where cveEdificio = ( select cveEdificio from tbljuzgados where cveDistrito = 16 and cveTipoJuzgado = 1) and activo = 'S' and desSala not like'%SALA ITINERANTE%')) AS tramite,
	                                    /*(((((DATEDIFF(CONCAT(YEAR(aud.fechaRegistro),'-', MONTH(aud.fechaRegistro), '-30 23:59:59') ,CONCAT(YEAR(aud.fechaRegistro), '-', MONTH(aud.fechaRegistro), '-01 00:00:00'))+.5) * 8))* (select count(*) from tbljuzgadoresjuzgados where cveJuzgado = (select cveJuzgado from tbljuzgados where cveDistrito = @idJuzgado and cveTipoJuzgado = 1) and Activo = 'S'))/(SUM(TIMESTAMPDIFF(HOUR,aud.fechaInicial, aud.fechaFinal))))* 100 AS calculo,*/
                                        @idIndicador AS Indicador,
                                        YEAR(aud.fechaRegistro) Anio,
                                        MONTH(aud.fechaRegistro) Mes
                                    FROM
                                        tblaudiencias AS aud
                                            INNER JOIN
                                        tblactuaciones AS act ON aud.idReferencia = act.idReferencia
                                            INNER JOIN
                                        tbljuzgadores AS jdr ON act.idJuzgadorAcuerdo = jdr.idJuzgador
                                            INNER JOIN
                                        tbljuzgados AS jdo ON aud.cveJuzgadoDesahoga = jdo.cveJuzgado
                                            INNER JOIN
                                        tblregiones AS rgn ON jdo.cveRegion = rgn.cveRegion
                                            INNER JOIN
                                        tbldistritos AS dst ON jdo.cveDistrito = dst.cveDistrito
                                    WHERE
                                        aud.fechaRegistro BETWEEN @fechaInicio AND @fechaFin
                                            AND jdo.cveDistrito = @idJuzgado
                                            AND jdo.cveTipojuzgado = 1
                                            AND aud.activo = 'S'
                                    GROUP BY YEAR(aud.fechaRegistro) , MONTH(aud.fechaRegistro)
                                    ORDER BY anio DESC , mes DESC) as a;"

                Dim cmd As New MySqlCommand(sql, Coneccion)

                cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
                cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                reader = cmd.ExecuteReader
            ElseIf idIndicador = 21 Then
            End If



            'If (reader.Read()) Then



            While reader.Read()
                'Console.WriteLine(reader(0).ToString)
                datosLlena.Add(New CalculoIndicador() With {
                        .idIndicador = reader("Indicador").ToString,
                        .idJuzgado = reader("juz_clave").ToString,
                        .DescripcionJuzgado = reader("juz_des").ToString,
                        .Porcentaje = reader("tramite").ToString,
                        .Total = reader("totales").ToString,
                        .Calculo = reader("calculo").ToString,
                        .Anio = reader("anio").ToString,
                        .mes = reader("mes").ToString
                                        })
            End While
            'Else
            '    datosLlena.Add(New CalculoIndicador() With {
            '                         .idIndicador = idIndicador,
            '                         .idJuzgado = 0,
            '                         .DescripcionJuzgado = "",
            '                         .Porcentaje = 0,
            '                         .Total = 0,
            '                         .Calculo = 0,
            '                         .Anio = 0,
            '                         .mes = 0
            '                                         })
            'End If

            Conexion.cerrar()

        Catch ex As Exception

        End Try
        Return datosLlena
    End Function

    <WebMethod>
    Public Shared Function ObtenInformacionGeneral(fechaInicio As String, fechaFin As String) As List(Of InfoGen)
        Dim datosLlena As New List(Of InfoGen)()
        Try
            ' Console.WriteLine()

            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(2)

            Dim sql As String = " SELECT 
                                        COUNT(*) as causas,
                                        j.cveJuzgado,
                                        d.desDistrito as desJuzgado,
                                        YEAR(cj.fechaRadicacion) Anio,
                                        MONTH(cj.fechaRadicacion) Mes,
                                        (select COUNT(*) from  tblcarpetasjudiciales cj INNER JOIN
                                        htsj_sigejupe.tbljuzgados j ON j.cveJuzgado = cj.cveJuzgado
                                        WHERE
                                        cj.fechaRadicacion BETWEEN @fechaInicio AND @fechaFin
                                        AND cj.cveTipoCarpeta = 2 ) as totalCarpetas
                                    FROM
                                        tblcarpetasjudiciales cj
                                        INNER JOIN
                                        tbljuzgados j ON j.cveJuzgado = cj.cveJuzgado
                                         INNER JOIN
                                            tbldistritos d ON j.cveDistrito = d.cveDistrito
                                    WHERE
                                        cj.fechaRadicacion BETWEEN @fechaInicio AND @fechaFin
                                        AND cj.cveTipoCarpeta = 2
                                    GROUP BY j.cveJuzgado , YEAR(cj.fechaRadicacion) , MONTH(cj.fechaRadicacion)
                                    ORDER BY j.cveDistrito asc , Anio desc, Mes desc;"

            Dim cmd As New MySqlCommand(sql, Coneccion)
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin)
            Dim reader As MySqlDataReader = cmd.ExecuteReader

            While reader.Read()
                'Console.WriteLine(reader(0).ToString)
                datosLlena.Add(New InfoGen() With {
                    .Causas = reader("causas").ToString,
                    .IdJuzgado = reader("cveJuzgado").ToString,
                    .DesJuzgado = reader("desJuzgado").ToString,
                    .Anio = reader("Anio").ToString,
                    .Mes = reader("Mes").ToString,
                    .TotalCarpetas = reader("totalCarpetas").ToString
                                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception

        End Try
        Return datosLlena
    End Function



    <WebMethod>
    Public Shared Function DatosIndicador(ind As Int32, fechaF As String, idDisJuz As String, materia As String) As List(Of DatosIndicador)

        Dim datos As New List(Of DatosIndicador)()
        Dim cons As New Consultas()
        Try

            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(3)

            Dim sql As String = (cons.consulta.Item(ind))(1)
            sql = sql.Replace("@nomJuz", "'%" + materia + "%'")

            'Debug.WriteLine(sql)


            Dim cmd As New MySqlCommand(sql, Coneccion)
            cmd.Parameters.AddWithValue("@fechaF", fechaF)
            cmd.Parameters.AddWithValue("@idDisJuz", idDisJuz)

            Dim reader As MySqlDataReader = cmd.ExecuteReader

            If ind <> "1" Then
                While reader.Read()
                    datos.Add(New DatosIndicador() With {
                        .anio = reader("anio").ToString,
                        .mes = reader("mes").ToString,
                        .valor = reader("valor").ToString,
                        .total = reader("total").ToString
                    })
                End While
            Else
                While reader.Read()

                    datos.Add(New DatosIndicador() With {
                        .anio = reader("anio").ToString,
                        .mes = reader("mes").ToString,
                        .valor = reader("valor").ToString,
                        .total = ""
                    })

                End While
            End If

            HistorialIndicadores(ind, idDisJuz, materia, datos.Item(0).anio, datos.Item(0).mes, datos.Item(0).valor, datos.Item(0).total)

            Conexion.cerrar()

        Catch ex As Exception
            Debug.WriteLine("Error " + ex.ToString)

        End Try

        Return datos

    End Function

    <WebMethod>
    Public Shared Function DatosIndicadores(fechaF As String, idDisJuz As String, materia As String) As List(Of DatosIndicadores)

        Dim listDatos As New List(Of DatosIndicadores)()

        Try

            Dim cons As New Consultas()

            Dim consultas As IDictionaryEnumerator = (cons.consulta).GetEnumerator()
            Dim k As DictionaryEntry

            While consultas.MoveNext()
                k = CType(consultas.Current, DictionaryEntry)

                Dim Conexion As New Conexion()
                Dim Coneccion = Conexion.conexion_global(3)

                Dim datos As New List(Of DatosIndicador)()
                Dim sql As String = k.Value(1)
                If k.Key = 15 Then
                    Select Case materia
                        Case "civil"
                            sql = sql.Replace("@tipo", "'%c%'")
                        Case "familiar"
                            sql = sql.Replace("@tipo", "'%f%'")
                        Case "mercantil"
                            sql = sql.Replace("@tipo", "'%m%'")
                    End Select
                Else
                    sql = sql.Replace("@nomJuz", "'%" + materia + "%'")
                End If

                Dim cmd As New MySqlCommand(sql, Coneccion)
                cmd.Parameters.AddWithValue("@fechaF", fechaF)
                cmd.Parameters.AddWithValue("@idDisJuz", idDisJuz)

                Debug.WriteLine("query: " + sql)

                Dim reader As MySqlDataReader = cmd.ExecuteReader

                If k.Key <> 1 Then
                    While reader.Read()
                        datos.Add(New DatosIndicador() With {
                        .anio = reader("anio").ToString,
                        .mes = reader("mes").ToString,
                        .valor = reader("valor").ToString,
                        .total = reader("total").ToString
                    })
                    End While

                    listDatos.Add(New DatosIndicadores() With {
                        .id = (k.Key).ToString,
                        .nomIndicador = k.Value(0),
                        .datoIndicador = datos
                    })
                Else
                    While reader.Read()
                        datos.Add(New DatosIndicador() With {
                        .anio = reader("anio").ToString,
                        .mes = reader("mes").ToString,
                        .valor = reader("valor").ToString,
                        .total = ""
                    })
                    End While

                    listDatos.Add(New DatosIndicadores() With {
                        .id = (k.Key).ToString,
                        .nomIndicador = k.Value(0),
                        .datoIndicador = datos
                    })

                End If

                Conexion.cerrar()

            End While

        Catch ex As Exception
            Debug.WriteLine("Error " + ex.ToString)

        End Try

        Return listDatos

    End Function

    <WebMethod>
    Public Shared Function DatosIndicadorTabla(ind As Int32, fecha As String, distrito As String, materia As String) As List(Of DatosIndicadorTabla)

        Dim listaDatos As New List(Of DatosIndicadorTabla)()
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion
        Dim cmd As MySqlCommand
        Dim reader As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Conexion = New Conexion()
            Coneccion = Conexion.conexion_global(3)
            sqlquery = cons.consulta.Item(ind)(1)
            sqlquery = sqlquery.Replace("@tipo", "'%" + materia + "%'")
            cmd = New MySqlCommand(sqlquery, Coneccion)
            cmd.Parameters.AddWithValue("@fechaF", fecha)
            cmd.Parameters.AddWithValue("@idDisJuz", distrito)
            reader = cmd.ExecuteReader

            While reader.Read()
                listaDatos.Add(New DatosIndicadorTabla() With {
                    .anio = reader.GetString("anio"),
                    .mes = reader.GetString("mes"),
                    .valor = reader.GetString("valor"),
                    .total = reader.GetString("total"),
                    .juicios = DatosJuicios(ind, reader.GetString("fecha"), distrito, materia)
                })
            End While

            HistorialIndicadores(ind, distrito, materia, listaDatos.Item(0).anio, listaDatos.Item(0).mes, listaDatos.Item(0).valor, listaDatos.Item(0).total)

            reader.Close()
            Conexion.cerrar()

        Catch ex As Exception
            Debug.WriteLine("Error en Indicador tabla: " + ex.ToString)
        End Try

        Return listaDatos

    End Function

    <WebMethod>
    Public Shared Function DatosJuicios(ind As Int32, fecha As String, distrito As String, materia As String) As List(Of DatosJuicios)

        Dim listaJucios As New List(Of DatosJuicios)()
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion2
        Dim cmd As MySqlCommand
        Dim reader2 As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Conexion = New Conexion()
            Coneccion2 = Conexion.conexion_global(3)
            sqlquery = cons.consulta.Item(ind)(2)

            Debug.WriteLine(sqlquery)

            sqlquery = sqlquery.Replace("@tipo", "'%" + materia + "%'")
            cmd = New MySqlCommand(sqlquery, Coneccion2)
            cmd.Parameters.AddWithValue("@fechaF", fecha)
            cmd.Parameters.AddWithValue("@idDisJuz", distrito)

            reader2 = cmd.ExecuteReader

            While reader2.Read()

                listaJucios.Add(New DatosJuicios() With {
                    .juicio = reader2.GetString("juicio"),
                    .totaljuicios = reader2.GetString("totaljuicios"),
                    .valor = reader2.GetString("valor")
                })

            End While

            reader2.Close()
            Conexion.cerrar()

        Catch ex As Exception
            Debug.WriteLine("Error en tabla juicios: " + ex.ToString)
        End Try

        Return listaJucios

    End Function


    Public Shared Function HistorialIndicadores(ind As Int32, distrito As String, materia As String, anio As String, mes As String, resultado As String, total As String) As Boolean

        Dim listaJucios As New List(Of DatosJuicios)()
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion
        Dim cmd As MySqlCommand
        Dim reader As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Select Case materia
                Case "civil"
                    materia = "1"
                Case "c"
                    materia = "1"
                Case "familiar"
                    materia = "2"
                Case "f"
                    materia = "2"
                Case "penal"
                    materia = "3"
                Case "p"
                    materia = "3"
                Case "mercantil"
                    materia = "4"
                Case "m"
                    materia = "4"
            End Select

            total = IIf(total <> Nothing, total, Nothing)

            Dim fechacreacion As Date = Date.Today
            Dim dia As String = Format(fechacreacion, "yyyy-M")

            Dim fechafin As String = IIf(dia = (anio + "-" + mes), "DATE_FORMAT(NOW(),'%Y-%m-%d')", "LAST_DAY('" + anio + "-" + mes + "-01')")

            Conexion = New Conexion()
            Coneccion = Conexion.conexion_global(1)
            sqlquery = "SELECT cvecalculo AS clave FROM tblcalcsisind
                        WHERE cveindicador = @indicador  
                        AND cvemateria = @materia 
                        AND cvedistrito = @distrito 
                        AND anioconsulta = @anio
                        AND mesconsulta = @mes"

            cmd = New MySqlCommand(sqlquery, Coneccion)
            cmd.Parameters.AddWithValue("@indicador", ind)
            cmd.Parameters.AddWithValue("@materia", materia)
            cmd.Parameters.AddWithValue("@distrito", distrito)
            cmd.Parameters.AddWithValue("@anio", anio)
            cmd.Parameters.AddWithValue("@mes", mes)

            Dim clave As String = ""
            clave = Convert.ToString(cmd.ExecuteScalar)

            Conexion.cerrar()

            If clave <> "" Then

                If dia = (anio + "-" + mes) Then

                    Debug.WriteLine("Este es la cve del registro existente: " + clave)

                    Conexion = New Conexion()
                    Coneccion = Conexion.conexion_global(1)
                    sqlquery = "UPDATE tblcalcsisind
                            SET resultado = @resultado, 
                                total = @total,
                                calculo = ROUND(@calculo, 2),
                                fechareg = NOW(),
                                fechafin = " + fechafin + "    
                            WHERE cvecalculo = @clave;"

                    cmd = New MySqlCommand(sqlquery, Coneccion)
                    cmd.Parameters.AddWithValue("@clave", clave)
                    cmd.Parameters.AddWithValue("@resultado", resultado)
                    cmd.Parameters.AddWithValue("@total", total)
                    cmd.Parameters.AddWithValue("@calculo", Calculos(ind, resultado, total))

                    cmd.ExecuteNonQuery()

                    Debug.WriteLine("ACTUALIZO UN REGISTRO")

                    Conexion.cerrar()

                End If

            Else

                Conexion = New Conexion()
                Coneccion = Conexion.conexion_global(1)
                sqlquery = "INSERT INTO tblcalcsisind (cveindicador, cvemateria, cvedistrito, resultado, total, calculo, fechainicio, fechafin, anioconsulta, mesconsulta)
                            VALUES(@indicador, @materia, @distrito, @resultado, @total, ROUND(@calculo, 2), @fechainicio, " + fechafin + ", @anio, @mes);"

                cmd = New MySqlCommand(sqlquery, Coneccion)
                cmd.Parameters.AddWithValue("@indicador", ind)
                cmd.Parameters.AddWithValue("@materia", materia)
                cmd.Parameters.AddWithValue("@distrito", distrito)
                cmd.Parameters.AddWithValue("@resultado", resultado)
                cmd.Parameters.AddWithValue("@total", total)
                cmd.Parameters.AddWithValue("@calculo", Calculos(ind, resultado, total))
                cmd.Parameters.AddWithValue("@fechainicio", anio + "-" + mes + "-01")
                cmd.Parameters.AddWithValue("@anio", anio)
                cmd.Parameters.AddWithValue("@mes", mes)

                cmd.ExecuteNonQuery()

                Debug.WriteLine("INSERTO UN NUEVO REGISTRO")

                Conexion.cerrar()

            End If

        Catch ex As Exception

            Debug.WriteLine("Error en Historial: " + ex.ToString)

        End Try

        Return True

    End Function


    Public Shared Function Calculos(ind As Int32, resultado As String, total As String) As Double

        If Regex.IsMatch(Convert.ToString(ind), "^(1)$") Then
            Return Nothing
        Else
            Return Math.Round((Long.Parse(Trim(resultado)) / Long.Parse(Trim(total)) * 100), 2)
        End If
    End Function


    <WebMethod>
    Public Shared Function ExitUser() As Boolean
        HttpContext.Current.Session.RemoveAll()
        HttpContext.Current.Session.Abandon()
        Return True

    End Function

    <WebMethod>
    Public Shared Function VerificaLogin() As String
        If HttpContext.Current.Session("Sesion") Is Nothing Then
            ExitUser()
            Return "no"
        Else
            Return "si"
        End If
    End Function

End Class