# 🚀 GUÍA RÁPIDA PARA FABRIZZIO - TechSolutions

## 👋 ¡Hola Fabrizzio!

Esta guía te ayudará a crear las vistas (Views) de forma **rápida, sencilla y funcional**. Encontrarás:
- ✅ Plantillas listas para copiar y pegar
- ✅ Explicación del flujo del sistema
- ✅ Ejemplos claros y funcionales
- ✅ Permisos para arreglar código si encuentras errores

---

## 🎯 REGLA DE ORO

**Si encuentras un error en los controladores, servicios o modelos: ¡ARRÉGLALO!**

No tengas miedo de modificar código si algo no funciona correctamente. Tu objetivo es que todo el sistema funcione de principio a fin.

---

## 🗺️ FLUJO COMPLETO DEL SISTEMA

### 1️⃣ Flujo de Autenticación (Login → Dashboard)

```
┌─────────────────────────────────────────────────────────────────┐
│                    INICIO DE LA APLICACIÓN                      │
└─────────────────────┬───────────────────────────────────────────┘
                      │
                      ↓
              ┌───────────────┐
              │   Home/Index  │  ← Página de bienvenida
              │  (pública)    │
              └───────┬───────┘
                      │
        ┌─────────────┴──────────────┐
        │                            │
        ↓                            ↓
┌──────────────┐            ┌──────────────┐
│ Usuario NO   │            │ Usuario SÍ   │
│ autenticado  │            │ autenticado  │
└──────┬───────┘            └──────┬───────┘
       │                           │
       ↓                           ↓
┌─────────────────┐      ┌──────────────────┐
│ Click en Login  │      │ Ya puede acceder │
└────────┬────────┘      │ a las funciones  │
         │               └─────────┬────────┘
         ↓                         │
┌──────────────────────┐          │
│ Autenticacion/Login  │          │
│ (Formulario)         │          │
└─────────┬────────────┘          │
          │                        │
          ↓                        │
     Ingresa email                 │
     y contraseña                  │
          │                        │
          ↓                        │
    ┌─────────────┐               │
    │ ¿Correcto?  │               │
    └──┬──────┬───┘               │
       │      │                    │
    NO │      │ SÍ                 │
       ↓      ↓                    ↓
   Muestra  ┌──────────────────────────────────┐
   error    │ Redirige según rol:              │
            │                                  │
            │ • Líder → /Proyectos/Index       │
            │ • Desarrollador → /Tareas/MisTareas │
            │ • Administrador → /Proyectos/Index  │
            └──────────────────────────────────┘
```

### 2️⃣ Flujo Principal por Rol

#### 🔵 LÍDER (Gestiona proyectos y tareas)
```
Login exitoso
    ↓
/Proyectos/Index (lista de proyectos)
    ↓
Puede:
├─ Ver proyectos → /Proyectos/Details/{id}
├─ Crear proyecto → /Proyectos/Create
├─ Editar proyecto → /Proyectos/Edit/{id}
├─ Eliminar proyecto → /Proyectos/Delete/{id}
├─ Generar reportes → /Reportes/Descargar
├─ Ver todas las tareas → /Tareas/Index
├─ Crear tareas → /Tareas/Create
├─ Editar tareas → /Tareas/Edit/{id}
├─ Eliminar tareas → /Tareas/Delete/{id}
├─ Cambiar estado de tareas → /Tareas/CambiarEstado
└─ Ver dashboard → /Seguimiento/Index
```

#### 🟢 DESARROLLADOR (Solo trabaja en sus tareas)
```
Login exitoso
    ↓
/Tareas/MisTareas (solo sus tareas asignadas)
    ↓
Puede:
├─ Ver sus tareas → /Tareas/MisTareas
├─ Cambiar estado de sus tareas → /Tareas/CambiarEstado
│   ├─ Pendiente → En Progreso
│   └─ En Progreso → Finalizado
├─ Ver proyectos (solo lectura) → /Proyectos/Index
└─ Ver dashboard → /Seguimiento/Index
```

### 3️⃣ Flujo de Cambio de Estado de Tarea (MUY IMPORTANTE)

```
Desarrollador entra a /Tareas/MisTareas
    ↓
Ve su tarea con estado "Pendiente"
    ↓
Click en botón "Iniciar Tarea"
    ↓
POST /Tareas/CambiarEstado
    │
    ├─ id: 5 (ID de la tarea)
    └─ nuevoEstado: "En Progreso"
    ↓
TareasController.CambiarEstado()
    ↓
TareaService.CambiarEstadoAsync()
    ↓
Base de datos actualizada
    ↓
Redirige de vuelta a /Tareas/MisTareas
    ↓
Ahora el botón dice "Marcar como Completa"
    ↓
Click en "Marcar como Completa"
    ↓
POST /Tareas/CambiarEstado
    │
    ├─ id: 5
    └─ nuevoEstado: "Finalizado"
    ↓
Tarea marcada como completada ✅
```

---

## 📋 PLANTILLAS LISTAS PARA USAR

### 🔹 Plantilla 1: Vista de Listado (Index.cshtml)

**Copiar y pegar esto para crear cualquier vista de listado:**

```html
@*
    VISTA: Index de [NOMBRE_ENTIDAD]
    QUÉ HACE: Muestra la lista de todos los [nombre entidad]
    CONTROLADOR: [Nombre]Controller.Index() [GET]
*?
@model IEnumerable<TechSolutions_program.Models.[NOMBRE_MODELO]>

<div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h1>[NOMBRE_ENTIDAD]</h1>
        
        @* Botón Nuevo (solo para roles permitidos) *@
        @if (User.IsInRole("Lider") || User.IsInRole("Administrador"))
        {
            <a asp-action="Create" class="btn btn-primary">
                <i class="bi bi-plus-circle"></i> Nuevo [Nombre]
            </a>
        }
    </div>

    <div asp-validation-summary="ModelOnly" class="alert alert-danger" role="alert"></div>

    <table class="table table-striped table-hover">
        <thead>
            <tr>
                <th>[Columna 1]</th>
                <th>[Columna 2]</th>
                <th>[Columna 3]</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model)
            {
                <tr>
                    <td>@item.[Propiedad1]</td>
                    <td>@item.[Propiedad2]</td>
                    <td>@item.[Propiedad3]</td>
                    <td>
                        <a asp-action="Details" asp-route-id="@item.Id" class="btn btn-sm btn-info">
                            <i class="bi bi-eye"></i> Ver
                        </a>
                        
                        @if (User.IsInRole("Lider") || User.IsInRole("Administrador"))
                        {
                            <a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-sm btn-warning">
                                <i class="bi bi-pencil"></i> Editar
                            </a>
                            <a asp-action="Delete" asp-route-id="@item.Id" class="btn btn-sm btn-danger">
                                <i class="bi bi-trash"></i> Eliminar
                            </a>
                        }
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

**Reemplaza:**
- `[NOMBRE_ENTIDAD]` → Proyectos, Tareas, Clientes, etc.
- `[NOMBRE_MODELO]` → Proyecto, Tarea, Cliente, etc.
- `[Propiedad1]`, `[Propiedad2]` → Nombre, Estado, Cliente, etc.

---

### 🔹 Plantilla 2: Vista de Creación (Create.cshtml)

```html
@*
    VISTA: Create de [NOMBRE_ENTIDAD]
    QUÉ HACE: Formulario para crear un nuevo [nombre]
    CONTROLADOR: [Nombre]Controller.Create() [POST]
*?
@model TechSolutions_program.Models.[NOMBRE_MODELO]

<div class="container mt-4">
    <h1>Crear [Nombre]</h1>
    <hr />

    <div class="row">
        <div class="col-md-6">
            <form asp-action="Create" method="post">
                @Html.AntiForgeryToken()
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>

                @* Campo 1 *@
                <div class="form-group mb-3">
                    <label asp-for="[Propiedad1]" class="form-label"></label>
                    <input asp-for="[Propiedad1]" class="form-control" />
                    <span asp-validation-for="[Propiedad1]" class="text-danger"></span>
                </div>

                @* Campo 2 *@
                <div class="form-group mb-3">
                    <label asp-for="[Propiedad2]" class="form-label"></label>
                    <input asp-for="[Propiedad2]" class="form-control" />
                    <span asp-validation-for="[Propiedad2]" class="text-danger"></span>
                </div>

                @* Agrega más campos según tu modelo *@

                <div class="form-group mt-4">
                    <button type="submit" class="btn btn-success">
                        <i class="bi bi-check-circle"></i> Guardar
                    </button>
                    <a asp-action="Index" class="btn btn-secondary">
                        <i class="bi bi-arrow-left"></i> Cancelar
                    </a>
                </div>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

### 🔹 Plantilla 3: Vista de Edición (Edit.cshtml)

```html
@*
    VISTA: Edit de [NOMBRE_ENTIDAD]
    QUÉ HACE: Formulario para editar un [nombre] existente
    CONTROLADOR: [Nombre]Controller.Edit(id) [POST]
*?
@model TechSolutions_program.Models.[NOMBRE_MODELO]

<div class="container mt-4">
    <h1>Editar [Nombre]</h1>
    <hr />

    <div class="row">
        <div class="col-md-6">
            <form asp-action="Edit" method="post">
                @Html.AntiForgeryToken()
                <input asp-for="Id" type="hidden" />  @* ¡MUY IMPORTANTE! *@
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>

                @* Campo 1 *@
                <div class="form-group mb-3">
                    <label asp-for="[Propiedad1]" class="form-label"></label>
                    <input asp-for="[Propiedad1]" class="form-control" />
                    <span asp-validation-for="[Propiedad1]" class="text-danger"></span>
                </div>

                @* Campo 2 *@
                <div class="form-group mb-3">
                    <label asp-for="[Propiedad2]" class="form-label"></label>
                    <input asp-for="[Propiedad2]" class="form-control" />
                    <span asp-validation-for="[Propiedad2]" class="text-danger"></span>
                </div>

                @* Agrega más campos según tu modelo *@

                <div class="form-group mt-4">
                    <button type="submit" class="btn btn-primary">
                        <i class="bi bi-save"></i> Guardar Cambios
                    </button>
                    <a asp-action="Index" class="btn btn-secondary">
                        <i class="bi bi-arrow-left"></i> Cancelar
                    </a>
                </div>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

**⚠️ IMPORTANTE:** Nunca olvides `<input asp-for="Id" type="hidden" />` en formularios de edición.

---

### 🔹 Plantilla 4: Vista de Eliminación (Delete.cshtml)

```html
@*
    VISTA: Delete de [NOMBRE_ENTIDAD]
    QUÉ HACE: Confirmación antes de eliminar
    CONTROLADOR: [Nombre]Controller.DeleteConfirmed(id) [POST]
*?
@model TechSolutions_program.Models.[NOMBRE_MODELO]

<div class="container mt-4">
    <h1>Eliminar [Nombre]</h1>
    <hr />

    <div class="alert alert-warning" role="alert">
        <i class="bi bi-exclamation-triangle"></i>
        <strong>¡Atención!</strong> ¿Está seguro que desea eliminar este registro?
    </div>

    <dl class="row">
        <dt class="col-sm-3">@Html.DisplayNameFor(model => model.[Propiedad1])</dt>
        <dd class="col-sm-9">@Html.DisplayFor(model => model.[Propiedad1])</dd>

        <dt class="col-sm-3">@Html.DisplayNameFor(model => model.[Propiedad2])</dt>
        <dd class="col-sm-9">@Html.DisplayFor(model => model.[Propiedad2])</dd>

        @* Agrega más propiedades según tu modelo *@
    </dl>

    <form asp-action="Delete" method="post">
        @Html.AntiForgeryToken()
        <button type="submit" class="btn btn-danger">
            <i class="bi bi-trash"></i> Confirmar Eliminación
        </button>
        <a asp-action="Index" class="btn btn-secondary">
            <i class="bi bi-arrow-left"></i> Cancelar
        </a>
    </form>
</div>
```

---

### 🔹 Plantilla 5: Vista de Detalles (Details.cshtml)

```html
@*
    VISTA: Details de [NOMBRE_ENTIDAD]
    QUÉ HACE: Muestra información detallada (solo lectura)
    CONTROLADOR: [Nombre]Controller.Details(id) [GET]
*?
@model TechSolutions_program.Models.[NOMBRE_MODELO]

<div class="container mt-4">
    <h1>Detalles de [Nombre]</h1>
    <hr />

    <dl class="row">
        <dt class="col-sm-3">@Html.DisplayNameFor(model => model.[Propiedad1])</dt>
        <dd class="col-sm-9">@Html.DisplayFor(model => model.[Propiedad1])</dd>

        <dt class="col-sm-3">@Html.DisplayNameFor(model => model.[Propiedad2])</dt>
        <dd class="col-sm-9">@Html.DisplayFor(model => model.[Propiedad2])</dd>

        @* Agrega más propiedades según tu modelo *@
    </dl>

    <div class="mt-4">
        @if (User.IsInRole("Lider") || User.IsInRole("Administrador"))
        {
            <a asp-action="Edit" asp-route-id="@Model.Id" class="btn btn-warning">
                <i class="bi bi-pencil"></i> Editar
            </a>
            <a asp-action="Delete" asp-route-id="@Model.Id" class="btn btn-danger">
                <i class="bi bi-trash"></i> Eliminar
            </a>
        }
        <a asp-action="Index" class="btn btn-secondary">
            <i class="bi bi-arrow-left"></i> Volver al Listado
        </a>
    </div>
</div>
```

---

## 🎯 CASOS ESPECIALES

### 🔥 Caso 1: Vista de Tareas con Botones de Estado (MisTareas.cshtml)

**ESTA ES LA VISTA MÁS IMPORTANTE PARA LOS DESARROLLADORES**

```html
@*
    VISTA: MisTareas
    QUÉ HACE: Muestra las tareas asignadas al desarrollador con botones para cambiar estado
    CONTROLADOR: TareasController.MisTareas() [GET] y TareasController.CambiarEstado() [POST]
*?
@model IEnumerable<TechSolutions_program.Models.Tarea>

<div class="container mt-4">
    <h1>Mis Tareas Asignadas</h1>
    <hr />

    <table class="table table-striped">
        <thead>
            <tr>
                <th>Descripción</th>
                <th>Proyecto</th>
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
                    <td>@tarea.Proyecto?.Nombre</td>
                    <td>
                        @if (tarea.Estado == "Pendiente")
                        {
                            <span class="badge bg-secondary">Pendiente</span>
                        }
                        else if (tarea.Estado == "En Progreso")
                        {
                            <span class="badge bg-warning text-dark">En Progreso</span>
                        }
                        else if (tarea.Estado == "Finalizado" || tarea.Estado == "Terminado")
                        {
                            <span class="badge bg-success">Finalizado</span>
                        }
                    </td>
                    <td>
                        @if (tarea.Prioridad == "Alta")
                        {
                            <span class="badge bg-danger">Alta</span>
                        }
                        else if (tarea.Prioridad == "Media")
                        {
                            <span class="badge bg-warning text-dark">Media</span>
                        }
                        else
                        {
                            <span class="badge bg-info">Baja</span>
                        }
                    </td>
                    <td>@tarea.FechaLimite?.ToString("dd/MM/yyyy")</td>
                    <td>
                        @* BOTONES DE CAMBIO DE ESTADO *@
                        <form asp-action="CambiarEstado" asp-route-id="@tarea.Id" method="post" style="display:inline;">
                            @Html.AntiForgeryToken()
                            
                            @if (tarea.Estado == "Pendiente")
                            {
                                <button type="submit" name="nuevoEstado" value="En Progreso" 
                                        class="btn btn-sm btn-warning" title="Iniciar esta tarea">
                                    <i class="bi bi-play-circle"></i> Iniciar
                                </button>
                            }
                            else if (tarea.Estado == "En Progreso")
                            {
                                <button type="submit" name="nuevoEstado" value="Finalizado" 
                                        class="btn btn-sm btn-success" title="Marcar como completada">
                                    <i class="bi bi-check-circle"></i> Completar
                                </button>
                            }
                            else
                            {
                                <span class="text-success">✓ Completada</span>
                            }
                        </form>
                    </td>
                </tr>
            }
        </tbody>
    </table>

    @if (!Model.Any())
    {
        <div class="alert alert-info" role="alert">
            <i class="bi bi-info-circle"></i> No tienes tareas asignadas en este momento.
        </div>
    }
</div>
```

---

### 🔥 Caso 2: Vista de Reportes (Index.cshtml en Reportes)

```html
@*
    VISTA: Index de Reportes
    QUÉ HACE: Permite seleccionar proyecto y generar reportes PDF/Excel
    CONTROLADOR: ReportesController.Descargar(tipoReporte, proyectoId) [GET/POST]
*?
@model IEnumerable<TechSolutions_program.Models.Proyecto>

<div class="container mt-4">
    <h1>Generador de Reportes</h1>
    <hr />

    <div class="alert alert-info" role="alert">
        <i class="bi bi-info-circle"></i>
        Selecciona un proyecto para generar su reporte en PDF o Excel.
    </div>

    <table class="table table-hover">
        <thead>
            <tr>
                <th>Proyecto</th>
                <th>Cliente</th>
                <th>Estado</th>
                <th>Presupuesto</th>
                <th>Reportes</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var proyecto in Model)
            {
                <tr>
                    <td>@proyecto.Nombre</td>
                    <td>@proyecto.Cliente</td>
                    <td>@proyecto.Estado</td>
                    <td>@proyecto.Presupuesto.ToString("C2")</td>
                    <td>
                        @* Botón para descargar PDF *@
                        <a asp-controller="Reportes" 
                           asp-action="Descargar" 
                           asp-route-tipoReporte="pdf" 
                           asp-route-proyectoId="@proyecto.Id" 
                           class="btn btn-sm btn-danger" 
                           title="Descargar reporte en PDF">
                            <i class="bi bi-file-pdf"></i> PDF
                        </a>

                        @* Botón para descargar Excel *@
                        <a asp-controller="Reportes" 
                           asp-action="Descargar" 
                           asp-route-tipoReporte="excel" 
                           asp-route-proyectoId="@proyecto.Id" 
                           class="btn btn-sm btn-success" 
                           title="Descargar reporte en Excel">
                            <i class="bi bi-file-excel"></i> Excel
                        </a>
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

---

### 🔥 Caso 3: Dashboard (Index.cshtml en Seguimiento)

```html
@*
    VISTA: Dashboard de Seguimiento
    QUÉ HACE: Muestra métricas e indicadores del sistema
    CONTROLADOR: SeguimientoController.Index() [GET]
*?
@model TechSolutions_program.Models.DashboardViewModel

<div class="container mt-4">
    <h1>Dashboard de Control</h1>
    <hr />

    <div class="row">
        @* Tarjeta 1: Total de Proyectos *@
        <div class="col-md-3 mb-4">
            <div class="card text-white bg-primary">
                <div class="card-body">
                    <h5 class="card-title">Proyectos Activos</h5>
                    <h2 class="card-text">@Model.TotalProyectos</h2>
                    <p class="card-text">
                        <small>Total de proyectos en el sistema</small>
                    </p>
                </div>
            </div>
        </div>

        @* Tarjeta 2: Presupuesto Total *@
        <div class="col-md-3 mb-4">
            <div class="card text-white bg-success">
                <div class="card-body">
                    <h5 class="card-title">Presupuesto Total</h5>
                    <h2 class="card-text">S/ @Model.PresupuestoTotal.ToString("N2")</h2>
                    <p class="card-text">
                        <small>Suma de todos los proyectos</small>
                    </p>
                </div>
            </div>
        </div>

        @* Tarjeta 3: Tareas Pendientes *@
        <div class="col-md-3 mb-4">
            <div class="card text-white bg-warning">
                <div class="card-body">
                    <h5 class="card-title">Tareas Pendientes</h5>
                    <h2 class="card-text">@Model.TareasPendientes</h2>
                    <p class="card-text">
                        <small>Tareas sin completar</small>
                    </p>
                </div>
            </div>
        </div>

        @* Tarjeta 4: Tareas Completadas *@
        <div class="col-md-3 mb-4">
            <div class="card text-white bg-info">
                <div class="card-body">
                    <h5 class="card-title">Tareas Completadas</h5>
                    <h2 class="card-text">@Model.TareasCompletadas</h2>
                    <p class="card-text">
                        <small>Tareas finalizadas</small>
                    </p>
                </div>
            </div>
        </div>
    </div>

    <div class="row mt-4">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <h4>Accesos Rápidos</h4>
                </div>
                <div class="card-body">
                    <a asp-controller="Proyectos" asp-action="Index" class="btn btn-primary">
                        <i class="bi bi-folder"></i> Ver Proyectos
                    </a>
                    
                    @if (User.IsInRole("Lider"))
                    {
                        <a asp-controller="Tareas" asp-action="Index" class="btn btn-info">
                            <i class="bi bi-list-task"></i> Gestionar Tareas
                        </a>
                    }
                    
                    @if (User.IsInRole("Desarrollador"))
                    {
                        <a asp-controller="Tareas" asp-action="MisTareas" class="btn btn-info">
                            <i class="bi bi-list-task"></i> Mis Tareas
                        </a>
                    }
                    
                    <a asp-controller="Reportes" asp-action="Index" class="btn btn-success">
                        <i class="bi bi-file-earmark-pdf"></i> Reportes
                    </a>
                </div>
            </div>
        </div>
    </div>
</div>
```

---

## 🔧 CONFIGURACIÓN DE REDIRECCIONES DESPUÉS DEL LOGIN

### Actualizar el Método Login en AutenticacionController

Si ves que el login no redirige correctamente, modifica el método así:

```csharp
[AllowAnonymous]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
{
    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
    {
        ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido");
        return View();
    }

    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
        ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido");
        return View();
    }

    var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
    if (result.Succeeded)
    {
        // Si hay una URL de retorno válida, redirigir ahí
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        // Redirigir según el rol del usuario
        var roles = await _userManager.GetRolesAsync(user);
        
        if (roles.Contains("Desarrollador"))
        {
            return RedirectToAction("MisTareas", "Tareas");
        }
        else if (roles.Contains("Lider") || roles.Contains("Administrador"))
        {
            return RedirectToAction("Index", "Proyectos");
        }

        // Si no tiene rol específico, ir al dashboard
        return RedirectToAction("Index", "Seguimiento");
    }

    ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido");
    return View();
}
```

---

## 🛠️ ERRORES COMUNES Y SOLUCIONES

### Error 1: "No existe una estrategia para el tipo solicitado" (Reportes)

**Problema:** Al generar reportes, da error.

**Solución:** Verifica que en `Program.cs` estén registradas las estrategias:

```csharp
// Registrar estrategias de reportes
builder.Services.AddTransient<IReporteStrategy, PdfReporteStrategy>();
builder.Services.AddTransient<IReporteStrategy, ExcelReporteStrategy>();
builder.Services.AddTransient<IReporteService, ReporteService>();
```

### Error 2: El método CambiarEstado no funciona

**Problema:** Los botones de cambiar estado no hacen nada.

**Solución:** Asegúrate de que el método existe en TareasController y redirige correctamente:

```csharp
[Authorize(Roles = "Desarrollador,Lider")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
{
    if (string.IsNullOrWhiteSpace(nuevoEstado))
    {
        ModelState.AddModelError(string.Empty, "Estado no válido.");
        return RedirectToAction(nameof(MisTareas));
    }

    try
    {
        await _tareaService.CambiarEstadoAsync(id, nuevoEstado);
        
        // Redirigir según el rol
        if (User.IsInRole("Desarrollador"))
        {
            return RedirectToAction(nameof(MisTareas));
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }
    catch (Exception ex)
    {
        ModelState.AddModelError(string.Empty, ex.Message);
        return RedirectToAction(nameof(MisTareas));
    }
}
```

### Error 3: No puedo ver los datos de navegación (proyecto.Tareas)

**Problema:** `@tarea.Proyecto.Nombre` da error o muestra null.

**Solución:** Incluye la navegación en el servicio:

```csharp
// En TareaService.cs
public async Task<IEnumerable<Tarea>> GetTareasAsync()
{
    return await _dbContext.Tareas
        .Include(t => t.Proyecto)  // Incluir navegación
        .AsNoTracking()
        .ToListAsync();
}
```

---

## 📝 CHECKLIST RÁPIDO PARA CADA VISTA

Antes de dar por terminada una vista, verifica:

### Para vistas con formularios (Create, Edit, Delete):
- [ ] ¿Agregué `@Html.AntiForgeryToken()`?
- [ ] ¿Incluí `<input asp-for="Id" type="hidden" />` en Edit?
- [ ] ¿Agregué validaciones (`asp-validation-for`)?
- [ ] ¿El `asp-action` coincide con el nombre del método del controlador?
- [ ] ¿Probé hacer submit y funciona?

### Para vistas de listado (Index):
- [ ] ¿Los botones Create/Edit/Delete se muestran solo para los roles correctos?
- [ ] ¿Los enlaces tienen `asp-route-id="@item.Id"`?
- [ ] ¿Probé hacer clic en cada botón y funciona?

### Para vistas especiales (MisTareas):
- [ ] ¿Los botones de CambiarEstado están en un formulario POST?
- [ ] ¿Incluí `@Html.AntiForgeryToken()`?
- [ ] ¿El parámetro `name="nuevoEstado"` coincide con el método del controlador?
- [ ] ¿Probé cambiar el estado y se actualiza correctamente?

---

## 🎨 TIPS DE DISEÑO

### Usar Bootstrap Icons

Agrega en `_Layout.cshtml` (en el `<head>`):

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css">
```

Luego puedes usar iconos:
- `<i class="bi bi-plus-circle"></i>` → ➕
- `<i class="bi bi-pencil"></i>` → ✏️
- `<i class="bi bi-trash"></i>` → 🗑️
- `<i class="bi bi-eye"></i>` → 👁️
- `<i class="bi bi-play-circle"></i>` → ▶️
- `<i class="bi bi-check-circle"></i>` → ✔️

### Colores de Botones Bootstrap

- `btn-primary` → Azul (acciones principales)
- `btn-success` → Verde (guardar, completar)
- `btn-danger` → Rojo (eliminar)
- `btn-warning` → Amarillo (editar, advertencias)
- `btn-info` → Celeste (ver detalles)
- `btn-secondary` → Gris (cancelar, volver)

---

## 🚀 ORDEN RECOMENDADO PARA CREAR LAS VISTAS

1. **Login** (Autenticacion/Login.cshtml) ← Ya está hecha
2. **Home** (Home/Index.cshtml) ← Página de bienvenida
3. **Clientes:**
   - Index.cshtml
   - Create.cshtml
   - Edit.cshtml
   - Details.cshtml
   - Delete.cshtml
4. **Proyectos:** (mismo orden que Clientes)
5. **Tareas:**
   - Index.cshtml (para Líderes)
   - MisTareas.cshtml (para Desarrolladores) ← CRÍTICA
   - Create.cshtml
   - Edit.cshtml
6. **Dashboard** (Seguimiento/Index.cshtml)
7. **Reportes** (Reportes/Index.cshtml)

---

## 💡 CONSEJO FINAL

**No te preocupes por hacer todo perfecto desde el inicio.**

1. Empieza con una vista simple que funcione
2. Prueba que funcione
3. Si funciona, pasa a la siguiente
4. Si encuentras un error, arréglalo
5. No tengas miedo de modificar controladores o servicios

**¡Tú puedes hacerlo, Fabrizzio! 💪**

---

## 📞 NECESITAS AYUDA?

Si algo no funciona:
1. Lee el mensaje de error completo
2. Busca en qué línea está el error
3. Revisa si olvidaste algo del checklist
4. Verifica que el método del controlador existe
5. Si es necesario, modifica el controlador o servicio

**¡Mucha suerte! 🍀**
