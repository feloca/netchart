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
    const nc_types = ['Debug', 'Histogram', 'Line', 'Scatter', 'Bubble', 'Temperature', 'Pie', 'Radar', 'Area3D',
        'Waterfall', 'AttachedColumnPercentage', 'AttachedColumn', 'OverlapAreaPercentage', 'OverlapArea',
        'MultipleColumn', 'MultipleLine', 'MultipleBar'];

    //Estos valores deben corresponderse con el enumerado VariableTypeEnum
    const nc_displayTypes = ['Discrete', 'Continuous', 'Nominal', 'Ordinal'];

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

        return nc;
    }

    //funcion que dibuja el grafico JS, reciba el JSON con los datos
    nc.draw = function (dataStr) {
        //como decodificar caracteres de escape de html
        //https://stackoverflow.com/questions/7394748/whats-the-right-way-to-decode-a-string-that-has-special-html-entities-in-it

        console.log('fn draw');
        var dataObj = JSON.parse(dataStr);
        //data_obj.ChartType;
        //data_obj.Suggestions; //[]
        //data_obj.SeriesDimensions; //[] -> cuando la serie esta difinida segun el caso es posible contar con las dimensiones
        //data_obj.Series[0].Descriptor;
        //data_obj.Series[0].VariableData; //[] -> la y -> ESTA ES LA PROPIEDAD PRINCIPAL, A TOMAR DE REFERENCIA EN TODOS LOS GRÁFICOS
        //data_obj.Series[0].DimensionData; //[] -> la x
        //data_obj.Series[0].ZVariableData; //[]
        //data_obj.Display.Title; -> titulo del grafico
        //data_obj.Display.VariableDisplayType; -> tipo de dato visual y
        //data_obj.Display.DimensionDisplayType; -> tipo de dato visual x
        //data_obj.Display.ZVariableDisplayType; -> tipo de dato visual z

        switch (nc_types[dataObj.ChartType]) {
            case 'Debug':
                nc_drawChartDebug(dataObj);
                break;
            case 'Histogram':
                nc_drawChartHistogram(dataObj);
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
            case 'Area3D':
                nc_drawChartNotImplemented(dataObj);
                break;
            case 'Waterfall':
                nc_drawChartNotImplemented(dataObj);
                break;
            case 'AttachedColumnPercentage':
                nc_drawChartNotImplemented(dataObj);
                break;
            case 'AttachedColumn':
                nc_drawChartNotImplemented(dataObj);
                break;
            case 'OverlapAreaPercentage':
                nc_drawChartNotImplemented(dataObj);
                break;
            case 'OverlapArea':
                nc_drawChartNotImplemented(dataObj);
                break;
            case 'MultipleColumn':
                nc_drawChartNotImplemented(dataObj);
                break;
            case 'MultipleLine':
                nc_drawChartNotImplemented(dataObj);
                break;
            case 'MultipleBar':
                nc_drawChartNotImplemented(dataObj);
                break;
            default:
                console.log('fn draw: no se ha encontrado el tipo ' + dataObj.ChartType);
                break;
        }
    }

    //Esta función se encarga de redibujar los gráficos cuando el usuario selecciona una sugerencia
    nc.debugSelection = function (combo, dataStr) {
        //alert('no va mal ' + combo.value);
        console.log('fn debugSelection');
        console.log(dataStr);
        var chartInfo = nc_document.getElementsByClassName('nc_chart_info')[0];
        var dataObj = dataStr;//JSON.parse(dataStr);
        switch (nc_types[combo.value]) {
            case 'Debug':
                nc_selection.innerHTML = nc_debugInformation(dataObj);
                chartInfo.innerHTML = '';
                break;
            case 'Histogram':
                nc_drawChartHistogram(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                                    '<ul>' +
                                    '<li>Comparison</li>' +
                                    '<li>Distribution</li>' +
                                    '<li>Composition</li>' +
                                    '</ul>';

                //'El grafico de barras permite representar: comparacion, distribucion y composicion';
                break;
            case 'Line':
                nc_drawChartLine(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                                    '<ul>' +
                                    '<li>Comparison</li>' +
                                    '<li>Distribution</li>' +
                                    '<li>Composition</li>' +
                                    '</ul>';

                //'El grafico de lineas permite representar: comparacion, distribucion y composicion';
                break;
            case 'Scatter':
                nc_drawChartScatter(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                                    '<ul>' +
                                    '<li>Distribution</li>' +
                                    '<li>Relationship</li>' +
                                    '</ul>';

                //'El grafico de dispersion permite representar: distribucion y relacion';
                break;
            case 'Bubble':
                nc_drawChartBubble(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                                    '<ul>' +
                                    '<li>Relationship</li>' +
                                    '</ul>';

                //'El grafico de burbujas permite representar: relacion';
                break;
            case 'Temperature':
                nc_drawChartTemperature(dataObj);
                //todo: completar descripcion gráficos pendientes
                chartInfo.innerHTML = 'Good for:' +
                                    '<ul>' +
                                    '<li>TODO</li>' +
                                    '</ul>';
                //'El grafico de temperatura permite representar: ';
                break;
            case 'Pie':
                nc_drawChartPie(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                                    '<ul>' +
                                    '<li>Composition</li>' +
                                    '</ul>';
                //'El frafico de tarta permite representar: composicion';
                break;
            case 'Radar':
                nc_drawChartRadar(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                                    '<ul>' +
                                    '<li>Comparison</li>' +
                                    '</ul>';
                //'El grafico de radar permite representar: comparacion';
                break;
            case 'Area3D':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Distribution</li>' +
                    '</ul>';
                break;
            case 'Waterfall':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Composition</li>' +
                    '</ul>';
                break;
            case 'AttachedColumnPercentage':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Composition</li>' +
                    '</ul>';
                break;
            case 'AttachedColumn':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Composition</li>' +
                    '</ul>';
                break;
            case 'OverlapAreaPercentage':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Composition</li>' +
                    '</ul>';
                break;
            case 'OverlapArea':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Composition</li>' +
                    '</ul>';
                break;
            case 'MultipleColumn':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Comparison</li>' +
                    '</ul>';
                break;
            case 'MultipleLine':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Comparison</li>' +
                    '</ul>';
                break;
            case 'MultipleBar':
                nc_drawChartNotImplemented(dataObj);
                chartInfo.innerHTML = 'Good for:' +
                    '<ul>' +
                    '<li>Comparison</li>' +
                    '</ul>';
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
                    '<select onchange=\'nc.debugSelection(this, ' + JSON.stringify(data) + ');\'>' + options +
			        '</select>' +
                    '</div>' +
                    '<br/>' +
                    '<h4>Chart description</h4>' +
                    '<div class="nc_chart_info"></div>';

        //padding:.5em; NO FUNCIONA, los estilos en svg se asignan distinto de los ELEMENT, lo pongo como atributo
        //nc_appendStyleAttribute(divControl, "padding", "2em;"); 
        nc_appendAttribute(divControl, "style", "padding:.5em;float:left;");

        nc_selection.innerHTML = '';
        nc_selection.appendChild(divControl);

        let divChart = nc_document.createElement('div');
        nc_appendAttribute(divChart, "style", "width:" + (nc_selection.clientWidth - divControl.clientWidth - 5) + "px;height:100%;float:right;");
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

    //Esta funcion muestra un mensaje generico cuando un tipo de grafico no esta implementado todavía, borrar cuando este completo
    function nc_drawChartNotImplemented(data) {
        //let svgRoot = nc_getSVGRoot();        
        let result =
            '<h4>Selected graph is not implemented</h4>' +
            '<br/>' +
            '<div> ' +
            '        <label>Chart type: </label>' +
            nc_types[data.ChartType] +
            '</div>';

        nc_selection.innerHTML = result;
        //nc_selection.appendChild(svgRoot);
    }

    //Esta función dibuja un gráfico de barras vertical
    function nc_drawChartHistogram(data) {
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

        let svgChart = nc_createSVGChartLayout(svgRoot, svgRootWidth, svgRootHeight, data);
        let chartWidth = svgChart.width.baseVal.value
        let chartHeight = svgChart.height.baseVal.value;
        //este grafico no admite series
        let serie = data.Series[0];

        //let svgChart = nc_createSVG(svgRoot, chartX, chartY, chartWidth, chartHeight);
        let maxVariable = nc_maxValue(serie.VariableData);
        let minVariable = nc_minValue(serie.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }//todo: si el rango inferior es negativo restarle un 5% (max-min*0.05) para que aparezca el valor minimo

        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable)
        //let columnCount = data.VariableData.length;
        let columnWidth = chartWidth / serie.VariableData.length;

        for (let i = 0; i < serie.VariableData.length; ++i) {
            //let dDimension = data.DimensionData[i];
            //todo: falta meter la escala a x
            let dVariable = scaleY.getDomainValue(serie.VariableData[i]);
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
        //este grafico no admite series
        let serie = data.Series[0];

        let maxVariable = nc_maxValue(serie.VariableData);
        let minVariable = nc_minValue(serie.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }//todo: si el rango inferior es negativo restarle un 5% (max-min*0.05) para que aparezca el valor minimo

        let maxDimension = nc_maxValue(serie.DimensionData);
        let minDimension = nc_minValue(serie.DimensionData);

        let scaleX = nc_createScaleLinear(0, chartWidth, minDimension, maxDimension);
        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable);

        //se usa variable y dimension
        //todo: ¿gestionar un unico dato?, de momento solo 2 o mas, meter un if y pintar un punto o un recta de extremo a extremo
        //aqui va el bucle de lineas
        for (let i = 1; i < serie.VariableData.length; ++i) {
            let x1 = scaleX.getDomainValue(serie.DimensionData[i - 1]);
            let y1 = scaleY.getDomainValue(serie.VariableData[i - 1]);
            let x2 = scaleX.getDomainValue(serie.DimensionData[i]);
            let y2 = scaleY.getDomainValue(serie.VariableData[i]);
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
        //este grafico no admite series
        let series = data.Series[0];

        let maxVariable = nc_maxValue(series.VariableData);
        let minVariable = nc_minValue(series.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }//todo: si el rango inferior es negativo restarle un 5% (max-min*0.05) para que aparezca el valor minimo

        let maxDimension = nc_maxValue(series.DimensionData);
        let minDimension = nc_minValue(series.DimensionData);

        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable);

        //Creo que nunca va a ser nominal, deberia entrar numeros u ordinal. 
        if (nc_displayTypes[data.Display.DimensionDisplayType] == 'Nominal' || nc_displayTypes[data.Display.DimensionDisplayType] == 'Ordinal') {
            let columnWidth = chartWidth / series.DimensionData.length;
            let columnCenter = columnWidth / 2;
            for (let i = 0; i < series.VariableData.length; ++i) {
                let x = (i * columnWidth) + columnCenter;
                let y = scaleY.getDomainValue(series.VariableData[i]);
                nc_createCircle(svgChart, x, chartHeight - y, 5, 'teal');
            }
        } else {
            let scaleX = nc_createScaleLinear(0, chartWidth, minDimension, maxDimension);
            //aquí van los puntos
            for (let i = 0; i < series.VariableData.length; ++i) {
                let x = scaleX.getDomainValue(series.DimensionData[i]);
                let y = scaleY.getDomainValue(series.VariableData[i]);

                nc_createCircle(svgChart, x, chartHeight - y, 5, 'teal');
            }
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
        //este grafico no admite series
        let series = data.Series[0];

        let maxVariable = nc_maxValue(series.VariableData);
        let minVariable = nc_minValue(series.VariableData);
        if (minVariable > 0) {
            minVariable = 0;
        }//todo: si el rango inferior es negativo restarle un 5% (max-min*0.05) para que aparezca el valor minimo

        let maxDimension = nc_maxValue(series.DimensionData);
        let minDimension = nc_minValue(series.DimensionData);

        let maxZVariable = nc_maxValue(series.ZVariableData);
        let minZVariable = nc_minValue(series.ZVariableData);
        if (minZVariable > 0) {
            minZVariable = 0;
        }//todo: lo mismo que con variable, hay que mirar los margenes

        let chartZMax = chartHeight / 8; //hago que el tamaño de la burbuja sea como maximo el 25% de la altura (OJO que esto es el radio)

        let scaleX = nc_createScaleLinear(0, chartWidth, minDimension, maxDimension);
        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable);
        let scaleZ = nc_createScaleLinear(0, chartZMax, minZVariable, maxZVariable);

        //creo que hace falta la escala z, ¿pensar en el dominio?
        for (let i = 0; i < series.VariableData.length; ++i) {
            let x = scaleX.getDomainValue(series.DimensionData[i]);
            let y = scaleY.getDomainValue(series.VariableData[i]);
            let r = scaleZ.getDomainValue(series.ZVariableData[i]);
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

        let dimesionSteps = data.SeriesDimensions; //nc_distinct(nc_sort(data.DimensionData));
        //let variableSteps = [];//nc_distinct(data.VariableData);

        let maxVariable = Number.MIN_SAFE_INTEGER; //nc_maxValue(data.ZVariableData);
        let minVariable = Number.MAX_SAFE_INTEGER; //nc_minValue(data.ZVariableData);

        let rowHeight = chartHeight / data.Series.length; //chartHeight / variableSteps.length;
        let columnWidth = chartWidth / dimesionSteps.length;

        //deberiamos de tener una UNICA fila en cada serie, a la hora de agrupar datos agruparlos
        //este grafico funciona con series
        //todo: POSIBLEMENTE este grafico no represente todas las casuisticas correctamente

        //calculo los maximos y minimos de referencia para Z
        for (let i = 0; i < data.Series.length; ++i) {
            let series = data.Series[i];

            let maxSeries = nc_maxValue(series.VariableData);
            let minSeries = nc_minValue(series.VariableData);

            if (maxSeries > maxVariable) {
                maxVariable = maxSeries;
            }
            if (minSeries < minVariable) {
                minVariable = minSeries;
            }
        }

        if (minVariable > 0) {
            minVariable = 0;
        }//todo: lo mismo que con variable, hay que mirar los margenes

        let scaleVar = nc_createScaleLinear(0, 1, minZVariable, maxZVariable);

        for (let i = 0; i < data.Series.length; ++i) {
            let series = data.Series[i];
            //cada fila de la serie tendra una dimension de distinto valor (no contemplo ahora mismo repetidos)
            //todo: contemplar series con valores de dimension repetidos
            for (let j = 0; j < dimesionSteps.length; ++j) {
                for (let k = 0; k < series.DimensionData.length; ++k) {
                    if (series.DimensionData[k] = dimesionSteps[j]) {
                        //dibujar la y y la z
                        let x = i;
                        let y = dimesionSteps.indexOf(series.DimensionData[j]);
                        let z = scaleVar.getDomainValue(series.VariableData[j]); //opacity

                        let rect = nc_createRect(svgChart, x * columnWidth, chartHeight - (y * rowHeight) - rowHeight,
                            columnWidth, rowHeight, 'teal');
                        nc_appendAttribute(rect, 'fill-opacity', z);
                    }
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

        //https://stackoverflow.com/questions/32750613/svg-draw-a-circle-with-4-sectors
        //https://jbkflex.wordpress.com/2011/07/28/creating-a-svg-pie-chart-html5/
        //solo trabajo con variabledata

        //este grafico admite series, una por cada porcion de la tarta
        //calculamos el monto total de los datos
        let totalData = 0;
        for (let i = 0; i < data.Series.length; ++i) {
            let series = data.Series[i];
            for (let j = 0; j < series.VariableData.length; ++j) {
                totalData = totalData + Math.abs(series.VariableData[j]);
            }
        }

        //calculamos el angulo correspondiente a cada dato en función 
        let angles = [];
        //let labels = [];
        for (let i = 0; i < data.Series.length; ++i) {
            let series = data.Series[i];
            let totalSeries = 0;
            for (let j = 0; j < series.VariableData.length; ++j) {
                totalSeries = totalSeries + Math.abs(series.VariableData[j]);
            }
            //let angle = Math.ceil(360 * Math.abs(data.VariableData[i]) / totalData);
            let angle = 360 * (totalSeries / totalData);
            angles.push(angle);
            //labels.push(series.Description);
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
            //labels[i] ties la descripcion del sector
            //nc_createText(svgChart, x1, y1, labels[i]);
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

        //este grafico no admite series
        let series = data.Series[0];

        //Aqui me quede, sacar el numero de x total, 
        //sacar el maximo de y, calcular las distintas porciones,
        //calcular la interseccion del radio sobre sobre la recta

        //calculamos el angulo correspondiente a cada dato en función 
        let angles = [];
        angles.push(0);
        for (let i = 1; i < series.VariableData.length; ++i) {
            //let angle = Math.ceil(360 * Math.abs(data.VariableData[i]) / totalData);
            let angle = (360 / series.VariableData.length) * i;
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

        let maxVariable = nc_maxValue(series.VariableData);
        let minVariable = nc_minValue(series.VariableData);
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
            startRadius = scaleY.getDomainValue(series.VariableData[i]);
            if (i < angles.length - 1) {
                endAngle = angles[i + 1];
                endRadius = scaleY.getDomainValue(series.VariableData[i + 1]);
            } else {
                endAngle = angles[0];
                endRadius = scaleY.getDomainValue(series.VariableData[0]);
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

            nc_createText(svgChart, axisX, axisY, series.DimensionData[i]);
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
            chartWidth = width - (chartX * 3),
            chartHeight = height - (chartY * 2);

        //todo: la siguiente linea tendria que depender del tipo de grafico, si no existen comentarios o ejes ajustar mejor
        chartX = chartX * 2; //dejo 2 partes a la izquierda para usar una de ellas para los textos del eje y
        let svgChart = nc_createSVG(parentSVG, chartX, chartY, chartWidth, chartHeight);

        //creo el titulo
        let title = nc_createText(parentSVG, '50%', 20, data.Display.Title);
        nc_appendAttribute(title, 'text-anchor', 'middle');
        nc_appendAttribute(title, 'font-weight', 'bold');

        let maxYRange = Number.MIN_SAFE_INTEGER;
        let minYRange = Number.MAX_SAFE_INTEGER;
        for (let i = 0; i < data.Series.length; ++i) {
            let series = data.Series[i];
            let maxSerie = nc_maxValue(series.VariableData);
            let minSerie = nc_minValue(series.VariableData);
            if (maxSerie > maxYRange) {
                maxYRange = maxSerie;
            }
            if (minSerie < minYRange) {
                minYRange = minSerie;
            }
        }
        if (minYRange > 0) {
            minYRange = 0;
        }//todo: revisar si gestiono negativos

        //espacio reservado para el contenido del eje y y del eje x
        let leftAxisGap = 40;
        let bottomAxisGap = 15;

        //el eje y siempre tiene numeros, el eje x puede tener cadenas (pasos) o numeros
        nc_createLine(parentSVG, chartX, chartY, chartX, chartY + chartHeight, 'black');
        let scaleY = nc_createScaleLinear(0, chartHeight, minYRange, maxYRange);
        let verticalMarks = 4; //100, 75, 50, 25, 0
        let verticalGap = (maxYRange - minYRange) / verticalMarks;
        for (let i = 0; i < verticalMarks + 1; ++i) {
            nc_createText(parentSVG, chartX - leftAxisGap,
                chartHeight - scaleY.getDomainValue(minYRange + (i * verticalGap)) + chartY,
                (minYRange + (i * verticalGap)));
        }

        //el eje x puede tener numeros y cadenas asi como pasos o evolucion lineal
        let maxXRange = Number.MIN_SAFE_INTEGER;
        let minXRange = Number.MAX_SAFE_INTEGER;
        nc_createLine(parentSVG, chartX, chartY + chartHeight, chartX + chartWidth, chartY + chartHeight, 'black');
        if (data.SeriesDimensions != null) {
            //si tenemos los valores de la dimension definidos los usamos
            //el primer caso es que sean textos
            if (nc_displayTypes[data.Display.DimensionDisplayType] == 'Nominal' || nc_displayTypes[data.Display.DimensionDisplayType] == 'Ordinal') {
                let columnWidth = (chartWidth - chartX) / data.SeriesDimensions.length;
                let columnCenter = columnWidth / 2;
                for (let i = 0; i < data.SeriesDimensions.length; ++i) {
                    nc_createText(parentSVG, (i * columnWidth) + columnCenter + chartX, chartY + chartHeight + bottomAxisGap, data.SeriesDimensions[i]);
                }
            } else {
                //el segundo caso es que sean numeros
                maxXRange = nc_maxValue(data.SeriesDimesions);
                minXRange = nc_minValue(data.SeriesDimesions);
                let scaleX = nc_createScaleLinear(0, chartWidth - chartX, minXRange, maxXRange);
                for (let i = 0; i < data.SeriesDimesions.length; ++i) {
                    nc_createText(parentSVG, scaleX.getDomainValue(data.SeriesDimensions[i]) + chartX, chartY + chartHeight + bottomAxisGap, data.SeriesDimensions[i]);
                }
            }
        } else {
            //por ultimo, si no esta definida los campos de la dimension tendremos que calcularlos
            //aqui NO pueden existir varias series
            let series = data.Series[0];
            if (nc_displayTypes[data.Display.DimensionDisplayType] == 'Nominal' || nc_displayTypes[data.Display.DimensionDisplayType] == 'Ordinal') {
                //let computedDimensions = //nc_distinct(series.DimensionData);
                let columnWidth = (chartWidth - chartX) / series.DimensionData.length;
                let columnCenter = columnWidth / 2;
                for (let i = 0; i < series.DimensionData.length; ++i) {
                    nc_createText(parentSVG, (i * columnWidth) + columnCenter + chartX, chartY + chartHeight + bottomAxisGap, series.DimensionData[i]);
                }
            } else {
                maxXRange = nc_maxValue(series.DimensionData);
                minXRange = nc_minValue(series.DimensionData);
                let scaleX = nc_createScaleLinear(0, chartWidth - chartX, minXRange, maxXRange);
                for (let i = 0; i < series.DimensionData.length; ++i) {
                    nc_createText(parentSVG, scaleX.getDomainValue(series.DimensionData[i]) + chartX, chartY + chartHeight + bottomAxisGap, series.DimensionData[i]);
                }
            }
        }

        return svgChart;
    }

    //Añade un nodo hijo a un nodo
    //function nc_appendChild(parentNode, childNode) {
    //    parentNode.appendChild(childNode);
    //}

    //Añade un atributo a un nodo
    function nc_appendAttribute(node, attrName, attrValue) {
        //node.hasAttribute(attrName) //TODO: mirar esto
        //let attr = nc_document.createAttribute(attrName);
        //attr.value = attrValue;
        //node.setAttributeNode(attr);
        //createAttribute no esta recomendado
        node.setAttribute(attrName, attrValue);
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