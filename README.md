# 🧩 Sistema de Reclamos con Arquitectura Limpia - Curso: Identity & OAuth2 — Proyecto Final

## 📘 Descripción General
Este proyecto implementa un sistema web completo para la **gestión de reclamos**, desarrollado siguiendo las mejores prácticas de programación vistas en clase.  
Incluye autenticación segura con **CAPTCHA**, registro de reclamos y consulta de reclamos por rango de fechas.  

---

## 🚀 Funcionalidades Principales

- ✅ **Autenticación Segura con CAPTCHA**  
  Se ha integrado un sistema CAPTCHA en el módulo de Login para reforzar la seguridad ante accesos automatizados o intentos de fuerza bruta.

- 📝 **Registro de Reclamos**  
  Los usuarios pueden registrar reclamos de manera sencilla a través de un formulario intuitivo.

- 📅 **Listado por Rango de Fechas**  
  Se puede visualizar el historial de reclamos aplicando filtros por fechas, facilitando la búsqueda y análisis de información.

- 🧱 **Arquitectura Limpia (Clean Architecture)**  
  El proyecto fue desarrollado bajo los principios de **Arquitectura Limpia**, promoviendo separación de responsabilidades, independencia de frameworks y fácil mantenibilidad.

- 🧩 **Patrones de Diseño Aplicados**  
  Se aplicaron los patrones de diseño abordados en clase, tales como:
  - Repository Pattern  
  - Dependency Injection  
  - DTOs y ViewModels  

---

## 🧠 Aprendizajes y Alcances

- Se implementaron **todos los temas vistos en clase**: autenticación, inyección de dependencias, patrones de diseño, controladores RESTful, validaciones y persistencia de datos.  
- Se fortalecieron las **buenas prácticas de arquitectura** y la **modularidad del código**.
- El sistema está preparado para **escalar nuevas funcionalidades** sin romper la estructura base.

---

## ⚙️ Tecnologías Utilizadas

- **Backend:** .NET 9 / ASP.NET Core  
- **Frontend:** Blazor
- **Base de Datos:** Posgrest
- **Seguridad:** CAPTCHA + Identity / JWT
- **Infraestructura:** Docker Compose  
- **Arquitectura:** Clean Architecture (Domain, Application, Infrastructure, Presentation)

---

## 🐳 Infraestructura con Docker

La infraestructura del proyecto se levanta mediante **Docker Compose**, utilizando cuatro servicios esenciales:

```yaml
services:
    postgresql_security:
        image: postgres:latest
        container_name: security_db
        environment:
            POSTGRES_USER: admin
            POSTGRES_PASSWORD: Password2025
            POSTGRES_DB: security_db
        ports:
            - "1500:5432"
        volumes:
            - D:\Capacitaciones\postgresql_db_security:/var/lib/postgresql    

    vault:
      image: hashicorp/vault:1.17
      ports:
        - "8200:8200"
      environment:
        VAULT_DEV_ROOT_TOKEN_ID: "Ucyi1ziPEHQO4b"
        VAULT_DEV_LISTEN_ADDRESS: "0.0.0.0:8200"
      command: "server -dev -dev-listen-address=0.0.0.0:8200"
      volumes:
        - D:\Capacitaciones\vault_security:/vault/file   

    redis:
        image: redis:7.4-alpine
        container_name: redis_auth
        restart: always
        ports:
            - "6379:6379"
        volumes:
            - D:\Capacitaciones\redis_security:/data
        command: ["redis-server", "--appendonly", "yes"]

    cap:
        image: tiago2/cap:latest
        container_name: cap-standalone
        ports:
            - "3000:3000"
        environment:
            ADMIN_KEY: 63f4945d921d599f27ae4fdf5bada3f1
            ENABLE_ASSETS_SERVER: "true"
            WIDGET_VERSION: "latest"
            WASM_VERSION: "latest"
            DATA_PATH: "./.data"
        restart: unless-stopped
        volumes:
            - D:\Capacitaciones\cap_security:/usr/src/app/.data
```

### 🧩 Descripción de los Servicios

| Servicio | Imagen | Propósito |
|-----------|--------|------------|
| **postgresql_security** | `postgres:latest` | Base de datos principal del sistema. Almacena la información de usuarios, reclamos y demás entidades del sistema. Los datos se persisten localmente mediante un volumen. |
| **vault** | `hashicorp/vault:1.17` | Servicio de gestión de secretos. Almacena tokens, contraseñas y claves de acceso (por ejemplo, el AccessToken del CAPTCHA) de forma segura. |
| **redis_auth** | `redis:7.4-alpine` | Caché en memoria de alta velocidad utilizada para autenticación, sesiones y almacenamiento temporal de datos críticos. Configurado con persistencia AOF. |
| **cap-standalone** | `tiago2/cap:latest` | Servicio externo de **CAPTCHA** utilizado en el Login para validar interacciones humanas. Permite reforzar la seguridad en el inicio de sesión. |

---

## 👨‍💻 Autor

Proyecto desarrollado por **Eder Arbulú**  
📅 Año: 2025  
