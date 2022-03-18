---
title:
  Instituto Tecnológico de Costa Rica\endgraf\bigskip \endgraf\bigskip\bigskip\
  Manual de Usuario Sistema Tabas \endgraf\bigskip\bigskip\bigskip\bigskip
author:
  - José Morales Vargas, carné 2019024270
  - Alejandro Soto Chacón, carné 2019008164
  - Ignacio Vargas Campos, carné 2019053776
  - José Retana Corrales, carné 2020144743
date: \bigskip\bigskip\bigskip\bigskip Área Académica de\endgraf Ingeniería en Computadores \endgraf\bigskip\bigskip\ Bases de Datos \endgraf  (CE3101) \endgraf\bigskip\bigskip Profesor Marco Rivera Meneses \endgraf\vfill  Semestre I 2022
header-includes:
  - \setlength\parindent{24pt}
  - \usepackage{url}
  - \usepackage{float}
  - \floatplacement{figure}{H}
lang: es-ES
papersize: letter
classoption: fleqn
geometry: margin=1in
fontsize: 12pt
fontfamily: sans
linestretch: 1.5
bibliography: bibliografia.bib
csl: /home/josfemova/UsefulRepos/styles/ieee.csl
nocite: |
...

\maketitle
\thispagestyle{empty}
\clearpage
\tableofcontents
\pagenumbering{roman}
\clearpage
\pagenumbering{arabic}
\setcounter{page}{1}

# Servidor

La interacción con el servidor se da por medio de consultas http a la dirección ip del servidor en el puerto 5000. A continuación se describe como llewvar a cabo las operaciones solicitadas en la especificación. Puede probar cada una de las operaciones en la aplicación de swagger.

## **Inicio de Sesión**

- `get /check_login` o `get /check_login_usuario` : Recibe un query string que contiene los valores de `cedula` y `password_hash` de un usuario. Retorna un valor JSON "success" cuyo valor es de 1 si las credenciales son correctas, y 0 de lo contrario. Si los credenciales son válidos, se espera que la aplicación cliente guarde registro de ellos y los utilice para llevar a cabo otras consultas.

## **Registro de trabajadores**

- `post /trabajadores`: Recibe un query string con los credenciales del usuario que registra trabajadores. Como cuerpo, recibe un objeto JSON con la siguiente estructura:

```Json
{
    cedula integer($int32)
    password_hash string
    nombre string
    primer_apellido string
    segundo_apellido string
    rol string
}
```

## **Registro de Usuarios**

- `post /usuarios`: Recibe un query string con los credenciales del usuario que registra usuarios. Como cuerpo, recibe un objeto JSON con la siguiente estructura:

```Json
{
    cedula integer($int32)
    password_hash string
    nombre string
    primer_apellido string
    segundo_apellido string
    telefono integer($int32)
}
```

## **Creación de maletas**

- `post /maletas`: Recibe un query string con las credenciales del trabajador autorizado para registrar maletas. recibe un objeto JSON con la siguiente estructura:

```Json
{
cedula_usuario integer($int32)
nvuelo integer($int32)
color integer($int32)
peso number($double)
costo_envio number($double)
}
```

El número de maleta se retorna en el objeto de respuesta a la consulta.

## **Creación de Bagcart**

- `post /bagcarts`: Recibe un query string con las credenciales del trabajador autorizado para registrar bagcarts. recibe un objeto JSON con la siguiente estructura:

```Json
{
    marca string
    modelo integer($int32)
}
```

El número de bagcart se retorna en el objeto de respuesta a la consulta.

## **Asignación de avión a vuelo**

- `post /vuelos`: Recibe un query string con las credenciales del trabajador autorizado para llevar a cabo la operación. Recibe un objeto json que solo contiene el número de serie del avión que se quiere asignar a un vuelo.

```JSON
{
    avion integer($int32)
}
```

## **Asignación de Bagcart a vuelo**

- `post /rel/vuelo_bagcart`: Recibe credenciales en el query string y un objeto JSON con los siguientes campos:

```JSON
{
    id_vuelo integer($int32)
    id_bagcart integer($int32)
}
```

## **Cierre de Bagcart**

- `post /rel/vuelo_bagcart/cierre/bagcart/{id}`: En el query string se dan las credenciales del trabajador que cerrará el bagcart y el valor del sello del bagcart. En la ruta recibe el id del bagcart a cerrar. El bagcart a cerrar debe estar previamente asignado a un vuelo.

## **Reporte Maletas por cliente**

- `get /reportes/maletas_x_cliente/{cedula}`: Recibe una ruta con la cedula del cliente. Retorna los datos del usuario y la lista de sus maletas.

## **Reporte Conciliación de maletas**

- `get /reportes/conciliacion_maletas/{nvuelo}`: Recibe una ruta con el número de vuelo y retorna los datos necesarios para el reporte solicitado.

## **Asignar/escaneo de una maleta a un bagcart y rechazo de una maleta**

- `post /rel/scan_rayosx_maleta`: Recibe el pase del trabajador(la cédula va implícita en el cuerpo de mensaje) que reliza el escaneo y un objeto JSON con la siguiente estructura:

```JSON
{
    cedula_trabajador integer($int32)
    numero_maleta integer($int32)
    aceptada boolean
    comentarios string
}
```

- El valor del campo "aceptada" indica si la maleta fue rechazada o aceptada en el paso de scan.

Si la maleta es aceptada se procede a enviar otro request:

- `post /rel/maleta_bagcart`: Recibe las credenciales de quien asigna una maleta al bagcart y un objeto JSON con la siguiente estructura:

```JSON
{
    numero_maleta integer($int32)
    id_bagcart integer($int32)
}
```

## **Asignación de maletas a un avion**

- `post /rel/scan_asignacion_maleta`: Recibe el pase del trabajador que escanea una maleta y la sube al avion en el query string. El resto de los datos necesarios para realizar la tarea se envían en un objeto JSON con la siguiente estructura:

```JSON
{
    cedula_trabajador integer($int32)
    numero_maleta integer($int32)
}
```

# Aplicación Web

# Aplicación Móvil
