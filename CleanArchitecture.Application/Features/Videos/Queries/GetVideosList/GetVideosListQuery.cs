using MediatR;


namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList
{

    // Si recuerdas en apuntes, para que podamos comunicar este query y el queryhandler necesitamos la librería IRequest de MediatR

    // VideosVm es un modelo que vamos a tener que generar a nivel de la carpeta getVideosList
    public class GetVideosListQuery : IRequest<List<VideosVm>>
    {
        // Inicializamos a un string vacío. 
        public string? _UserName { get; set; } = string.Empty;

        //Constructor
        public GetVideosListQuery(string username)
        {
            _UserName = username ?? throw new ArgumentNullException(nameof(username));


        }


    }
}
