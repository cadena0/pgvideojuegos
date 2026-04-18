# Migraciones de Base de Datos

Ejecuta estos comandos en la terminal dentro de la carpeta del proyecto:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Si no tienes EF Tools instalado globalmente:
```bash
dotnet tool install --global dotnet-ef
```
