# SchoolHub

SchoolHub es una aplicación web para la gestión escolar, desarrollada con ASP.NET Core MVC, Entity Framework Core y SQL Server.

El proyecto se encuentra en desarrollo. Algunas funcionalidades todavía están pendientes de completar.

## Tecnologías

- .NET 10
- ASP.NET Core MVC
- Entity Framework Core 10
- SQL Server
- ASP.NET Core Identity
- Autenticación mediante cookies
- Control de acceso basado en roles (RBAC)
- Mercado Pago Checkout Pro
- MailKit para envío de correos

## Requisitos

- .NET SDK 10
- SQL Server 2022 o superior
- `dotnet-ef`

La herramienta de Entity Framework está configurada en `dotnet-tools.json`.

## Configuración

1. Copia `.env.example` como `.env`.
2. Completa la cadena de conexión, configuración SMTP, Mercado Pago y el usuario administrador inicial.
3. No agregues `.env` al repositorio, ya que contiene información sensible.

Ejemplo de configuración mínima:

```env
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__ConnectionSQL="Server=localhost;Database=SchoolHubDb;User Id=sa;Password=CHANGE_ME;TrustServerCertificate=True"

SeedAdminUser__UserName=admin
SeedAdminUser__Email=admin@schoolhub.local
SeedAdminUser__Password=CHANGE_ME_Admin123
```

La contraseña del administrador debe cumplir las políticas configuradas en Identity: mínimo 8 caracteres y al menos un número.

## Base de datos

Para aplicar las migraciones existentes:

```bash
dotnet ef database update
```

Al iniciar la aplicación se crean automáticamente los roles definidos (`Admin`, `Student` y `Teacher`). También se crea el usuario administrador si están configuradas las variables `SeedAdminUser__*`.

## Ejecución

Desde la raíz del proyecto:

```bash
dotnet restore
dotnet build
dotnet run
```

La aplicación estará disponible normalmente en:

```text
http://localhost:5095
```

## Autenticación y autorización

- El login está disponible en `/Account/Login`.
- La autenticación utiliza cookies de ASP.NET Core Identity.
- Las vistas administrativas están restringidas al rol `Admin`.
- El menú principal muestra las opciones según el rol del usuario autenticado.
- El cierre de sesión se realiza mediante una solicitud `POST` protegida con antiforgery.

El flujo para establecer la contraseña de nuevos estudiantes utiliza un enlace enviado por correo y está disponible mediante `/Account/ResetPassword`.

## Pagos

El proyecto integra Mercado Pago Checkout Pro. El webhook está disponible en:

```text
POST /api/payments/webhook/mercadopago
```

La configuración correspondiente se encuentra en `.env.example`.

## Estado del proyecto

Actualmente están implementados, entre otros:

- Gestión de estudiantes y catálogos administrativos.
- Autenticación y autorización por roles.
- Envío de enlaces para establecer contraseña.
- Integración inicial con Mercado Pago.

El registro completo de usuarios y otras funcionalidades del sistema continuarán desarrollándose.
