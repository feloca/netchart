﻿entiendo que con la propiedad x deberia de inferir la propiedad y a partir de sus maximos y minimos
al enchufar las propiedades de datos permitir tipar (incluir opcion sin tipo), las listas de datos object?, y validar que coincidan con el tipo del dato
1, 2 o 3 series de datos
Tipo -> enumerado, tipo de grafico seleccionado
ListaX, ListaY, ListZ
PropiedadX, PropiedadY, PropiedadZ
        
ModoDeveloper 
SUGERENCIAS[]


temas de presentacion
titulo
titulo eje x, eje y (incluir propiedad de configuración)

Pensar como presentar la ayuda de las sugerencias y como dejar que el desarrollador copie la configuración.

04/08/2017
una unica lista, 
campox
campoy
valoresX
valoresY
filtroY, mejor agregadaY -> si filtro en y automaticamente aplico un grupo segun el valor de campox

clase:bool
amplitudclase
=> si creo clases, se podria usar filtro x para saber si muestro el minimo, el maximo, la media, etc...

otra opcion, si solo meto campox y no indico el campoy, no podre aplicar filtro y ademas tomare valores y la posicion en el array

ordenar por campoy, de manera ascendente y descendente 

SI PONEMOS CAMPO Z, pues los campos x e y definen los lugares y z es el valor de la otra propiedad, meter filtroz y aplicar clase y amplitud clase

NOTAS:
hay que empezar a definir modelos a representar y segun ellos ver como ajustamos la clase.
Me gusta edad y altura, media como ejemplo de coleccion.
HOJA EXCEL


incluir propiedad para mostrar datos relativos?, si lo hago, coger la propiedad z o la principal, sumar los valores de todos sus datos y 
ese es el 100%, luego la propiedad con datos tendria el valor del porcentaje, ademas, cambiar uno de los ejes para que sus valores vayan del 
0 al 100 % (en este caso, es la propiedad de los datos)

PENDIENTE: no he tenido en cuenta la gestión de agregados o variables de tipo string (categóricos), tampoco gestiono que la propiedad sea un objeto y no un tipo básico

METER EL ARBOL DE REGLAS EN EL TFM
combinaciones de variables
variable principal tiene que existir siempre
	a)si variable secundaria nula -> usar valores discretos (1, 2, 3, 4, ...) según el número de datos
	a)si variable secundaria no nula -> usar el valor correspondiente de esa fila
	b)si variable z nula -> no hacer nada
	b)si variable z distinto de nula, poner el valor de z en la posicion de x union y

TEMA DE AGRUPACIONES
a) con solo variable principal si se agrupa no hacer nada (agregacion con un unico dato, luego siempre obtenemos el mismo dato)
b) si existe segunda variable y agregacion en la principal, perfecto, caso facil agrupar por los distintos valores de la variable secundaria
c) caso de z, 
	c1) si variable principal no agregada, => ¿creo que debe estar agregada siempre? => si no secundaria, poner el valor de z directamente, si secundaria hacer un grupo que cumpla x e y y hacer el agregado de z
	c2) si variable principal agregada -> buscar todos los elementos de esa agregacion, la sera el valor de la agregacion de la var principal y la z sera el nuevo agregado de z

Creo que para output, vamos a necesitar un tipo complejo con valor x, y, y una lista de objetos cumplan estos valores x e y, esto podría simplificar mucho la lógica

06/09/2017 MAÑANA
1 -crear la propiedad de output con x e y mas la lista de valores. Pensar en el formato JSON a la vez
2 -revisar la generacion de datos de salida
3 - revisar las sugerencias
4- prueba
5 - limpiar codigo comentado y empezar a borrar cosas viejas

09/09/2017 
Me he quedado en el generador de sugrencias de la clase Chart
Hay que crear un generador de CSV para probar en tableau la logica, generar los mismos datos en objetos para poder probar los resultados.

11/09/2017
Definir un nuevo tipo dimensionProperty que herede de property y permitir definir intervalos -> pensar si es necesario, propiedades limites, y podría trabajar sin nombre de variable
=> aqui ando, he metido la propiedad para dimension y he ajustado la funcion de validaciones, hay que seguir revisando el uso de la propiedad dimension

17/10/2017 => ESTO DEBERIA IR EN LA MEMORIA
Como distribuir ficheros JS -> google "c# can i create a javascript files dll and use it on mvc"
Primera opcion, meter los ficheros en la dll y en el cliente usar un helper que falsee una ruta web compartida y devuelva el fichero -> uso poco natural
https://stackoverflow.com/questions/11842988/mvc-razor-include-js-css-files-from-another-project

Crear una carpeta comun y lincar los ficheros a esta carpeta común -> creo que ahora mismo es lo más interesante, me encaja para el desarrollo
https://stackoverflow.com/questions/8540292/how-do-you-share-scripts-among-multiple-projects-in-one-solution

NOTA: La solución optima sería construir un paquete de nuget porque tengo una dll y un script, e instalarlo mediante el gestor de paquetes de nuget. 
Pero esto llevaría un tiempo. No me vale bower o npm porque no solucionaria el tema de la dll.

OJO,los links en debug no van porque no se crean bundles -> no crea el bundle correctamente, incluso en modo release. Lo copio
https://stackoverflow.com/questions/17814965/bundling-a-linked-javascript-file

31/10/2017
en el script de js hace falta revisar el layout para que no incluya barras de ejes cuando el gráfico no lo requiera.