# Sistema de Solicitudes de Créditos

Este proyecto es una solución completa para la gestión de solicitudes de crédito bancario, dividido en backend y frontend, cumpliendo con los requerimientos técnicos establecidos.

## 🧱 Estructura del Proyecto

- /aspnet-core → Backend desarrollado en ASP.NET Core 6 (API REST).
- /angular → Frontend SPA desarrollado con Angular 16.

## 🛠 Requisitos

- .NET 6 SDK
- Node.js 18+
- Angular CLI
- SQL Server Express

## 🚀 Configuración y Ejecución

### Backend (aspnet-core)

1. **Abrir la carpeta del backend**  
   En el árbol del explorador, abre la carpeta `aspnet-core`.

2. **Restaurar dependencias**  
   Abre el archivo `ProjectBlack.sln` con Visual Studio y espera a que se restauren automáticamente los paquetes NuGet.

3. **Aplicar migraciones a la base de datos**  
   - Abre la **Consola del Administrador de paquetes NuGet**:  
     `Herramientas > Administrador de paquetes NuGet > Consola`.
   - En la lista desplegable, selecciona el proyecto:  
     `ProjectBlack.EntityFrameworkCore`.
   - Ejecuta el comando:
     ```
     Update-Database
     ```

4. **Ejecutar el backend**  

### Frontend (angular-app)

1. Entrar a la carpeta:
   cd angular-

2. Instalar dependencias:
   yarn install --force

3. Ejecutar aplicación Angular:
   npm start

## 🔐 Autenticación

- Se utiliza autenticación basada en JWT.
- Angular almacena el token y lo envía en el header Authorization:
  Authorization: Bearer <token>

## 📤 Exportación
Se puede exportar el historial de solicitudes a PDF o Excel.

# Restaurar base de datos, si prefiere con datos ya duardados

1. Abrir SQL Server Management Studio (SSMS)
2. Clic derecho en "Databases" > Restore Database...
3. Seleccionar "Device" > Agregar > Buscar `ProjectBankDb.bak`
4. Seguir el asistente y restaurar
### 🔐 NOTA
- Por defecto se incluye el usuario admin:
1. user:admin
2. pass:123qwe

## ✅ Evaluación

El sistema incluye:
- Separación lógica de backend y frontend
- Código organizado y comentado
- Evaluación automática de solicitudes
- Control de roles (Solicitante / Analista)
- Registro de acciones

