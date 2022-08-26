using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

StreamerDbContext dbContext = new();
// QueryStreaming();
// await AddNewRecords();

//await QueryFilter();

//await QueryMethods();

//await QueryLinq();

//await TrackingAndNotTracking();

//await AddNewStreamerWithVideo();

//await AddNewStreamerWithVideoId();

//await AddNewActorWithVideo();

await MultipleEntitiesQuery();


async Task MultipleEntitiesQuery()
{
    //Esto sería como hacer un left join, parece que coge la tabla videoActores
    var videoWithActores = await dbContext!.Videos!.Include(q => q.Actores)
        .FirstOrDefaultAsync(q => q.Id == 1);
    // no imprimimos


    // aquí seleccionamos solo UNA columnas
    var actor = await dbContext!.Actor!.Select(q => q.Nombre).ToListAsync();

    // aquí seleccionamos solo varias columnas, está explicado en el ultimo video 
    // antes de la sección DDD.
    // El select por defecto solo nos deja una columna, entonces por eso tenemos que hacer
    // toda la vaina
    var videoWithDirector = await dbContext!.Videos!
        .Where(q => q.Director != null)
        .Include(q => q.Director)
        .Select(q =>
           new
           {
               Director_Nombre_Compleo = $"{q.Director.Nombre} {q.Director.Apellido}",
               Movie = q.Nombre
           }
        )
        .ToListAsync();

    foreach(var pelicula in videoWithDirector)
    {
        Console.WriteLine($"{pelicula.Movie} - {pelicula.Director_Nombre_Compleo}");
    }



}





// Añadir un actor y al mismo tiempo una referencia libroAutor
async Task AddNewActorWithVideo()
{
    var actor = new Actor
    {
        Nombre = "Brad",
        Apellido = "Pitt"
    };

    await dbContext.AddAsync(actor);
    await dbContext.SaveChangesAsync();

    var videoActor = new VideoActor
    {
        ActorId = actor.Id, //Actor tiene un ID porque lo hemos guardado antes. 
        VideoId = 1
    };

    await dbContext.AddAsync(videoActor);
    await dbContext.SaveChangesAsync();
    
}



// Añadir una nueva compañía de streaming con un vídeo

async Task AddNewStreamerWithVideo()
{
    var pantalla = new Streamer
    {
        Nombre = "pantalla"
    };

    var hungerGames = new Video
    {
        Nombre = "Hunger Games",
        Streamer = pantalla //Aquí le relacionamos este vídeo con este Streamer
    };

    await dbContext.AddAsync(hungerGames);
    await dbContext.SaveChangesAsync();
}

async Task AddNewStreamerWithVideoId()
{
    var batmanForever = new Video
    {
        Nombre = "Batman Forever",
        StreamerId = 4 
    };

    await dbContext.AddAsync(batmanForever);
    await dbContext.SaveChangesAsync();
}


async Task QueryFilter()
{

    Console.WriteLine("Presione cualquier tecla para terminar el programa");

    Console.ReadKey();    
    
    
    Console.WriteLine("Ingrese una compañía de streaming: ");
    var streamingNombre = Console.ReadLine();


    // Aquí vemos un where donde le tenemos que da una exp lambda
    // Aquí vemos un ejemplo de expresión lambda donde x es igual a 
    // streamers, entonces tenemos acceso a sus propiedades como Nombre
    var streamers = await dbContext!.Streamers!.Where(x => x.Nombre == streamingNombre ).ToListAsync();
    // Le tenemos que confirmar que no es un valor nulo poniendo los ! en dbContext y en la
    // tabla Streamers. 
    // se podría haber hecho con Nombre.equals()


    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre} ");
    }


    // Esta es una consulta en la que comprobamos si parte del nombre es contenido
    // por ejemplo si buscamos "Netf" te aparecerá netflix. 
    var streamerPartialResuls = await dbContext!.Streamers!.Where(x => x.Nombre!.Contains(streamingNombre)).ToListAsync();

    foreach(var streamer in streamerPartialResuls)
    {
        Console.WriteLine($" {streamer.Id} - {streamer.Nombre} ");
    }

    
    // esta sería la manera de hacer lo mismo que un contains pero con los métodos propios de  EF que se asemejan bastante a
    // la nomenclatura de SQL. 

    var streamerPartialResuls2 = await dbContext!.Streamers!.Where(x => EF.Functions.Like(x.Nombre, $"%{streamingNombre}%")).ToListAsync();

    foreach (var streamer in streamerPartialResuls2)
    {
        Console.WriteLine($" {streamer.Id} - {streamer.Nombre} ");
    }




}

async Task QueryMethods()
{

    var streamer = dbContext!.Streamers!;
    
    //Devolver un solo registro, si no hay nada, lanza una excepción
    var firstAsync = await streamer.Where(y => y.Nombre.Contains("a")).FirstAsync();

    // Si no hay nada devolvemos un valor por defecto de null
    var firstOrDefaultAsync = await streamer.Where(y => y.Nombre.Contains("a")).FirstOrDefaultAsync();

    // Sin el where
    var firstOrDefault_v2 = await streamer.FirstOrDefaultAsync(y => y.Nombre.Contains("a"));

    // En los ejemplos anteriores, cuando decimos que nos busque un resultado, este resultado
    // podía ser una colección de resutlados. 
    // Aquí no, dará excepción.
    var singleAsync = await streamer.Where(y => y.Id == 1).SingleAsync();


    // Si no hay records, no lanzará excepción, simplemente nos devolverá nulo
    var singleOrDefaultAsync = await streamer.Where(y => y.Id == 1).SingleOrDefaultAsync();


    // Usar directamtente el valor de la PK
    var resultado = streamer.FindAsync(1);
}

async Task TrackingAndNotTracking()
{
    var streamerWithTracking = await dbContext!.Streamers!.FirstOrDefaultAsync(x => x.Id == 1);

    var streamerWithNoTracking = await dbContext!.Streamers!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 2);


    streamerWithTracking.Nombre = "Netflix Super";

    streamerWithNoTracking.Nombre = "Amazon Plus"; // Esto no se va a poder actualizar
    //Porque el AsNoTracking borra los datos de la memoria temporal
    // tan pronto como se ejecuta la consulta. 

    await dbContext.SaveChangesAsync();
}

async Task QueryLinq()
{
    Console.WriteLine("Ingrese un nombre de streamer para buscar: ");
    var streamerNombre = Console.ReadLine();
    
    
    // Aquí linq es muy similar a una consulta de tipo SQL. i hace referencia a las columnas
    var streamers = await (from i in dbContext.Streamers
                           where EF.Functions.Like(i.Nombre, $"%{streamerNombre}%")
                           select i).ToListAsync(); // Nos devuelve todas las consultas

    foreach(var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }


}





