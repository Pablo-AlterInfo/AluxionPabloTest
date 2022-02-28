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




## Endpoints

### ​/api​/Users​/Register

Registro de usuarios en el sistema

### /api/Users/Login

Autenticación de usuarios en el sistema, este endpoint regresa un token del tipo "bearer" el cual es necesario para poder acceder a los otros endpoints de la API
![alt text](https://imgur.com/RWb04wK)

```python
import foobar

# returns 'words'
foobar.pluralize('word')

# returns 'geese'
foobar.pluralize('goose')

# returns 'phenomenon'
foobar.singularize('phenomena')
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
