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
    cedula	integer($int32)
    password_hash string
    nombre string
    primer_apellido	string
    segundo_apellido string
    rol	string
}
```

## **Registro de Usuarios**

- `post /usuarios`: Recibe un query string con los credenciales del usuario que registra usuarios. Como cuerpo, recibe un objeto JSON con la siguiente estructura:

```Json
{
    cedula	integer($int32)
    password_hash string
    nombre string
    primer_apellido	string
    segundo_apellido string
    telefono integer($int32)
}
```

## **Creación de maletas**

- `post /maletas`: Recibe un query string con las credenciales del trabajador autorizado para registrar maletas. recibe un objeto JSON con la siguiente estructura:

```Json
{
cedula_usuario	integer($int32)
nvuelo	integer($int32)
color	integer($int32)
peso	number($double)
costo_envio	number($double)
}

```
El número de maleta se retorna en el objeto de respuesta a la consulta.

## **Creación de Bagcart**

- `post /bagcarts`: Recibe un query string con las credenciales del trabajador autorizado para registrar bagcarts. recibe un objeto JSON con la siguiente estructura:

```Json
{
    marca	string
    modelo	integer($int32)
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
    id_vuelo	integer($int32)
    id_bagcart	integer($int32)
    sello	string
}
```

## **Cierre de Bagcart**
## **Reporte Maletas por cliente**
## **Reporte Conciliación de maletas**
## **Asignar/escaneo de una maleta a un bagcart**
## **Rechazo de una Maleta**
## **Asignación de maletas a un avion**

# Aplicación Web


# Aplicación Móvil
