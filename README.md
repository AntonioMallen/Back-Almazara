# 🚀 Back-Almazara

Backend para la gestión de Almazara, implementado en **.NET 8** con arquitectura **MVC**, empleando prácticas modernas como Entity Framework Core, LINQ, AutoMapper, FluentValidation y middlewares personalizados. Proporciona una API RESTful robusta y escalable, lista para producción y contenedorizada mediante Docker.

---

## 🛠️ Tecnologías y Herramientas Utilizadas

- **.NET 8**  
- **ASP.NET Core MVC**  
- **Entity Framework Core**  
- **LINQ**
- **AutoMapper**
- **FluentValidation**
- **Middlewares personalizados**
- **Docker**
- **SQL Server**
- **Redis**

---

## 📦 Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/es-es/sql-server) (o dockerizado)
- [Docker](https://www.docker.com/) (opcional, para despliegues)
- [Redis](https://redis.io) (opcional, para cache)

---

## 🚀 Instalación y Puesta en Marcha

1. **Clona el repositorio:**
```
git clone https://github.com/AntonioMallen/Back-Almazara.git
cd Back-Almazara
```

2. **Configura la conexión a la base de datos:**
Edita `appsettings.json`:
```
"ConnectionStrings": {
"DefaultConnection": "Server=TU_SERVIDOR;Database=AlmazaraDB;User Id=USUARIO;Password=CONTRASEÑA;"
}
```

3. **Genera y aplica las migraciones:**
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. **Lanza el proyecto:**
`dotnet run --project BackAlmazara.Api`

6. **(Opcional) Despliegue con Docker:**
```
docker build -t back-almazara .
docker run -d -p 5000:5000 -p 5001:5001 --name back-almazara back-almazara
```

---

📝 Documentación interactiva en `/swagger`

---

## 📁 Estructura del Proyecto
```
Back-Almazara/
├─ BackAlmazara.Controller/ # Controladores, configuración de app
├─ BackAlmazara.Models/ # Entidades, EntityFramework Connection
├─ BackAlmazara.DTOS/ # DTOs
├─ BackAlmazara.Repository/# Persistencia, EF context
├─ BackAlmazara.Services/ # Lógica de negocio, servicios, mapeado DTOs
├─ BackAlmazara.Validation/ # Validadores FluentValidation
├─ BackAlmazara.Middlewares/ # Middlewares personalizados
└─ Dockerfile
```


---

## 🏗️ Patrón y Buenas Prácticas

- **Modelo MVC**
- **Repository + Unit Of Work**
- **Validaciones desacopladas (FluentValidation)**
- **Automapeo (AutoMapper)**
- **Middlewares para gestión de errores y registros**
- **Control de acceso y autenticación**
- **Versionamiento**
- **Cache**

---


## Autor 👨💻

**Antonio Mallen**  
[![GitHub](https://img.shields.io/badge/GitHub-Profile-blue?logo=github)](https://github.com/AntonioMallen)

---

## 🤝 Contribución

1. Haz un fork🍴 del proyecto
2. Crea tu rama (`git checkout -b feature/nueva-feature`)
3. Realiza tus cambios y haz commit (`git commit -am 'Feature terminada'`)
4. Haz push a tu rama (`git push origin feature/nueva-feature`)
5. Abre un Pull Request

---

## 📃 Licencia

Distribuido bajo la licencia **MIT**. Consulta el archivo [LICENSE](LICENSE) para más información.

---

## ⭐️ ¡Gracias por tu interés!

