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

# Tarea Corta 3

![Diagrama Entidad Relación](DiagramaER.png)

## Mapeo de Diagrama ER a Modelo Relacional


### Mapeo de entidad TipoAvion

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| nombre(PK)        | string        |
| secciones_bodega  | entero        |
| capacidad         | entero        |

### Mapeo de entidad Avion

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| nserie(PK)        | entero        |
| horas_uso         | entero        |
| tipo(FK)          | string        |

### Mapeo de entidad Vuelo

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| numero(PK)        | entero        |
| avion(FK)         | entero        |

### Mapeo de entidad Rol

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| nombre(PK)        | string        |
| descripcion       | string        |

### Mapeo de entidad Trabajador y relación "tiene" con Rol

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| cedula(PK)        | entero        |
| password_hash     | string        |
| nombre            | string        |
| primer_apellido   | string        |
| segundo_apellido  | string        |
| rol               | string        |

### Mapeo de entidad Usuario

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| cedula(PK)        | entero        |
| password_hash     | string        |
| nombre            | string        |
| primer_apellido   | string        |
| segundo_apellido  | string        |
| telefono          | entero        |

### Mapeo de entidad maleta y relaciones "Dueño de" y "Asignada a"

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| numero(PK)        | entero        |
| cedula_usuario(FK)| entero        |
| color             | entero        |
| peso              | flotante      |
| costo_envio       | flotante      |
| nvuelo(FK)        | entero        |

### Mapeo de entidad BagCart

| Campo             |  Tipo de Dato |
:------------------:|:-------------:|
| id(PK)            | entero        |
| marca             | string        |
| modelo            | entero        |

### Mapeo de relación Trabajador-Maleta "Scan Rayos X"

| Campo                 |  Tipo de Dato |
:----------------------:|:-------------:|
| cedula_trabajador(FK) | entero        |
| numero_maleta(FK)     | entero        |
| aceptada              | bool          |
| comentarios           | string        |

### Mapeo de relación Trabajador-Maleta "Escaneo/Asignación"

| Campo                 |  Tipo de Dato |
:----------------------:|:-------------:|
| cedula_trabajador(FK) | entero        |
| numero_maleta(FK)     | entero        |

### Mapeo de relación Maleta-Bagcart "Contiene"

| Campo                 |  Tipo de Dato |
:----------------------:|:-------------:|
| id_bagcart(FK)        | entero        |
| numero_maleta(FK)     | entero        |

### Mapeo de relación "Asignación/Cierre"

| Campo                 |  Tipo de Dato |
:----------------------:|:-------------:|
| sello(PK)             | string        |
| id_bagcart(FK)        | entero        |
| nvuelo(FK)            | entero        |


