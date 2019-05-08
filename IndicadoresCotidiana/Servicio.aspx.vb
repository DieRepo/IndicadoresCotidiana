Imports System.Web.UI
Imports System.Web.Services
Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports IronPdf
Imports System.Web.Script.Serialization

Public Class Servicio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <WebMethod>
    Public Shared Function ObtenDistritos() As List(Of Distritos)
        Dim datosLlena As New List(Of Distritos)()
        Try

            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(1)

            Dim sql As String = " select * from estadistica.tbldistritos;"
            Dim cmd As New MySqlCommand(sql, Coneccion)
            Dim reader As MySqlDataReader = cmd.ExecuteReader

            While reader.Read()

                datosLlena.Add(New Distritos() With {
                    .idDistrito = reader("cveDistrito").ToString,
                    .Descripcion = reader("nombre").ToString
                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception
            Debug.WriteLine("Error en funcion ObtenDistritos: " + ex.ToString)
        End Try
        Return datosLlena
    End Function

    <WebMethod>
    Public Shared Function Obtencalculos(idIndicador As Integer, Anio As Integer, Semana As Integer) As List(Of CalculoSemanas)
        Dim datosLlena As New List(Of CalculoSemanas)()
        Try
            ' Console.WriteLine()


            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(1)



            Dim sql As String = "SELECT *,DATE_FORMAT(fecha_inicio_reporte,'%d/%m/%Y') AS fecha_inicio_reporte1, DATE_FORMAT(fecha_fin_reporte,'%d/%m/%Y') AS fecha_fin_reporte1  
            FROM indicadores_pjem_cotidiana.calculo_indicadores where semana = @Semana and idIndicador = @idIndicador and activo=1 and Anio = @Anio;"
            Dim cmd As New MySqlCommand(sql, Coneccion)
            cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
            cmd.Parameters.AddWithValue("@Anio", Anio)
            cmd.Parameters.AddWithValue("@Semana", Semana)
            Dim reader As MySqlDataReader = cmd.ExecuteReader



            While reader.Read()
                'Console.WriteLine(reader(0).ToString)
                datosLlena.Add(New CalculoSemanas() With {
                    .idIndicador = reader("idIndicador").ToString,
                    .idJuzgado = reader("idJuzgado").ToString,
                    .DescripcionJuzgado = reader("Nombre_Juzgado").ToString,
                    .Semana = reader("semana").ToString,
                     .Fecha1 = reader("fecha_Inicio").ToString,
                    .Fecha2 = reader("fecha_Fin").ToString,
                    .Fecha1R = reader("fecha_inicio_reporte1").ToString,
                    .Fecha2R = reader("fecha_fin_reporte1").ToString,
                    .Calculo = reader("calculo").ToString
                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception

        End Try
        Return datosLlena
    End Function

    <WebMethod>
    Public Shared Function ObtenDatosGrafica(idIndicador As Integer, idJuzgado As Integer, Anio As Integer, Semana As Integer) As List(Of CalculoSemanasIndicador)
        Dim datosLlena As New List(Of CalculoSemanasIndicador)()
        Try
            ' Console.WriteLine()

            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(1)

            Dim sql As String = "SELECT " &
                                " * " &
                                " FROM " &
                                " indicadores_pjem_cotidiana.calculo_indicadores " &
                                " WHERE " &
                                " idJuzgado = @idJuzgado" &
                                " AND idIndicador = @idIndicador" &
                                " AND anio <= @Anio " &
                                "  AND semana <= @Semana" &
                                " order by semana limit 4;"
            Dim cmd As New MySqlCommand(sql, Coneccion)
            cmd.Parameters.AddWithValue("@idIndicador", idIndicador)
            cmd.Parameters.AddWithValue("@idJuzgado", idJuzgado)
            cmd.Parameters.AddWithValue("@Anio", Anio)
            cmd.Parameters.AddWithValue("@Semana", Semana)
            Dim reader As MySqlDataReader = cmd.ExecuteReader

            While reader.Read()
                'Console.WriteLine(reader(0).ToString)
                datosLlena.Add(New CalculoSemanasIndicador() With {
                    .idIndicador = reader("idIndicador").ToString,
                    .idJuzgado = reader("idJuzgado").ToString,
                    .DescripcionJuzgado = reader("Nombre_Juzgado").ToString,
                    .Semana = reader("semana").ToString,
                    .Anio = reader("anio").ToString,
                    .Fecha1 = reader("fecha_Inicio").ToString,
                    .Fecha2 = reader("fecha_Fin").ToString,
                    .Fecha1R = reader("fecha_inicio_reporte").ToString,
                    .Fecha2R = reader("fecha_fin_reporte").ToString,
                    .Calculo = reader("calculo").ToString
                    })
            End While

            Conexion.cerrar()

        Catch ex As Exception

        End Try
        Return datosLlena
    End Function

    <WebMethod>
    Public Shared Function ObtenJuzgados(cveDistrito As String, materia As String) As List(Of Juzgado)

        Dim juzgados As New List(Of Juzgado)

        Dim Conexion As New Conexion()
        Dim Coneccion = Conexion.conexion_global(1)

        Dim query As String = "SELECT cveAdscripcion,NomJuzgado FROM estadistica.tbljuzgados 
                            WHERE cveDistrito = " + cveDistrito + " and upper(NomJuzgado) like '%" + materia + "%' and upper(NomJuzgado) not like '%sala%' and activo = 1;"
        Dim cmd As New MySqlCommand(query, Coneccion)
        cmd.CommandTimeout = 600

        Dim r As MySqlDataReader = cmd.ExecuteReader

        While r.Read()
            juzgados.Add(New Juzgado() With {
                .CveJuzgado = r.GetString("cveAdscripcion"),
                .NomJuzgado = r.GetString("NomJuzgado")
                         })
        End While

        Return juzgados
    End Function


    <WebMethod>
    Public Shared Function DatosIndicador(ind As Int32, fechaF As String, idDisJuz As String, materia As String, cveJuzgado As String) As List(Of DatosIndicador)

        Dim datos As New List(Of DatosIndicador)()
        Dim cons As New Consultas()
        Try

            Dim Conexion As New Conexion()
            Dim Coneccion = Conexion.conexion_global(1)

            Dim sql As String = (cons.consulta.Item(ind))(1)
            sql = sql.Replace("@nomJuz", "'%" + materia + "%'")
            sql = sql.Replace("@cveJuzgado", "'%" + cveJuzgado + "%'")

            Debug.WriteLine(sql)
            Debug.WriteLine(fechaF)
            Debug.WriteLine(idDisJuz)

            Dim cmd As New MySqlCommand(sql, Coneccion)
            cmd.CommandTimeout = 6000
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
    Public Shared Function DatosIndicadores(fechaF As String, idDisJuz As String, materia As String, cveJuzgado As String) As List(Of DatosIndicadores)

        Dim listDatos As New List(Of DatosIndicadores)()

        Try

            Dim cons As New Consultas()

            Dim consultas As IDictionaryEnumerator = (cons.consulta).GetEnumerator()
            Dim k As DictionaryEntry

            While consultas.MoveNext()
                k = CType(consultas.Current, DictionaryEntry)

                Dim Conexion As New Conexion()
                Dim Coneccion = Conexion.conexion_global(1)

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

                sql = sql.Replace("@cveJuzgado", "'%" + cveJuzgado + "%'")

                Dim cmd As New MySqlCommand(sql, Coneccion)
                cmd.CommandTimeout = 6000
                cmd.Parameters.AddWithValue("@fechaF", fechaF)
                cmd.Parameters.AddWithValue("@idDisJuz", idDisJuz)

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
    Public Shared Function DatosIndicadorTabla(ind As Int32, fecha As String, distrito As String, materia As String, cveJuzgado As String) As List(Of DatosIndicadorTabla)

        Dim listaDatos As New List(Of DatosIndicadorTabla)()
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion
        Dim cmd As MySqlCommand
        Dim reader As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Conexion = New Conexion()
            Coneccion = Conexion.conexion_global(1)
            sqlquery = cons.consulta.Item(ind)(1)
            sqlquery = sqlquery.Replace("@tipo", "'%" + materia + "%'")
            sqlquery = sqlquery.Replace("@cveJuzgado", "'%" + cveJuzgado + "%'")
            cmd = New MySqlCommand(sqlquery, Coneccion)
            cmd.CommandTimeout = 6000
            cmd.Parameters.AddWithValue("@fechaF", fecha)
            cmd.Parameters.AddWithValue("@idDisJuz", distrito)
            reader = cmd.ExecuteReader

            While reader.Read()
                listaDatos.Add(New DatosIndicadorTabla() With {
                    .anio = reader.GetString("anio"),
                    .mes = reader.GetString("mes"),
                    .valor = reader.GetString("valor"),
                    .total = reader.GetString("total"),
                    .juicios = DatosJuicios(ind, reader.GetString("fecha"), distrito, materia, cveJuzgado
                    )
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
    Public Shared Function DatosIndicadorTermino(ind As Int32, fecha As String, distrito As String, materia As String, cveJuzgado As String) As List(Of DatosIndicadorTermino)

        Dim listaDatos As New List(Of DatosIndicadorTermino)()
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion
        Dim cmd As MySqlCommand
        Dim reader As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Conexion = New Conexion()
            Coneccion = Conexion.conexion_global(1)
            sqlquery = cons.consulta.Item(ind)(1)
            sqlquery = sqlquery.Replace("@nomJuz", "'%" + materia + "%'")
            sqlquery = sqlquery.Replace("@cveJuzgado", "'%" + cveJuzgado + "%'")
            cmd = New MySqlCommand(sqlquery, Coneccion)
            cmd.CommandTimeout = 6000
            cmd.Parameters.AddWithValue("@fechaF", fecha)
            cmd.Parameters.AddWithValue("@idDisJuz", distrito)
            reader = cmd.ExecuteReader

            While reader.Read()
                listaDatos.Add(New DatosIndicadorTermino() With {
                    .anio = reader.GetString("anio"),
                    .mes = reader.GetString("mes"),
                    .valor = reader.GetString("valor"),
                    .total = reader.GetString("total"),
                    .resoluciones = DatosResoluciones(ind, reader.GetString("fecha"), distrito, materia, cveJuzgado
                    )
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
    Public Shared Function DatosIndicadorTramite(ind As Int32, fecha As String, distrito As String, materia As String, cveJuzgado As String) As ArrayList

        Dim listaDatos As New ArrayList
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion
        Dim cmd As MySqlCommand
        Dim reader As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Conexion = New Conexion()
            Coneccion = Conexion.conexion_global(1)
            sqlquery = cons.consulta.Item(ind)(1)
            sqlquery = sqlquery.Replace("@nomJuz", "'%" + materia + "%'")
            sqlquery = sqlquery.Replace("@cveJuzgado", "'%" + cveJuzgado + "%'")
            cmd = New MySqlCommand(sqlquery, Coneccion)
            cmd.CommandTimeout = 6000
            cmd.Parameters.AddWithValue("@fechaF", fecha)
            cmd.Parameters.AddWithValue("@idDisJuz", distrito)
            reader = cmd.ExecuteReader

            While reader.Read()
                listaDatos.Add(New DatosIndicadorTramite() With {
                    .anio = reader.GetString("anio"),
                    .mes = reader.GetString("mes"),
                    .valor = reader.GetString("valor"),
                    .total = reader.GetString("total"),
                    .tramites = DatosTramites(ind, reader.GetString("fecha"), distrito, materia, cveJuzgado
                    )
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
    Public Shared Function DatosJuicios(ind As Int32, fecha As String, distrito As String, materia As String, cveJuzgado As String) As List(Of DatosJuicios)

        Dim listaJucios As New List(Of DatosJuicios)()
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion2
        Dim cmd As MySqlCommand
        Dim reader2 As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Conexion = New Conexion()
            Coneccion2 = Conexion.conexion_global(1)
            sqlquery = cons.consulta.Item(ind)(2)

            Debug.WriteLine(sqlquery)

            sqlquery = sqlquery.Replace("@tipo", "'%" + materia + "%'")
            sqlquery = sqlquery.Replace("@cveJuzgado", "'%" + cveJuzgado + "%'")
            cmd = New MySqlCommand(sqlquery, Coneccion2)
            cmd.CommandTimeout = 6000
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


    <WebMethod>
    Public Shared Function DatosResoluciones(ind As Int32, fecha As String, distrito As String, materia As String, cveJuzgado As String) As List(Of DatosResoluciones)

        Dim ListaRes As New List(Of DatosResoluciones)()
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion2
        Dim cmd As MySqlCommand
        Dim reader2 As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Conexion = New Conexion()
            Coneccion2 = Conexion.conexion_global(1)
            sqlquery = cons.consulta.Item(ind)(2)

            'Debug.WriteLine(sqlquery)

            sqlquery = sqlquery.Replace("@nomJuz", "'%" + materia + "%'")
            sqlquery = sqlquery.Replace("@cveJuzgado", "'%" + cveJuzgado + "%'")
            cmd = New MySqlCommand(sqlquery, Coneccion2)
            cmd.CommandTimeout = 6000
            cmd.Parameters.AddWithValue("@fechaF", fecha)
            cmd.Parameters.AddWithValue("@idDisJuz", distrito)

            reader2 = cmd.ExecuteReader

            While reader2.Read()

                ListaRes.Add(New DatosResoluciones() With {
                    .resolucion = reader2.GetString("Resolucion"),
                    .total = reader2.GetString("Total")
                })

            End While

            reader2.Close()
            Conexion.cerrar()

        Catch ex As Exception
            Debug.WriteLine("Error en tabla juicios: " + ex.ToString)
        End Try

        Return ListaRes

    End Function

    <WebMethod>
    Public Shared Function DatosTramites(ind As Int32, fecha As String, distrito As String, materia As String, cveJuzgado As String) As List(Of DatosTramites)

        Dim ListaRes As New List(Of DatosTramites)()
        Dim cons As New Consultas()
        Dim Conexion As Conexion
        Dim Coneccion2
        Dim cmd As MySqlCommand
        Dim reader2 As MySqlDataReader
        Dim sqlquery As String = ""

        Try

            Conexion = New Conexion()
            Coneccion2 = Conexion.conexion_global(1)
            sqlquery = cons.consulta.Item(ind)(2)

            'Debug.WriteLine(sqlquery)

            sqlquery = sqlquery.Replace("@materia", "'%" + materia + "%'")
            sqlquery = sqlquery.Replace("@cveAdsc", "'%" + cveJuzgado + "%'")
            cmd = New MySqlCommand(sqlquery, Coneccion2)
            cmd.CommandTimeout = 6000
            cmd.Parameters.AddWithValue("@fecha", fecha)
            cmd.Parameters.AddWithValue("@cveDistrito", distrito)

            reader2 = cmd.ExecuteReader

            While reader2.Read()

                ListaRes.Add(New DatosTramites() With {
                    .expediente = reader2.GetString("expediente") + "/" + reader2.GetString("anio"),
                    .dias = reader2.GetString("dias"),
                    .fecharad = reader2.GetString("fecharad"),
                    .fechacon = reader2.GetString("fechacon"),
                    .juicio = reader2.GetString("juicio"),
                    .juzgado = reader2.GetString("juzgado")
                })

            End While

            reader2.Close()
            Conexion.cerrar()

        Catch ex As Exception
            Debug.WriteLine("Error en tabla tramites: " + ex.ToString)
        End Try

        Return ListaRes

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
            sqlquery = "SELECT cvecalculo AS clave FROM indicadores_pjem_cotidiana.tblcalcsisind
                        WHERE cveindicador = @indicador  
                        AND cvemateria = @materia 
                        AND cvedistrito = @distrito 
                        AND anioconsulta = @anio
                        AND mesconsulta = @mes"

            cmd = New MySqlCommand(sqlquery, Coneccion)
            cmd.CommandTimeout = 6000
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
                    sqlquery = "UPDATE indicadores_pjem_cotidiana.tblcalcsisind
                            SET resultado = @resultado, 
                                total = @total,
                                calculo = ROUND(@calculo, 2),
                                fechareg = NOW(),
                                fechafin = " + fechafin + "    
                            WHERE cvecalculo = @clave;"

                    cmd = New MySqlCommand(sqlquery, Coneccion)
                    cmd.CommandTimeout = 10
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
                sqlquery = "INSERT INTO indicadores_pjem_cotidiana.tblcalcsisind (cveindicador, cvemateria, cvedistrito, resultado, total, calculo, fechainicio, fechafin, anioconsulta, mesconsulta)
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