//consideraciones
//no valido que ya este definida nc
//chuleta element
//https://www.w3schools.com/jsref/dom_obj_all.asp

!function (window) {
    console.log('cargo netchart');
    nc = {
        version:1.0
    };

    var nc_document = window.document;
    var nc_selection = null;

    //estos valores deben corresponderse con el enumerado ChartTypeEnum
    const nc_types = ['Debug', 'Bar', 'Line', 'Scatter', 'Bubble', 'Temperature', 'Pie', 'Radar'];

    nc.saludar = function () {
        alert('hola 2');
    }


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
    nc.draw = function (data) {
        console.log('fn draw');
        var data_obj = JSON.parse(data);
        var a1=  data_obj.ChartType;
        var a2 = data_obj.Suggestions; //[]
        var a3 = data_obj.VariableData; //[]
        var a4 = data_obj.DimensionData; //[]
        var a5 = data_obj.ZVariableData; //[]
    }

    function nc_drawBar(data){
        nc_selection.innerHTML = '';
        var svg = nc_document.createElement('svg');
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

    //automatizar la creacion de las formas, rectangulos, burbujas, sectores, triangulos y lineas
    function nc_createRect(x, y, width, height) {

    }

}(window);