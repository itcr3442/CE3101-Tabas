---
title:
  Instituto Tecnológico de Costa Rica\endgraf\bigskip \endgraf\bigskip\bigskip\
  Tarea Corta 3 \endgraf\bigskip\bigskip\bigskip\bigskip
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
  - \usepackage{caption}

lang: es-ES
papersize: letter
classoption: fleqn
geometry: margin=1in
fontsize: 12pt
fontfamily: sans
linestretch: 1.15
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

# Mapeo de Diagrama ER a Modelo Relacional

## Diagrama

![Diagrama Entidad Relación](DiagramaER.png)

## Mapeo de entidad TipoAvion

Esta entidad es una entidad fuerte con solo atributos simples, por lo cual se mapea a una relación con los mismos atributos que tiene como llave primaria el atributo `nombre`.

Table: Relación `TipoAvion`

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| nombre(PK)        | string        |
| secciones_bodega  | entero        |
| capacidad         | entero        |

## Mapeo de entidad Avion y relación "es de modelo"

La entidad `Avion` es una entidad fuerte con atributos simples. Inicialmente se mapea como una relación con los mismos atributos de la entidad, con el atributo numero de serie(`nserie`) como la llave primaria.

Posteriormente, se identifica que `TipoAvion` y `Avion` se encuentran en una relación binaria 1:N, por lo que se agrega un atributo `modelo` en la relación `Avion` que referencia como llave foránea al atributo `nombre` de `TipoAvion`.

Table: Relación `Avion`

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| nserie(PK)        | entero        |
| horas_uso         | entero        |
| modelo(FK)        | string        |

## Mapeo de entidad Vuelo y relación "asociado a"

`Vuelo` es una entidad fuerte de un solo atributo simple. Inicialmente se mapea como una relación con el atributo `numero` como llave primaria de la relación.

Se nota que las entidades `Vuelo` y `Avion` se encuentran en una relación binaria N:1 (Un vuelo solo se asocia a un avión, pero a través del tiempo un avión se asocia a varios vuelos). Se decide entonces agregar un atributo `avion` como llave foránea que referencia a un número de serie de un avión en la relación `Avion`

Table: Relación `Vuelo`

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| numero(PK)        | entero        |
| avion(FK)         | entero        |

## Mapeo de entidad Rol

`Rol` es una entidad fuente con dos atributos simples. Se mapea como una relación con los mismos atributos y se toma `nombre` como la llave primaria.

Table: Relación `Rol`

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| nombre(PK)        | string        |
| descripcion       | string        |

## Mapeo de entidad Trabajador y relación "tiene" con Rol

`Trabajador` es una entidad fuerte con varios atributos simples. Inicialmente se mapea a una relación con estos mismos atributos y se toma `cedula` como la llave primaria.

`Trabajador` y `Rol` se relacionan por medio de la relación "tiene" la cual es de 1:N (un trabajador puede tener un solo rol, pero un rol no es exclusivo para un trabajador). Esta relación se mapeo agregando un atributo `rol` que funciona como llave foránea que referencia el atributo `nombre` de `Rol`.

Table: Relación `Trabajador`

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| cedula(PK)        | entero        |
| password_hash     | string        |
| nombre            | string        |
| primer_apellido   | string        |
| segundo_apellido  | string        |
| rol               | string        |

## Mapeo de entidad Usuario

Usuario es un entidad fuerte con varios atributos simples y uno complejo (nombre completo) el cual a la hora de realizar el mapeo se descompone en atributos simples `nombre`, `primer_apellido`, `segundo_apellido`. Una vez descompuesto nombre completo, se mapea la entidad a una relación del mismo nombre y con los mismos atributos. Se utiliza el atributo `cédula` como llave primaria.

Table: Relación `Usuario`

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| cedula(PK)        | entero        |
| password_hash     | string        |
| nombre            | string        |
| primer_apellido   | string        |
| segundo_apellido  | string        |
| telefono          | entero        |

## Mapeo de entidad Maleta y relaciones "Dueño de" y "Asignada a"

`Maleta` es una entidad fuerte con solo atributos simples. Inicialmente se mapea a una relación con mismo nombre y atributos en la cual `numero` funciona como la llave primaria.

Se nota que existen dos relaciones concernientes a la maleta, el mapeo para cada una es el siguiente:

- "Dueño de" (`Maleta`-`Usuario`): Es una relación N:1, por lo que se agrega un atributo `cedula_usuario` como llave foránea que referencia el atributo `cedula` de la relación `Usuario`.
- "Asignada a" (`Maleta`-`Vuelo`): Es una relación N:1, de igual manera se decide agregar un atributo `nvuelo` como llave foránea que referencia al atributo `numero` de la relación `vuelo.

Table: Relación `Maleta`

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| numero(PK)        | entero        |
| cedula_usuario(FK)| entero        |
| color             | entero        |
| peso              | flotante      |
| costo_envio       | flotante      |
| nvuelo(FK)        | entero        |

## Mapeo de entidad BagCart

`BagCart` es una entidad fuerte con varios atributos simples. Se mapea a una relación con estos mismos atributos y se utiliza el atributo `id` como llave primaria.

Table: Relación `BagCart`

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| id(PK)            | entero        |
| marca             | string        |
| modelo            | entero        |

## Mapeo de relación Trabajador-Maleta "Scan Rayos X"

Esta relación es de tipo 0:N (una maleta puede no haber sido escaneada en rayos x todavía, y un trabajador puede haber escaneado varias maletas), por lo cual se mapea a una referencia cruzada que contiene los atributos de la relación.
En esta referencia cruzada, `cedula_trabajador` y `numero_maleta` son llaves foráneas que representan al atributo `cedula`
de la relación `Trabajador` y el atributo `numero` de la relación Maleta.

Table: Relación `RelScanRayosXMaleta`

| Campo                 |  Tipo de Dato |
:----------------------:|:-------------:|
| cedula_trabajador(FK) | entero        |
| numero_maleta(FK)     | entero        |
| aceptada              | bool          |
| comentarios           | string        |

## Mapeo de relación Trabajador-Maleta "Escaneo/Asignación"

De igual manera al caso de `Scan Rayos X` Esta relación es de tipo 0:N (Una maleta puede no estar ya escaneada y asignada a un avión, pero un trabajador puede procesar varias maletas), por lo cual se mapea a una referencia cruzada que contiene los atributos de la relación.
En esta referencia cruzada, `cedula_trabajador` y `numero_maleta` son llaves foráneas que representan al atributo `cedula`
de la relación `Trabajador` y el atributo `numero` de la relación Maleta.

Table: Relación `RelScanAsignacionMaleta`

| Campo                 |  Tipo de Dato |
:----------------------:|:-------------:|
| cedula_trabajador(FK) | entero        |
| numero_maleta(FK)     | entero        |

## Mapeo de relación Maleta-Bagcart "Contiene"

Esta relación es de tipo 0:N (un bagcart puede contener varias o ninguna maleta, y puede ser que una maleta no esté contenida en un bagcart), por lo que también se mapea como una referencia cruzada.
El atributo `id_bagcart` funciona como llave foránea del atributo `id` de la relación `Bagcart`.
El atributo `numero_maleta` funciona como llave foránea del atributo ``

Table: Relación `RelMaletaBagCart`

| Campo                 |  Tipo de Dato |
:----------------------:|:-------------:|
| id_bagcart(FK)        | entero        |
| numero_maleta(FK)     | entero        |

## Mapeo de relación "Asignación/Cierre"

Esta relación es de cardinalidad 0:N (Un bagcart puede no estar cerrado, un vuelo puede tener asignados varios bagcarts) y tiene un atributo llave `sello`. Esto se mapea a una relación de referencia cruzada en la que `id_bagcart` y `nvuelo` funcionan como llaves foráneas que referencian al atributo `id` de la relación `BagCart` y `numero` de la relación `Vuelo` respectivamente. El atributo `sello` se mapea como un atributo de la relación que funciona como llave primaria.

Table: Relación `RelVueloBagCart`

| Campo                 |  Tipo de Dato |
:----------------------:|:-------------:|
| sello(PK)             | string        |
| id_bagcart(FK)        | entero        |
| nvuelo(FK)            | entero        |
