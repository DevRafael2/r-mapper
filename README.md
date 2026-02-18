# RMapper

RMapper es una librería ligera y de alto rendimiento para mapeo de
objetos en .NET, inspirada en AutoMapper.

Está construida sobre **Expression Trees**, lo que permite compilar
dinámicamente los mapeos y obtener un rendimiento extremadamente alto en
tiempo de ejecución.

------------------------------------------------------------------------

## 🚀 Características

-   ⚡ Alto rendimiento gracias a Expression Trees compiladas
-   🔧 Configuración basada en perfiles
-   🧩 Soporte para objetos complejos
-   📚 Soporte automático para colecciones (`IEnumerable<>`)
-   🎯 Mapeo personalizado con `ForMember`
-   🚫 Opción para ignorar conversiones fallidas en propiedades básicas
-   💉 Integración con Dependency Injection

------------------------------------------------------------------------

## 📦 Instalación

Registra el mapper dentro de tu contenedor de dependencias:

``` csharp
.AddMapper(typeof(TypeProjectProfiles).Assembly)
```

------------------------------------------------------------------------

## ⚙️ Configuración Opcional

Por defecto, si una propiedad básica no puede convertirse (ejemplo:
`long` → `int`), se lanzará una excepción.

Si deseas ignorar esos errores:

``` csharp
.AddMapper(typeof(ErrorProfile).Assembly, 
    e => e.IgnoreBasicPropertiesFailedCast())
```

Con esta configuración: - No se lanzará excepción. - La propiedad
quedará con su valor por defecto (`default` o `null`).

------------------------------------------------------------------------

## 🧩 Creación de Perfiles

Debes crear una clase que herede de:

``` csharp
RMapper.Configurations.Profile
```

Ejemplo:

``` csharp
public class TestProfile : Profile
{
    public TestProfile()
    {
        CreateMap<SourceEntity, DestinationEntity>();
    }
}
```

------------------------------------------------------------------------

## 🔁 Mapeo Básico

``` csharp
CreateMap<SourceEntity, DestinationEntity>();
```

Mapea automáticamente propiedades con el mismo nombre y tipo compatible.

------------------------------------------------------------------------

## 🎯 Mapeo Personalizado

``` csharp
CreateMap<SourceEntity, DestinationEntity>()
    .ForMember(d => d.CustomName,
               s => $"{s.Name} ({s.Age})");
```

Permite personalizar propiedades, listas y objetos complejos.

------------------------------------------------------------------------

## 🧱 Objetos Complejos

### Registrar ambos mapeos

``` csharp
CreateMap<SourceComplexObj, DestinationComplexObj>();
CreateMap<SourceEntity, DestinationEntity>();
```

### O mapear manualmente

``` csharp
CreateMap<SourceEntity, DestinationEntity>()
    .ForMember(d => d.ComplexObj,
               s => new DestinationComplexObj 
               { 
                   Name = s.ComplexObj.Name 
               });
```

------------------------------------------------------------------------

## 🚫 Ignorar Propiedades

``` csharp
CreateMap<SourceEntity, DestinationEntity>()
    .Ignore(d => d.ComplexObj);
```

------------------------------------------------------------------------

## 💉 Uso mediante Inyección de Dependencias

Inyecta:

``` csharp
RMapper.Interfaces.IMapper
```

### Métodos disponibles

``` csharp
Map<TSource, TDestination>(TSource source)

Adapt<TSource, TDestination>(TSource source, TDestination destination)

MapCollection<TSource, TDestination>(IEnumerable<TSource> source)
```

------------------------------------------------------------------------

# 🚀 Ejemplos Prácticos

## 1️⃣ Map

``` csharp
var dto = _mapper.Map<UserEntity, UserDto>(entity);
```

## 2️⃣ Adapt

``` csharp
_mapper.Adapt(sourceDto, existingEntity);
```

## 3️⃣ MapCollection

``` csharp
var dtos = _mapper.MapCollection<UserEntity, UserDto>(entities);
```

------------------------------------------------------------------------

## 🏗 Estructura del Proyecto

    RMapper
    │
    ├── Adapters
    ├── Builder
    ├── Configurations
    ├── DependencyInjection
    ├── Exceptions
    ├── Helpers
    ├── Implementation
    ├── Interfaces
    └── README.md

------------------------------------------------------------------------

# 📜 Licencia

MIT License

Copyright (c) 2026

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
