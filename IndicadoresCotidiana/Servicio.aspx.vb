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
        Dim Coneccion = Conexion.conexion_global(2)

        Dim query As String = "SELECT cveJuzgadoSEJ, NomJuzgado FROM `indicadores_pjem_cotidiana`.tbljuzgados 
                            WHERE cveDistrito = " + cveDistrito + " AND cveMateria LIKE '" + materia + "';"
        Dim cmd As New MySqlCommand(query, Coneccion)
        cmd.CommandTimeout = 600

        Dim r As MySqlDataReader = cmd.ExecuteReader

        While r.Read()
            juzgados.Add(New Juzgado() With {
                .CveJuzgado = r.GetString("cveJuzgadoSEJ"),
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

            'Debug.WriteLine(sql)
            'Debug.WriteLine(fechaF)
            'Debug.WriteLine(idDisJuz)

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
                    .juicios = DatosJuicios(ind, reader.GetString("fecha"), distrito, materia, cveJuzgado)
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

            'Debug.WriteLine(sqlquery)

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

    <WebMethod>
    Public Shared Function Construye(mat As String) As String

        Dim combos As String = ""

        If mat = "1" Or mat = "0" Then
            combos = combos + " <option value='1' selected>Civil</option>"
        End If

        If mat = "2" Or mat = "0" Then
            combos = combos + " <option value='2' selected>Familiar</option>"
        End If

        If mat = "3" Or mat = "0" Then
            combos = combos + " <option value='3' selected>Mercantil</option>"
        End If

        If mat = "5" Or mat = "0" Then
            combos = combos + " <option value='5' selected>Mixto</option>"
        End If

        Return combos

    End Function

    ' CODIGO INDICADORES DE GESTION 

    <WebMethod>
    Public Shared Function IndicadorGestion(id As Integer, anio As String, mes As String, cveDistrito As String, materia As String, cveJuzgado As Integer) As List(Of DatosGestion)

        Dim cg As New ConsultasGestion()
        Dim cj As New CambioJuzgados()
        Dim l As New List(Of DatosGestion)()

        If Now().ToString("yyyy-M") = (anio + "-" + mes) Then

            Dim Consulta As String = (cg.consulta.Item(id))(1)

            Dim Fecha As New Date(anio, mes, 1, 0, 0, 0)
            Dim FechaIni = (Fecha.AddMonths(-3)).ToString("yyyy-MM-dd HH:mm:ss")
            'Dim FechaIni = Fecha.ToString("yyyy-MM-dd HH:mm:ss")
            Dim FechaFin = (Fecha.AddMonths(1).AddSeconds(-1)).ToString("yyyy-MM-dd HH:mm:ss")

            Select Case materia
                Case 1
                    Consulta = Consulta.Replace("@materia", "(j.desJuz like '%Civil%' or j.desJuz like '%Menor%' or j.desJuz like '%C.M.%')")
                Case 2
                    Consulta = Consulta.Replace("@materia", "(j.desJuz like '%Familiar%' or j.desJuz like '%Adopcion%')")
                Case 3
                    Consulta = Consulta.Replace("@materia", "(j.desJuz like '%Mercantil')")
                Case 4
                    Consulta = Consulta.Replace("@materia", "(j.desJuz like '%Mixto')")
            End Select

            If cveJuzgado > 0 Then
                Consulta = Consulta.Replace("@cveJuzgado", "'" + (cj.juzgado.Item(cveJuzgado)).ToString + "'")
            Else
                Consulta = Consulta.Replace("@cveJuzgado", "'%%'")
            End If

            Dim Conexion As New Conexion()

            Try

                Dim c = Conexion.conexion_global(1)

                Dim cmd As New MySqlCommand(Consulta, c)
                cmd.CommandTimeout = 6000
                cmd.Parameters.AddWithValue("@fechaIni", FechaIni)
                cmd.Parameters.AddWithValue("@fechaFin", FechaFin)
                cmd.Parameters.AddWithValue("@cveDistrito", cveDistrito)

                Dim r As MySqlDataReader = cmd.ExecuteReader

                While r.Read

                    l.Add(New DatosGestion() With {
                         .Anio = r.GetString("anio"),
                         .Mes = r.GetString("mes"),
                         .Fecha = r.GetString("anio") + "-" + r.GetString("mes").PadLeft(2, "0"),
                         .Valor = r.GetString("valor1"),
                         .Total = r.GetString("valor2"),
                         .Calculo = DameCalculoGestion(id, r.GetInt64("valor1"), r.GetInt64("valor2")),
                         .Semaforo = DameSemaforo(id, r.GetInt64("valor1"), r.GetInt64("valor2"))
                    })

                End While

                Conexion.cerrar()

                l = ValidadorGestion(l, anio, mes)
                Return l

            Catch ex As MySqlException

                Debug.WriteLine("Error al procesar los datos de gestion: " + ex.Message)

                Conexion.cerrar()
                Return l

            End Try

        Else
            ' ENTRA A ATECEDENTESCARPETAS 

            Dim Consulta As String = "SELECT ai.anio anio, ai.mes mes, sum(ai.valor1) valor1, sum(ai.valor2) valor2, round(avg(ai.calculo),2) calculo
                                    FROM indicadores_pjem_cotidiana.tblantecedesindicadores ai
                                    INNER JOIN indicadores_pjem_cotidiana.tbljuzgados j on j.cveJuzgadoGES = ai.cveJuzgado
                                    WHERE
                                    ai.fechaCorte between @fechaIni AND @fechaFin
                                    AND ai.cveTipoIndicador = 2                                    
                                    AND ai.cveIndicador = @cveIndicador
                                    AND j.cveMateria = @cveMateria
                                    AND ai.cveJuzgado LIKE @cveJuzgado
                                    AND j.cveDistrito = @cveDistrito
                                    GROUP BY ai.anio, ai.mes
                                    ORDER BY ai.anio desc, ai.mes desc;"

            Dim Fecha As New Date(anio, mes, 1, 0, 0, 0)
            'Dim FechaIni = (Fecha.AddMonths(-3)).ToString("yyyy-MM-dd")
            Dim FechaIni = (New Date(anio, 1, 1, 0, 0, 0)).ToString("yyyy-MM-dd")
            Dim FechaFin = (Fecha.AddMonths(1).AddSeconds(-1)).ToString("yyyy-MM-dd")

            If cveJuzgado > 0 Then

                Dim val As String = ""

                If (cj.juzgado.Item(cveJuzgado) <> Nothing) Then

                    val = cj.juzgado.Item(cveJuzgado).ToString()
                Else
                    val = "0"
                End If

                'IIf(cj.juzgado.Item(cveJuzgado) <> Nothing, cj.juzgado.Item(cveJuzgado).ToString()

                Consulta = Consulta.Replace("@cveJuzgado", "'" + val + "'")
            Else
                Consulta = Consulta.Replace("@cveJuzgado", "'%%'")
            End If

            Dim Conexion As New Conexion()

            Try

                Dim c = Conexion.conexion_global(2)

                Dim cmd As New MySqlCommand(Consulta, c)
                cmd.CommandTimeout = 6000
                cmd.Parameters.AddWithValue("@fechaIni", FechaIni)
                cmd.Parameters.AddWithValue("@fechaFin", FechaFin)
                cmd.Parameters.AddWithValue("@cveIndicador", id)
                cmd.Parameters.AddWithValue("@cveDistrito", cveDistrito)
                cmd.Parameters.AddWithValue("@cveMateria", materia)


                Dim r As MySqlDataReader = cmd.ExecuteReader

                While r.Read

                    l.Add(New DatosGestion() With {
                         .Anio = r.GetString("anio"),
                         .Mes = r.GetString("mes"),
                         .Fecha = r.GetString("anio") + "-" + r.GetString("mes").PadLeft(2, "0"),
                         .Valor = r.GetString("valor1"),
                         .Total = r.GetString("valor2"),
                         .Calculo = DameCalculoGestion(id, r.GetInt64("valor1"), r.GetInt64("valor2")),
                        .Semaforo = DameSemaforo(id, r.GetInt64("valor1"), r.GetInt64("valor2"))
                    })

                End While

                Conexion.cerrar()

                l = ValidadorGestion(l, anio, mes)
                Return l

            Catch ex As MySqlException

                Debug.WriteLine("Error al procesar los datos de gestion: " + ex.Message)

                Conexion.cerrar()
                Return l

            End Try

        End If
    End Function

    <WebMethod>
    Public Shared Function IndicadoresGestionGeneral(anio As String, mes As String, cveDistrito As String, materia As String, cveJuzgado As Integer) As List(Of GeneralGestion)

        Dim cg As New ConsultasGestion()
        Dim cj As New CambioJuzgados()
        Dim lt As New List(Of GeneralGestion)()

        If Now().ToString("yyyy-M") = (anio + "-" + mes) Then

            For i As Integer = 1 To 4

                Dim l As New List(Of DatosGestion)()

                Dim Consulta As String = (cg.consulta.Item(i))(1)

                Dim Fecha As New Date(anio, mes, 1, 0, 0, 0)
                Dim FechaIni = (Fecha.AddMonths(-3)).ToString("yyyy-MM-dd")
                'Dim FechaIni = Fecha.ToString("yyyy-MM-dd")
                Dim FechaFin = (Fecha.AddMonths(1).AddSeconds(-1)).ToString("yyyy-MM-dd")

                If cveJuzgado > 0 Then
                    Consulta = Consulta.Replace("@cveJuzgado", "'" + (cj.juzgado.Item(cveJuzgado)).ToString + "'")
                Else
                    Consulta = Consulta.Replace("@cveJuzgado", "'%%'")
                End If

                Select Case materia
                    Case 1
                        Consulta = Consulta.Replace("@materia", "(j.desJuz like '%Civil%' or j.desJuz like '%Menor%' or j.desJuz like '%C.M.%')")
                    Case 2
                        Consulta = Consulta.Replace("@materia", "(j.desJuz like '%Familiar%' or j.desJuz like '%Adopcion%')")
                    Case 3
                        Consulta = Consulta.Replace("@materia", "(j.desJuz like '%Mercantil')")
                    Case 5
                        Consulta = Consulta.Replace("@materia", "(j.desJuz like '%Mixto')")
                End Select

                Dim Conexion As New Conexion()

                Try

                    Dim c = Conexion.conexion_global(1)

                    Dim cmd As New MySqlCommand(Consulta, c)
                    cmd.CommandTimeout = 6000
                    cmd.Parameters.AddWithValue("@fechaIni", FechaIni)
                    cmd.Parameters.AddWithValue("@fechaFin", FechaFin)
                    cmd.Parameters.AddWithValue("@cveIndicador", i)
                    cmd.Parameters.AddWithValue("@cveDistrito", cveDistrito)


                    Dim r As MySqlDataReader = cmd.ExecuteReader

                    While r.Read

                        l.Add(New DatosGestion() With {
                             .Anio = r.GetString("anio"),
                             .Mes = r.GetString("mes"),
                             .Fecha = r.GetString("anio") + "-" + r.GetString("mes").PadLeft(2, "0"),
                             .Valor = r.GetString("valor1"),
                             .Total = r.GetString("valor2"),
                             .Calculo = DameCalculoGestion(i, r.GetInt64("valor1"), r.GetInt64("valor2")),
                            .Semaforo = DameSemaforo(i, r.GetInt64("valor1"), r.GetInt64("valor2"))
                        })

                    End While

                    Conexion.cerrar()

                Catch ex As MySqlException

                    Debug.WriteLine("Error al procesar los datos de gestion: " + ex.Message)
                    Conexion.cerrar()

                End Try
                l = ValidadorGestion(l, anio, mes)

                lt.Add(New GeneralGestion() With {
                    .IdGestion = i,
                    .Datos = l
                })

            Next

        Else

            ' Antecedes carpetas

            Dim Fecha As New Date(anio, mes, 1, 0, 0, 0)
            'Dim FechaIni = (Fecha.AddMonths(-3)).ToString("yyyy-MM-dd")
            Dim FechaIni = (New Date(anio, 1, 1, 0, 0, 0)).ToString("yyyy-MM-dd")
            Dim FechaFin = (Fecha.AddMonths(1).AddSeconds(-1)).ToString("yyyy-MM-dd")


            For i As Integer = 1 To 4

                Dim l As New List(Of DatosGestion)()

                Dim Consulta As String = "SELECT ai.anio anio, ai.mes mes, sum(ai.valor1) valor1, sum(ai.valor2) valor2, round(AVG(ai.calculo),2) calculo
                                    FROM indicadores_pjem_cotidiana.tblantecedesindicadores ai
                                    INNER JOIN indicadores_pjem_cotidiana.tbljuzgados j on j.cveJuzgadoGES = ai.cveJuzgado
                                    WHERE
                                    ai.fechaCorte between @fechaIni AND @fechaFin
                                    AND ai.cveTipoIndicador = 2                                    
                                    AND ai.cveIndicador = @cveIndicador
                                    AND j.cveMateria = @cveMateria
                                    AND ai.cveJuzgado LIKE @cveJuzgado
                                    AND j.cveDistrito = @cveDistrito
                                    GROUP BY ai.anio, ai.mes
                                    ORDER BY ai.anio desc, ai.mes desc;"


                If cveJuzgado > 0 Then
                    Consulta = Consulta.Replace("@cveJuzgado", "'" + (cj.juzgado.Item(cveJuzgado)).ToString + "'")
                Else
                    Consulta = Consulta.Replace("@cveJuzgado", "'%%'")
                End If

                Dim Conexion As New Conexion()

                Try

                    Dim c = Conexion.conexion_global(2)

                    Dim cmd As New MySqlCommand(Consulta, c)
                    cmd.CommandTimeout = 6000
                    cmd.Parameters.AddWithValue("@fechaIni", FechaIni)
                    cmd.Parameters.AddWithValue("@fechaFin", FechaFin)
                    cmd.Parameters.AddWithValue("@cveIndicador", i)
                    cmd.Parameters.AddWithValue("@cveDistrito", cveDistrito)
                    cmd.Parameters.AddWithValue("@cveMateria", materia)


                    Dim r As MySqlDataReader = cmd.ExecuteReader

                    While r.Read

                        l.Add(New DatosGestion() With {
                             .Anio = r.GetString("anio"),
                             .Mes = r.GetString("mes"),
                             .Fecha = r.GetString("anio") + "-" + r.GetString("mes").PadLeft(2, "0"),
                             .Valor = r.GetString("valor1"),
                             .Total = r.GetString("valor2"),
                             .Calculo = DameCalculoGestion(i, r.GetInt64("valor1"), r.GetInt64("valor2")),
                             .Semaforo = DameSemaforo(i, r.GetInt64("valor1"), r.GetInt64("valor2"))
                        })

                    End While

                    Conexion.cerrar()

                    l = ValidadorGestion(l, anio, mes)

                    If materia = 1 And i = 4 Then

                    Else
                        lt.Add(New GeneralGestion() With {
                        .IdGestion = i,
                        .Datos = l
                        })
                    End If




                Catch ex As MySqlException

                    Debug.WriteLine("Error al procesar los datos de gestion: " + ex.Message)
                    Conexion.cerrar()

                End Try



            Next

        End If

        Return lt

    End Function

    Public Shared Function DameCalculoGestion(id As String, valor As Int64, total As Int64) As String

        Dim Calc As String = ""

        Select Case id
            Case "2", "3"
                Calc = Math.Round((valor / total), 2)
            Case "1", "4"
                Calc = Format((valor / total), "Percent")
        End Select

        Return Calc

    End Function

    Public Shared Function DameSemaforo(id As String, valor As Int64, total As Int64) As String

        Dim Verde As String = "#65baaf"
        Dim Amarillo As String = "#ffb259"
        Dim Rojo As String = "#9d2424"
        Dim Sem As String

        Select Case id

            Case "1"
                If (valor / total) * 100 >= 0 And (valor / total) * 100 < 4 Then
                    Sem = Verde
                ElseIf (valor / total) * 100 >= 4 And (valor / total) * 100 < 5 Then
                    Sem = Amarillo
                ElseIf (valor / total) * 100 >= 5 Then
                    Sem = Rojo
                End If

            Case "2"
                If (valor / total) >= 0 And (valor / total) <= 24 Then
                    Sem = Verde
                Else
                    Sem = Rojo
                End If

            Case "3"
                If (valor / total) >= 0 And (valor / total) <= 20 Then
                    Sem = Verde
                ElseIf (valor / total) > 20 And (valor / total) < 24 Then
                    Sem = Amarillo
                ElseIf (valor / total) >= 24 Then
                    Sem = Rojo
                End If
            Case "4"
                If ((valor / total) * 100) >= 95 Then
                    Sem = Verde
                Else
                    Sem = Rojo
                End If
        End Select

        Return Sem

    End Function

    Public Shared Function ValidadorGestion(l As List(Of DatosGestion), anio As String, mes As String) As List(Of DatosGestion)

        Dim Fecha As New Date(anio, mes, 1, 0, 0, 0)

        Dim nl As New List(Of DatosGestion)
        Dim w As Integer = 0

        While w < mes

            Dim ban As Boolean = True

            For Each g As DatosGestion In l

                If (Fecha.AddMonths(-w)).ToString("yyyy-MM") = g.Fecha Then
                    nl.Add(g)
                    ban = False
                    Exit For
                End If

            Next

            If ban Then

                nl.Add(New DatosGestion() With {
                  .Anio = Fecha.AddMonths(-w).ToString("yyyy"),
                  .Mes = Fecha.AddMonths(-w).Month,
                  .Valor = 0,
                  .Total = 0,
                  .Calculo = 0,
                  .Fecha = Fecha.AddMonths(-w).ToString("yyyy-MM"),
                  .Semaforo = 0
                })

            End If

            w = w + 1

        End While

        Return nl
    End Function

    ' CODIGO NUEVO INDICADORES COTIDIANA

    <WebMethod>
    Public Shared Function IndicadorCotidiana(id As Integer, anio As String, mes As String, cveDistrito As String, materia As String, cveJuzgado As Integer) As List(Of DatosCotidiana)


        Dim l As New List(Of DatosCotidiana)()
        Dim cc As New Consultas()

        'If (Now().ToString("yyyy-M").Contains(anio + "-" + mes)) Then
        If (1 = 1) Then


            Dim Consulta As String = (cc.consulta.Item(id))(1)

            Dim Fecha As New Date(anio, mes, 1, 0, 0, 0)
            Dim FechaIni = (Fecha.AddMonths(-3)).ToString("yyyy-MM-dd")
            Dim FechaFin = (Fecha.AddMonths(1).AddSeconds(-1)).ToString("yyyy-MM-dd")

            Select Case materia
                Case 1
                    Consulta = Consulta.Replace("@materia", "(j.NomJuzgado like '%Civil%' or j.NomJuzgado like '%Menor%' or j.NomJuzgado like '%Mixto%'  or j.NomJuzgado like '%C.M.%')")
                Case 2
                    Consulta = Consulta.Replace("@materia", "(j.NomJuzgado like '%Familiar%' or j.NomJuzgado like '%Adopcion%')")
                Case 3
                    Consulta = Consulta.Replace("@materia", "(j.NomJuzgado like '%Mercantil')")
            End Select

            If cveJuzgado > 0 Then
                Consulta = Consulta.Replace("@cveJuzgado", "'" + cveJuzgado.ToString + "'")
            Else
                Consulta = Consulta.Replace("@cveJuzgado", "'%%'")
            End If

            Dim Conexion As New Conexion()

            Try

                Dim c = Conexion.conexion_global(1)

                Dim cmd As New MySqlCommand(Consulta, c)
                cmd.CommandTimeout = 6000
                cmd.Parameters.AddWithValue("@fechaIni", FechaIni)
                cmd.Parameters.AddWithValue("@fechaFin", FechaFin)
                cmd.Parameters.AddWithValue("@cveDistrito", cveDistrito)

                Dim r As MySqlDataReader = cmd.ExecuteReader

                If id = 1 Then
                    While r.Read
                        l.Add(New DatosCotidiana() With {
                             .Anio = r.GetString("anio"),
                             .Mes = r.GetString("mes"),
                             .Fecha = r.GetString("anio") + "-" + r.GetString("mes").PadLeft(2, "0"),
                             .Valor = r.GetString("valor1")
                        })
                    End While
                Else
                    While r.Read

                        l.Add(New DatosCotidiana() With {
                             .Anio = r.GetString("anio"),
                             .Mes = r.GetString("mes"),
                             .Fecha = r.GetString("anio") + "-" + r.GetString("mes").PadLeft(2, "0"),
                             .Valor = r.GetString("valor1"),
                             .Total = r.GetString("valor2"),
                             .Calculo = DameCalculoCotidiana(id, r.GetInt64("valor1"), r.GetInt64("valor2"))
                        })
                    End While
                End If

                Conexion.cerrar()
                l = ValidadorCotidiana(l, anio, mes)

                Return l

            Catch ex As MySqlException

                Debug.WriteLine("Error al procesar los datos de gestion: " + ex.Message)

                Conexion.cerrar()
                Return l

            End Try
        Else
            ' ENTRA A ANTECEDENTESCARPETAS 

            Dim Consulta As String = "SELECT ai.anio anio, ai.mes mes, sum(ai.valor1) valor1, sum(ai.valor2) valor2, AVG(ai.calculo) calculo
                                    FROM indicadores_pjem_cotidiana.tblantecedesindicadores ai
                                    INNER JOIN indicadores_pjem_cotidiana.tbljuzgados j on j.cveJuzgadoSEJ = ai.cveJuzgado
                                    WHERE
                                    ai.fechaCorte between @fechaIni AND @fechaFin
                                    AND ai.cveTipoIndicador = 1                                    
                                    AND ai.cveIndicador = @cveIndicador
                                    AND j.cveMateria = @materia
                                    AND ai.cveJuzgado LIKE @cveJuzgado
                                    AND j.cveDistrito = @cveDistrito
                                    GROUP BY anio,mes
                                    ORDER BY anio,mes;"

            Dim Fecha As New Date(anio, mes, 1, 0, 0, 0)
            Dim FechaIni = (Fecha.AddMonths(-3)).ToString("yyyy-MM-dd")
            Dim FechaFin = (Fecha.AddMonths(1).AddSeconds(-1)).ToString("yyyy-MM-dd")

            If cveJuzgado > 0 Then
                Consulta = Consulta.Replace("@cveJuzgado", "'" + cveJuzgado.ToString + "'")
            Else
                Consulta = Consulta.Replace("@cveJuzgado", "'%%'")
            End If

            Dim Conexion As New Conexion()

            Try

                Dim c = Conexion.conexion_global(2)

                Dim cmd As New MySqlCommand(Consulta, c)
                cmd.CommandTimeout = 6000
                cmd.Parameters.AddWithValue("@fechaIni", FechaIni)
                cmd.Parameters.AddWithValue("@fechaFin", FechaFin)
                cmd.Parameters.AddWithValue("@cveIndicador", id)
                cmd.Parameters.AddWithValue("@cveDistrito", cveDistrito)
                cmd.Parameters.AddWithValue("@materia", materia)


                Dim r As MySqlDataReader = cmd.ExecuteReader

                If id = 1 Then

                    While r.Read

                        l.Add(New DatosCotidiana() With {
                             .Anio = r.GetString("anio"),
                             .Mes = r.GetString("mes"),
                             .Fecha = r.GetString("anio") + "-" + r.GetString("mes").PadLeft(2, "0"),
                             .Valor = r.GetString("valor1")
                        })

                    End While

                Else

                    While r.Read

                        l.Add(New DatosCotidiana() With {
                             .Anio = r.GetString("anio"),
                             .Mes = r.GetString("mes"),
                             .Fecha = r.GetString("anio") + "-" + r.GetString("mes").PadLeft(2, "0"),
                             .Valor = r.GetString("valor1"),
                             .Total = r.GetString("valor2"),
                             .Calculo = r.GetString("calculo")
                        })

                    End While

                End If


                Conexion.cerrar()
                l = ValidadorCotidiana(l, anio, mes)

                Return l

            Catch ex As MySqlException

                Debug.WriteLine("Error al procesar los datos de gestion: " + ex.Message)

                Conexion.cerrar()

                Return l

            End Try

        End If

    End Function

    Public Shared Function DameCalculoCotidiana(id As String, valor As Int64, total As Int64) As String

        Dim Calc As String = ""

        Select Case id
            Case "1"
                Calc = Nothing
            Case "3", "4", "7", "8", "18", "11", "12", "13"
                Calc = Format((valor / total), "Percent")
            Case "9", "14", "15"
                Calc = Math.Round((valor / total), 2)
            Case "7"
                Calc = Format((total / valor), "Percent")
        End Select

        Return Calc

    End Function

    Public Shared Function ValidadorCotidiana(l As List(Of DatosCotidiana), anio As String, mes As String) As List(Of DatosCotidiana)

        Dim Fecha As New Date(anio, mes, 1, 0, 0, 0)

        Dim nl As New List(Of DatosCotidiana)

        For i As Integer = -3 To 0

            Dim ban As Boolean = True

            For Each c As DatosCotidiana In l

                If (Fecha.AddMonths(i)).ToString("yyyy-MM") = c.Fecha Then
                    nl.Add(c)
                    ban = False
                    Exit For
                End If

            Next

            If ban Then

                nl.Add(New DatosCotidiana() With {
                  .Anio = Fecha.AddMonths(i).ToString("yyyy"),
                  .Mes = Fecha.AddMonths(i).ToString("MM"),
                  .Valor = 0,
                  .Total = 0,
                  .Calculo = 0,
                  .Fecha = Fecha.AddMonths(i).ToString("yyyy-MM")
                })

            End If

        Next

        Return nl
    End Function

End Class