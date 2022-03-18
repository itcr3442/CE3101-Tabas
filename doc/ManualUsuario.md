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
## **Creación de maletas**
## **Creacuón de Bagcart**
## **Asignación de avión a vuelo**
## **Asignación de Bagcart a vuelo**
## **Cierre de Bagcart**
## **Reporte Maletas por cliente**
## **Reporte Conciliación de maletas**
## **Asignar/escaneo de una maleta**
## ****
## ****
## ****

# Aplicación Web


# Aplicación Móvil
