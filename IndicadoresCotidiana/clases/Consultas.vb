Public Class Consultas

    Public consulta As Hashtable = New Hashtable()

    Public Sub New()
        consulta.Add(1, {"Iniciados por juzgados", "SELECT YEAR(inijuz.fecharad) AS anio, MONTH(inijuz.fecharad) AS mes, count(distinct cveini) AS valor 
                    FROM estadistica.tblinijuzgados AS inijuz 
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = inijuz.cvejuzgado 
                    WHERE 
                    DATE_FORMAT(inijuz.fecharad,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    GROUP BY anio, mes 
                    ORDER BY anio DESC, mes DESC 
                    LIMIT 4;"})

        consulta.Add(3, {"Sentencias definitivas por juzgados", "SELECT IFNULL(tblfil.anio,tbltotal.anio) AS anio, IFNULL(tblfil.mes,tbltotal.mes) AS mes, IFNULL(tblfil.valor,0) AS valor, tbltotal.valor AS total FROM 
	                    (SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes,  
		                    count(distinct ter.cveter) as valor
		                    FROM estadistica.tblterjuzgados AS ter
		                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ter.cvejuzgado 
		                    WHERE
		                    DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
		                    AND juz.cvedistrito = @idDisJuz
                            AND juz.NomJuzgado LIKE @nomJuz
                            AND juz.CveAdscripcion LIKE @cveJuzgado
		                    GROUP BY anio, mes
		                    ORDER BY anio DESC, mes DESC
	                    ) AS tbltotal
	                    LEFT JOIN
	                    (
		                    SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes,  
		                    count(distinct ter.cveter) as valor
		                    FROM estadistica.tblterjuzgados AS ter
		                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ter.cvejuzgado 
		                    WHERE
		                    ter.cveresolucion = 'A031001'
		                    AND DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
		                    AND juz.cvedistrito = @idDisJuz
                            AND juz.NomJuzgado LIKE @nomJuz
                            AND juz.CveAdscripcion LIKE @cveJuzgado
		                    GROUP BY anio, mes
		                    ORDER BY anio DESC, mes DESC
	                    ) AS tblfil ON CONCAT(tblfil.anio,'/',tblfil.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
	                    LIMIT 4;"})

        consulta.Add(4, {"Sentencias interlocutorias por juzgados", "SELECT IFNULL(tblfil.anio,tbltotal.anio) AS anio, IFNULL(tblfil.mes,tbltotal.mes) AS mes, IFNULL(tblfil.valor,0) AS valor, tbltotal.valor AS total FROM 
	                    (SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes,  
		                    count(distinct ter.cveter) as valor
		                    FROM estadistica.tblterjuzgados AS ter
		                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ter.cvejuzgado 
		                    WHERE
		                    DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
		                    AND juz.cvedistrito = @idDisJuz
                            AND juz.NomJuzgado LIKE @nomJuz
                            AND juz.CveAdscripcion LIKE @cveJuzgado
		                    GROUP BY anio, mes
		                    ORDER BY anio DESC, mes DESC
	                    ) AS tbltotal
	                    LEFT JOIN
	                    (
		                    SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes,  
		                    count(distinct ter.cveter) as valor
		                    FROM estadistica.tblterjuzgados AS ter
		                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ter.cvejuzgado 
		                    WHERE
		                    ter.cveresolucion = 'A031002'
		                    AND DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
		                    AND juz.cvedistrito = @idDisJuz
                            AND juz.NomJuzgado LIKE @nomJuz
                            AND juz.CveAdscripcion LIKE @cveJuzgado
		                    GROUP BY anio, mes
		                    ORDER BY anio DESC, mes DESC
	                    ) AS tblfil ON CONCAT(tblfil.anio,'/',tblfil.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
	                    LIMIT 4;"})

        consulta.Add(7, {"Audiencias celebradas por juzgados", "SELECT (tot.anio) AS anio, (tot.mes) AS mes, (datos.datosT) AS valor, (tot.total) AS total
                    FROM 
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) total
                    FROM estadistica.tbldatadicionales AS dat
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072001','A072002')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m') 
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS tot
                    LEFT JOIN                            
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) datosT
                    FROM estadistica.tbldatadicionales AS dat
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072001')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m') 
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS datos ON CONCAT(datos.anio,'/',datos.mes) = CONCAT(tot.anio,'/',tot.mes)
                    LIMIT 4;"})

        consulta.Add(8, {"Audencias no celebradas por juzgados", "SELECT (tot.anio) AS anio, (tot.mes) AS mes, (datos.datosT) AS valor, (tot.total) AS total
                    FROM 
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) total
                    FROM estadistica.tbldatadicionales AS dat
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072001', 'A072002')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS tot
                    LEFT JOIN                            
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) datosT
                    FROM estadistica.tbldatadicionales AS dat
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072002')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m') 
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS datos ON CONCAT(datos.anio,'/',datos.mes) = CONCAT(tot.anio,'/',tot.mes)
                    LIMIT 4;"})

        consulta.Add(18, {"Audencias videograbadas por juzgados", "SELECT (tot.anio) AS anio, (tot.mes) AS mes, (datos.datosT) AS valor, (tot.total) AS total
                    FROM 
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) total
                    FROM estadistica.tbldatadicionales AS dat
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072001', 'A072002')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS tot
                    LEFT JOIN                            
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) datosT
                    FROM estadistica.tbldatadicionales AS dat
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072002')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m') 
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS datos ON CONCAT(datos.anio,'/',datos.mes) = CONCAT(tot.anio,'/',tot.mes)
                    LIMIT 4;"})

        consulta.Add(9, {"Tiempo promedio de expedientes en trámite",
                     "SELECT YEAR(SUBDATE(@fechaF, INTERVAL 0 MONTH)) AS anio, 
                    MONTH(SUBDATE(@fechaF, INTERVAL 0 MONTH)) AS mes,
                     date_format(SUBDATE(@fechaF, INTERVAL 0 MONTH),'%Y-%m') as fecha, 
                    SUM(TIMESTAMPDIFF(DAY,t.fechaRadST, SUBDATE(@fechaF, INTERVAL 0 MONTH))) AS valor, 
                    (f.total) AS total
                    FROM 
                    (SELECT ini.fecharad AS fechaRadST
                    FROM estadistica.tblinijuzgados AS ini
                    LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND ini.fecharad <= SUBDATE(@fechaF, INTERVAL 0 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    ) AS t, 
                    (SELECT COUNT(ini.cveini) as total
                    FROM estadistica.tblinijuzgados AS ini
                    LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    ) AS f

                    UNION

                    SELECT YEAR(SUBDATE(@fechaF, INTERVAL 1 MONTH)) AS anio, 
                    MONTH(SUBDATE(@fechaF, INTERVAL 1 MONTH)) AS mes, 
                    date_format(SUBDATE(@fechaF, INTERVAL 1 MONTH),'%Y-%m') as fecha, 
                    SUM(TIMESTAMPDIFF(DAY,t.fechaRadST, SUBDATE(@fechaF, INTERVAL 1 MONTH))) AS valor, 
                    (f.total) AS total
                    FROM 
                    (SELECT ini.fecharad AS fechaRadST
                    FROM estadistica.tblinijuzgados AS ini
                    LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND ini.fecharad <= SUBDATE(@fechaF, INTERVAL 1 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    ) AS t, 
                    (SELECT COUNT(ini.cveini) as total
                    FROM estadistica.tblinijuzgados AS ini
                    LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    ) AS f

                    UNION

                    SELECT YEAR(SUBDATE(@fechaF, INTERVAL 2 MONTH)) AS anio, 
                    MONTH(SUBDATE(@fechaF, INTERVAL 2 MONTH)) AS mes, 
                    date_format(SUBDATE(@fechaF, INTERVAL 2 MONTH),'%Y-%m') as fecha, 
                    SUM(TIMESTAMPDIFF(DAY,t.fechaRadST, SUBDATE(@fechaF, INTERVAL 2 MONTH))) AS valor, 
                    (f.total) AS total
                    FROM 
                    (SELECT ini.fecharad AS fechaRadST
                    FROM estadistica.tblinijuzgados AS ini
                    LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND ini.fecharad <= SUBDATE(@fechaF, INTERVAL 2 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    ) AS t, 
                    (SELECT COUNT(ini.cveini) as total
                    FROM estadistica.tblinijuzgados AS ini
                    LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    ) AS f

                    UNION

                    SELECT YEAR(SUBDATE(@fechaF, INTERVAL 3 MONTH)) AS anio, 
                    MONTH(SUBDATE(@fechaF, INTERVAL 3 MONTH)) AS mes, 
                    date_format(SUBDATE(@fechaF, INTERVAL 3 MONTH),'%Y-%m') as fecha, 
                    SUM(TIMESTAMPDIFF(DAY,t.fechaRadST, SUBDATE(@fechaF, INTERVAL 3 MONTH))) AS valor, 
                    (f.total) AS total
                    FROM 
                    (SELECT ini.fecharad AS fechaRadST
                    FROM estadistica.tblinijuzgados AS ini
                    LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND ini.fecharad <= SUBDATE(@fechaF, INTERVAL 3 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    ) AS t, 
                    (SELECT COUNT(ini.cveini) as total
                    FROM estadistica.tblinijuzgados AS ini
                    LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    AND juz.CveAdscripcion LIKE @cveJuzgado
                    ) AS f",
        "SELECT 
        ini.CveExp expediente,
        year(ini.FechaRad) anio,
        TIMESTAMPDIFF(DAY,ini.FechaRad, NOW()) as dias,
        date_format(ini.fechaRad,'%d/%m/%Y') fecharad,
        date_format(now(),'%d/%m/%Y') fechacon,
        case when r.CveJuicioDelito = 'A102210' then 'Acción confesoria'
        when r.CveJuicioDelito = 'A103133' then 'Acción de obra'
        when r.CveJuicioDelito = 'A102190' then 'Acción de obra nueva'
        when r.CveJuicioDelito = 'A102240' then 'Acción negatoria'
        when r.CveJuicioDelito = 'A102202' then 'Acción oblicua'
        when r.CveJuicioDelito = 'A102241' then 'Acción pauliana'
        when r.CveJuicioDelito = 'A102220' then 'Acción rescisoria'
        when r.CveJuicioDelito = 'A103078' then 'Acreditación de concubinato'
        when r.CveJuicioDelito = 'A103079' then 'Acreditación de relación materno-filial'
        when r.CveJuicioDelito = 'A103082' then 'Acreditación del ejercicio de la patria protestad'
        when r.CveJuicioDelito = 'A102139' then 'Acreditación de concubinato'
        when r.CveJuicioDelito = 'A103084' then 'Acreditar estado civil'
        when r.CveJuicioDelito = 'A102164' then 'Acreditar relación contractual'
        when r.CveJuicioDelito = 'A102074' then 'Actos prejudiciales'
        when r.CveJuicioDelito = 'A103001' then 'Actos prejudiciales'
        when r.CveJuicioDelito = 'A102006' then 'Actos prejudiciales'
        when r.CveJuicioDelito = 'A103073' then 'Actos previos a juicio'
        when r.CveJuicioDelito = 'A102085' then 'Adopción'
        when r.CveJuicioDelito = 'A103012' then 'Adopción'
        when r.CveJuicioDelito = 'A103095' then 'Anotaciones de acta de defunción'
        when r.CveJuicioDelito = 'A102014' then 'Apeo y deslinde'
        when r.CveJuicioDelito = 'A103119' then 'Apeo y deslinde'
        when r.CveJuicioDelito = 'A103134' then 'Apeo y deslinde'
        when r.CveJuicioDelito = 'A103069' then 'Aprobar convenio'
        when r.CveJuicioDelito = 'A103115' then 'Aseguramiento de bienes'
        when r.CveJuicioDelito = 'A103105' then 'Asentamiento extemporáneo de acta'
        when r.CveJuicioDelito = 'A102145' then 'Autorización para vender y grabar bienes y transferir derechos'
        when r.CveJuicioDelito = 'A103063' then 'Autorización para vender y grabar bienes y transferir derechos'
        when r.CveJuicioDelito = 'A103074' then 'Auto declarativo'
        when r.CveJuicioDelito = 'A102126' then 'Autorización judicial'
        when r.CveJuicioDelito = 'A103053' then 'Autorización judicial'
        when r.CveJuicioDelito = 'A102086' then 'Autorización para contratar entre cónyuges'
        when r.CveJuicioDelito = 'A103013' then 'Autorización para contratar entre cónyuges'
        when r.CveJuicioDelito = 'A103083' then 'Autorización para expedir pasaporte'
        when r.CveJuicioDelito = 'A102087' then 'Autorización para salir del país'
        when r.CveJuicioDelito = 'A103014' then 'Autorización para salir del país'
        when r.CveJuicioDelito = 'A102088' then 'Autorización para venta de bienes de menor'
        when r.CveJuicioDelito = 'A103015' then 'Autorización para venta de bienes de menor'
        when r.CveJuicioDelito = 'A102089' then 'Cambio de régimen conyugal'
        when r.CveJuicioDelito = 'A103016' then 'Cambio de régimen conyugal'
        when r.CveJuicioDelito = 'A103113' then 'Cambio de régimen patrimonial'
        when r.CveJuicioDelito = 'A102193' then 'Cancelación de contrato'
        when r.CveJuicioDelito = 'A102141' then 'Cancelación de crédito'
        when r.CveJuicioDelito = 'A102216' then 'Cancelación de hipoteca'
        when r.CveJuicioDelito = 'A102098' then 'Cancelación de pensión alimenticia'
        when r.CveJuicioDelito = 'A103025' then 'Cancelación de pensión alimenticia'
        when r.CveJuicioDelito = 'A102212' then 'Celebración de asamblea'
        when r.CveJuicioDelito = 'A103085' then 'Cesación de pensión alimenticia'
        when r.CveJuicioDelito = 'A102051' then 'Concursos'
        when r.CveJuicioDelito = 'A102025' then 'Consignación'
        when r.CveJuicioDelito = 'A102056' then 'Consignación de pago'
        when r.CveJuicioDelito = 'A103135' then 'Consignación de pago'
        when r.CveJuicioDelito = 'A102119' then 'Consignación de pensión alimenticia'
        when r.CveJuicioDelito = 'A103046' then 'Consignación de pensión alimenticia'
        when r.CveJuicioDelito = 'A102136' then 'Consignación de pensión alimenticia'
        when r.CveJuicioDelito = 'A102055' then 'Consignación de renta'
        when r.CveJuicioDelito = 'A102026' then 'Consignación de rentas'
        when r.CveJuicioDelito = 'A102213' then 'Consolidación de la propiedad'
        when r.CveJuicioDelito = 'A102135' then 'Constitución de patrimonio familiar'
        when r.CveJuicioDelito = 'A103060' then 'Constitución de patrimonio familiar'
        when r.CveJuicioDelito = 'A102200' then 'Consumación de la usucapión por inscripción de la posesión'
        when r.CveJuicioDelito = 'A102209' then 'Consumación de propiedad'
        when r.CveJuicioDelito = 'A102132' then 'Controversia de orden familiar'
        when r.CveJuicioDelito = 'A103059' then 'Controversia de orden familiar'
        when r.CveJuicioDelito = 'A102225' then 'Convivencia familiar'
        when r.CveJuicioDelito = 'A103068' then 'Convivencia familiar'
        when r.CveJuicioDelito = 'A102183' then 'Copropiedad derecho del tanto'
        when r.CveJuicioDelito = 'A103136' then 'Cumplimiento de contrato'
        when r.CveJuicioDelito = 'A102179' then 'Cumplimiento de acuerdo de voluntades'
        when r.CveJuicioDelito = 'A102161' then 'Cumplimiento de carta compromiso'
        when r.CveJuicioDelito = 'A102067' then 'Cumplimiento de contrato'
        when r.CveJuicioDelito = 'A102039' then 'Cumplimiento de contrato de arrendamiento'
        when r.CveJuicioDelito = 'A102061' then 'Cumplimiento de contrato de compra venta'
        when r.CveJuicioDelito = 'A102237' then 'Cumplimiento de contrato de donación'
        when r.CveJuicioDelito = 'A102134' then 'Cumplimiento de convenio'
        when r.CveJuicioDelito = 'A103062' then 'Cumplimiento de convenio'
        when r.CveJuicioDelito = 'A102167' then 'Cumplimiento de obligación derivada de la responsabilidad civil'
        when r.CveJuicioDelito = 'A102099' then 'Custodia de menor'
        when r.CveJuicioDelito = 'A103026' then 'Custodia de menor'
        when r.CveJuicioDelito = 'A102186' then 'Daño moral'
        when r.CveJuicioDelito = 'A102232' then 'Declaración de ausencia'
        when r.CveJuicioDelito = 'A103107' then 'Declaración de ausencia'
        when r.CveJuicioDelito = 'A102090' then 'Declaración de dependencia económica'
        when r.CveJuicioDelito = 'A103017' then 'Declaración de dependencia económica'
        when r.CveJuicioDelito = 'A102114' then 'Declaración de estado de interdicción'
        when r.CveJuicioDelito = 'A103041' then 'Declaración de estado de interdicción'
        when r.CveJuicioDelito = 'A103103' then 'Declaración de estado de minoría de edad'
        when r.CveJuicioDelito = 'A103116' then 'Declaración de fallecimiento'
        when r.CveJuicioDelito = 'A102137' then 'Declaración judicial'
        when r.CveJuicioDelito = 'A103093' then 'Declaración judicial de entrega de adopción'
        when r.CveJuicioDelito = 'A103090' then 'Declaración judicial de incapacidad'
        when r.CveJuicioDelito = 'A103123' then 'Declaración judicial de patria protestad'
        when r.CveJuicioDelito = 'A102124' then 'Decretar deposito judicial'
        when r.CveJuicioDelito = 'A103051' then 'Decretar deposito judicial'
        when r.CveJuicioDelito = 'A102185' then 'Demolición de construcción'
        when r.CveJuicioDelito = 'A103080' then 'Dependencia económica'
        when r.CveJuicioDelito = 'A102010' then 'Depósito de personas'
        when r.CveJuicioDelito = 'A102075' then 'Depósito de personas'
        when r.CveJuicioDelito = 'A103002' then 'Depósito de personas'
        when r.CveJuicioDelito = 'A102188' then 'Derecho de reconocimiento de copropietario'
        when r.CveJuicioDelito = 'A102046' then 'Desahucio'
        when r.CveJuicioDelito = 'A103137' then 'Desahucio'
        when r.CveJuicioDelito = 'A102100' then 'Desconocimiento o contradicción de paternidad'
        when r.CveJuicioDelito = 'A103027' then 'Desconocimiento o contradicción de paternidad'
        when r.CveJuicioDelito = 'A102166' then 'Desocupación y entrega de inmueble'
        when r.CveJuicioDelito = 'A102182' then 'Devolución de cantidad'
        when r.CveJuicioDelito = 'A103114' then 'Devolución de pensión alimenticia'
        when r.CveJuicioDelito = 'A102063' then 'Devolución y entrega de documento.'
        when r.CveJuicioDelito = 'A103138' then 'Diligencia de apeo y deslinde'
        when r.CveJuicioDelito = 'A102229' then 'Disminución de pensión alimenticia'
        when r.CveJuicioDelito = 'A103087' then 'Disminución de pensión alimenticia'
        when r.CveJuicioDelito = 'A102204' then 'Disolución de copropiedad'
        when r.CveJuicioDelito = 'A102101' then 'Disolución de sociedad conyugal'
        when r.CveJuicioDelito = 'A103028' then 'Disolución de sociedad conyugal'
        when r.CveJuicioDelito = 'A102091' then 'Dispensa para contraer matrimonio'
        when r.CveJuicioDelito = 'A103018' then 'Dispensa para contraer matrimonio'
        when r.CveJuicioDelito = 'A102181' then 'División de la cosa común'
        when r.CveJuicioDelito = 'A102230' then 'Divorcio incausado'
        when r.CveJuicioDelito = 'A103126' then 'Divorcio incausado'
        when r.CveJuicioDelito = 'A102102' then 'Divorcio necesario'
        when r.CveJuicioDelito = 'A103029' then 'Divorcio necesario'
        when r.CveJuicioDelito = 'A102115' then 'Divorcio por mutuo consentimiento'
        when r.CveJuicioDelito = 'A103042' then 'Divorcio por mutuo consentimiento'
        when r.CveJuicioDelito = 'A102146' then 'Ejecución de convenio'
        when r.CveJuicioDelito = 'A103129' then 'Ejecución de convenio'
        when r.CveJuicioDelito = 'A102173' then 'Ejecución de garantía'
        when r.CveJuicioDelito = 'A102180' then 'Ejecución de laudo arbitral'
        when r.CveJuicioDelito = 'A103096' then 'Ejecución de sentencia'
        when r.CveJuicioDelito = 'A102057' then 'Ejecutivo civil'
        when r.CveJuicioDelito = 'A102002' then 'Ejecutivo mercantil'
        when r.CveJuicioDelito = 'A103121' then 'Ejecutivo mercantil'
        when r.CveJuicioDelito = 'A103161' then 'Ejecutivo mercantil'
        when r.CveJuicioDelito = 'A103086' then 'Elevar convenio de cosa juzgada'
        when r.CveJuicioDelito = 'A102157' then 'Entrega de administración'
        when r.CveJuicioDelito = 'A102158' then 'Entrega de estructura metálica'
        when r.CveJuicioDelito = 'A102092' then 'Entrega de menor a institución de asistencia'
        when r.CveJuicioDelito = 'A103019' then 'Entrega de menor a institución de asistencia'
        when r.CveJuicioDelito = 'A102163' then 'Entrega posesión del inmueble'
        when r.CveJuicioDelito = 'A102223' then 'Entrega real de titulo'
        when r.CveJuicioDelito = 'A102217' then 'Especial de fianza'
        when r.CveJuicioDelito = 'A102027' then 'Especial hipotecario'
        when r.CveJuicioDelito = 'A102175' then 'Especial mercantil'
        when r.CveJuicioDelito = 'A103076' then 'Estado de interdicción'
        when r.CveJuicioDelito = 'A103108' then 'Exclusión de bienes inmuebles'
        when r.CveJuicioDelito = 'A102195' then 'Firma de factura'
        when r.CveJuicioDelito = 'A102123' then 'Firma y contrato de donación'
        when r.CveJuicioDelito = 'A103050' then 'Firma y contrato de donación'
        when r.CveJuicioDelito = 'A102052' then 'Funciones de sindico'
        when r.CveJuicioDelito = 'A102120' then 'Guarda y custodia de menor'
        when r.CveJuicioDelito = 'A103047' then 'Guarda y custodia de menor'
        when r.CveJuicioDelito = 'A102143' then 'Homologación de sentencia'
        when r.CveJuicioDelito = 'A103065' then 'Homologación de sentencia'
        when r.CveJuicioDelito = 'A102093' then 'Identidad de personas'
        when r.CveJuicioDelito = 'A103020' then 'Identidad de personas'
        when r.CveJuicioDelito = 'A103088' then 'Incapacidad para heredar'
        when r.CveJuicioDelito = 'A102154' then 'Incidente de liquidación'
        when r.CveJuicioDelito = 'A102214' then 'Incidente para convocar asamblea'
        when r.CveJuicioDelito = 'A103094' then 'Incluir bien a la sociedad conyugal'
        when r.CveJuicioDelito = 'A102103' then 'Incremento de pensión alimenticia'
        when r.CveJuicioDelito = 'A103030' then 'Incremento de pensión alimenticia'
        when r.CveJuicioDelito = 'A102172' then 'Incumplimiento de contrato'
        when r.CveJuicioDelito = 'A103091' then 'Ineficiencia parcial del testamento público'
        when r.CveJuicioDelito = 'A103139' then 'Interdicto'
        when r.CveJuicioDelito = 'A102206' then 'Inexistencia de contrato'
        when r.CveJuicioDelito = 'A103140' then 'Inexistencia de donación'
        when r.CveJuicioDelito = 'A103141' then 'Información de dominio'
        when r.CveJuicioDelito = 'A102128' then 'Información ad-perpetuam'
        when r.CveJuicioDelito = 'A103055' then 'Información ad-perpetuam'
        when r.CveJuicioDelito = 'A102015' then 'Información ad-perpetuam'
        when r.CveJuicioDelito = 'A102016' then 'Información de domino'
        when r.CveJuicioDelito = 'A102017' then 'Información posesoria'
        when r.CveJuicioDelito = 'A102117' then 'Información testimonial'
        when r.CveJuicioDelito = 'A103044' then 'Información testimonial'
        when r.CveJuicioDelito = 'A102018' then 'Inmatriculación'
        when r.CveJuicioDelito = 'A103142' then 'Inmatriculación'
        when r.CveJuicioDelito = 'A103106' then 'Impugnación de testamento'
        when r.CveJuicioDelito = 'A102028' then 'Interdictos'
        when r.CveJuicioDelito = 'A102065' then 'Interpelación judicial'
        when r.CveJuicioDelito = 'A103143' then 'Interpelación judicial'
        when r.CveJuicioDelito = 'A102029' then 'Jactancia'
        when r.CveJuicioDelito = 'A102047' then 'Juicio arbitral'
        when r.CveJuicioDelito = 'A102243' then 'Juicio ejecutivo mercantil oral'
        when r.CveJuicioDelito = 'A102001' then 'Juicio mercantil'
        when r.CveJuicioDelito = 'A102005' then 'Jurisdicción contenciosa'
        when r.CveJuicioDelito = 'A102050' then 'Jurisdicción mixta'
        when r.CveJuicioDelito = 'A102080' then 'Jurisdicción mixta'
        when r.CveJuicioDelito = 'A103007' then 'Jurisdicción mixta'
        when r.CveJuicioDelito = 'A102013' then 'Jurisdicción voluntaria'
        when r.CveJuicioDelito = 'A102084' then 'Jurisdicción voluntaria'
        when r.CveJuicioDelito = 'A103011' then 'Jurisdicción voluntaria'
        when r.CveJuicioDelito = 'A102169' then 'Liberación de reserva del dominio'
        when r.CveJuicioDelito = 'A102122' then 'Licencia para contratar'
        when r.CveJuicioDelito = 'A103049' then 'Licencia para contratar'
        when r.CveJuicioDelito = 'A103071' then 'Liquidación de pensión adeudas'
        when r.CveJuicioDelito = 'A103070' then 'Liquidación de sociedad'
        when r.CveJuicioDelito = 'A103144' then 'Medios preparatorios a juicio'
        when r.CveJuicioDelito = 'A103145' then 'Medios preparatorios a juicio reivindicatorio'
        when r.CveJuicioDelito = 'A103162' then 'Medios preparatorios a juicio ejecutivo mercantil'
        when r.CveJuicioDelito = 'A102152' then 'Medios preparatorios a juicio ejecutivo civil'
        when r.CveJuicioDelito = 'A102151' then 'Medios preparatorios a juicio ejecutivo mercantil'
        when r.CveJuicioDelito = 'A102007' then 'Medios preparatorios a juicio'
        when r.CveJuicioDelito = 'A102076' then 'Medios preparatorios a juicio'
        when r.CveJuicioDelito = 'A103003' then 'Medios preparatorios a juicio'
        when r.CveJuicioDelito = 'A102149' then 'Medios preparatorios a juicio ordinario civil'
        when r.CveJuicioDelito = 'A102150' then 'Medios preparatorios a juicio ordinario mercantil'
        when r.CveJuicioDelito = 'A102133' then 'Modificación de convenio'
        when r.CveJuicioDelito = 'A103061' then 'Modificación de convenio'
        when r.CveJuicioDelito = 'A103120' then 'Modificación de régimen de convivencia'
        when r.CveJuicioDelito = 'A103097' then 'Modificar resolución'
        when r.CveJuicioDelito = 'A102ZZZ' then 'No existe expediente'
        when r.CveJuicioDelito = 'A103ZZZ' then 'No existe expediente'
        when r.CveJuicioDelito = 'A102094' then 'Nombramiento de tutores y curadores'
        when r.CveJuicioDelito = 'A103021' then 'Nombramiento de tutores y curadores'
        when r.CveJuicioDelito = 'A102019' then 'Notificación judicial'
        when r.CveJuicioDelito = 'A102121' then 'Notificación judicial'
        when r.CveJuicioDelito = 'A103048' then 'Notificación judicial'
        when r.CveJuicioDelito = 'A102162' then 'Novación de contrato de arrendamiento'
        when r.CveJuicioDelito = 'A102222' then 'Nulidad absoluta de acto jurídico'
        when r.CveJuicioDelito = 'A103146' then 'Nulidad de acta de asamblea'
        when r.CveJuicioDelito = 'A102129' then 'Nulidad de acta de nacimiento'
        when r.CveJuicioDelito = 'A103056' then 'Nulidad de acta de nacimiento'
        when r.CveJuicioDelito = 'A103072' then 'Nulidad de actuaciones'
        when r.CveJuicioDelito = 'A102191' then 'Nulidad de asamblea'
        when r.CveJuicioDelito = 'A103100' then 'Nulidad de cesión de derechos hereditarios'
        when r.CveJuicioDelito = 'A102073' then 'Nulidad de contrato'
        when r.CveJuicioDelito = 'A103147' then 'Nulidad de contrato'
        when r.CveJuicioDelito = 'A102208' then 'Nulidad de contrato de arrendamiento'
        when r.CveJuicioDelito = 'A102064' then 'Nulidad de contrato de compra venta'
        when r.CveJuicioDelito = 'A102177' then 'Nulidad de convenio'
        when r.CveJuicioDelito = 'A102140' then 'Nulidad de escritura'
        when r.CveJuicioDelito = 'A103109' then 'Nulidad de escritura'
        when r.CveJuicioDelito = 'A102207' then 'Nulidad de inscripción'
        when r.CveJuicioDelito = 'A102038' then 'Nulidad de juicio'
        when r.CveJuicioDelito = 'A102118' then 'Nulidad de juicio'
        when r.CveJuicioDelito = 'A103045' then 'Nulidad de juicio'
        when r.CveJuicioDelito = 'A103148' then 'Nulidad de juicio concluido'
        when r.CveJuicioDelito = 'A102104' then 'Nulidad de matrimonio'
        when r.CveJuicioDelito = 'A103031' then 'Nulidad de matrimonio'
        when r.CveJuicioDelito = 'A102105' then 'Nulidad de testamento'
        when r.CveJuicioDelito = 'A103032' then 'Nulidad de testamento'
        when r.CveJuicioDelito = 'A102201' then 'Nulidad de título de propiedad'
        when r.CveJuicioDelito = 'A102226' then 'Oral mercantil'
        when r.CveJuicioDelito = 'A103163' then 'Oral mercantil'
        when r.CveJuicioDelito = 'A102097' then 'Ordinario civil'
        when r.CveJuicioDelito = 'A103024' then 'Ordinario civil'
        when r.CveJuicioDelito = 'A102024' then 'Ordinario civil o verbal'
        when r.CveJuicioDelito = 'A102242' then 'Extinción de dominio'
        when r.CveJuicioDelito = 'A102003' then 'Ordinario mercantil'
        when r.CveJuicioDelito = 'A103164' then 'Ordinario mercantil'
        when r.CveJuicioDelito = 'A103149' then 'Otorgamiento y firma de escritura'
        when r.CveJuicioDelito = 'A102168' then 'Otorgamiento de contrato de prestación de servicio'
        when r.CveJuicioDelito = 'A102238' then 'Otorgamiento de título o derecho de perpetuidad'
        when r.CveJuicioDelito = 'A102184' then 'Otorgamiento y firma de contrato'
        when r.CveJuicioDelito = 'A103150' then 'Otorgamiento y firma de escrito'
        when r.CveJuicioDelito = 'A102030' then 'Otorgamiento y firma de escritura'
        when r.CveJuicioDelito = 'A102004' then 'Otros'
        when r.CveJuicioDelito = 'A102012' then 'Otros'
        when r.CveJuicioDelito = 'A102023' then 'Otros'
        when r.CveJuicioDelito = 'A102044' then 'Otros'
        when r.CveJuicioDelito = 'A102049' then 'Otros'
        when r.CveJuicioDelito = 'A102053' then 'Otros'
        when r.CveJuicioDelito = 'A102079' then 'Otros'
        when r.CveJuicioDelito = 'A102083' then 'Otros'
        when r.CveJuicioDelito = 'A102096' then 'Otros'
        when r.CveJuicioDelito = 'A102112' then 'Otros'
        when r.CveJuicioDelito = 'A102116' then 'Otros'
        when r.CveJuicioDelito = 'A103006' then 'Otros'
        when r.CveJuicioDelito = 'A103010' then 'Otros'
        when r.CveJuicioDelito = 'A103023' then 'Otros'
        when r.CveJuicioDelito = 'A103039' then 'Otros'
        when r.CveJuicioDelito = 'A103043' then 'Otros'
        when r.CveJuicioDelito = 'A103101' then 'Pago de cantidad'
        when r.CveJuicioDelito = 'A102031' then 'Pago de daños'
        when r.CveJuicioDelito = 'A103098' then 'Pago de gastos hospitalarios'
        when r.CveJuicioDelito = 'A102218' then 'Pago de honorarios'
        when r.CveJuicioDelito = 'A102032' then 'Pago de pesos'
        when r.CveJuicioDelito = 'A103151' then 'Pago de pesos'
        when r.CveJuicioDelito = 'A102106' then 'Pensión alimenticia'
        when r.CveJuicioDelito = 'A103033' then 'Pensión alimenticia'
        when r.CveJuicioDelito = 'A102107' then 'Perdida de patria potestad'
        when r.CveJuicioDelito = 'A103034' then 'Perdida de patria potestad'
        when r.CveJuicioDelito = 'A102108' then 'Petición de herencia'
        when r.CveJuicioDelito = 'A103035' then 'Petición de herencia'
        when r.CveJuicioDelito = 'A102033' then 'Plenaria de posesión'
        when r.CveJuicioDelito = 'A103152' then 'Plenario de posesión'
        when r.CveJuicioDelito = 'A103022' then 'Posesión de estado de hijo'
        when r.CveJuicioDelito = 'A102095' then 'Posesión de estado de hijo'
        when r.CveJuicioDelito = 'A103153' then 'Posesión de vehículo'
        when r.CveJuicioDelito = 'A102008' then 'Preliminares de consignación'
        when r.CveJuicioDelito = 'A102077' then 'Preliminares de consignación'
        when r.CveJuicioDelito = 'A103004' then 'Preliminares de consignación'
        when r.CveJuicioDelito = 'A102011' then 'Preparación a juicio arbitral'
        when r.CveJuicioDelito = 'A102034' then 'Prescripción'
        when r.CveJuicioDelito = 'A102176' then 'Prescripción de la acción hipotecaria'
        when r.CveJuicioDelito = 'A102189' then 'Prescripción positiva'
        when r.CveJuicioDelito = 'A102233' then 'Presunción de muerte'
        when r.CveJuicioDelito = 'A103131' then 'Presunción de muerte'
        when r.CveJuicioDelito = 'A102113' then 'Procedimiento especial'
        when r.CveJuicioDelito = 'A103040' then 'Procedimiento especial'
        when r.CveJuicioDelito = 'A102148' then 'Procedimiento especial mercantil'
        when r.CveJuicioDelito = 'A102205' then 'Procedimiento incidental'
        when r.CveJuicioDelito = 'A102170' then 'Procedimiento judicial de ejecución de garantías'
        when r.CveJuicioDelito = 'A102147' then 'Procedimiento judicial no contencioso'
        when r.CveJuicioDelito = 'A103077' then 'Procedimiento judicial no contencioso'
        when r.CveJuicioDelito = 'A102045' then 'Procedimientos especiales'
        when r.CveJuicioDelito = 'A102069' then 'Prorroga de contrato'
        when r.CveJuicioDelito = 'A102041' then 'Prorroga de contrato de arrendamiento'
        when r.CveJuicioDelito = 'A102228' then 'Protocolización de régimen de propiedad'
        when r.CveJuicioDelito = 'A102009' then 'Providencias precautorias'
        when r.CveJuicioDelito = 'A102078' then 'Providencias precautorias'
        when r.CveJuicioDelito = 'A103005' then 'Providencias precautorias'
        when r.CveJuicioDelito = 'A102071' then 'Quiebras y suspensión de pagos'
        when r.CveJuicioDelito = 'A102194' then 'Ratificación de contrato'
        when r.CveJuicioDelito = 'A102022' then 'Ratificación de convenio'
        when r.CveJuicioDelito = 'A103127' then 'Ratificación de convenio'
        when r.CveJuicioDelito = 'A102235' then 'Reconducción de contrato de arrendamiento'
        when r.CveJuicioDelito = 'A103125' then 'Reconocimiento de beneficiarios'
        when r.CveJuicioDelito = 'A103092' then 'Reconocimiento de bienes de sociedad conyugal'
        when r.CveJuicioDelito = 'A102192' then 'Reconocimiento de contrato'
        when r.CveJuicioDelito = 'A102109' then 'Reconocimiento de hijo'
        when r.CveJuicioDelito = 'A103036' then 'Reconocimiento de hijo'
        when r.CveJuicioDelito = 'A103111' then 'Reconocimiento de identidad'
        when r.CveJuicioDelito = 'A103110' then 'Reconocimiento de maternidad'
        when r.CveJuicioDelito = 'A102203' then 'Reconocimiento de pago'
        when r.CveJuicioDelito = 'A103075' then 'Reconocimiento de paternidad'
        when r.CveJuicioDelito = 'A102187' then 'Reconocimiento de personalidad'
        when r.CveJuicioDelito = 'A102110' then 'Rectificación de acta'
        when r.CveJuicioDelito = 'A103037' then 'Rectificación de acta'
        when r.CveJuicioDelito = 'A102196' then 'Rectificación de asiento registral'
        when r.CveJuicioDelito = 'A102131' then 'Rectificación de estado civil'
        when r.CveJuicioDelito = 'A103058' then 'Rectificación de estado civil'
        when r.CveJuicioDelito = 'A102160' then 'Rectificación de medida'
        when r.CveJuicioDelito = 'A102197' then 'Rectificación de propiedades'
        when r.CveJuicioDelito = 'A102111' then 'Reducción de pensión alimenticia'
        when r.CveJuicioDelito = 'A103038' then 'Reducción de pensión alimenticia'
        when r.CveJuicioDelito = 'A102219' then 'Reducción de precio'
        when r.CveJuicioDelito = 'A102127' then 'Régimen de visitas'
        when r.CveJuicioDelito = 'A103054' then 'Régimen de visitas'
        when r.CveJuicioDelito = 'A102066' then 'Registro de escritura'
        when r.CveJuicioDelito = 'A102020' then 'Registro de sociedad'
        when r.CveJuicioDelito = 'A103117' then 'Registro extemporáneo de acta'
        when r.CveJuicioDelito = 'A103099' then 'Registro extemporáneo de acta de defunción'
        when r.CveJuicioDelito = 'A102159' then 'Regularización de solar urbano'
        when r.CveJuicioDelito = 'A102125' then 'Reincorporación de menores'
        when r.CveJuicioDelito = 'A103052' then 'Reincorporación de menores'
        when r.CveJuicioDelito = 'A102035' then 'Reivindicatoria'
        when r.CveJuicioDelito = 'A103067' then 'Remoción de albacea'
        when r.CveJuicioDelito = 'A102178' then 'Rendición de cuentas'
        when r.CveJuicioDelito = 'A102234' then 'Rendición de cuentas de sociedad conyugal'
        when r.CveJuicioDelito = 'A103132' then 'Rendición de cuentas de sociedad conyugal'
        when r.CveJuicioDelito = 'A102215' then 'Reparación de daño moral'
        when r.CveJuicioDelito = 'A102165' then 'Reparación e indemnización de daños y perjuicios'
        when r.CveJuicioDelito = 'A102062' then 'Requerimiento de pago'
        when r.CveJuicioDelito = 'A102070' then 'Rescisión de contrato'
        when r.CveJuicioDelito = 'A103124' then 'Rescisión de contrato'
        when r.CveJuicioDelito = 'A103154' then 'Rescisión de contrato'
        when r.CveJuicioDelito = 'A102042' then 'Rescisión de contrato de arrendamiento'
        when r.CveJuicioDelito = 'A102060' then 'Rescisión de contrato de compraventa'
        when r.CveJuicioDelito = 'A102153' then 'Rescisión de convenio'
        when r.CveJuicioDelito = 'A103112' then 'Rescisión de convenio'
        when r.CveJuicioDelito = 'A102072' then 'Responsabilidad civil'
        when r.CveJuicioDelito = 'A103118' then 'Restitución de bien'
        when r.CveJuicioDelito = 'A102211' then 'Restitución de fracción de terreno'
        when r.CveJuicioDelito = 'A102221' then 'Restitución de la posesión de bien mueble'
        when r.CveJuicioDelito = 'A102199' then 'Restitución y pago de lo indebido'
        when r.CveJuicioDelito = 'A103155' then 'Reivindicatorio'
        when r.CveJuicioDelito = 'A102043' then 'Revisión de contrato de arrendamiento'
        when r.CveJuicioDelito = 'A102156' then 'Revocación de contrato de donación'
        when r.CveJuicioDelito = 'A103104' then 'Revocación de donación'
        when r.CveJuicioDelito = 'A103156' then 'Revocación de donación'
        when r.CveJuicioDelito = 'A102142' then 'Revocatoria a asamblea'
        when r.CveJuicioDelito = 'A103064' then 'Revocatoria a asamblea'
        when r.CveJuicioDelito = 'A103081' then 'Separación de personas'
        when r.CveJuicioDelito = 'A102036' then 'Servidumbre de paso'
        when r.CveJuicioDelito = 'A102239' then 'Servidumbre legal de desagüe'
        when r.CveJuicioDelito = 'A103089' then 'Solicitud de legalidad de sentencia'
        when r.CveJuicioDelito = 'A102021' then 'Solicitud de orden judicial'
        when r.CveJuicioDelito = 'A102227' then 'Subrogación de derechos'
        when r.CveJuicioDelito = 'A102081' then 'Sucesorio intestamentario'
        when r.CveJuicioDelito = 'A103008' then 'Sucesorio intestamentario'
        when r.CveJuicioDelito = 'A102082' then 'Sucesorio testamentario'
        when r.CveJuicioDelito = 'A103009' then 'Sucesorio testamentario'
        when r.CveJuicioDelito = 'A102236' then 'Suspensión de obra nueva'
        when r.CveJuicioDelito = 'A102130' then 'Suspensión de patria potestad'
        when r.CveJuicioDelito = 'A103057' then 'Suspensión de patria potestad'
        when r.CveJuicioDelito = 'A103122' then 'Suspensión de régimen de visita'
        when r.CveJuicioDelito = 'A102155' then 'Terminación de copropiedad'
        when r.CveJuicioDelito = 'A102048' then 'Tercerías'
        when r.CveJuicioDelito = 'A103157' then 'Terminación de comodato'
        when r.CveJuicioDelito = 'A103158' then 'Terminación de contrato verbal'
        when r.CveJuicioDelito = 'A102068' then 'Terminación de contrato'
        when r.CveJuicioDelito = 'A102059' then 'Terminación de contrato civil'
        when r.CveJuicioDelito = 'A102040' then 'Terminación de contrato de arrendamiento'
        when r.CveJuicioDelito = 'A102054' then 'Terminación de contrato de comodato'
        when r.CveJuicioDelito = 'A103102' then 'Tramitación especial'
        when r.CveJuicioDelito = 'A102058' then 'Transacción de convenio'
        when r.CveJuicioDelito = 'A102037' then 'Usucapión'
        when r.CveJuicioDelito = 'A103159' then 'Usucapión'
        when r.CveJuicioDelito = 'A102231' then 'Usufructo vitalicio'
        when r.CveJuicioDelito = 'A103128' then 'Usufructo vitalicio'
        when r.CveJuicioDelito = 'A102198' then 'Validez de convenio'
        when r.CveJuicioDelito = 'A103160' then 'Vencimiento anticipado'
        when r.CveJuicioDelito = 'A102171' then 'Vencimiento anticipado de contrato'
        when r.CveJuicioDelito = 'A102174' then 'Vencimiento anticipado del plazo de pago de crédito'
        when r.CveJuicioDelito = 'A102224' then 'Venta y repartición de inmueble'
        when r.CveJuicioDelito = 'A103130' then 'Vía de apremio'
        when r.CveJuicioDelito = 'A102144' then 'Violencia familiar'
        when r.CveJuicioDelito = 'A103066' then 'Violencia familiar'
        when r.CveJuicioDelito = 'A102245' then 'Usucapión ordinario'
        when r.CveJuicioDelito like '%ZZZ' then 'No existe expediente'
        else 'Error' end as juicio,
        juz.NomJuzgado as juzgado
        FROM estadistica.tblinijuzgados AS ini
        LEFT JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
        INNER JOIN estadistica.tblrepjuidel As r on r.CveIni =  ini.CveIni
        WHERE 
        ter.cveini IS NULL
        AND date_format(ini.fecharad, '%Y-%m')<= @fecha
        AND juz.cvedistrito = @cveDistrito
        AND juz.NomJuzgado LIKE @materia
        AND juz.CveAdscripcion LIKE @cveAdsc
        ORDER BY fecharad;"})

        consulta.Add(11, {"Exhortos diligenciados por juzgados", "SELECT tbltotal.anio AS anio, tbltotal.mes AS mes, IFNULL(tbldatos.valor, 0) AS valor, tbltotal.valor AS total 
        FROM 
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM estadistica.tbldatadicionales AS adic
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071002','A081002','A071004','A071005','A071006','A071007')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbltotal
        LEFT JOIN
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM estadistica.tbldatadicionales AS adic
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071002','A081002')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbldatos
        ON  CONCAT(tbldatos.anio,'/',tbldatos.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
        LIMIT 4;"})

        consulta.Add(12, {"Exhortos no diligenciados por juzgados", "SELECT tbltotal.anio AS anio, tbltotal.mes AS mes, IFNULL(tbldatos.valor, 0) AS valor, tbltotal.valor AS total 
        FROM 
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM estadistica.tbldatadicionales AS adic
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071002','A081002','A071004','A071005','A071006','A071007')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbltotal
        LEFT JOIN
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM estadistica.tbldatadicionales AS adic
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071004','A071005','A071006')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbldatos
        ON  CONCAT(tbldatos.anio,'/',tbldatos.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
        LIMIT 4;"})

        consulta.Add(13, {"Exhortos pacialmente diligenciados por juzgados", "SELECT tbltotal.anio AS anio, tbltotal.mes AS mes, IFNULL(tbldatos.valor, 0) AS valor, tbltotal.valor AS total 
        FROM 
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM estadistica.tbldatadicionales AS adic
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071002','A081002','A071004','A071005','A071006','A071007')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbltotal
        LEFT JOIN
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM estadistica.tbldatadicionales AS adic
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071007')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbldatos
        ON  CONCAT(tbldatos.anio,'/',tbldatos.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
        LIMIT 4;"})

        consulta.Add(14, {"Tiempo para dictar una sentencia definitiva por juzgados", "SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes, SUM(TIMESTAMPDIFF(DAY,ini.fecharad, ter.fechater)) AS valor, count(ter.cveter) AS total
        FROM estadistica.tblterjuzgados ter
        INNER JOIN estadistica.tblinijuzgados AS ini ON ini.cveini = ter.cveini
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
        WHERE
        DATE_FORMAT(ter.fechater,'%Y-%m') <= @fechaF
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        AND ter.cveresolucion IN ('A012001','A021001','A031001')
        GROUP BY anio, mes
        ORDER BY anio DESC, mes DESC
        LIMIT 4;"})

        consulta.Add(15, {"Tiempo para dictar una sentencia definitiva por juicios", "SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes, DATE_FORMAT(ter.fechater,'%Y-%m') AS fecha, SUM(TIMESTAMPDIFF(DAY,ini.fecharad, ter.fechater)) AS valor, COUNT(DISTINCT ter.cveter) AS total
        FROM estadistica.tblcatalogos AS cat
        INNER JOIN estadistica.tblrepjuidel AS juidel ON juidel.cvejuiciodelito = CONCAT(cat.codigo, cat.rango)
        INNER JOIN estadistica.tblinijuzgados AS ini ON ini.cveini = juidel.cveini
        INNER JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.cveadscripcion = ini.cvejuzgado 
        WHERE
        DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND cat.tipo LIKE @tipo
        AND juz.cvedistrito = @idDisJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        AND cat.activo = 1
        AND ter.cveresolucion IN ('A012001','A021001','A031001')
        GROUP BY anio, mes
        ORDER BY anio DESC, mes DESC
        LIMIT 4;",
        "SELECT
        case when c.Rango = '2210' then 'Acción confesoria'
        when c.Rango = '3133' then 'Acción de obra'
        when c.Rango = '2190' then 'Acción de obra nueva'
        when c.Rango = '2240' then 'Acción negatoria'
        when c.Rango = '2202' then 'Acción oblicua'
        when c.Rango = '2241' then 'Acción pauliana'
        when c.Rango = '2220' then 'Acción rescisoria'
        when c.Rango = '3078' then 'Acreditación de concubinato'
        when c.Rango = '3079' then 'Acreditación de relación materno-filial'
        when c.Rango = '3082' then 'Acreditación del ejercicio de la patria protestad'
        when c.Rango = '2139' then 'Acreditación de concubinato'
        when c.Rango = '3084' then 'Acreditar estado civil'
        when c.Rango = '2164' then 'Acreditar relación contractual'
        when c.Rango = '2074' then 'Actos prejudiciales'
        when c.Rango = '3001' then 'Actos prejudiciales'
        when c.Rango = '2006' then 'Actos prejudiciales'
        when c.Rango = '3073' then 'Actos previos a juicio'
        when c.Rango = '2085' then 'Adopción'
        when c.Rango = '3012' then 'Adopción'
        when c.Rango = '3095' then 'Anotaciones de acta de defunción'
        when c.Rango = '2014' then 'Apeo y deslinde'
        when c.Rango = '3119' then 'Apeo y deslinde'
        when c.Rango = '3134' then 'Apeo y deslinde'
        when c.Rango = '3069' then 'Aprobar convenio'
        when c.Rango = '3115' then 'Aseguramiento de bienes'
        when c.Rango = '3105' then 'Asentamiento extemporáneo de acta'
        when c.Rango = '2145' then 'Autorización para vender y grabar bienes y transferir derechos'
        when c.Rango = '3063' then 'Autorización para vender y grabar bienes y transferir derechos'
        when c.Rango = '3074' then 'Auto declarativo'
        when c.Rango = '2126' then 'Autorización judicial'
        when c.Rango = '3053' then 'Autorización judicial'
        when c.Rango = '2086' then 'Autorización para contratar entre cónyuges'
        when c.Rango = '3013' then 'Autorización para contratar entre cónyuges'
        when c.Rango = '3083' then 'Autorización para expedir pasaporte'
        when c.Rango = '2087' then 'Autorización para salir del país'
        when c.Rango = '3014' then 'Autorización para salir del país'
        when c.Rango = '2088' then 'Autorización para venta de bienes de menor'
        when c.Rango = '3015' then 'Autorización para venta de bienes de menor'
        when c.Rango = '2089' then 'Cambio de régimen conyugal'
        when c.Rango = '3016' then 'Cambio de régimen conyugal'
        when c.Rango = '3113' then 'Cambio de régimen patrimonial'
        when c.Rango = '2193' then 'Cancelación de contrato'
        when c.Rango = '2141' then 'Cancelación de crédito'
        when c.Rango = '2216' then 'Cancelación de hipoteca'
        when c.Rango = '2098' then 'Cancelación de pensión alimenticia'
        when c.Rango = '3025' then 'Cancelación de pensión alimenticia'
        when c.Rango = '2212' then 'Celebración de asamblea'
        when c.Rango = '3085' then 'Cesación de pensión alimenticia'
        when c.Rango = '2051' then 'Concursos'
        when c.Rango = '2025' then 'Consignación'
        when c.Rango = '2056' then 'Consignación de pago'
        when c.Rango = '3135' then 'Consignación de pago'
        when c.Rango = '2119' then 'Consignación de pensión alimenticia'
        when c.Rango = '3046' then 'Consignación de pensión alimenticia'
        when c.Rango = '2136' then 'Consignación de pensión alimenticia'
        when c.Rango = '2055' then 'Consignación de renta'
        when c.Rango = '2026' then 'Consignación de rentas'
        when c.Rango = '2213' then 'Consolidación de la propiedad'
        when c.Rango = '2135' then 'Constitución de patrimonio familiar'
        when c.Rango = '3060' then 'Constitución de patrimonio familiar'
        when c.Rango = '2200' then 'Consumación de la usucapión por inscripción de la posesión'
        when c.Rango = '2209' then 'Consumación de propiedad'
        when c.Rango = '2132' then 'Controversia de orden familiar'
        when c.Rango = '3059' then 'Controversia de orden familiar'
        when c.Rango = '2225' then 'Convivencia familiar'
        when c.Rango = '3068' then 'Convivencia familiar'
        when c.Rango = '2183' then 'Copropiedad derecho del tanto'
        when c.Rango = '3136' then 'Cumplimiento de contrato'
        when c.Rango = '2179' then 'Cumplimiento de acuerdo de voluntades'
        when c.Rango = '2161' then 'Cumplimiento de carta compromiso'
        when c.Rango = '2067' then 'Cumplimiento de contrato'
        when c.Rango = '2039' then 'Cumplimiento de contrato de arrendamiento'
        when c.Rango = '2061' then 'Cumplimiento de contrato de compra venta'
        when c.Rango = '2237' then 'Cumplimiento de contrato de donación'
        when c.Rango = '2134' then 'Cumplimiento de convenio'
        when c.Rango = '3062' then 'Cumplimiento de convenio'
        when c.Rango = '2167' then 'Cumplimiento de obligación derivada de la responsabilidad civil'
        when c.Rango = '2099' then 'Custodia de menor'
        when c.Rango = '3026' then 'Custodia de menor'
        when c.Rango = '2186' then 'Daño moral'
        when c.Rango = '2232' then 'Declaración de ausencia'
        when c.Rango = '3107' then 'Declaración de ausencia'
        when c.Rango = '2090' then 'Declaración de dependencia económica'
        when c.Rango = '3017' then 'Declaración de dependencia económica'
        when c.Rango = '2114' then 'Declaración de estado de interdicción'
        when c.Rango = '3041' then 'Declaración de estado de interdicción'
        when c.Rango = '3103' then 'Declaración de estado de minoría de edad'
        when c.Rango = '3116' then 'Declaración de fallecimiento'
        when c.Rango = '2137' then 'Declaración judicial'
        when c.Rango = '3093' then 'Declaración judicial de entrega de adopción'
        when c.Rango = '3090' then 'Declaración judicial de incapacidad'
        when c.Rango = '3123' then 'Declaración judicial de patria protestad'
        when c.Rango = '2124' then 'Decretar deposito judicial'
        when c.Rango = '3051' then 'Decretar deposito judicial'
        when c.Rango = '2185' then 'Demolición de construcción'
        when c.Rango = '3080' then 'Dependencia económica'
        when c.Rango = '2010' then 'Depósito de personas'
        when c.Rango = '2075' then 'Depósito de personas'
        when c.Rango = '3002' then 'Depósito de personas'
        when c.Rango = '2188' then 'Derecho de reconocimiento de copropietario'
        when c.Rango = '2046' then 'Desahucio'
        when c.Rango = '3137' then 'Desahucio'
        when c.Rango = '2100' then 'Desconocimiento o contradicción de paternidad'
        when c.Rango = '3027' then 'Desconocimiento o contradicción de paternidad'
        when c.Rango = '2166' then 'Desocupación y entrega de inmueble'
        when c.Rango = '2182' then 'Devolución de cantidad'
        when c.Rango = '3114' then 'Devolución de pensión alimenticia'
        when c.Rango = '2063' then 'Devolución y entrega de documento.'
        when c.Rango = '3138' then 'Diligencia de apeo y deslinde'
        when c.Rango = '2229' then 'Disminución de pensión alimenticia'
        when c.Rango = '3087' then 'Disminución de pensión alimenticia'
        when c.Rango = '2204' then 'Disolución de copropiedad'
        when c.Rango = '2101' then 'Disolución de sociedad conyugal'
        when c.Rango = '3028' then 'Disolución de sociedad conyugal'
        when c.Rango = '2091' then 'Dispensa para contraer matrimonio'
        when c.Rango = '3018' then 'Dispensa para contraer matrimonio'
        when c.Rango = '2181' then 'División de la cosa común'
        when c.Rango = '2230' then 'Divorcio incausado'
        when c.Rango = '3126' then 'Divorcio incausado'
        when c.Rango = '2102' then 'Divorcio necesario'
        when c.Rango = '3029' then 'Divorcio necesario'
        when c.Rango = '2115' then 'Divorcio por mutuo consentimiento'
        when c.Rango = '3042' then 'Divorcio por mutuo consentimiento'
        when c.Rango = '2146' then 'Ejecución de convenio'
        when c.Rango = '3129' then 'Ejecución de convenio'
        when c.Rango = '2173' then 'Ejecución de garantía'
        when c.Rango = '2180' then 'Ejecución de laudo arbitral'
        when c.Rango = '3096' then 'Ejecución de sentencia'
        when c.Rango = '2057' then 'Ejecutivo civil'
        when c.Rango = '2002' then 'Ejecutivo mercantil'
        when c.Rango = '3121' then 'Ejecutivo mercantil'
        when c.Rango = '3161' then 'Ejecutivo mercantil'
        when c.Rango = '3086' then 'Elevar convenio de cosa juzgada'
        when c.Rango = '2157' then 'Entrega de administración'
        when c.Rango = '2158' then 'Entrega de estructura metálica'
        when c.Rango = '2092' then 'Entrega de menor a institución de asistencia'
        when c.Rango = '3019' then 'Entrega de menor a institución de asistencia'
        when c.Rango = '2163' then 'Entrega posesión del inmueble'
        when c.Rango = '2223' then 'Entrega real de titulo'
        when c.Rango = '2217' then 'Especial de fianza'
        when c.Rango = '2027' then 'Especial hipotecario'
        when c.Rango = '2175' then 'Especial mercantil'
        when c.Rango = '3076' then 'Estado de interdicción'
        when c.Rango = '3108' then 'Exclusión de bienes inmuebles'
        when c.Rango = '2195' then 'Firma de factura'
        when c.Rango = '2123' then 'Firma y contrato de donación'
        when c.Rango = '3050' then 'Firma y contrato de donación'
        when c.Rango = '2052' then 'Funciones de sindico'
        when c.Rango = '2120' then 'Guarda y custodia de menor'
        when c.Rango = '3047' then 'Guarda y custodia de menor'
        when c.Rango = '2143' then 'Homologación de sentencia'
        when c.Rango = '3065' then 'Homologación de sentencia'
        when c.Rango = '2093' then 'Identidad de personas'
        when c.Rango = '3020' then 'Identidad de personas'
        when c.Rango = '3088' then 'Incapacidad para heredar'
        when c.Rango = '2154' then 'Incidente de liquidación'
        when c.Rango = '2214' then 'Incidente para convocar asamblea'
        when c.Rango = '3094' then 'Incluir bien a la sociedad conyugal'
        when c.Rango = '2103' then 'Incremento de pensión alimenticia'
        when c.Rango = '3030' then 'Incremento de pensión alimenticia'
        when c.Rango = '2172' then 'Incumplimiento de contrato'
        when c.Rango = '3091' then 'Ineficiencia parcial del testamento público'
        when c.Rango = '3139' then 'Interdicto'
        when c.Rango = '2206' then 'Inexistencia de contrato'
        when c.Rango = '3140' then 'Inexistencia de donación'
        when c.Rango = '3141' then 'Información de dominio'
        when c.Rango = '2128' then 'Información ad-perpetuam'
        when c.Rango = '3055' then 'Información ad-perpetuam'
        when c.Rango = '2015' then 'Información ad-perpetuam'
        when c.Rango = '2016' then 'Información de domino'
        when c.Rango = '2017' then 'Información posesoria'
        when c.Rango = '2117' then 'Información testimonial'
        when c.Rango = '3044' then 'Información testimonial'
        when c.Rango = '2018' then 'Inmatriculación'
        when c.Rango = '3142' then 'Inmatriculación'
        when c.Rango = '3106' then 'Impugnación de testamento'
        when c.Rango = '2028' then 'Interdictos'
        when c.Rango = '2065' then 'Interpelación judicial'
        when c.Rango = '3143' then 'Interpelación judicial'
        when c.Rango = '2029' then 'Jactancia'
        when c.Rango = '2047' then 'Juicio arbitral'
        when c.Rango = '2243' then 'Juicio ejecutivo mercantil oral'
        when c.Rango = '2001' then 'Juicio mercantil'
        when c.Rango = '2005' then 'Jurisdicción contenciosa'
        when c.Rango = '2050' then 'Jurisdicción mixta'
        when c.Rango = '2080' then 'Jurisdicción mixta'
        when c.Rango = '3007' then 'Jurisdicción mixta'
        when c.Rango = '2013' then 'Jurisdicción voluntaria'
        when c.Rango = '2084' then 'Jurisdicción voluntaria'
        when c.Rango = '3011' then 'Jurisdicción voluntaria'
        when c.Rango = '2169' then 'Liberación de reserva del dominio'
        when c.Rango = '2122' then 'Licencia para contratar'
        when c.Rango = '3049' then 'Licencia para contratar'
        when c.Rango = '3071' then 'Liquidación de pensión adeudas'
        when c.Rango = '3070' then 'Liquidación de sociedad'
        when c.Rango = '3144' then 'Medios preparatorios a juicio'
        when c.Rango = '3145' then 'Medios preparatorios a juicio reivindicatorio'
        when c.Rango = '3162' then 'Medios preparatorios a juicio ejecutivo mercantil'
        when c.Rango = '2152' then 'Medios preparatorios a juicio ejecutivo civil'
        when c.Rango = '2151' then 'Medios preparatorios a juicio ejecutivo mercantil'
        when c.Rango = '2007' then 'Medios preparatorios a juicio'
        when c.Rango = '2076' then 'Medios preparatorios a juicio'
        when c.Rango = '3003' then 'Medios preparatorios a juicio'
        when c.Rango = '2149' then 'Medios preparatorios a juicio ordinario civil'
        when c.Rango = '2150' then 'Medios preparatorios a juicio ordinario mercantil'
        when c.Rango = '2133' then 'Modificación de convenio'
        when c.Rango = '3061' then 'Modificación de convenio'
        when c.Rango = '3120' then 'Modificación de régimen de convivencia'
        when c.Rango = '3097' then 'Modificar resolución'
        when c.Rango = '2ZZZ' then 'No existe expediente'
        when c.Rango = '3ZZZ' then 'No existe expediente'
        when c.Rango = '2094' then 'Nombramiento de tutores y curadores'
        when c.Rango = '3021' then 'Nombramiento de tutores y curadores'
        when c.Rango = '2019' then 'Notificación judicial'
        when c.Rango = '2121' then 'Notificación judicial'
        when c.Rango = '3048' then 'Notificación judicial'
        when c.Rango = '2162' then 'Novación de contrato de arrendamiento'
        when c.Rango = '2222' then 'Nulidad absoluta de acto jurídico'
        when c.Rango = '3146' then 'Nulidad de acta de asamblea'
        when c.Rango = '2129' then 'Nulidad de acta de nacimiento'
        when c.Rango = '3056' then 'Nulidad de acta de nacimiento'
        when c.Rango = '3072' then 'Nulidad de actuaciones'
        when c.Rango = '2191' then 'Nulidad de asamblea'
        when c.Rango = '3100' then 'Nulidad de cesión de derechos hereditarios'
        when c.Rango = '2073' then 'Nulidad de contrato'
        when c.Rango = '3147' then 'Nulidad de contrato'
        when c.Rango = '2208' then 'Nulidad de contrato de arrendamiento'
        when c.Rango = '2064' then 'Nulidad de contrato de compra venta'
        when c.Rango = '2177' then 'Nulidad de convenio'
        when c.Rango = '2140' then 'Nulidad de escritura'
        when c.Rango = '3109' then 'Nulidad de escritura'
        when c.Rango = '2207' then 'Nulidad de inscripción'
        when c.Rango = '2038' then 'Nulidad de juicio'
        when c.Rango = '2118' then 'Nulidad de juicio'
        when c.Rango = '3045' then 'Nulidad de juicio'
        when c.Rango = '3148' then 'Nulidad de juicio concluido'
        when c.Rango = '2104' then 'Nulidad de matrimonio'
        when c.Rango = '3031' then 'Nulidad de matrimonio'
        when c.Rango = '2105' then 'Nulidad de testamento'
        when c.Rango = '3032' then 'Nulidad de testamento'
        when c.Rango = '2201' then 'Nulidad de título de propiedad'
        when c.Rango = '2226' then 'Oral mercantil'
        when c.Rango = '3163' then 'Oral mercantil'
        when c.Rango = '2097' then 'Ordinario civil'
        when c.Rango = '3024' then 'Ordinario civil'
        when c.Rango = '2024' then 'Ordinario civil o verbal'
        when c.Rango = '2242' then 'Extinción de dominio'
        when c.Rango = '2003' then 'Ordinario mercantil'
        when c.Rango = '3164' then 'Ordinario mercantil'
        when c.Rango = '3149' then 'Otorgamiento y firma de escritura'
        when c.Rango = '2168' then 'Otorgamiento de contrato de prestación de servicio'
        when c.Rango = '2238' then 'Otorgamiento de título o derecho de perpetuidad'
        when c.Rango = '2184' then 'Otorgamiento y firma de contrato'
        when c.Rango = '3150' then 'Otorgamiento y firma de escrito'
        when c.Rango = '2030' then 'Otorgamiento y firma de escritura'
        when c.Rango = '2004' then 'Otros'
        when c.Rango = '2012' then 'Otros'
        when c.Rango = '2023' then 'Otros'
        when c.Rango = '2044' then 'Otros'
        when c.Rango = '2049' then 'Otros'
        when c.Rango = '2053' then 'Otros'
        when c.Rango = '2079' then 'Otros'
        when c.Rango = '2083' then 'Otros'
        when c.Rango = '2096' then 'Otros'
        when c.Rango = '2112' then 'Otros'
        when c.Rango = '2116' then 'Otros'
        when c.Rango = '3006' then 'Otros'
        when c.Rango = '3010' then 'Otros'
        when c.Rango = '3023' then 'Otros'
        when c.Rango = '3039' then 'Otros'
        when c.Rango = '3043' then 'Otros'
        when c.Rango = '3101' then 'Pago de cantidad'
        when c.Rango = '2031' then 'Pago de daños'
        when c.Rango = '3098' then 'Pago de gastos hospitalarios'
        when c.Rango = '2218' then 'Pago de honorarios'
        when c.Rango = '2032' then 'Pago de pesos'
        when c.Rango = '3151' then 'Pago de pesos'
        when c.Rango = '2106' then 'Pensión alimenticia'
        when c.Rango = '3033' then 'Pensión alimenticia'
        when c.Rango = '2107' then 'Perdida de patria potestad'
        when c.Rango = '3034' then 'Perdida de patria potestad'
        when c.Rango = '2108' then 'Petición de herencia'
        when c.Rango = '3035' then 'Petición de herencia'
        when c.Rango = '2033' then 'Plenaria de posesión'
        when c.Rango = '3152' then 'Plenario de posesión'
        when c.Rango = '3022' then 'Posesión de estado de hijo'
        when c.Rango = '2095' then 'Posesión de estado de hijo'
        when c.Rango = '3153' then 'Posesión de vehículo'
        when c.Rango = '2008' then 'Preliminares de consignación'
        when c.Rango = '2077' then 'Preliminares de consignación'
        when c.Rango = '3004' then 'Preliminares de consignación'
        when c.Rango = '2011' then 'Preparación a juicio arbitral'
        when c.Rango = '2034' then 'Prescripción'
        when c.Rango = '2176' then 'Prescripción de la acción hipotecaria'
        when c.Rango = '2189' then 'Prescripción positiva'
        when c.Rango = '2233' then 'Presunción de muerte'
        when c.Rango = '3131' then 'Presunción de muerte'
        when c.Rango = '2113' then 'Procedimiento especial'
        when c.Rango = '3040' then 'Procedimiento especial'
        when c.Rango = '2148' then 'Procedimiento especial mercantil'
        when c.Rango = '2205' then 'Procedimiento incidental'
        when c.Rango = '2170' then 'Procedimiento judicial de ejecución de garantías'
        when c.Rango = '2147' then 'Procedimiento judicial no contencioso'
        when c.Rango = '3077' then 'Procedimiento judicial no contencioso'
        when c.Rango = '2045' then 'Procedimientos especiales'
        when c.Rango = '2069' then 'Prorroga de contrato'
        when c.Rango = '2041' then 'Prorroga de contrato de arrendamiento'
        when c.Rango = '2228' then 'Protocolización de régimen de propiedad'
        when c.Rango = '2009' then 'Providencias precautorias'
        when c.Rango = '2078' then 'Providencias precautorias'
        when c.Rango = '3005' then 'Providencias precautorias'
        when c.Rango = '2071' then 'Quiebras y suspensión de pagos'
        when c.Rango = '2194' then 'Ratificación de contrato'
        when c.Rango = '2022' then 'Ratificación de convenio'
        when c.Rango = '3127' then 'Ratificación de convenio'
        when c.Rango = '2235' then 'Reconducción de contrato de arrendamiento'
        when c.Rango = '3125' then 'Reconocimiento de beneficiarios'
        when c.Rango = '3092' then 'Reconocimiento de bienes de sociedad conyugal'
        when c.Rango = '2192' then 'Reconocimiento de contrato'
        when c.Rango = '2109' then 'Reconocimiento de hijo'
        when c.Rango = '3036' then 'Reconocimiento de hijo'
        when c.Rango = '3111' then 'Reconocimiento de identidad'
        when c.Rango = '3110' then 'Reconocimiento de maternidad'
        when c.Rango = '2203' then 'Reconocimiento de pago'
        when c.Rango = '3075' then 'Reconocimiento de paternidad'
        when c.Rango = '2187' then 'Reconocimiento de personalidad'
        when c.Rango = '2110' then 'Rectificación de acta'
        when c.Rango = '3037' then 'Rectificación de acta'
        when c.Rango = '2196' then 'Rectificación de asiento registral'
        when c.Rango = '2131' then 'Rectificación de estado civil'
        when c.Rango = '3058' then 'Rectificación de estado civil'
        when c.Rango = '2160' then 'Rectificación de medida'
        when c.Rango = '2197' then 'Rectificación de propiedades'
        when c.Rango = '2111' then 'Reducción de pensión alimenticia'
        when c.Rango = '3038' then 'Reducción de pensión alimenticia'
        when c.Rango = '2219' then 'Reducción de precio'
        when c.Rango = '2127' then 'Régimen de visitas'
        when c.Rango = '3054' then 'Régimen de visitas'
        when c.Rango = '2066' then 'Registro de escritura'
        when c.Rango = '2020' then 'Registro de sociedad'
        when c.Rango = '3117' then 'Registro extemporáneo de acta'
        when c.Rango = '3099' then 'Registro extemporáneo de acta de defunción'
        when c.Rango = '2159' then 'Regularización de solar urbano'
        when c.Rango = '2125' then 'Reincorporación de menores'
        when c.Rango = '3052' then 'Reincorporación de menores'
        when c.Rango = '2035' then 'Reivindicatoria'
        when c.Rango = '3067' then 'Remoción de albacea'
        when c.Rango = '2178' then 'Rendición de cuentas'
        when c.Rango = '2234' then 'Rendición de cuentas de sociedad conyugal'
        when c.Rango = '3132' then 'Rendición de cuentas de sociedad conyugal'
        when c.Rango = '2215' then 'Reparación de daño moral'
        when c.Rango = '2165' then 'Reparación e indemnización de daños y perjuicios'
        when c.Rango = '2062' then 'Requerimiento de pago'
        when c.Rango = '2070' then 'Rescisión de contrato'
        when c.Rango = '3124' then 'Rescisión de contrato'
        when c.Rango = '3154' then 'Rescisión de contrato'
        when c.Rango = '2042' then 'Rescisión de contrato de arrendamiento'
        when c.Rango = '2060' then 'Rescisión de contrato de compraventa'
        when c.Rango = '2153' then 'Rescisión de convenio'
        when c.Rango = '3112' then 'Rescisión de convenio'
        when c.Rango = '2072' then 'Responsabilidad civil'
        when c.Rango = '3118' then 'Restitución de bien'
        when c.Rango = '2211' then 'Restitución de fracción de terreno'
        when c.Rango = '2221' then 'Restitución de la posesión de bien mueble'
        when c.Rango = '2199' then 'Restitución y pago de lo indebido'
        when c.Rango = '3155' then 'Reivindicatorio'
        when c.Rango = '2043' then 'Revisión de contrato de arrendamiento'
        when c.Rango = '2156' then 'Revocación de contrato de donación'
        when c.Rango = '3104' then 'Revocación de donación'
        when c.Rango = '3156' then 'Revocación de donación'
        when c.Rango = '2142' then 'Revocatoria a asamblea'
        when c.Rango = '3064' then 'Revocatoria a asamblea'
        when c.Rango = '3081' then 'Separación de personas'
        when c.Rango = '2036' then 'Servidumbre de paso'
        when c.Rango = '2239' then 'Servidumbre legal de desagüe'
        when c.Rango = '3089' then 'Solicitud de legalidad de sentencia'
        when c.Rango = '2021' then 'Solicitud de orden judicial'
        when c.Rango = '2227' then 'Subrogación de derechos'
        when c.Rango = '2081' then 'Sucesorio intestamentario'
        when c.Rango = '3008' then 'Sucesorio intestamentario'
        when c.Rango = '2082' then 'Sucesorio testamentario'
        when c.Rango = '3009' then 'Sucesorio testamentario'
        when c.Rango = '2236' then 'Suspensión de obra nueva'
        when c.Rango = '2130' then 'Suspensión de patria potestad'
        when c.Rango = '3057' then 'Suspensión de patria potestad'
        when c.Rango = '3122' then 'Suspensión de régimen de visita'
        when c.Rango = '2155' then 'Terminación de copropiedad'
        when c.Rango = '2048' then 'Tercerías'
        when c.Rango = '3157' then 'Terminación de comodato'
        when c.Rango = '3158' then 'Terminación de contrato verbal'
        when c.Rango = '2068' then 'Terminación de contrato'
        when c.Rango = '2059' then 'Terminación de contrato civil'
        when c.Rango = '2040' then 'Terminación de contrato de arrendamiento'
        when c.Rango = '2054' then 'Terminación de contrato de comodato'
        when c.Rango = '3102' then 'Tramitación especial'
        when c.Rango = '2058' then 'Transacción de convenio'
        when c.Rango = '2037' then 'Usucapión'
        when c.Rango = '3159' then 'Usucapión'
        when c.Rango = '2231' then 'Usufructo vitalicio'
        when c.Rango = '3128' then 'Usufructo vitalicio'
        when c.Rango = '2198' then 'Validez de convenio'
        when c.Rango = '3160' then 'Vencimiento anticipado'
        when c.Rango = '2171' then 'Vencimiento anticipado de contrato'
        when c.Rango = '2174' then 'Vencimiento anticipado del plazo de pago de crédito'
        when c.Rango = '2224' then 'Venta y repartición de inmueble'
        when c.Rango = '3130' then 'Vía de apremio'
        when c.Rango = '2144' then 'Violencia familiar'
        when c.Rango = '3066' then 'Violencia familiar'
        when c.Rango = '2245' then 'Usucapión ordinario'
        when c.Rango like '%ZZZ' then 'No existe expediente'
        else 'Error' end as juicio 
        , COUNT(DISTINCT ter.cveter) AS totaljuicios
        , AVG(TIMESTAMPDIFF(DAY, ini.fecharad, ter.fechater)) AS valor
        FROM estadistica.tblcatalogos AS c
        INNER JOIN estadistica.tblrepjuidel AS juidel ON juidel.cvejuiciodelito = CONCAT(c.codigo, c.rango)
        INNER JOIN estadistica.tblinijuzgados AS ini ON ini.cveini = juidel.cveini
        INNER JOIN estadistica.tblterjuzgados AS ter ON ter.cveini = ini.cveini
        INNER JOIN estadistica.tbljuzgados AS juz ON juz.cveadscripcion = ini.cvejuzgado 
        WHERE
        DATE_FORMAT(ter.fechater,'%Y-%m') = @fechaF
        AND c.tipo LIKE @tipo
        AND juz.cvedistrito = @idDisJuz
        AND juz.CveAdscripcion LIKE @cveJuzgado
        AND c.activo = 1
        AND ter.cveresolucion IN ('A012001','A021001','A031001')
        GROUP BY juicio
        ORDER BY juicio;"})

        consulta.Add(17, {"Iniciados contra Concluidos",
        "select ini.anio as anio, ini.mes as mes, fecha, iniciados as valor, terminados as total
        from
        (select Year(i.FechaRad) anio, Month(i.FechaRad) mes, DATE_FORMAT(i.FechaRad,'%Y-%m') as fecha,count(distinct i.cveini) as iniciados 
        from estadistica.tblinijuzgados i
        inner join estadistica.tbljuzgados j  on j.CveAdscripcion = i.CveJuzgado
        where date_format(FechaRad, '%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        and j.CveDistrito = @idDisJuz
        and j.NomJuzgado like @nomJuz
        and j.CveAdscripcion like @cveJuzgado
        group by anio, mes) ini 
        left join
        (select Year(t.FechaTer) anio, Month(t.FechaTer) mes, count(distinct t.cveter) as terminados 
        from estadistica.tblterjuzgados t
        inner join estadistica.tbljuzgados j  on j.CveAdscripcion = t.CveJuzgado
        where date_format(FechaTer, '%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        and j.CveDistrito = @idDisJuz
        and j.NomJuzgado like @nomJuz
        and j.CveAdscripcion like @cveJuzgado
        AND t.CveResolucion != 'A031002'
        group by anio, mes) ter on ter.anio = ini.anio and ter.mes = ini.mes
        Order by anio desc, mes desc LIMIT 4;",
        "SELECT
        CASE
        WHEN t.CveResolucion = 'A031001' THEN 'Definitiva'
        WHEN t.CveResolucion = 'A031002' THEN 'Interlocutoria'
        WHEN t.CveResolucion = 'A032001' THEN 'Caducidad'
        WHEN t.CveResolucion = 'A032002' THEN 'Desistimiento'
        WHEN t.CveResolucion = 'A032003' THEN 'Convenio'
        WHEN t.CveResolucion = 'A032004' THEN 'Incompetencia'
        WHEN t.CveResolucion = 'A032005' THEN 'Otros'
        WHEN t.CveResolucion = 'A032006' THEN 'Falta de requisitos legales y/o formales'
        WHEN t.CveResolucion = 'A032007' THEN 'Desahogo de prevención'
        WHEN t.CveResolucion = 'A032008' THEN 'Cumple fin'
        WHEN t.CveResolucion = 'A032009' THEN 'Firma'
        WHEN t.CveResolucion = 'A032010' THEN 'Traslado'
        WHEN t.CveResolucion = 'A032011' THEN 'Falta de personalidad'
        WHEN t.CveResolucion = 'A032012' THEN 'Incompetencia por cuantía'
        WHEN t.CveResolucion = 'A032013' THEN 'Proceso restaurativo'
        END Resolucion, 
        COUNT(DISTINCT t.cveter) as Total 
        FROM estadistica.tblterjuzgados t
        INNER JOIN estadistica.tbljuzgados j ON j.CveAdscripcion = t.CveJuzgado
        WHERE DATE_FORMAT(FechaTer, '%Y-%m') = @fechaF
        AND j.CveDistrito = @idDisJuz
        AND j.NomJuzgado LIKE @nomJuz
        AND j.CveAdscripcion LIKE @cveJuzgado
        AND t.CveResolucion != 'A031002'
        GROUP BY Resolucion
        ORDER BY t.CveResolucion;"})

    End Sub


End Class