# Instalar Proyecto

Proyecto de Prueba para Aluxion Labs

## Requerimientos

* Visual Studio (2019 o 2022)
  - Con .NET Core 5
  - Con Complementos de instalación para proyectos ASP NET
* SQL Server Management Studio
* SQL Server
* Amazon AWS CLI

## Instalación

Descomprimir el archivo **SoloAluxion.zip** utilizando la contraseña provista en el mail enviado previamente

En el CLI de AWS, realizar configuración de credenciales con las credenciales que se encuentran en el archivo de texto dentro del archivo zip. Utilizar perfil con nombre **PabloTest**

```bash
aws configure --profile PabloTest
```


Abrir el archivo **AluxionTest.sln** para abrir la Solution en Visual Studio.

Si Visual Studio no hace un "restore" de los nuget packages automáticamente, ejecutar el comando en el terminal de Visual Studio: 
```bash
dotnet restore
```

Luego, en el Package Manager Console, ejecutar estos comandos, para realizar la creación de la Base de Datos en Microsoft SQL Server.
```bash
add-migration SetupMigration
```
```bash
update-database
```


Finalmente, ejecutar el proyecto.




## Endpoints de Acceso

### ​/api​/Users​/Register

Registro de usuarios en el sistema

### /api/Users/Login

Autenticación de usuarios en el sistema, este endpoint regresa un token del tipo "bearer" el cual es necesario para poder acceder a los otros endpoints de la API. 

Para introducir el bearer token desde Swagger, acceder al botón "Authorize" en la parte superior derecha de la página
![alt text](https://i.imgur.com/RWb04wK.jpg)

Luego, colocar el token con el formato mostrado en la imagen:
![alt text](https://i.imgur.com/myYa6HX.jpg)

Hecho esto, se podrá acceder al resto de Endpoints disponibles.

## Endpoints de AWS S3

Por defecto, todos los endpoints de AWS S3 están apuntando al bucket "bucket-prueba-pablo" que creé para este proyecto. 

Están implementados todos los casos solicitados por la prueba práctica:
* Subir archivos
* Descargar archivos
* Listar archivos
* Eliminar archivos
* Renombrar archivos
* Obtener URL de archivos
* Subir directamente imagen desde API externa de imagenes (Unsplash)

## Endpoints de Unsplash

Se cuentan con los siguientes endpoints

* Obtener lista de imágenes
* Obtener imagen específica utilizando el Id de la misma


# Que faltó

* Realizar pruebas unitarias automatizadas
* Dockerizar el proyecto (tuve muchas dificultades intentando lograr este paso, nunca lo habia hecho antes y me quede sin tiempo)
* Subir los archivos al bucket de AWS S3 que me indicaron. Aparentemente las llaves de acceso están vencidas o son inválidas.

# Finalmente

Gracias por la oportunidad. Estoy atento a sus comentarios,

Pablo Izquierdo
