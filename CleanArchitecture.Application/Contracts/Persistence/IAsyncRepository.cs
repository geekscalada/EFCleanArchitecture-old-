using CleanArchitecture.Domain.Common;
using System.Linq.Expressions;


//Esto son métodos genéricos.

namespace CleanArchitecture.Application.Contracts.Persistence
{
    // Es un interfaz que va a tomar valores genéricos desde T donde 
    // T debe de ser de tipo BaseDomainModel
    public interface IAsyncRepository<T> where T : BaseDomainModel
    {
        // Decimos que es asíncrona porqe por delante le estamos diciendo que va a devolver un task 
        // IReadOnlyList es una lista genérica
        // T representa lo que va a devolver el tipo de objeto que va a devolver que puede ser
        // video, actor, director. 
        Task<IReadOnlyList<T>> GetAllAsync();

        // coleccion de datos pero con una determinada condición lógica dentro del query
        // esta condición lógica nosotros la manejtamos usando expression functions

        // le tienes que pasar como parámetro un objeto de tipo expression que a su vez contiene
        // a un function T boolean y esta expresión se transformará a futuro en una expresión
        // e tipo SQL, entonces se ejecutará el query en la BDD
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        //esta vaina la explico en apuntes ¿qué significa un expression y qué significa un func?

        //es una función con muchos parámetros, separados por comas e identados.
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true);


        //usamos includes que es la sintaxis para agregarle entidades a la query, por eso le llamamos igual al parámetro. 
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
                                        List<Expression<Func<T, Object>>> includes = null,
                                        bool disableTracking = true);


        // este sería una query, la más básica. Buscar por ID.
        Task<T> GetByIDAsync(int id);



        //manipular data

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(T entity);










    }
}
