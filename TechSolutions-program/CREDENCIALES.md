# 🔐 Credenciales de Acceso - TechSolutions

Este documento contiene las credenciales de los usuarios de prueba creados automáticamente en la base de datos.

## 👥 Usuarios de Prueba

Los siguientes usuarios se crean automáticamente al ejecutar la aplicación por primera vez:

### 👤 Administrador del Sistema
- **Email:** `admin@techsolutions.com`
- **Contraseña:** `Admin123!`
- **Rol:** Administrador
- **Código Empleado:** ADM001
- **Permisos:** 
  - ✅ Acceso total al sistema
  - ✅ Gestión de proyectos
  - ✅ Gestión de clientes
  - ✅ Gestión de tareas
  - ✅ Visualización de dashboard
  - ✅ Generación de reportes

---

### 👤 Líder de Proyecto
- **Email:** `lider@techsolutions.com`
- **Contraseña:** `Lider123!`
- **Rol:** Líder
- **Nombre Completo:** Juan Pérez - Líder de Proyecto
- **Código Empleado:** LID001
- **Permisos:**
  - ✅ Gestión de proyectos
  - ✅ Gestión de clientes
  - ✅ Gestión de todas las tareas
  - ✅ Visualización de dashboard
  - ✅ Generación de reportes

---

### 👤 Desarrollador
- **Email:** `desarrollador@techsolutions.com`
- **Contraseña:** `Dev123!`
- **Rol:** Desarrollador
- **Nombre Completo:** María García - Desarrolladora
- **Código Empleado:** DEV001
- **Permisos:**
  - ✅ Ver proyectos y clientes
  - ✅ Ver y actualizar **solo sus tareas asignadas**
  - ✅ Visualización de dashboard
  - ⛔ No puede crear/editar proyectos
  - ⛔ No puede asignar tareas a otros

---

## 🚀 Cómo Iniciar Sesión

1. **Ejecutar la aplicación:**
   ```bash
   dotnet run
   ```

2. **Abrir el navegador en:**
   ```
   https://localhost:5126
   ```

3. **La aplicación te redirigirá automáticamente a la página de login** (`/Autenticacion/Login`)

4. **Ingresar credenciales:**
   - Utiliza cualquiera de los usuarios listados arriba
   - La página de login incluye un acordeón con las credenciales visibles

5. **Después del login exitoso:**
   - Serás redirigido automáticamente al **Dashboard** (`/Seguimiento/Index`)
   - El menú de navegación se mostrará según tu rol

---

## 🔒 Seguridad Implementada

### Autenticación
- ✅ ASP.NET Core Identity para gestión de usuarios
- ✅ Contraseñas hasheadas en la base de datos (nunca en texto plano)
- ✅ Tokens Anti-CSRF en formularios
- ✅ EmailConfirmed = true para permitir login inmediato

### Autorización (RBAC - Role-Based Access Control)
- ✅ Atributo `[Authorize]` en todos los controladores
- ✅ Restricciones por rol en acciones específicas
- ✅ Menú de navegación dinámico según rol del usuario

### Protección de Rutas
- ⛔ Sin autenticación → Redirige a `/Autenticacion/Login`
- ⛔ Sin permisos → Redirige a `/Autenticacion/AccessDenied`

---

## 📊 Estructura de la Base de Datos

### Tabla: AspNetUsers (Usuarios)
Los usuarios se almacenan en la tabla `AspNetUsers` con las siguientes columnas adicionales:
- `NombreCompleto` (string)
- `CodigoEmpleado` (string, opcional)
- Columnas estándar de Identity: `Email`, `UserName`, `PasswordHash`, etc.

### Tabla: AspNetRoles (Roles)
- Lider
- Desarrollador
- Administrador

### Tabla: AspNetUserRoles (Relación Usuario-Rol)
Relaciona cada usuario con su(s) rol(es) asignado(s).

---

## 🔄 Página de Inicio

### Antes del Login
- **Ruta por defecto:** `/Autenticacion/Login`
- **Menú:** Oculto (solo se muestra el botón de login)

### Después del Login
- **Ruta por defecto:** `/Seguimiento/Index` (Dashboard)
- **Menú:** Visible con opciones según el rol del usuario

---

## 📝 Notas Importantes

1. **Eliminación de `/Home/Index`:**
   - Se eliminó el controlador `HomeController` y su vista
   - La ruta por defecto ahora es `/Autenticacion/Login`

2. **Creación Automática de Usuarios:**
   - Los usuarios se crean automáticamente en `Program.cs` al iniciar la aplicación
   - Si los usuarios ya existen, no se duplican

3. **Requisitos de Contraseña:**
   - Mínimo 6 caracteres
   - Al menos una letra mayúscula
   - Al menos un dígito
   - Al menos un carácter especial

4. **Para Desarrollo:**
   - `RequireConfirmedAccount = false` permite login sin confirmar email
   - **En producción, cambiar a `true` y configurar servicio de email**

---

## 🛠️ Solución de Problemas

### "No puedo iniciar sesión"
- Verifica que estés usando el email completo (ej: `admin@techsolutions.com`)
- Las contraseñas distinguen mayúsculas y minúsculas
- Verifica que la base de datos esté creada (`dotnet ef database update`)

### "No veo el menú después de iniciar sesión"
- Verifica que el usuario tenga un rol asignado
- Revisa la consola para errores de autorización

### "Acceso denegado a una página"
- Tu usuario no tiene el rol necesario para acceder a esa funcionalidad
- Contacta al administrador para ajustar permisos

---

**Última actualización:** 13 de febrero de 2026
