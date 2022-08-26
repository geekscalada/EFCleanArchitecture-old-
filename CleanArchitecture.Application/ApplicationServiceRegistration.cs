ususing CleanArchitecture.Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

//Video "crear dependencias en proyecto application" de momento marcado en rojo. 

namespace CleanArchitecture.Application
{
    // Inyección de dependencias de servicios. En versiones anteriores se inyectaba en el proyecto principal API dentro de startup.cs.
    // Ahora se puede inyectar sobre program.cs
    // Por buenas prácticas, mejor creamos un componente en core y posteriormente crear una instancia
    // de esta inyección en API
    public static class ApplicationServiceRegistration
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Aquí leera todas las clases que hereden las inerfaces del automapper y las va a inyectar
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }

    }
}
