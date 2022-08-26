using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using MediatR;


namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList
{
    //ahora vamos a hacer la implementación de la comunicación mediante la interfaz mediaTr
    public class GetVideosListQueryHandler : IRequestHandler<GetVideosListQuery, List<VideosVm>>
    {
        //inyectar la interface necesaria (leer más abajo)
        private readonly IVideoRepository _videoRepository;

        //Necesitamos mapear. Porque _videoRepository va a ser de tipo lista de video, pero cuando lo devuelvas
        // debes de devolverlo como VideosVm. COnvertir un tipo de valor a otro, necesitamos imapper

        private readonly IMapper _mapper;

        //podemos hacerlo con menú de ctrl + .
        public GetVideosListQueryHandler(IVideoRepository videoRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _mapper = mapper;
        }




        // este método nos lo obliga implementar el IRequestHandler
        //aquí se supone que debemos de insertar la lógica
        // pero recuerda que es IVeideoRepository la encargada de ello entonces tenemos que inyectar. 
        public async Task<List<VideosVm>> Handle(GetVideosListQuery request, CancellationToken cancellationToken)
        {
            var videoList = await _videoRepository.GetVideoByUsername(request._UserName);

            //Aquí el destino sea VideosVm el resultado de videoList.
            //mejor explicado en los commands de streamer
            // cuidado porque el mapper hay que inicializarle una configuración. 
                return _mapper.Map<List<VideosVm>>(videoList);

        }
    }
}
