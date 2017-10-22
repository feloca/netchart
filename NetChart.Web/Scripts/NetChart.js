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

    //funcion para GUI de sugerencias

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
                //nc_drawChartTemperature(dataObj);
                break;
            case 'Pie':
                break;
            case 'Radar':
                break;
            default:
                break;
        }
    }

    function nc_drawChartDebug(data) {

    }

    function nc_drawChartBar(data) {
        //TODO: sacar la creación del svg a una funcion
        //var svg = nc_document.createElementNS(nc_svgns, 'svg');
        //nc_appendStyleAttribute(svg, 'width', '100%');
        //nc_appendStyleAttribute(svg, 'height', '100%');
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

        let scaleX = nc_createScaleLinear(0, chartWidth, minDimension, maxDimension);
        let scaleY = nc_createScaleLinear(0, chartHeight, minVariable, maxVariable);

        //creo que hace glat la escala z, ¿pensar en el dominio?
        //AQUI ME HE QUEDADO

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