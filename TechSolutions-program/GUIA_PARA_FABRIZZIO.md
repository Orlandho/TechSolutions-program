# 📚 GUÍA COMPLETA PARA FABRIZZIO - TechSolutions

## 🎯 Propósito de este Documento
Esta guía te ayudará a entender **qué métodos de los controladores y servicios debes usar en cada vista (View)** y **por qué**. 

---

## 🏗️ Arquitectura del Sistema

```
┌─────────────────┐
│     VISTAS      │ ← Lo que ve el usuario (archivos .cshtml)
│   (Views)       │
└────────┬────────┘
         │ asp-action="NombreMetodo"
         │ asp-controller="NombreControlador"
         ↓
┌─────────────────┐
│  CONTROLADORES  │ ← Reciben las peticiones y coordinan
│ (Controllers)   │
└────────┬────────┘
         │ Llaman a métodos de servicios
         ↓
┌─────────────────┐
│   SERVICIOS     │ ← Contienen la lógica de negocio
│  (Services)     │
└────────┬────────┘
         │ Usan repositorios o DbContext
         ↓
┌─────────────────┐
│  BASE DE DATOS  │ ← Almacena la información
│    (SQL Server) │
└─────────────────┘
```

---

## 📋 CÓMO CONECTAR VISTAS CON MÉTODOS DE CONTROLADORES

### 🔗 Sintaxis Básica para Llamar Métodos desde las Vistas

#### 1. **Enlaces (Links)**
```html
<!-- Llama al método Index del controlador Clientes -->
<a asp-controller="Clientes" asp-action="Index">Ver Clientes</a>

<!-- Llama al método Details pasando un ID -->
<a asp-action="Details" asp-route-id="@cliente.Id">Ver Detalles</a>

<!-- Llama al método Descargar con múltiples parámetros -->
<a asp-action="Descargar" 
   asp-route-tipoReporte="pdf" 
   asp-route-proyectoId="@proyecto.Id">
   Descargar PDF
</a>
```

#### 2. **Formularios (Forms)**
```html
<!-- Llama al método Create [POST] cuando se hace submit -->
<form asp-action="Create" method="post">
    @Html.AntiForgeryToken()  <!-- SIEMPRE incluir esto -->
    <input asp-for="Nombre" />
    <button type="submit">Guardar</button>
</form>

<!-- Llama al método CambiarEstado con parámetros -->
<form asp-action="CambiarEstado" asp-route-id="@tarea.Id" method="post">
    @Html.AntiForgeryToken()
    <button type="submit" name="nuevoEstado" value="Finalizado">
        Marcar como Completa
    </button>
</form>
```

---

## 🗂️ RELACIÓN ENTRE VISTAS Y MÉTODOS DE CONTROLADORES

### 📁 **CLIENTES** (ClientesController)

| Vista | Método del Controlador | Qué Hace | Cómo Usarlo en la Vista |
|-------|------------------------|----------|-------------------------|
| `Index.cshtml` | `ClientesController.Index() [GET]` | Muestra la lista de clientes | Se carga automáticamente |
| | `ClientesController.Create() [GET]` | Botón "Nuevo Cliente" | `<a asp-action="Create">Nuevo</a>` |
| | `ClientesController.Details(id) [GET]` | Botón "Ver Detalles" | `<a asp-action="Details" asp-route-id="@cliente.Id">Detalles</a>` |
| | `ClientesController.Edit(id) [GET]` | Botón "Editar" | `<a asp-action="Edit" asp-route-id="@cliente.Id">Editar</a>` |
| | `ClientesController.Delete(id) [GET]` | Botón "Eliminar" | `<a asp-action="Delete" asp-route-id="@cliente.Id">Eliminar</a>` |
| `Create.cshtml` | `ClientesController.Create() [GET]` | Muestra el formulario vacío | Se carga automáticamente |
| | `ClientesController.Create(cliente) [POST]` | Procesa el formulario | `<form asp-action="Create" method="post">` |
| `Edit.cshtml` | `ClientesController.Edit(id) [GET]` | Muestra el formulario con datos | Se carga automáticamente |
| | `ClientesController.Edit(id, cliente) [POST]` | Guarda los cambios | `<form asp-action="Edit" method="post">` |
| `Delete.cshtml` | `ClientesController.Delete(id) [GET]` | Muestra confirmación | Se carga automáticamente |
| | `ClientesController.DeleteConfirmed(id) [POST]` | Elimina el cliente | `<form asp-action="Delete" method="post">` |
| `Details.cshtml` | `ClientesController.Details(id) [GET]` | Muestra información | Se carga automáticamente |

---

### 📁 **PROYECTOS** (ProyectosController)

| Vista | Método del Controlador | Qué Hace | Seguridad | Cómo Usarlo |
|-------|------------------------|----------|-----------|-------------|
| `Index.cshtml` | `ProyectosController.Index() [GET]` | Lista proyectos | Todos | Carga automática |
| | `ProyectosController.Create() [GET]` | Botón "Nuevo Proyecto" | Líder/Admin | `<a asp-action="Create">Nuevo</a>` |
| | `ProyectosController.Edit(id) [GET]` | Botón "Editar" | Líder/Admin | `<a asp-action="Edit" asp-route-id="@id">Editar</a>` |
| | `ProyectosController.Delete(id) [GET]` | Botón "Eliminar" | Líder/Admin | `<a asp-action="Delete" asp-route-id="@id">Eliminar</a>` |
| | `ReportesController.Descargar(tipo, id)` | Descargar reportes | Todos | `<a asp-controller="Reportes" asp-action="Descargar" asp-route-tipoReporte="pdf" asp-route-proyectoId="@id">PDF</a>` |
| `Create.cshtml` | `ProyectosController.Create() [POST]` | Crea proyecto | Líder/Admin | `<form asp-action="Create" method="post">` |
| `Edit.cshtml` | `ProyectosController.Edit(id) [POST]` | Actualiza proyecto | Líder/Admin | `<form asp-action="Edit" method="post">` |
| `Delete.cshtml` | `ProyectosController.DeleteConfirmed(id) [POST]` | Elimina proyecto | Líder/Admin | `<form asp-action="Delete" method="post">` |

**IMPORTANTE:** Los botones Create, Edit y Delete deben mostrarse SOLO si el usuario tiene rol Líder o Administrador:
```html
@if (User.IsInRole("Lider") || User.IsInRole("Administrador"))
{
    <a asp-action="Create">Nuevo Proyecto</a>
}
```

---

### 📁 **TAREAS** (TareasController)

| Vista | Método del Controlador | Qué Hace | Seguridad | Cómo Usarlo |
|-------|------------------------|----------|-----------|-------------|
| `Index.cshtml` | `TareasController.Index() [GET]` | Lista TODAS las tareas | Solo Líder | Carga automática |
| | `TareasController.CambiarEstado(id, estado) [POST]` | **MÉTODO CLAVE** para botones | Líder/Dev | Ver ejemplo abajo |
| `MisTareas.cshtml` | `TareasController.MisTareas() [GET]` | Lista tareas del usuario | Solo Desarrollador | Carga automática |
| | `TareasController.CambiarEstado(id, estado) [POST]` | **MÉTODO CLAVE** para botones | Líder/Dev | Ver ejemplo abajo |
| `Create.cshtml` | `TareasController.Create() [POST]` | Crea tarea | Solo Líder | `<form asp-action="Create">` |
| `Edit.cshtml` | `TareasController.Edit(id) [POST]` | Actualiza tarea | Solo Líder | `<form asp-action="Edit">` |

#### 🔥 **MÉTODO MÁS IMPORTANTE: CambiarEstado**

Este método es CRÍTICO para que los desarrolladores puedan actualizar el avance de sus tareas:

```html
<!-- En Index.cshtml o MisTareas.cshtml -->
<form asp-action="CambiarEstado" asp-route-id="@tarea.Id" method="post">
    @Html.AntiForgeryToken()
    
    <!-- Si la tarea está Pendiente, mostrar botón para iniciarla -->
    @if (tarea.Estado == "Pendiente")
    {
        <button type="submit" name="nuevoEstado" value="En Progreso" class="btn btn-warning">
            Iniciar Tarea
        </button>
    }
    
    <!-- Si está En Progreso, mostrar botón para completarla -->
    @if (tarea.Estado == "En Progreso")
    {
        <button type="submit" name="nuevoEstado" value="Finalizado" class="btn btn-success">
            Marcar como Completa
        </button>
    }
</form>
```

**¿Por qué es importante?**
- Es la única forma en que los Desarrolladores pueden reportar su avance
- El Líder puede ver en tiempo real cómo van las tareas
- Alimenta el Dashboard con datos actualizados

---

### 📁 **AUTENTICACIÓN** (AutenticacionController)

| Vista | Método del Controlador | Qué Hace | Cómo Usarlo |
|-------|------------------------|----------|-------------|
| `Login.cshtml` | `AutenticacionController.Login() [GET]` | Muestra formulario de login | Carga automática |
| | `AutenticacionController.Login(email, password) [POST]` | Valida y autentica usuario | `<form asp-action="Login" method="post">` |
| `_LoginPartial.cshtml` | `AutenticacionController.Logout() [POST]` | Cierra sesión | `<form asp-action="Logout" method="post">` |

**Formulario de Login Completo:**
```html
<form asp-action="Login" method="post">
    @Html.AntiForgeryToken()
    <input type="hidden" name="returnUrl" value="@ViewData["ReturnUrl"]" />
    
    <div>
        <label>Email</label>
        <input name="email" type="email" required />
    </div>
    
    <div>
        <label>Contraseña</label>
        <input name="password" type="password" required />
    </div>
    
    <button type="submit">Iniciar Sesión</button>
</form>
```

---

### 📁 **REPORTES** (ReportesController)

| Vista | Método del Controlador | Qué Hace | Cómo Usarlo |
|-------|------------------------|----------|-------------|
| `Index.cshtml` | `ReportesController.Index() [GET]` | Muestra opciones | Carga automática |
| | `ReportesController.Descargar(tipo, proyectoId)` | Genera PDF o Excel | Ver ejemplo abajo |

**Generación de Reportes:**
```html
<!-- Descargar PDF -->
<a asp-controller="Reportes" 
   asp-action="Descargar" 
   asp-route-tipoReporte="pdf" 
   asp-route-proyectoId="@proyecto.Id" 
   class="btn btn-danger">
   📄 Descargar PDF
</a>

<!-- Descargar Excel -->
<a asp-controller="Reportes" 
   asp-action="Descargar" 
   asp-route-tipoReporte="excel" 
   asp-route-proyectoId="@proyecto.Id" 
   class="btn btn-success">
   📊 Descargar Excel
</a>
```

---

### 📁 **DASHBOARD** (SeguimientoController)

| Vista | Método del Controlador | Qué Hace | Datos que Muestra |
|-------|------------------------|----------|-------------------|
| `Index.cshtml` | `SeguimientoController.Index() [GET]` | Muestra métricas | `@Model.TotalProyectos`, `@Model.PresupuestoTotal`, `@Model.TareasPendientes`, `@Model.TareasCompletadas` |

**Mostrar los Datos:**
```html
<div class="dashboard">
    <div class="card">
        <h3>@Model.TotalProyectos</h3>
        <p>Proyectos Activos</p>
    </div>
    
    <div class="card">
        <h3>S/ @Model.PresupuestoTotal.ToString("N2")</h3>
        <p>Presupuesto Total</p>
    </div>
    
    <div class="card">
        <h3>@Model.TareasPendientes</h3>
        <p>Tareas Pendientes</p>
    </div>
    
    <div class="card">
        <h3>@Model.TareasCompletadas</h3>
        <p>Tareas Completadas</p>
    </div>
</div>
```

---

## 🔐 SEGURIDAD Y ROLES

### Roles del Sistema
- **Desarrollador**: Solo puede ver sus tareas y cambiar su estado
- **Líder**: Puede crear, editar y eliminar proyectos y tareas
- **Administrador**: Acceso completo al sistema

### Verificar Roles en las Vistas
```html
<!-- Mostrar solo si el usuario está autenticado -->
@if (User.Identity.IsAuthenticated)
{
    <p>Bienvenido, @User.Identity.Name</p>
}

<!-- Mostrar solo para Líderes -->
@if (User.IsInRole("Lider"))
{
    <a asp-action="Create">Nuevo Proyecto</a>
}

<!-- Mostrar solo para Desarrolladores -->
@if (User.IsInRole("Desarrollador"))
{
    <a asp-action="MisTareas">Mis Tareas</a>
}

<!-- Mostrar para múltiples roles -->
@if (User.IsInRole("Lider") || User.IsInRole("Administrador"))
{
    <a asp-action="Edit">Editar</a>
}
```

---

## ⚡ REGLAS IMPORTANTES

### 1. **SIEMPRE incluir el token anti-falsificación en formularios POST**
```html
<form asp-action="Create" method="post">
    @Html.AntiForgeryToken()  <!-- ¡NUNCA OLVIDAR ESTO! -->
    <!-- resto del formulario -->
</form>
```

### 2. **En formularios de edición, incluir el ID como campo oculto**
```html
<form asp-action="Edit" method="post">
    <input asp-for="Id" type="hidden" />  <!-- CRÍTICO -->
    <input asp-for="Nombre" />
    <button type="submit">Guardar</button>
</form>
```

### 3. **Mostrar errores de validación**
```html
<!-- Resumen de errores -->
<div asp-validation-summary="ModelOnly" class="text-danger"></div>

<!-- Error de un campo específico -->
<input asp-for="Nombre" />
<span asp-validation-for="Nombre" class="text-danger"></span>
```

---

## 🎨 EJEMPLOS COMPLETOS

### Ejemplo 1: Listado de Proyectos con Acciones
```html
@model IEnumerable<Proyecto>

<h1>Proyectos</h1>

@if (User.IsInRole("Lider") || User.IsInRole("Administrador"))
{
    <a asp-action="Create" class="btn btn-primary">Nuevo Proyecto</a>
}

<table class="table">
    <thead>
        <tr>
            <th>Nombre</th>
            <th>Cliente</th>
            <th>Presupuesto</th>
            <th>Acciones</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var proyecto en Model)
        {
            <tr>
                <td>@proyecto.Nombre</td>
                <td>@proyecto.Cliente</td>
                <td>@proyecto.Presupuesto.ToString("C2")</td>
                <td>
                    <a asp-action="Details" asp-route-id="@proyecto.Id">Ver</a>
                    
                    @if (User.IsInRole("Lider") || User.IsInRole("Administrador"))
                    {
                        <a asp-action="Edit" asp-route-id="@proyecto.Id">Editar</a>
                        <a asp-action="Delete" asp-route-id="@proyecto.Id">Eliminar</a>
                    }
                    
                    <a asp-controller="Reportes" 
                       asp-action="Descargar" 
                       asp-route-tipoReporte="pdf" 
                       asp-route-proyectoId="@proyecto.Id">PDF</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

### Ejemplo 2: Formulario de Creación de Proyecto
```html
@model Proyecto

<h1>Crear Proyecto</h1>

<div asp-validation-summary="ModelOnly" class="text-danger"></div>

<form asp-action="Create" method="post">
    @Html.AntiForgeryToken()
    
    <div class="form-group">
        <label asp-for="Nombre"></label>
        <input asp-for="Nombre" class="form-control" />
        <span asp-validation-for="Nombre" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <label asp-for="Cliente"></label>
        <input asp-for="Cliente" class="form-control" />
        <span asp-validation-for="Cliente" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <label asp-for="Presupuesto"></label>
        <input asp-for="Presupuesto" type="number" step="0.01" class="form-control" />
        <span asp-validation-for="Presupuesto" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <label asp-for="FechaInicio"></label>
        <input asp-for="FechaInicio" type="date" class="form-control" />
        <span asp-validation-for="FechaInicio" class="text-danger"></span>
    </div>
    
    <button type="submit" class="btn btn-success">Crear Proyecto</button>
    <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
</form>
```

### Ejemplo 3: Mis Tareas con Botones de Estado
```html
@model IEnumerable<Tarea>

<h1>Mis Tareas</h1>

<table class="table">
    <thead>
        <tr>
            <th>Descripción</th>
            <th>Estado</th>
            <th>Prioridad</th>
            <th>Fecha Límite</th>
            <th>Acciones</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var tarea in Model)
        {
            <tr>
                <td>@tarea.Descripcion</td>
                <td>
                    @if (tarea.Estado == "Pendiente")
                    {
                        <span class="badge bg-secondary">Pendiente</span>
                    }
                    else if (tarea.Estado == "En Progreso")
                    {
                        <span class="badge bg-warning">En Progreso</span>
                    }
                    else
                    {
                        <span class="badge bg-success">Finalizado</span>
                    }
                </td>
                <td>@tarea.Prioridad</td>
                <td>@tarea.FechaLimite?.ToString("dd/MM/yyyy")</td>
                <td>
                    <form asp-action="CambiarEstado" asp-route-id="@tarea.Id" method="post" style="display:inline;">
                        @Html.AntiForgeryToken()
                        
                        @if (tarea.Estado == "Pendiente")
                        {
                            <button type="submit" name="nuevoEstado" value="En Progreso" 
                                    class="btn btn-sm btn-warning">
                                Iniciar
                            </button>
                        }
                        else if (tarea.Estado == "En Progreso")
                        {
                            <button type="submit" name="nuevoEstado" value="Finalizado" 
                                    class="btn btn-sm btn-success">
                                Completar
                            </button>
                        }
                    </form>
                </td>
            </tr>
        }
    </tbody>
</table>
```

---

## 📊 FLUJO DE DATOS COMPLETO

```
USUARIO                         VISTA                   CONTROLADOR                 SERVICIO                BASE DE DATOS
   │                              │                          │                          │                          │
   │ Hace clic en "Mis Tareas"   │                          │                          │                          │
   │─────────────────────────────>│                          │                          │                          │
   │                              │ asp-action="MisTareas"   │                          │                          │
   │                              │─────────────────────────>│                          │                          │
   │                              │                          │ GetTareasPorResponsable()│                          │
   │                              │                          │─────────────────────────>│                          │
   │                              │                          │                          │ SELECT * FROM Tareas... │
   │                              │                          │                          │────────────────────────>│
   │                              │                          │                          │<─ Tareas del usuario ───│
   │                              │                          │<─ Lista de Tareas ───────│                          │
   │                              │<─ return View(tareas) ───│                          │                          │
   │<─ Renderiza MisTareas.cshtml │                          │                          │                          │
   │                              │                          │                          │                          │
   │ Hace clic en "Completar"    │                          │                          │                          │
   │─────────────────────────────>│                          │                          │                          │
   │                              │ POST CambiarEstado       │                          │                          │
   │                              │─────────────────────────>│                          │                          │
   │                              │                          │ CambiarEstadoAsync()     │                          │
   │                              │                          │─────────────────────────>│                          │
   │                              │                          │                          │ UPDATE Tareas SET...    │
   │                              │                          │                          │────────────────────────>│
   │                              │                          │                          │<─ OK ────────────────────│
   │                              │                          │<─ Task completado ────────│                          │
   │                              │<─ RedirectToAction() ────│                          │                          │
   │<─ Recarga MisTareas.cshtml ──│                          │                          │                          │
```

---

## 🆘 PREGUNTAS FRECUENTES

### P: ¿Cuándo usar `asp-action` vs `asp-controller`?
**R:** 
- Usa solo `asp-action` cuando estás en la misma controladora (ej: dentro de ClientesController)
- Usa ambos `asp-controller` y `asp-action` cuando necesitas ir a otra controladora

### P: ¿Por qué algunos métodos son [GET] y otros [POST]?
**R:**
- **[GET]**: Para LEER información (mostrar vistas, obtener datos). Se usa en enlaces `<a>`
- **[POST]**: Para MODIFICAR información (crear, editar, eliminar). Se usa en formularios `<form>`

### P: ¿Qué hace `@Html.AntiForgeryToken()`?
**R:** Protege contra ataques CSRF. Es un token de seguridad que valida que el formulario fue enviado desde tu sitio y no desde un sitio malicioso. **SIEMPRE inclúyelo en formularios POST**.

### P: ¿Cómo sé qué parámetros recibe un método del controlador?
**R:** Mira la firma del método en el controlador:
```csharp
public async Task<IActionResult> Edit(int id, Proyecto proyecto)
```
Necesitas pasar `id` (con `asp-route-id`) y los datos del `proyecto` (desde el formulario con `asp-for`).

### P: ¿Qué es el patrón Strategy en ReportesController?
**R:** Es un patrón de diseño que permite seleccionar el algoritmo (PDF o Excel) en tiempo de ejecución. El controlador no sabe CÓMO generar el reporte, solo le dice al servicio QUÉ tipo quiere, y el servicio elige la estrategia correcta.

---

## ✅ CHECKLIST PARA FABRIZZIO

Cuando trabajes en una vista, verifica:

- [ ] ¿Agregué `@Html.AntiForgeryToken()` en todos los formularios POST?
- [ ] ¿Incluí `<input asp-for="Id" type="hidden" />` en formularios de edición?
- [ ] ¿Los botones de Create/Edit/Delete se muestran solo para los roles correctos?
- [ ] ¿Los enlaces tienen los parámetros correctos (`asp-route-id`, etc.)?
- [ ] ¿Agregué validaciones (`asp-validation-for`, `asp-validation-summary`)?
- [ ] ¿El método que llamo existe en el controlador?
- [ ] ¿El `asp-action` coincide con el nombre del método?

---

## 📞 CONTACTO

Si tienes dudas:
1. Revisa los comentarios en el código de cada vista (.cshtml)
2. Revisa los comentarios en el código de cada controlador
3. Consulta este documento
4. Pregunta a tu equipo

---

**¡Éxito con tu desarrollo, Fabrizzio! 🚀**
