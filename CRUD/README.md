# API CRUD

## Descripci�n

- Aplicaci�n ASP.NET Core
- Esta Api, se basa en micro servicios que realizan operaciones CRUD a una base de datos SQL SERVER.
- Este archivo describe como ejecutar la API en un entorno local en WINDOWS 11

## Caracter�sticas

- Listar las principales caracter�sticas de tu API.

Autenticaci�n : Utiliza JWT (Json Web Token) para la autenticaci�n del usuario y Barer Token en los encabezados para permmitir el uso de los endpoints.
Gesti�n de recursos : Maneja inyeccion de dependencias con ciclos de Vida SCOPE, metodos asincronos y ejecucion de validaciones en multihilo.
Conexion BD : Utiliza EntityFrameworCore, con consultas LINQ, cadenas de conexion se definen en appsettings.json.
Login y registro de usuario: Las contrase�as se almacenan en HEX SHA-256 
Validaciones : El api, cuenta con validaciones para cada servicio, lo que indica si un campo es requerido o no cumple con su formato.


## Requisitos Previos 

- Visual Studio 2022
- .NET Core SDK 8.0 o superior
- SQL Server Express
- SQL Server Managment Studio
- Postman v11.5.0 o superior
- SO Windows 10/11 (Aunque es multiplataforma, por el momento solo se explica como ejecutar en W10/W11)
- Navegador WEB, como Chrome, Firefox, Edgue o Brave.

## Instalaci�n/Configuraci�n

1. Clona o descarga el repositorio

    https://github.com/Diego-ABM/CRUD.git

2. Navega al directorio del proyecto

    ruta-tu-directorio/CRUD

3.  Restaura los paquetes de NuGet

    En el directorio del proyecto desde cmd o power shell ejecuta 'dotnet restore'

4. En el directorio del proyecto dentro entra en la carpeta 'CRUD' y abre el archivo 'SQL SERVER - Crear Base de datos.sql' con SQL Server Managment Studio
    
    Selecciona todo el contenido y ejecuta el QUERY.

4. Configura la cadena de conexi�n a la base de datos en `appsettings.json`

   En el directorio del proyecto, remplaza la cadena de conexion por la info de tu BD local
   Si tienes problemas con la autenticaci�n agrega 'TrustServerCertificate=True' a la cadena de conexion.

   Ejemplo autenticaci�n usuario y contrase�a :
    ```json
    {
      "ConnectionStrings": {
        "crud": "Server=tu_servidor;Database=CRUD;User Id=tu_usuario;Password=tu_contrase�a;"
      }
    }
    ```

    Ejemplo autenticaci�n de Windows :
    ```json
    {
      "ConnectionStrings": {
        "crud": "Server='tu_servidor';Database=CRUD;User=tu_usuario;TrustServerCertificate=True;Trusted_Connection=True;"
      }
    }
    ```

5. Abre CRUD.sln con Visual Studio 2022
    
    Inicia la aplicaci�n.

## Uso

### Autenticaci�n

    Para autenticarse, env�a una solicitud POST a `/Auth/login`, desde Postman con el siguiente cuerpo:

    ```json
    {
      "correoElectronico": "admin@domain.com",
      "contrasenia": "Password123!"
    }

### Consultar Usuario

    Luego de autenticarse, copia el token que regreso el servicio
    En postman selecciona Authorization -> Auth Type -> Bearer Token y pega el token
    luego envia una solicitud GET a `/User/Read/admin%40gmail.com`

    Esto deberia mostrar los datos del usuario con el que iniciaste sesi�n

    A partir de este punto puedes utilizar los endpoints desde postman, con el token generado.

    Nota : El token tiene timepo de expiraci�n, por lo que si recibes '401 Unauthorized' deberas generar otro de nuevo.

## Documentaci�n
    La documentaci�n completa de la API est� disponible en Swagger. Para acceder, ejecuta la aplicaci�n y navega a http://localhost:puerto/swagger.