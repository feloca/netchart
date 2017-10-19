//consideraciones
//no valido que ya este definida nc
//chuleta element
//https://www.w3schools.com/jsref/dom_obj_all.asp

//Problema espacios de nombre
//https://stackoverflow.com/questions/17520337/dynamically-rendered-svg-is-not-displaying

!function (window) {
    console.log('cargo netchart');
    nc = {
        version:1.0
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
        //data_obj.VariableData; //[] -> la y
        //data_obj.DimensionData; //[] -> la x
        //data_obj.ZVariableData; //[]

        switch (nc_types[dataObj.ChartType]) {
            case 'Debug':
                nc_drawDebug(dataObj);
                break;
            case 'Bar':
                nc_drawBar(dataObj);
                break;
            case 'Line':
                break;
            case 'Scatter':
                break;
            case 'Bubble':
                break;
            case 'Temperature':
                break;
            case 'Pie':
                break;
            case 'Radar':
                break;
            default:
                break;
        }
    }

    function nc_drawDebug(data) {

    }

    function nc_drawBar(data) {
        //TODO: sacar la creación del svg a una funcion
        var svg = nc_document.createElementNS(nc_svgns, 'svg');
        nc_appendStyleAttribute(svg, 'width', '100%');
        nc_appendStyleAttribute(svg, 'height', '100%');

        let svgHeight = nc_selection.clientHeight;
        let svgWidth = nc_selection.clientWidth;

        //calcular la escala
        for (let i = 0; i < data.DimensionData.length; ++i) {
            let dDimension = data.DimensionData[i];
            let dVariable = data.VariableData[i];
            let laY = svgHeight - dDimension;
            nc_createRect(svg, i * 10, laY, 10, dDimension, 'teal');
        }

        nc_selection.innerHTML = '';
        nc_appendChild(nc_selection, svg);
    }

    //function nc_drawLine() { }
    //function nc_drawScatter() { }
    //function nc_drawBubble() { }
    //function nc_drawTemperature() { }
    //function nc_drawPie() { }
    //function nc_drawRadar() { }

    function nc_appendChild(parentNode, childNode) {
        parentNode.appendChild(childNode);
    }

    function nc_appendAttribute(node, attrName, attrValue) {
        //node.hasAttribute(attrName) //TODO: mirar esto
        var attr = nc_document.createAttribute(attrName);
        attr.value = attrValue;
        node.setAttributeNode(attr);
    }

    function nc_appendStyleAttribute(node, attrName, attrValue) {
        node.style[attrName] = attrValue;
    }

    //automatizar la creacion de las formas, rectangulos, burbujas, sectores, triangulos y lineas
    function nc_createRect(parentNode, x, y, width, height, color) {
        var rect = nc_document.createElementNS(nc_svgns, 'rect');
        nc_appendAttribute(rect, 'x', x);
        nc_appendAttribute(rect, 'y', y);
        nc_appendAttribute(rect, 'width', width);
        nc_appendAttribute(rect, 'height', height);
        nc_appendAttribute(rect, 'fill', color);
        parentNode.appendChild(rect);
    }

    //FUNCIONES AUXILIARES
    //max
    //min
    //scaleLinear

    //Calcula el máximo de una colección
    function nc_maxValue(list) {
        var max = null;
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

    //Calcula el minimo de una colección
    function nc_minValue(list) {
        var min = null;
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
    function nc_createScaleLinear(domainMin, domainMax, rangeMin, rangeMax){
        var scale = {
            domainMin: domainMin,
            domainMax: domainMax,
            rangeMin: rangeMin,
            rangeMax: rangeMax,
            getValue: function (dataValue) {
                //regla de 3

                var outputRange = domainMax - domainMin;
                var inputRange = rangeMax - rangeMin;
                var inputValue = dataValue - rangeMin;
                //inputRange es el 100%
                //AQUI ME HE QUEDADO

            }
        }

        return scale;
    }

    //FUNCIONES AUUXILIARES FIN

}(window);