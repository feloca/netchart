//consideraciones
//no valido que ya este definida nc
//chuleta element
//https://www.w3schools.com/jsref/dom_obj_all.asp

//Problema espacios de nombre
//https://stackoverflow.com/questions/17520337/dynamically-rendered-svg-is-not-displaying

!function (window) {
    console.log('cargo netchart');
    nc = {
        version: 1.0
    };

    //Espacio de nombre de svg
    const nc_svgns = 'http://www.w3.org/2000/svg';
    var nc_document = window.document;
    //Contenedor donde se va a incrustar el gráfico
    var nc_selection = null;

    //Estos valores deben corresponderse con el enumerado ChartTypeEnum
    const nc_types = ['Debug', 'Bar', 'Line', 'Scatter', 'Bubble', 'Temperature', 'Pie', 'Radar'];

    //Paleta de colores por defecto -> 14
    const nc_colors = ['navy', 'blue', 'aqua', 'teal', 'olive', 'green', 'lime', 'yellow', 'orange', 'red', 'maroon', 'fuchsia', 'purple', 'gray'];

    //Color actual de la paleta
    var nc_currentColor = 0;

    //funcion para seleccionar el contenedor?, que admita tambien this (sin parametros)?
    //busco unicamente un elemento, no trabajo con arrays
    //si me pasan el objeto dom lo empleo
    nc.select = function (selector) {
        console.log('fn select')
        nc_selection = null;
        if (typeof selector === 'string') {
            if (selector.indexOf('.') == 0) {
                nc_selection = nc_document.getElementsByClassName(selector.substring(1)).shift();
            }
            if (selector.indexOf('#') == 0) {
                nc_selection = nc_document.getElementById(selector.substring(1));
            }
        }
        if (typeof selector === Element) {
            nc_selection = selector;
        }

        return nc;//probar esto
    }

    //funcion que reciba el JSON 
    nc.draw = function (dataStr) {
        console.log('fn draw');
        var dataObj = JSON.parse(dataStr);
        //data_obj.ChartType;
        //data_obj.Suggestions; //[]
        //data_obj.VariableData; //[] -> la y -> ESTA ES LA PROPIEDAD PRINCIPAL, A TOMAR DE REFERENCIA EN TODOS LOS GRÁFICOS
        //data_obj.DimensionData; //[] -> la x
        //data_obj.ZVariableData; //[]

        switch (nc_types[dataObj.ChartType]) {
            case 'Debug':
                nc_drawChartDebug(dataObj);
                break;
            case 'Bar':
                nc_drawChartBar(dataObj);
                break;
            case 'Line':
                nc_drawChartLine(dataObj);
                break;
            case 'Scatter':
                nc_drawChartScatter(dataObj);
                break;
            case 'Bubble':
                nc_drawChartBubble(dataObj);
                break;
            case 'Temperature':
                nc_drawChartTemperature(dataObj);
                break;
            case 'Pie':
                nc_drawChartPie(dataObj);
                break;
            case 'Radar':
                nc_drawChartRadar(dataObj);
                break;
            default:
                break;
        }
    }

    //Esta función se encarga de redibujar los gráficos cuando el usuario selecciona una sugerencia
    nc.debugSelection = function(combo, dataStr) {
        //alert('no va mal ' + combo.value);
        console.log('fn debugSelection');
        console.log(dataStr);
        var chartInfo = nc_document.getElementsByClassName('nc_chart_info')[0];
        var dataObj = dataStr;//JSON.parse(dataStr);
        switch (nc_types[combo.value]) {
            case 'Debug':                
                nc_selection.innerHTML = nc_debugInformation(dataObj);
                chartInfo.innerHTML = 'Seleccione el grafico que mejor se ajuste al objetivo deseado';
                break;
            case 'Bar':
                nc_drawChartBar(dataObj);
                chartInfo.innerHTML = '<span display="block">El grafico de barras permite representar: comparacion, distribucion y composicion</span>';
                break;
            case 'Line':
                nc_drawChartLine(dataObj);
                chartInfo.innerHTML = 'El grafico de lineas permite representar: comparacion, distribucion y composicion';
                break;
            case 'Scatter':
                nc_drawChartScatter(dataObj);
                chartInfo.innerHTML = 'El grafico de dispersion permite representar: distribucion y relacion';
                break;
            case 'Bubble':
                nc_drawChartBubble(dataObj);
                chartInfo.innerHTML = 'El grafico de burbujas permite representar: relacion';
                break;
            case 'Temperature':
                nc_drawChartTemperature(dataObj);
                //todo: completar descripcion gráficos pendientes
                chartInfo.innerHTML = 'El grafico de temperatura permite representar: ';
                break;
            case 'Pie':
                nc_drawChartPie(dataObj);
                chartInfo.innerHTML = 'El frafico de tarta permite representar: composicion';
                break;
            case 'Radar':
                nc_drawChartRadar(dataObj);
                chartInfo.innerHTML = 'El grafico de radar permite representar: comparacion';
                break;
            default:
                break;
        }
    }

    //Esta función dibuja el control con las sugerencias y las opciones de configuración seleccionadas.
    function nc_drawChartDebug(data) {
        //data_obj.Suggestions;
        let options = '';
        for (let i = 0; i < data.Suggestions.length; ++i) {
            options += '<option value="' + data.Suggestions[i] + '">' + nc_types[data.Suggestions[i]] + '</option>'
        }

        //todo: queda pendiente sacar la informacion de variable, dimension y zvariable

        let divControl = nc_document.createElement('div');
        divControl.innerHTML =
                    '<h4><strong>NetChart</strong></h4>' +
                    '<br/>' +
                    '<div>' +
                    '<label>Suggestions: </label>' +
                    '<select onchange=\'nc.debugSelection(this, ' + JSON.stringify(data)+ ');\'>' + options +       
			        '</select>' +
                    '</div>' +
                    '<br/>' +
                    '<h4>Chart description</h4>' +
                    '<div class="nc_chart_info">Seleccione el grafico que mejor se ajuste al objetivo deseado</div>'
                    //'<fieldset>' +
                    ////'    <legend>Current configuration</legend>' +
                    //'<div> ' +
                    //'        <label>Variable: </label>' +
                    //'        Nominal - SUM                ' +
                    //'</br>ESTO ESTA HARDCODEADO ' +
                    //'</div>' +
                    //'<div>' +
                    //'    <label>Dimension: </label>' +
                    //'    Discrete                ' +
                    //'</div>' +
                    //'<div>' +
                    //'    <label>ZVariable: </label>' +
                    //'    No defined  ' +
                    //'</div>' +
                    //'</fieldset>';


        //padding:.5em; NO FUNCIONA, los estilos en svg se asignan distinto de los ELEMENT, lo pongo como atributo
        //nc_appendStyleAttribute(divControl, "padding", "2em;"); 
        nc_appendAttribute(divControl, "style", "padding:.5em;float:left;");

        nc_selection.innerHTML = '';
        nc_selection.appendChild(divControl);
        
        let divChart = nc_document.createElement('div');        
        nc_appendAttribute(divChart, "style", "width:"+(nc_selection.clientWidth - divControl.clientWidth - 5)+"px;height:100%;float:right;");
        divChart.innerHTML = nc_debugInformation(data);//'<p>Aqui hay que poner una nota informativa</p>';
        nc_selection.appendChild(divChart);

        //ahora dibujo los gráficos en un subespacio.
        nc_selection = divChart;
    }

    //Esta función muestra la información de la pantalla de entrada
    function nc_debugInformation(data) {
        let result =
            '<h4>Please, select a suggestion</h4>' +
            '<br/>' +
            '<h4>Current configuration</h4>' +
            '<fieldset>' +
            //'    <legend>Current configuration</legend>' +
            '<div> ' +
            '        <label>Variable: </label>' +
            data.VariableInfo +
            '</div>' +
            '<div>' +
            '    <label>Dimension: </label>' +
            data.DimensionInfo +
            '</div>' +
            '<div>' +
            '    <label>ZVariable: </label>' +
            data.ZVariableInfo +
            '</div>' +
            '</fieldset>';

        return result;
    }

    //Esta función dibuja un gráfico de barras vertical
    function nc_drawChartBar(data) {
        let svgRoot = nc_getSVGRoot();
        let svgRootWidth = nc_selection.clientWidth;
        let svgRootHeight = nc_selection.clientHeight;

        //OJO: si metemos titulo o ejes, habria que reservar espacio
        //todo: meter porcentajes para que no toquen los bordes del contenedor
        //todo: el alto y el ancho del area disponible, aplicar una funcion para poner el titulo y los ejes, 
        //deberia de quedar un cuadro mas pequeño para el gráfico
        //https://stackoverflow.com/questions/479591/svg-positioning
        //meter el grafico en un "g" y transladarlo, o crear SVG nuevo para el gráfico
        //el problema de g es que no recorta lo que queda afuera de sus dimensiones

        //aqui me he quedado, creo que esta funcion podria poner el titulo y los ejes
        //function nc_createSVGChart(svgRoot, svgRootWidth, svgRootHeight, y data)
        let svgChart = nc_createSVGChartLayout(svgRoot, svgRootWidth, svgRootHeight, data);
        let chartWidth = svgChart.width.baseVal.value
        let chartHeight = svgChart.height.baseVal.value;
        //alert(chartHeight);

        //let svgChart = nc_createSVG(svgRoot, chartX, chartY, chartWidth, chartHeight);
        let maxVariable = nc_maxValue(data.VariableData);
        let minVariable = nc_minValue(data.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }//todo: si el rango inferior es negativo restarle un 5% (max-min*0.05) para que aparezca el valor minimo

        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable)
        //let columnCount = data.VariableData.length;
        let columnWidth = chartWidth / data.VariableData.length;

        for (let i = 0; i < data.VariableData.length; ++i) {
            //let dDimension = data.DimensionData[i];
            //todo: falta meter la escala a x
            let dVariable = scaleY.getDomainValue(data.VariableData[i]);
            nc_createRect(svgChart, i * columnWidth, chartHeight - dVariable, columnWidth, dVariable, 'teal');
        }

        //Añado al final el nodo para evitar que el gráfico aparezca a golpes
        nc_selection.innerHTML = '';
        nc_selection.appendChild(svgRoot);
    }

    //Esta dibuja un gráfico de lineas
    function nc_drawChartLine(data) {
        let svgRoot = nc_getSVGRoot();
        let svgRootWidth = nc_selection.clientWidth;
        let svgRootHeight = nc_selection.clientHeight;
        let svgChart = nc_createSVGChartLayout(svgRoot, svgRootWidth, svgRootHeight, data);
        let chartWidth = svgChart.width.baseVal.value
        let chartHeight = svgChart.height.baseVal.value;

        let maxVariable = nc_maxValue(data.VariableData);
        let minVariable = nc_minValue(data.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }//todo: si el rango inferior es negativo restarle un 5% (max-min*0.05) para que aparezca el valor minimo

        let maxDimension = nc_maxValue(data.DimensionData);
        let minDimension = nc_minValue(data.DimensionData);

        let scaleX = nc_createScaleLinear(0, chartWidth, minDimension, maxDimension);
        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable);

        //se usa variable y dimension
        //todo: ¿gestionar un unico dato?, de momento solo 2 o mas, meter un if y pintar un punto o un recta de extremo a extremo
        //aqui va el bucle de lineas
        for (let i = 1; i < data.VariableData.length; ++i) {
            let x1 = scaleX.getDomainValue(data.DimensionData[i - 1]);
            let y1 = scaleY.getDomainValue(data.VariableData[i - 1]);
            let x2 = scaleX.getDomainValue(data.DimensionData[i]);
            let y2 = scaleY.getDomainValue(data.VariableData[i]);
            nc_createLine(svgChart, x1, chartHeight - y1, x2, chartHeight - y2, 'teal');
        }

        nc_selection.innerHTML = '';
        nc_selection.appendChild(svgRoot);
    }

    //Esta funcion dibuja un fráfico de dispersión
    function nc_drawChartScatter(data) {
        let svgRoot = nc_getSVGRoot();
        let svgRootWidth = nc_selection.clientWidth;
        let svgRootHeight = nc_selection.clientHeight;
        let svgChart = nc_createSVGChartLayout(svgRoot, svgRootWidth, svgRootHeight, data);
        let chartWidth = svgChart.width.baseVal.value
        let chartHeight = svgChart.height.baseVal.value;

        let maxVariable = nc_maxValue(data.VariableData);
        let minVariable = nc_minValue(data.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }//todo: si el rango inferior es negativo restarle un 5% (max-min*0.05) para que aparezca el valor minimo

        let maxDimension = nc_maxValue(data.DimensionData);
        let minDimension = nc_minValue(data.DimensionData);

        let scaleX = nc_createScaleLinear(0, chartWidth, minDimension, maxDimension);
        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable);

        //aquí van los puntos
        for (let i = 0; i < data.VariableData.length; ++i) {
            let x = scaleX.getDomainValue(data.DimensionData[i]);
            let y = scaleY.getDomainValue(data.VariableData[i]);

            nc_createCircle(svgChart, x, chartHeight - y, 5, 'teal');
        }

        nc_selection.innerHTML = '';
        nc_selection.appendChild(svgRoot);
    }

    //Esta funcion dibuja un gráfico de burbujas
    function nc_drawChartBubble(data) {
        let svgRoot = nc_getSVGRoot();
        let svgRootWidth = nc_selection.clientWidth;
        let svgRootHeight = nc_selection.clientHeight;
        let svgChart = nc_createSVGChartLayout(svgRoot, svgRootWidth, svgRootHeight, data);
        let chartWidth = svgChart.width.baseVal.value
        let chartHeight = svgChart.height.baseVal.value;

        let maxVariable = nc_maxValue(data.VariableData);
        let minVariable = nc_minValue(data.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }//todo: si el rango inferior es negativo restarle un 5% (max-min*0.05) para que aparezca el valor minimo

        let maxDimension = nc_maxValue(data.DimensionData);
        let minDimension = nc_minValue(data.DimensionData);

        let maxZVariable = nc_maxValue(data.ZVariableData);
        let minZVariable = nc_minValue(data.ZVariableData);
        if (minZVariable > 0) {
            minZVariable = 0;
        }//todo: lo mismo que con variable, hay que mirar los margenes

        let chartZMax = chartHeight / 8; //hago que el tamaño de la burbuja sea como maximo el 25% de la altura (OJO que esto es el radio)

        let scaleX = nc_createScaleLinear(0, chartWidth, minDimension, maxDimension);
        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable);
        let scaleZ = nc_createScaleLinear(0, chartZMax, minZVariable, maxZVariable);

        //creo que hace falta la escala z, ¿pensar en el dominio?
        for (let i = 0; i < data.VariableData.length; ++i) {
            let x = scaleX.getDomainValue(data.DimensionData[i]);
            let y = scaleY.getDomainValue(data.VariableData[i]);
            let r = scaleZ.getDomainValue(data.ZVariableData[i]);
            nc_createCircle(svgChart, x, chartHeight - y, r, 'teal');
        }

        nc_selection.innerHTML = '';
        nc_selection.appendChild(svgRoot);
    }

    //Esta función dibuja un gráfico de temperatura
    function nc_drawChartTemperature(data) {
        let svgRoot = nc_getSVGRoot();
        let svgRootWidth = nc_selection.clientWidth;
        let svgRootHeight = nc_selection.clientHeight;
        let svgChart = nc_createSVGChartLayout(svgRoot, svgRootWidth, svgRootHeight, data);
        let chartWidth = svgChart.width.baseVal.value
        let chartHeight = svgChart.height.baseVal.value;

        let dimesionSteps = nc_distinct(nc_sort(data.DimensionData));
        let variableSteps = nc_distinct(data.VariableData);

        let maxZVariable = nc_maxValue(data.ZVariableData);
        let minZVariable = nc_minValue(data.ZVariableData);
        if (minZVariable > 0) {
            minZVariable = 0;
        }//todo: lo mismo que con variable, hay que mirar los margenes

        let scaleZ = nc_createScaleLinear(0, 1, minZVariable, maxZVariable);

        //sacar los elementos distintos sin repetidos
        //crear un nc_arrayScaleArray -> crear un nc_shortDistinct o dos

        let rowHeight = chartHeight / variableSteps.length;
        let columnWidth = chartWidth / dimesionSteps.length;

        for (let i = 0; i < dimesionSteps.length; ++i) {
            for (let j = 0; j < data.DimensionData.length; ++j) {
                if (data.DimensionData[j] == dimesionSteps[i]) {
                    //dibujar la y y la z
                    let x = i;
                    let y = variableSteps.indexOf(data.VariableData[j]);
                    let z = scaleZ.getDomainValue(data.ZVariableData[j]); //opacity

                    let rect = nc_createRect(svgChart, x * columnWidth, chartHeight - (y * rowHeight) - rowHeight,
                        columnWidth, rowHeight, 'teal');
                    nc_appendAttribute(rect, 'fill-opacity', z);
                }
            }
        }

        nc_selection.innerHTML = '';
        nc_selection.appendChild(svgRoot);
    }

    //Esta función dibuja un gráfico de tarta
    function nc_drawChartPie(data) {
        let svgRoot = nc_getSVGRoot();
        let svgRootWidth = nc_selection.clientWidth;
        let svgRootHeight = nc_selection.clientHeight;
        let svgChart = nc_createSVGChartLayout(svgRoot, svgRootWidth, svgRootHeight, data);
        let chartWidth = svgChart.width.baseVal.value
        let chartHeight = svgChart.height.baseVal.value;

        //AQUI ME HE QUEDADO
        //https://stackoverflow.com/questions/32750613/svg-draw-a-circle-with-4-sectors
        //https://jbkflex.wordpress.com/2011/07/28/creating-a-svg-pie-chart-html5/
        //solo trabajo con variabledata

        //todo: hay que sumar los valores en funcion de la dimension, es decir, si existe una dimensión repetida sumar sus valores

        //calculamos el monto total de los datos
        let totalData = 0;
        for (let i = 0; i < data.VariableData.length; ++i) {
            totalData = totalData + Math.abs(data.VariableData[i]);
        }

        //calculamos el angulo correspondiente a cada dato en función 
        let angles = [];
        for (let i = 0; i < data.VariableData.length; ++i) {
            //let angle = Math.ceil(360 * Math.abs(data.VariableData[i]) / totalData);
            let angle = 360 * (Math.abs(data.VariableData[i]) / totalData);
            angles.push(angle);
        }

        //calcular el centro y el radio
        let centerX = chartWidth / 2;
        let centerY = chartHeight / 2;
        let radius = centerY;
        if (centerX < centerY) {
            radius = centerX;
        }

        let startAngle = 0;
        let endAngle = 0;

        for (let i = 0; i < angles.length; ++i) {
            startAngle = endAngle;
            endAngle = startAngle + angles[i];

            x1 = parseFloat(centerX + radius * Math.cos(Math.PI * startAngle / 180));
            y1 = parseFloat(centerY + radius * Math.sin(Math.PI * startAngle / 180));

            x2 = parseFloat(centerX + radius * Math.cos(Math.PI * endAngle / 180));
            y2 = parseFloat(centerY + radius * Math.sin(Math.PI * endAngle / 180));

            nc_createPathSector(svgChart, centerX, centerY, radius, x1, y1, x2, y2, nc_nextColor());
        }

        nc_selection.innerHTML = '';
        nc_selection.appendChild(svgRoot);
    }

    //Esta función dibuja un gráfico de radar
    function nc_drawChartRadar(data) {
        let svgRoot = nc_getSVGRoot();
        let svgRootWidth = nc_selection.clientWidth;
        let svgRootHeight = nc_selection.clientHeight;
        let svgChart = nc_createSVGChartLayout(svgRoot, svgRootWidth, svgRootHeight, data);
        let chartWidth = svgChart.width.baseVal.value
        let chartHeight = svgChart.height.baseVal.value;

        //Aqui me quede, sacar el numero de x total, 
        //sacar el maximo de y, calcular las distintas porciones,
        //calcular la interseccion del radio sobre sobre la recta

        //calculamos el angulo correspondiente a cada dato en función 
        let angles = [];
        angles.push(0);
        for (let i = 1; i < data.VariableData.length; ++i) {
            //let angle = Math.ceil(360 * Math.abs(data.VariableData[i]) / totalData);
            let angle = (360 / data.VariableData.length) * i;
            angles.push(angle);
        }

        //calcular el centro y el radio
        let centerX = chartWidth / 2;
        let centerY = chartHeight / 2;

        //REVISAR ESTO, EL RADIO SERIA LO MAXIMO, METER MEJOR UNA ESCALAY
        let radius = centerY;
        if (centerX < centerY) {
            radius = centerX;
        }
       
        let maxVariable = nc_maxValue(data.VariableData);
        let minVariable = nc_minValue(data.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }

        let scaleY = nc_createScaleLinear(0, radius, minVariable, maxVariable);

        let startAngle = 0;
        let endAngle = 0;
        let startRadius = 0;
        let endRadius = 0;

        for (let i = 0; i < angles.length; ++i) {
            startAngle = angles[i];
            startRadius = scaleY.getDomainValue(data.VariableData[i]);
            if (i < angles.length - 1) {
                endAngle = angles[i + 1];
                endRadius = scaleY.getDomainValue(data.VariableData[i+1]);
            } else {
                endAngle = angles[0];
                endRadius = scaleY.getDomainValue(data.VariableData[0]);
            }

            //Dibujamos los ejes
            axisX = parseFloat(centerX + radius * Math.cos(Math.PI * startAngle / 180));
            axisY = parseFloat(centerY + radius * Math.sin(Math.PI * startAngle / 180));
            nc_createLine(svgChart, centerX, centerY, axisX, axisY, 'black');

            //Dibujamos las lineas del radar
            let x1 = parseFloat(centerX + startRadius * Math.cos(Math.PI * startAngle / 180));
            let y1 = parseFloat(centerY + startRadius * Math.sin(Math.PI * startAngle / 180));
            let x2 = parseFloat(centerX + endRadius * Math.cos(Math.PI * endAngle / 180));
            let y2 = parseFloat(centerY + endRadius * Math.sin(Math.PI * endAngle / 180));

            nc_createLine(svgChart, x1, y1, x2, y2, 'teal');
           
            nc_createText(svgChart, axisX, axisY, data.DimensionData[i]);
        }
       // nc_createCircle(svgChart, centerX, centerY, 4, 'blue');

        nc_selection.innerHTML = '';
        nc_selection.appendChild(svgRoot);
    }

    //Crea el svg donde se dibuja el gráfico, ademas añade los ejes, leyendas y titulo
    function nc_createSVGChartLayout(parentSVG, width, height, data) {
        //todo: cuando defina los parametros de configuración (titulo, leyenda,etc) meterlos aqui
        let chartX = 30,
            chartY = 30,
            chartWidth = width - (chartX * 2),
            chartHeight = height - (chartY * 2);
        let svgChart = nc_createSVG(parentSVG, chartX, chartY, chartWidth, chartHeight);

        //creamos el eje y
        let maxRange = nc_maxValue(data.VariableData);
        let minRange = nc_minValue(data.VariableData);
        if (minRange > 0) {
            minRange = 0;
        }//todo: revisar si gestiono negativos

        //434 y max, y coge 494
        let scaleY = nc_createScaleLinear(0, chartHeight, minRange, maxRange)
        nc_createLine(parentSVG, chartX, chartY, chartX, chartY + chartHeight, 'black');
        for (let i = 0; i < data.VariableData.length; ++i) {
            nc_createText(parentSVG, chartX - 20,
                chartHeight - scaleY.getDomainValue(data.VariableData[i]) + chartY,
                data.VariableData[i]);
        }

        //creamos el eje x
        nc_createLine(parentSVG, chartX, chartY + chartHeight, chartX + chartWidth, chartY + chartHeight, 'black');
        let columnWidth = chartWidth / data.DimensionData.length;
        let columnCenter = columnWidth / 2;
        for (let i = 0; i < data.DimensionData.length; ++i) {
            nc_createText(parentSVG, (i * columnWidth) + columnCenter, chartY + chartHeight + 15, data.DimensionData[i]);
        }

        //creo el titulo
        let title = nc_createText(parentSVG, '50%', 20, 'EL TITULO');
        //alignment-baseline="middle" text-anchor="middle"
        //nc_appendAttribute(title, 'alignment-baseline', 'middle');
        nc_appendAttribute(title, 'text-anchor', 'middle');
        nc_appendAttribute(title, 'font-weight', 'bold');

        return svgChart;
    }

    //function nc_drawChartLine() { }
    //function nc_drawChartScatter() { }
    //function nc_drawChartBubble() { }
    //function nc_drawChartTemperature() { }
    //function nc_drawChartPie() { }
    //function nc_drawChartRadar() { }

    //Añade un nodo hijo a un nodo
    //function nc_appendChild(parentNode, childNode) {
    //    parentNode.appendChild(childNode);
    //}

    //Añade un atributo a un nodo
    function nc_appendAttribute(node, attrName, attrValue) {
        //node.hasAttribute(attrName) //TODO: mirar esto
        let attr = nc_document.createAttribute(attrName);
        attr.value = attrValue;
        node.setAttributeNode(attr);
    }

    //Añade un estilo a un nodo
    function nc_appendStyleAttribute(node, attrName, attrValue) {
        node.style[attrName] = attrValue;
    }

    //Crea un nodo SVG
    function nc_createSVG(parentNode, x, y, width, height) {
        let svg = nc_document.createElementNS(nc_svgns, 'svg');
        nc_appendAttribute(svg, 'x', x);
        nc_appendAttribute(svg, 'y', y);
        nc_appendAttribute(svg, 'width', width);
        nc_appendAttribute(svg, 'height', height);
        //nc_appendStyleAttribute(svg, 'width', width);
        //nc_appendStyleAttribute(svg, 'height', height);
        parentNode.appendChild(svg);
        return svg;
    }

    //automatizar la creacion de las formas, rectangulos, burbujas, sectores, triangulos y lineas

    //Crea un path con forma de sector
    function nc_createPathSector(parentNode, cx, cy, r, x1, y1, x2, y2, color) {
        let path = nc_document.createElementNS('http://www.w3.org/2000/svg', 'path');

        let d = "M" + cx + "," + cy + "  L" + x1 + "," + y1 +
            " A" + r + "," + r + " 0 0,1 " + x2 + "," + y2 + " z"; //1 means clockwise
        //alert(d);
        //arc = paper.path(d);
        nc_appendAttribute(path, 'd', d);
        nc_appendAttribute(path, 'fill', color);
        //arc.setAttribute('d', d);
        //arc.setAttribute('fill', color);

        parentNode.appendChild(path);
        return path;
    }

    //Crea un texto svg
    function nc_createText(parentNode, x, y, text) {
        let svgText = nc_document.createElementNS(nc_svgns, 'text');
        let textNode = nc_document.createTextNode(text);
        svgText.appendChild(textNode);
        nc_appendAttribute(svgText, 'x', x);
        nc_appendAttribute(svgText, 'y', y);
        parentNode.appendChild(svgText);
        return svgText;
    }

    //Crea un circulo svg
    function nc_createCircle(parentNode, x, y, r, color) {
        let circle = nc_document.createElementNS(nc_svgns, 'circle');
        nc_appendAttribute(circle, 'cx', x);
        nc_appendAttribute(circle, 'cy', y);
        nc_appendAttribute(circle, 'r', r);
        nc_appendAttribute(circle, 'fill', color);

        parentNode.appendChild(circle);
        return circle;
    }

    //Crea una linea svg
    function nc_createLine(parentNode, x1, y1, x2, y2, color) {
        let line = nc_document.createElementNS(nc_svgns, 'line');
        nc_appendAttribute(line, 'x1', x1);
        nc_appendAttribute(line, 'y1', y1);
        nc_appendAttribute(line, 'x2', x2);
        nc_appendAttribute(line, 'y2', y2);

        //tb va con estilos
        //nc_appendStyleAttribute(line, 'stroke', 'black');
        //nc_appendStyleAttribute(line, 'stroke-width', 2);
        nc_appendAttribute(line, 'stroke', color);
        nc_appendAttribute(line, 'stroke-width', 2);
        parentNode.appendChild(line);
        return line;
    }

    //Crea un rectangulo svg
    function nc_createRect(parentNode, x, y, width, height, color) {
        let rect = nc_document.createElementNS(nc_svgns, 'rect');
        nc_appendAttribute(rect, 'x', x);
        nc_appendAttribute(rect, 'y', y);
        nc_appendAttribute(rect, 'width', width);
        nc_appendAttribute(rect, 'height', height);
        nc_appendAttribute(rect, 'fill', color);
        parentNode.appendChild(rect);
        return rect;
    }

    //Crea un svg con los estilos apropiados para ser el contenedor base del gráfico
    function nc_getSVGRoot() {
        let svg = nc_document.createElementNS(nc_svgns, 'svg');
        nc_appendStyleAttribute(svg, 'width', '100%');
        nc_appendStyleAttribute(svg, 'height', '100%');
        return svg;
    }

    //VA AQUI

    //FUNCIONES AUXILIARES
    //max
    //min
    //scaleLinear

    //Esta función devuelve el siguiente color de la paleta
    function nc_nextColor() {
        if (nc_currentColor > nc_colors.length) {
            nc_currentColor = 0;
        }
        return nc_colors[nc_currentColor++];
    }

    //Esta función devuelve un nuevo array ordenado
    function nc_sort(data) {
        let result = data.slice(0);
        return result.sort();
    }

    //Esta función devuelve un nuevo array de elementos distintos
    function nc_distinct(data) {
        let cloneData = data.slice(0);
        let result = [];
        for (let i = 0; i < cloneData.length; ++i) {
            if (result.indexOf(cloneData[i]) < 0) {
                result.push(cloneData[i]);
            }
        }
        return result;
    }

    //Calcula el máximo de una colección
    function nc_maxValue(list) {
        let max = null;
        if (list.length > 0) {
            max = list[0];
        }
        for (let i = 0; i < list.length; ++i) {
            if (list[i] > max) {
                max = list[i];
            }
        }
        return max;
    }

    //Calcula el mínimo de una colección
    function nc_minValue(list) {
        let min = null;
        if (list.length > 0) {
            min = list[0];
        }
        for (let i = 0; i < list.length; ++i) {
            if (list[i] < min) {
                min = list[i];
            }
        }
        return min;
    }

    //Crea una escala lineal
    function nc_createScaleLinear(domainMin, domainMax, rangeMin, rangeMax) {
        let scale = {
            domainMin: domainMin,
            domainMax: domainMax,
            rangeMin: rangeMin,
            rangeMax: rangeMax,
            getDomainValue: function (rangeValue) {
                //regla de 3 doble, primero calculo el porcentaje del rango y luego ese porcentaje sobre el valor equivalente en el dominio
                let outputGap = domainMax - domainMin;
                let inputGap = rangeMax - rangeMin;
                let inputValue = rangeValue - rangeMin; //celda d3

                //calculo % sobre rango
                let rangePercentage = (inputValue * 100) / inputGap;

                //calculo el valor equivalente sobre dominio
                let domainValue = (rangePercentage * outputGap) / 100;

                return domainMin + domainValue;
            }
        }

        return scale;
    }

    //FUNCIONES AUXILIARES FIN

    //BORRAR ESTA FUNCION, PRUEBAS
    nc.testManual = function (dataStr) {
        var dataObj = JSON.parse(dataStr);
        var maximo = nc_maxValue(dataObj.DimensionData);
        var minimo = nc_minValue(dataObj.DimensionData);

        var escala = nc_createScaleLinear(200, 600, 25, 150);
        var resultado = escala.getDomainValue(50);
        alert('makumba');
    }

}(window);