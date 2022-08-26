using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer
{
    public class UpdateStreamerCommandHandler : IRequestHandler<UpdateStreamerCommand>
    { 



        private readonly IStreamerRepository _streamerRepository;

        private readonly IMapper _mapper;

        private readonly ILogger<UpdateStreamerCommandHandler> _logger;

        public UpdateStreamerCommandHandler(IStreamerRepository streamerRepository, IMapper mapper, ILogger<UpdateStreamerCommandHandler> logger)
        {
            _streamerRepository = streamerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateStreamerCommand request, CancellationToken cancellationToken)
        {
            // Comprobar si lo que vamos a actualizar existe o no
            var streamerToUpdate = await _streamerRepository.GetByIDAsync(request.Id);

            if(streamerToUpdate == null)
            {

                // Hacemos log y posteriormente exception
                _logger.LogError($"No se encontró el streamer  id {request.Id}");

                //nameof parece que es para convertir a string
                // entiendo que no pasas un streamer sino la "clase" para decir que no se ha 
                // encontado cierta entidad con un ID particular. 

                throw new NotFoundException(nameof(Streamer), request.Id);

               
            };

            //reemplazamos

            // origen request, destino streamertoupdate, y ponemos los tipos de datos también

            _mapper.Map(request, streamerToUpdate, typeof(UpdateStreamerCommand), typeof(Streamer));

            //ahora enviamos a la base de datos. 
            // lo hacemos con el repositorio

            await _streamerRepository.UpdateAsync(streamerToUpdate);

            _logger.LogInformation($"La operación se realizó con éxito actualizando el streamer {request.Id}");

            return Unit.Value;



        }
    }
}
