# TechSolutions Program

## Descripción

Sistema web orientado a la gestión de clientes, proyectos, tareas y reportes de la empresa.

## Stack Tecnológico

* **Backend:** C# con ASP.NET Core MVC
* **Base de Datos:** SQL Server gestionado con Entity Framework Core
* **Autenticación:** ASP.NET Core Identity
* **Frontend:** HTML, CSS, JavaScript, Bootstrap y jQuery

## Patrones de Diseño Aplicados

* Inyección de Dependencias
* Patrón Repositorio para la abstracción del acceso a datos
* Patrón Estrategia para la generación dinámica de reportes en PDF y Excel

## Requisitos Previos

* .NET SDK
* SQL Server

## Instrucciones de Instalación

1. Clonar el repositorio en tu máquina local.
2. Modificar la cadena de conexión de la base de datos dentro del archivo appsettings.json.
3. Ejecutar los archivos SQL ubicados en la carpeta SQLscripts para crear la base de datos y cargar la información de prueba.
4. Restaurar los paquetes NuGet requeridos.
5. Compilar y ejecutar la aplicación.

## Acceso al Sistema

Revisa el archivo CREDENCIALES.md incluido en la raíz del repositorio para obtener los usuarios y contraseñas de inicio de sesión predeterminados.
