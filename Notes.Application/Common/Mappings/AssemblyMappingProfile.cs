//хороший способ организовать конфигурацию мапинга это создать класс, наследующийся от Profile
//и положить конфигурацию в конструктор

using AutoMapper;
using System.Reflection;

namespace Notes.Application.Common.Mappings;

public class AssemblyMappingProfile : Profile
{
    //тип Assembly представляет нашу сборку
    public AssemblyMappingProfile(Assembly assembly) =>
        ApplyMappingsFromAssembly(assembly);

    //класс применяет маппинг из сборки с помощью данного метода, который сканирует сборку
    //и ищет любые типы, реализующие интерфейс IMapWith
    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
            .ToList();
        foreach(var type in types)
        {
            var instance = Activator.CreateInstance(type);
            //вызывает метод Mapping из отнаследованного типа или из интерфейса, если тип не
            //реализует этот интерфейс
            var methodInfo = type.GetMethod("Mapping");
            methodInfo?.Invoke(instance, new object[] {this});
        }
    }
}

