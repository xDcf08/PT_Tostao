Sistema de Gestión Documental y Analítica - Prueba Técnica Fullstack

Este repositorio contiene la solución completa para la prueba técnica de Desarrollador Semi-Senior Fullstack. El proyecto consiste en un sistema de gestión documental que incluye una API principal en .NET Core, un microservicio de notificación en Laravel y una interfaz de usuario en React.

Stack Tecnológico 💻

    Backend Principal: C# .NET 8

    Microservicio: Laravel 11 (PHP 8.2+)

    Base de Datos: Microsoft SQL Server

    Frontend: React 18 con Vite + TypeScript

    Control de Versiones: Git

Funcionalidades Implementadas ✨

    Gestión de Documentos: Creación, lectura, actualización y eliminación (CRUD) de documentos.

    Paginación: La lista de documentos se carga de forma paginada para un rendimiento óptimo.

    Búsqueda Avanzada: Endpoint para filtrar documentos por autor, tipo y estado.

    Filtro en Tiempo Real: La interfaz de usuario permite buscar por autor y actualiza los resultados mientras el usuario escribe.

    Notificación Asíncrona: Un microservicio en Laravel simula la recepción de un webhook para actualizar el estado de un documento.

    Proceso Automatizado: Un servicio en segundo plano (Hosted Service) en .NET archiva automáticamente los documentos antiguos.

    Optimización de Base de Datos: Creación de índices y queries optimizadas para mejorar el rendimiento.

Prerrequisitos 📝

Antes de empezar, asegúrate de tener instalado el siguiente software:

    .NET SDK (versión 8.0 o superior)

    Node.js (versión 18.x o superior)

    PHP (versión 8.2 o superior) y Composer

    Microsoft SQL Server (2019 o superior, se puede usar en Linux vía Docker)

Guía de Instalación ⚙️

Sigue estos pasos para configurar el entorno de desarrollo.

1. Clonar el Repositorio

git clone https://github.com/xDcf08/PT_Tostao.git

cd PT_Tostao

2. Configuración de la Base de Datos

    Abre SQL Server y crea una nueva base de datos llamada GestionDocumentalDB.

    Ejecuta el script sql/database_setup.sql para crear la tabla Documentos y el Stored Procedure.

    Ejecuta el script sql/seed_data.sql para poblar la base de datos con datos de prueba.

    Ejecuta el script sql/queries_optimizadas.sql para añadir la columna FechaValidado y crear los índices.

3. Configuración del Backend (.NET API)

    Navega a la carpeta del proyecto de .NET: cd PruebaTostao (o el nombre de tu proyecto).

    Abre el archivo appsettings.json.

    Modifica la sección ConnectionStrings con tus credenciales de SQL Server:

    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=GestionDocumentalDB;User Id=SA;Password=TuContraseñaSuperSegura;Encrypt=False;TrustServerCertificate=True;"
    }

4. Configuración del Microservicio (Laravel)

    Navega a la carpeta del proyecto de Laravel: cd GestionDocumental.Notifier.

    Copia el archivo de ejemplo de entorno: cp .env.example .env.

    Modifica el archivo .env con tus credenciales de la base de datos:

    DB_CONNECTION=sqlsrv
    DB_HOST=localhost
    DB_DATABASE=GestionDocumentalDB
    DB_USERNAME=SA
    DB_PASSWORD=TuContraseñaSuperSegura

Instala las dependencias de Composer:

    composer install

5. Configuración del Frontend (React)

    Navega a la carpeta del proyecto de React: cd gestion-documental-ui.

    Instala las dependencias de Node:

    npm install

Ejecución de la Aplicación ▶️

Para correr el proyecto completo, necesitarás tres terminales abiertas, una para cada servicio.

1. Iniciar Backend (.NET API)

    Ubicación: Terminal en la carpeta raíz del proyecto .NET.

    Comando:

    dotnet run

    URL: La API estará disponible en http://localhost:5044.

2. Iniciar Microservicio (Laravel)

    Ubicación: Terminal en la carpeta raíz del proyecto Laravel.

    Comando:

    php artisan serve

    URL: El microservicio estará disponible en http://localhost:8000.

3. Iniciar Frontend (React)

    Ubicación: Terminal en la carpeta raíz del proyecto React.

    Comando:
    
    npm run dev

    URL: La interfaz de usuario estará disponible en http://localhost:5173. Abre esta URL en tu navegador para usar la aplicación.


