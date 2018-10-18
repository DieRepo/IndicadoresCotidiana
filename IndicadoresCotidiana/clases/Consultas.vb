Public Class Consultas

    Public consulta As Hashtable = New Hashtable()

    Public Sub New()
        consulta.Add(1, {"Iniciados por juzgados", "SELECT YEAR(inijuz.fecharad) AS anio, MONTH(inijuz.fecharad) AS mes, count(distinct cveini) AS valor 
                    FROM tblinijuzgados AS inijuz 
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = inijuz.cvejuzgado 
                    WHERE 
                    DATE_FORMAT(inijuz.fecharad,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    GROUP BY anio, mes 
                    ORDER BY anio DESC, mes DESC 
                    LIMIT 4;"})

        consulta.Add(3, {"Sentencias definitivas por juzgados", "SELECT IFNULL(tblfil.anio,tbltotal.anio) AS anio, IFNULL(tblfil.mes,tbltotal.mes) AS mes, IFNULL(tblfil.valor,0) AS valor, tbltotal.valor AS total FROM 
	                    (SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes,  
		                    count(distinct ter.cveter) as valor
		                    FROM tblterjuzgados AS ter
		                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ter.cvejuzgado 
		                    WHERE
		                    ter.cveresolucion in ('A031001','A031002')
		                    AND DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
		                    AND juz.cvedistrito = @idDisJuz
                            AND juz.NomJuzgado LIKE @nomJuz
		                    GROUP BY anio, mes
		                    ORDER BY anio DESC, mes DESC
	                    ) AS tbltotal
	                    LEFT JOIN
	                    (
		                    SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes,  
		                    count(distinct ter.cveter) as valor
		                    FROM tblterjuzgados AS ter
		                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ter.cvejuzgado 
		                    WHERE
		                    ter.cveresolucion = 'A031001'
		                    AND DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
		                    AND juz.cvedistrito = @idDisJuz
                            AND juz.NomJuzgado LIKE @nomJuz
		                    GROUP BY anio, mes
		                    ORDER BY anio DESC, mes DESC
	                    ) AS tblfil ON CONCAT(tblfil.anio,'/',tblfil.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
	                    LIMIT 4;"})

        consulta.Add(4, {"Sentencias interlocutorias por juzgados", "SELECT IFNULL(tblfil.anio,tbltotal.anio) AS anio, IFNULL(tblfil.mes,tbltotal.mes) AS mes, IFNULL(tblfil.valor,0) AS valor, tbltotal.valor AS total FROM 
	                    (SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes,  
		                    count(distinct ter.cveter) as valor
		                    FROM tblterjuzgados AS ter
		                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ter.cvejuzgado 
		                    WHERE
		                    ter.cveresolucion in ('A031001','A031002')
		                    AND DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
		                    AND juz.cvedistrito = @idDisJuz
                            AND juz.NomJuzgado LIKE @nomJuz
		                    GROUP BY anio, mes
		                    ORDER BY anio DESC, mes DESC
	                    ) AS tbltotal
	                    LEFT JOIN
	                    (
		                    SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes,  
		                    count(distinct ter.cveter) as valor
		                    FROM tblterjuzgados AS ter
		                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ter.cvejuzgado 
		                    WHERE
		                    ter.cveresolucion = 'A031002'
		                    AND DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
		                    AND juz.cvedistrito = @idDisJuz
                            AND juz.NomJuzgado LIKE @nomJuz
		                    GROUP BY anio, mes
		                    ORDER BY anio DESC, mes DESC
	                    ) AS tblfil ON CONCAT(tblfil.anio,'/',tblfil.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
	                    LIMIT 4;"})

        consulta.Add(7, {"Audiencias celebradas por juzgados", "SELECT (tot.anio) AS anio, (tot.mes) AS mes, (datos.datosT) AS valor, (tot.total) AS total
                    FROM 
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) total
                    FROM tbldatadicionales AS dat
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072001','A072002')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m') 
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS tot
                    LEFT JOIN                            
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) datosT
                    FROM tbldatadicionales AS dat
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072001')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m') 
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS datos ON CONCAT(datos.anio,'/',datos.mes) = CONCAT(tot.anio,'/',tot.mes)
                    LIMIT 4;"})

        consulta.Add(8, {"Audencias no celebradas por juzgados", "SELECT (tot.anio) AS anio, (tot.mes) AS mes, (datos.datosT) AS valor, (tot.total) AS total
                    FROM 
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) total
                    FROM tbldatadicionales AS dat
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072001', 'A072002')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS tot
                    LEFT JOIN                            
                    (SELECT YEAR(dat.fechacap) AS anio, MONTH(dat.fechacap) AS mes, SUM(dat.total) datosT
                    FROM tbldatadicionales AS dat
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = dat.cvejuzgado
                    WHERE
                    dat.cvedatadi IN ('A072002')
                    AND DATE_FORMAT(dat.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m') 
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    GROUP BY anio, mes
                    ORDER BY anio DESC, mes DESC
                    ) AS datos ON CONCAT(datos.anio,'/',datos.mes) = CONCAT(tot.anio,'/',tot.mes)
                    LIMIT 4;"})

        consulta.Add(9, {"Tiempo promedio rezago por juzgados", "SELECT YEAR(SUBDATE(@fechaF, INTERVAL 0 MONTH)) AS anio, MONTH(SUBDATE(@fechaF, INTERVAL 0 MONTH)) AS mes, SUM(TIMESTAMPDIFF(HOUR,t.fechaRadST, SUBDATE(@fechaF, INTERVAL 0 MONTH))) AS valor, (f.total) AS total
                    FROM 
                    (SELECT ini.fecharad AS fechaRadST
                    FROM tblinijuzgados AS ini
                    LEFT JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND ini.fecharad <= SUBDATE(@fechaF, INTERVAL 0 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    ) AS t, 
                    (SELECT COUNT(ini.cveini) as total
                    FROM tblinijuzgados AS ini
                    INNER JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ini.fecharad <= SUBDATE(@fechaF, INTERVAL 0 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    ) AS f
                    UNION
                    SELECT YEAR(SUBDATE(@fechaF, INTERVAL 1 MONTH)) AS anio, MONTH(SUBDATE(@fechaF, INTERVAL 1 MONTH)) AS mes, SUM(TIMESTAMPDIFF(HOUR,t.fechaRadST, SUBDATE(@fechaF, INTERVAL 1 MONTH))) AS valor, (f.total) AS total
                    FROM 
                    (SELECT ini.fecharad AS fechaRadST
                    FROM tblinijuzgados AS ini
                    LEFT JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND ini.fecharad <= SUBDATE(@fechaF, INTERVAL 1 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    ) AS t, 
                    (SELECT COUNT(ini.cveini) as total
                    FROM tblinijuzgados AS ini
                    INNER JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ini.fecharad <= SUBDATE(@fechaF, INTERVAL 1 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    ) AS f
                    UNION
                    SELECT YEAR(SUBDATE(@fechaF, INTERVAL 2 MONTH)) AS anio, MONTH(SUBDATE(@fechaF, INTERVAL 2 MONTH)) AS mes, SUM(TIMESTAMPDIFF(HOUR,t.fechaRadST, SUBDATE(@fechaF, INTERVAL 2 MONTH))) AS valor, (f.total) AS total
                    FROM 
                    (SELECT ini.fecharad AS fechaRadST
                    FROM tblinijuzgados AS ini
                    LEFT JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND ini.fecharad <= SUBDATE(@fechaF, INTERVAL 2 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    ) AS t, 
                    (SELECT COUNT(ini.cveini) as total
                    FROM tblinijuzgados AS ini
                    INNER JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ini.fecharad <= SUBDATE(@fechaF, INTERVAL 2 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    ) AS f
                    UNION
                    SELECT YEAR(SUBDATE(@fechaF, INTERVAL 3 MONTH)) AS anio, MONTH(SUBDATE(@fechaF, INTERVAL 3 MONTH)) AS mes, SUM(TIMESTAMPDIFF(HOUR,t.fechaRadST, SUBDATE(@fechaF, INTERVAL 3 MONTH))) AS valor, (f.total) AS total
                    FROM 
                    (SELECT ini.fecharad AS fechaRadST
                    FROM tblinijuzgados AS ini
                    LEFT JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ter.cveini IS NULL
                    AND ini.fecharad <= SUBDATE(@fechaF, INTERVAL 3 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    ) AS t, 
                    (SELECT COUNT(ini.cveini) as total
                    FROM tblinijuzgados AS ini
                    INNER JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
                    INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
                    WHERE 
                    ini.fecharad <= SUBDATE(@fechaF, INTERVAL 3 MONTH)
                    AND juz.cvedistrito = @idDisJuz
                    AND juz.NomJuzgado LIKE @nomJuz
                    ) AS f"})

        consulta.Add(11, {"Exhortos diligenciados por juzgados", "SELECT tbltotal.anio AS anio, tbltotal.mes AS mes, IFNULL(tbldatos.valor, 0) AS valor, tbltotal.valor AS total 
        FROM 
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM tbldatadicionales AS adic
        INNER JOIN tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071002','A081002','A071004','A071005','A071006','A071007')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbltotal
        LEFT JOIN
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM tbldatadicionales AS adic
        INNER JOIN tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071002','A081002')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbldatos
        ON  CONCAT(tbldatos.anio,'/',tbldatos.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
        LIMIT 4;"})

        consulta.Add(12, {"Exhortos no diligenciados por juzgados", "SELECT tbltotal.anio AS anio, tbltotal.mes AS mes, IFNULL(tbldatos.valor, 0) AS valor, tbltotal.valor AS total 
        FROM 
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM tbldatadicionales AS adic
        INNER JOIN tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071002','A081002','A071004','A071005','A071006','A071007')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbltotal
        LEFT JOIN
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM tbldatadicionales AS adic
        INNER JOIN tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071004','A071005','A071006')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbldatos
        ON  CONCAT(tbldatos.anio,'/',tbldatos.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
        LIMIT 4;"})

        consulta.Add(13, {"Exhortos pacialmente diligenciados por juzgados", "SELECT tbltotal.anio AS anio, tbltotal.mes AS mes, IFNULL(tbldatos.valor, 0) AS valor, tbltotal.valor AS total 
        FROM 
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM tbldatadicionales AS adic
        INNER JOIN tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071002','A081002','A071004','A071005','A071006','A071007')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbltotal
        LEFT JOIN
        (SELECT YEAR(adic.fechacap) AS anio, MONTH(adic.fechacap) AS mes, sum(adic.total) AS valor FROM tbldatadicionales AS adic
        INNER JOIN tbljuzgados AS juz ON juz.cveadscripcion = adic.cvejuzgado 
        WHERE
        adic.cvedatadi IN('A071007')
        AND  DATE_FORMAT(adic.fechacap,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        GROUP BY anio, mes 
        ORDER BY anio DESC, mes DESC) AS tbldatos
        ON  CONCAT(tbldatos.anio,'/',tbldatos.mes) = CONCAT(tbltotal.anio,'/',tbltotal.mes)
        LIMIT 4;"})

        consulta.Add(14, {"Tiempo para dictar una sentencia definitiva por juzgados", "SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes, SUM(TIMESTAMPDIFF(HOUR,ini.fecharad, ter.fechater)) AS valor, count(ter.cveter) AS total
        FROM tblterjuzgados ter
        INNER JOIN tblinijuzgados AS ini ON ini.cveini = ter.cveini
        INNER JOIN tbljuzgados AS juz ON juz.CveAdscripcion = ini.CveJuzgado
        WHERE
        DATE_FORMAT(ter.fechater,'%Y-%m') <= @fechaF
        AND juz.cvedistrito = @idDisJuz
        AND juz.NomJuzgado LIKE @nomJuz
        AND ter.cveresolucion IN ('A012001','A021001','A031001')
        GROUP BY anio, mes
        ORDER BY anio DESC, mes DESC
        LIMIT 4;"})

        consulta.Add(15, {"Tiempo para dictar una sentencia definitiva por juicios", "SELECT YEAR(ter.fechater) AS anio, MONTH(ter.fechater) AS mes, DATE_FORMAT(ter.fechater,'%Y-%m') AS fecha, SUM(TIMESTAMPDIFF(HOUR,ini.fecharad, ter.fechater)) AS valor, COUNT(DISTINCT ter.cveter) AS total
        FROM tblcatalogos AS cat
        INNER JOIN tblrepjuidel AS juidel ON juidel.cvejuiciodelito = CONCAT(cat.codigo, cat.rango)
        INNER JOIN tblinijuzgados AS ini ON ini.cveini = juidel.cveini
        INNER JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
        INNER JOIN tbljuzgados AS juz ON juz.cveadscripcion = ini.cvejuzgado 
        WHERE
        DATE_FORMAT(ter.fechater,'%Y-%m') <= DATE_FORMAT(@fechaF,'%Y-%m')
        AND cat.tipo LIKE @tipo
        AND juz.cvedistrito = @idDisJuz
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
        , SUM(TIMESTAMPDIFF(HOUR, ini.fecharad, ter.fechater)) AS valor
        FROM tblcatalogos AS c
        INNER JOIN tblrepjuidel AS juidel ON juidel.cvejuiciodelito = CONCAT(c.codigo, c.rango)
        INNER JOIN tblinijuzgados AS ini ON ini.cveini = juidel.cveini
        INNER JOIN tblterjuzgados AS ter ON ter.cveini = ini.cveini
        INNER JOIN tbljuzgados AS juz ON juz.cveadscripcion = ini.cvejuzgado 
        WHERE
        DATE_FORMAT(ter.fechater,'%Y-%m') = @fechaF
        AND c.tipo LIKE @tipo
        AND juz.cvedistrito = @idDisJuz
        AND c.activo = 1
        AND ter.cveresolucion IN ('A012001','A021001','A031001')
        GROUP BY juicio
        ORDER BY juicio;"})


    End Sub


End Class
