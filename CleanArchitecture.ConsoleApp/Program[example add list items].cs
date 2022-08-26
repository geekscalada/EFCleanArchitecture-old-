using CleanArchitecture.Data;
using CleanArchitecture.Domain;

StreamerDbContext dbContext = new();

Streamer streamer = new()
{
    Nombre = "Amazon Primer",
    Url = "www.amazon.com"
};

// le ponemos ! para indicarle que el objeto está instanciado

dbContext!.Streamers!.Add(streamer);
// Como es un método asyncrono, tenemos que decir await. 
await dbContext.SaveChangesAsync();


var movies = new List<Video>
{
    new Video {
        Nombre = "Mad Max",
        // Aquí es curioso porque el Id lo obtenemos automáticamente al hacer la transacción
        // anterior (saveChangesAsync)
        StreamerId = streamer.Id
    },

    new Video {
        Nombre = "Batman",
        StreamerId = streamer.Id
    },

    new Video {
        Nombre = "Crepusculo",
        StreamerId = streamer.Id
    },

    new Video {
        Nombre = "Citizen Kane",
        StreamerId = streamer.Id
    },


};

await dbContext.AddRangeAsync(movies);

await dbContext.SaveChangesAsync();






